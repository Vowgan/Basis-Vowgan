using Basis;
using Basis.Scripts.BasisSdk.Interactions;
using UnityEngine;

namespace Vowgan
{
    [Cilboxable]
    public class SprayBottle : MonoBehaviour
    {

        [SerializeField] private ParticleSystem _particles;
        [SerializeField] private AudioSource _sounds;

        private BasisNetworkShim _networkShim;


        private void Awake()
        {
            BasisPickupInteractable pickup = GetComponent<BasisPickupInteractable>();
            pickup.OnPickupUse.AddListener(OnPickupUse);

            _networkShim = SafeUtil.MakeNetworkable(this);
            _networkShim.NetworkMessageReceived = (_, _, _) => PlayParticles();
        }

        private void OnPickupUse(BasisPickUpUseMode mode)
        {
            switch (mode)
            {
                default:
                case BasisPickUpUseMode.OnPickUpUseUp:
                    break;
                case BasisPickUpUseMode.OnPickUpUseDown:
                    if (_networkShim.HasNetworkID) _networkShim.SendCustomNetworkEvent();
                    PlayParticles();
                    break;
                case BasisPickUpUseMode.OnPickUpStillDown:
                    break;
            }
        }

        private void PlayParticles()
        {
            if (_particles) _particles.Play();
            if (_sounds) _sounds.Play();
        }

    }
}
