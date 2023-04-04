#if PCSOFT_ADS_ADMOB
using GoogleMobileAds.Api;
using UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Assets;
using UnityEngine;

namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components
{
    [DisallowMultipleComponent]
    public sealed partial class BannerAd : AdBase<AdsBannerItem>
    {
        #region Properties

        public override bool SupportHide { get; } = true;
        public override bool IsReady => _banner != null;

        #endregion

        private BannerView _banner;
        private AdRequest _request;

        #region Builtin Methods

        protected override void Start()
        {
            base.Start();
            if (Preset.ShowImmediately)
            {
                Show();
            }
        }

        protected override void OnDisable()
        {
            if (IsShown)
            {
                Hide();
            }

            base.OnDisable();
        }

        #endregion
    }
}
#endif