#if PCSOFT_ADS_ADMOB
using System;
using GoogleMobileAds.Api;
using UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Assets;

namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components
{
    public sealed partial class BannerAd
    {
        protected override void DoRequest(string id, AdRequest request)
        {
            if (_banner != null)
            {
                DoDispose();
            }

            var adSize = Preset.Size switch
            {
                AdBannerSize.Banner => AdSize.Banner,
                AdBannerSize.Leaderboard => AdSize.Leaderboard,
                AdBannerSize.IabBanner => AdSize.IABBanner,
                AdBannerSize.MediumRectangle => AdSize.MediumRectangle,
                AdBannerSize.AnchorLandscape => AdSize.GetLandscapeAnchoredAdaptiveBannerAdSizeWithWidth(Preset.Width),
                AdBannerSize.AnchorPortrait => AdSize.GetPortraitAnchoredAdaptiveBannerAdSizeWithWidth(Preset.Width),
                AdBannerSize.AnchorOrientationDepend => AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(Preset.Width),
                AdBannerSize.Custom => new AdSize(Preset.Width, Preset.Height),
                _ => throw new NotImplementedException(Preset.Size.ToString())
            };

            _banner = new BannerView(id, adSize, Preset.Position);
            _banner.OnBannerAdLoaded += OnAdLoaded;
            _banner.OnBannerAdLoadFailed += OnAdFailedToLoad;

            _banner.OnAdFullScreenContentClosed += OnAdClosed;
            _banner.OnAdFullScreenContentOpened += OnAdOpening;

            _banner.OnAdPaid += OnAdPaid;
            _banner.OnAdClicked += OnAdClicked;
            _banner.OnAdImpressionRecorded += OnAdImpressionRecorded;

            _request = request;
        }

        protected override void DoDispose()
        {
            if (_banner == null)
                return;

            _banner.OnBannerAdLoaded -= OnAdLoaded;
            _banner.OnBannerAdLoadFailed -= OnAdFailedToLoad;

            _banner.OnAdFullScreenContentClosed -= OnAdClosed;
            _banner.OnAdFullScreenContentOpened -= OnAdOpening;

            _banner.OnAdPaid -= OnAdPaid;
            _banner.OnAdClicked -= OnAdClicked;
            _banner.OnAdImpressionRecorded -= OnAdImpressionRecorded;

            _banner.Destroy();
            _banner = null;
        }

        private void OnAdLoaded()
        {
            base.OnAdLoaded("");
        }

        private void OnAdFailedToLoad(LoadAdError error)
        {
            base.OnAdFailedToLoad("", error, true);
        }
    }
}
#endif