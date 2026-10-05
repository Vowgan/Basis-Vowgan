using System;
using Basis;
using Basis.Network.Core;
using Basis.Scripts.BasisSdk.Interactions;
using Basis.Scripts.Device_Management.Devices;
using Basis.Scripts.Networking.NetworkedAvatar;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VowganVR
{
    [Cilboxable]
    [RequireComponent(typeof(BasisInteractableObject))]
    public class SpinTheBottle : MonoBehaviour
    {

        [SerializeField] private float _bottleRotation;
        [SerializeField] private float _timeBetweenClicks = 1;

        private BasisNetworkShim _networkShim;
        private BasisInteractableObject _interactable;
        private Animator _anim;
        private int _hashSpin;
        private float _lastSpinTime;


        private void Start()
        {
            _anim = GetComponent<Animator>();
            _hashSpin = Animator.StringToHash("Spin");

            _interactable = GetComponent<BasisInteractableObject>();
            _interactable.OnInteractStartEvent.AddListener(OnInteract);

            _networkShim = SafeUtil.MakeNetworkable(this);
            _networkShim.NetworkMessageReceived = NetworkMessageReceived;
            _networkShim.ServerOwnershipDestroyed = OnServerOwnershipDestroyed;
            _networkShim.PlayerJoined = PlayerConnected;
        }

        public void NetworkMessageReceived(ushort playerId, byte[] buffer, DeliveryMethod deliveryMethod)
        {
            if (buffer.Length == 0) return;
            _bottleRotation = FromNetValue(buffer[0]);

            ApplyRotation();
        }

        private void ApplyRotation()
        {
            transform.rotation = Quaternion.Euler(0, _bottleRotation, 0);
            _anim.SetTrigger(_hashSpin);
            _lastSpinTime = Time.timeSinceLevelLoad;
        }


        public void BroadcastSpin()
        {
            if (Time.timeSinceLevelLoad - _lastSpinTime < _timeBetweenClicks) return;

            _bottleRotation = Random.Range(0, 360);

            _networkShim.SendCustomNetworkEvent(
                new byte[] { ToNetValue(_bottleRotation) },
                DeliveryMethod.ReliableOrdered);

            ApplyRotation();
        }

        private void OnInteract(BasisInput input)
        {
            _networkShim.TakeOwnership();
            BroadcastSpin();
        }

        public void OnServerOwnershipDestroyed()
        {
            _networkShim.TakeOwnership();
        }

        public void PlayerConnected(BasisNetworkPlayer player)
        {
            if (_networkShim.IsLocalOwner())
            {
                _networkShim.SendCustomNetworkEvent(
                    new byte[] { ToNetValue(_bottleRotation) },
                    DeliveryMethod.ReliableOrdered,
                    new ushort[] { player.playerId });
            }
        }

        private static byte ToNetValue(float rotation)
        {
            float point = Mathf.Lerp(0, 360, rotation);
            float lerp = Mathf.InverseLerp(byte.MinValue, byte.MaxValue, point);
            return Convert.ToByte(lerp);
        }

        private static float FromNetValue(byte netValue)
        {
            float lerp = Mathf.InverseLerp(byte.MinValue, byte.MaxValue, netValue);
            return Mathf.Lerp(0, 360, lerp);
        }
    }
}
