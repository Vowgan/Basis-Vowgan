using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class CameraRender : IDisposable
{

    public bool isCreated { get; private set; }
    public bool isRendering { get; private set; }
    public RenderTexture rt { get; private set; }


    [field: SerializeField] public int width { get; private set; } = 1920;
    [field: SerializeField] public int height { get; private set; } = 1080;
    [field: SerializeField] public RenderTextureFormat format { get; private set; } = RenderTextureFormat.ARGB32;
    [field: SerializeField] public int depth { get; private set; } = 24;

    /// <summary>
    /// Number of frames rendered per-second.
    /// </summary>
    [field: SerializeField] public float framerate { get; private set; } = 30;
    [field: SerializeField] public int msaaSamples { get; private set; } = 1;

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
        _ = RenderCamera(camera, owner);
    }

    private async Task RenderCamera(Camera camera, MonoBehaviour owner)
    {
        while (isRendering)
        {
            if (!owner)
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
        RenderPipeline.SubmitRenderRequest(camera, _renderRequest);
    }

    /// <summary>
    /// Cancel the rendering process.
    /// </summary>
    public void EndRender()
    {
        isRendering = false;
    }





    public void Resize(int newWidth, int newHeight)
    {
        if (width == newWidth && height == newHeight) return;

        width = newWidth;
        height = newHeight;

        rt.width = width;
        rt.height = height;
    }

    public void SetDepth(int newDepth)
    {
        if (depth == newDepth) return;
        depth = newDepth;
        rt.depth = depth;
    }

    public void SetFramerate(float newFramerate)
    {
        if (Mathf.Approximately(framerate, newFramerate)) return;
        // no divide by zero buts
        framerate = Mathf.Max(0.001f, newFramerate);
    }
}
