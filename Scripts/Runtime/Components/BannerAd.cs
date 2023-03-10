#if GOOGLE_ADMOB
using GoogleMobileAds.Api;
using UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Assets;
using UnityEngine;
#endif

namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components
{
#if GOOGLE_ADMOB
    [DisallowMultipleComponent]
    public sealed class BannerAd : AdBase<AdsBannerItem>
    {
        #region Properties

        public override bool SupportHide { get; } = true;

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

        protected override void DoRequest(string id, AdRequest request)
        {
            _banner = new BannerView(id, AdSize.SmartBanner, Preset.Position);
            _banner.OnAdLoaded += BannerOnAdLoaded;
            _banner.OnAdClosed += BannerOnAdClosed;
            _banner.OnAdOpening += BannerOnAdOpening;
            _banner.OnAdFailedToLoad += BannerOnAdFailedToLoad;
            _banner.OnPaidEvent += BannerOnAdPaid;

            _request = request;
        }

        protected override void DoDispose()
        {
            _banner.OnAdLoaded -= BannerOnAdLoaded;
            _banner.OnAdClosed -= BannerOnAdClosed;
            _banner.OnAdOpening -= BannerOnAdOpening;
            _banner.OnAdFailedToLoad -= BannerOnAdFailedToLoad;
            _banner.OnPaidEvent -= BannerOnAdPaid;
            
            _banner.Destroy();
            _banner = null;
        }

        protected override void DoShow()
        {
            _banner.LoadAd(_request);
        }

        protected override void DoHide()
        {
            _banner.Hide();
            Dispose();
            Request();
        }
    }
#endif
}