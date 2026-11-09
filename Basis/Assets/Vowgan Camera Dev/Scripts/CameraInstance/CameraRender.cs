using System;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

namespace Vowgan.HandheldCameraSystem
{

    // References BasisHandHeldCameraCapture.cs

    [Serializable]
    public class CameraRender : IDisposable
    {

        public bool isCreated { get; private set; }
        public bool isRendering { get; private set; }
        public RenderTexture rt { get; private set; }

        [Tooltip("Capture format (EXR/PNG)")]
        public string captureFormat = "EXR";


        public int width
        {
            get => _width;
            set
            {
                _width = Mathf.Max(value, 1);
                rt.width = width;
            }
        }
        private int _width = 1920;

        public int height
        {
            get => _height;
            set
            {
                _height = Mathf.Max(value, 1);
                rt.height = height;
            }
        }
        private int _height = 1080;

        public int depth
        {
            get => _depth;
            set
            {
                _depth = Mathf.Max(value, 1);
                rt.depth = depth;
            }
        }
        private int _depth = 24;


        /// <summary>
        /// Number of frames rendered per-second.
        /// </summary>
        public float framerate
        {
            get => _framerate;
            set => _framerate = Mathf.Max(value, 0.001f);
        }
        private float _framerate = 30;


        [field: SerializeField] public RenderTextureFormat format { get; private set; } = RenderTextureFormat.ARGB32;
        [SerializeField] public bool isMirrored = false;

        [field: SerializeField] public int msaaSamples { get; private set; } = 1;


        [field: SerializeField] public string lastPhotoPath { get; private set; }
        [field: SerializeField] public string lastPhotoFailure { get; private set; }


        private RenderPipeline.StandardRequest _renderRequest;


        /// <summary>
        /// Initialize the render texture and render request.
        /// </summary>
        public void Create()
        {
            if (isCreated) return;
            isCreated = true;

            rt = new(new RenderTextureDescriptor(width, height, format, depth)
            {
                msaaSamples = msaaSamples,
                useMipMap = false,
                autoGenerateMips = false,
                sRGB = true,
            });

            _renderRequest = new() { destination = rt };
        }

        /// <summary>
        /// Cleanup the render texture and end the render loop.
        /// </summary>
        public void Dispose()
        {
            isCreated = false;
            isRendering = false;

            if (rt)
            {
                rt.Release();
                UnityEngine.Object.Destroy(rt);
                rt = null;
            }
        }


        /// <summary>
        /// Start rendering the image at a set framerate on a given camera.
        /// If the camera or the owner is destroyed, the render loop will end.
        /// </summary>
        public void StartRender(Camera camera, MonoBehaviour owner)
        {
            camera.targetTexture = rt;
            isRendering = true;
            _ = RenderCameraLoop(camera, owner);
        }

        /// <summary>
        /// Renders the camera at a set framerate until the owner is destroyed of EndRender is called.
        /// </summary>
        private async Task RenderCameraLoop(Camera camera, MonoBehaviour owner)
        {
            while (isRendering)
            {
                if (!owner || owner.destroyCancellationToken.IsCancellationRequested)
                {
                    EndRender();
                    break;
                }

                RenderOnce(camera);
                await Awaitable.WaitForSecondsAsync(1 / framerate, owner.destroyCancellationToken);
            }

            camera.targetTexture = null;
        }

        /// <summary>
        /// Render a camera a single time.
        /// </summary>
        public void RenderOnce(Camera camera)
        {
            Matrix4x4 matrix = camera.projectionMatrix;
            if (isMirrored)
            {
                camera.ResetProjectionMatrix();
                camera.projectionMatrix = matrix * Matrix4x4.Scale(new(-1, 1, 1));
                GL.invertCulling = true;
            }

            RenderPipeline.SubmitRenderRequest(camera, _renderRequest);

            GL.invertCulling = false;
            camera.ResetProjectionMatrix();
        }

        /// <summary>
        /// End the rendering process.
        /// </summary>
        public void EndRender()
        {
            isRendering = false;
        }





        public void SavePhoto(Camera camera, Transform ignoreRoot)
        {
            _ = SavePhotoAsFile(camera, ignoreRoot);
        }





        private async Task SavePhotoAsFile(Camera camera, Transform ignoreRoot)
        {
            RenderOnce(camera);

            BasisHandHeldCameraPhotoMetadata.PhotoMetadata photoMetadata = BasisHandHeldCameraPhotoMetadata.CollectMetadata(camera, ignoreRoot);

            bool exr = captureFormat == "EXR";

            // TODO: Swap this out for a pooled solution.
            // Formatting based on BasisCameraCaptureFormats.cs
            Texture2D render = new(rt.width, rt.height, exr ? TextureFormat.RGBAFloat : TextureFormat.RGBA32, false);

            // Copy the texture from the render into an actual RT.
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = rt;
            render.ReadPixels(new(0, 0, rt.width, rt.height), 0, 0);
            render.Apply();
            RenderTexture.active = previous;


            NativeArray<byte> rawData = render.GetRawTextureData<byte>();
            byte[] pixels = new byte[rawData.Length];
            NativeArray<byte>.Copy(rawData, pixels, rawData.Length);

            GraphicsFormat graphicsFormat = render.graphicsFormat;

            string stamp = BasisCameraPhotoFolder.Timestamp();
            string path = BasisCameraPhotoFolder.PathFor($"Screenshot_{stamp ?? BasisCameraPhotoFolder.Timestamp()}_{render.width}x{render.height}.{BasisCameraCaptureFormats.Extension(captureFormat)}");

            try
            {
                byte[] imageData = await BasisTasks.Run(() =>
                {
                    byte[] encoded = exr ?
                        ImageConversion.EncodeArrayToEXR(pixels, graphicsFormat, (uint)width, (uint)height, 0, Texture2D.EXRFlags.CompressZIP) :
                        ImageConversion.EncodeArrayToPNG(pixels, graphicsFormat, (uint)width, (uint)height, 0);
                    if (photoMetadata != null) encoded = BasisHandHeldCameraPhotoMetadata.Embed(encoded, captureFormat, photoMetadata, width, height);

                    return encoded;
                });

                await BasisFiles.WriteAllBytesAsync(path, imageData);
            }
            catch (Exception e)
            {
                RecordPhotoFailed(e);
                return;
            }

            RecordPhotoSaved(path);
        }

        /// <summary>
        /// Stores and logs the reason a photo save failed.
        /// </summary>
        private void RecordPhotoFailed(Exception e)
        {
            lastPhotoFailure = $"{e.GetType().Name}: {e.Message}";
            BasisDebug.LogError($"Could not save photo: {lastPhotoFailure}", BasisDebug.LogTag.Camera);
        }

        /// <summary>
        /// Stores the on-device filepath to the most recent saved photo.
        /// </summary>
        private void RecordPhotoSaved(string path)
        {
            lastPhotoPath = path;
            lastPhotoFailure = null;
        }



    }
}
