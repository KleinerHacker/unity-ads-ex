#if DEMO
using UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityAdvertisementEx.Demo.ads_ex.Scripts.Demo
{
    public sealed class UIAdvertisement : UIBehaviour
    {
        [SerializeField]
        private Toggle banner;

        protected override void OnEnable()
        {
            banner.onValueChanged.AddListener(OnValueChanged);
        }

        protected override void OnDisable()
        {
            banner.onValueChanged.RemoveListener(OnValueChanged);
        }

        public void HandleInterstitial()
        {
            AdvertisementSystem.ShowInterstitial("Interstitial");
        }

        public void HandleRewarded()
        {
            AdvertisementSystem.ShowRewarded("Rewarded");
        }

        private void OnValueChanged(bool v)
        {
            if (v)
            {
                AdvertisementSystem.ShowBanner("Banner");
            }
            else
            {
                AdvertisementSystem.HideBanner("Banner");
            }
        }
    }
}
#endif