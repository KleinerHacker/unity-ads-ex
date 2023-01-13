#if GOOGLE_ADMOB
using System;
using GoogleMobileAds.Api;
using UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Assets;
using UnityEngine;
#endif

namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components
{
#if GOOGLE_ADMOB
    [DisallowMultipleComponent]
    public sealed class InterstitialAd : AdBase<AdsInterstitialItem>
    {
        #region Properties

        public override bool SupportHide { get; } = false;

        #endregion

        private GoogleMobileAds.Api.InterstitialAd _interstitialAd;
        private Action _finishAction;

        public void Show(Action onFinished)
        {
            if (IsShown)
            {
#if LOGGING_ADMOB
                Debug.Log("[ADVERTISEMENT] Ad already shown");
#endif

                onFinished?.Invoke();
                return;
            }
            
#if LOGGING_ADMOB
            Debug.Log("[ADVERTISEMENT] Show Interstitial Ad");
#endif

#if !UNITY_EDITOR && GOOGLE_ADMOB && (UNITY_ANDROID || UNITY_IPHONE)
            _finishAction = onFinished;
            base.Show();
#else
            onFinished?.Invoke();
#endif
        }

        protected override void DoRequest(string id, AdRequest request)
        {
            _interstitialAd = new GoogleMobileAds.Api.InterstitialAd(id);
            _interstitialAd.OnAdLoaded += BannerOnAdLoaded;
            _interstitialAd.OnAdClosed += BannerOnAdClosed;
            _interstitialAd.OnAdOpening += BannerOnAdOpening;
            _interstitialAd.OnAdFailedToLoad += BannerOnAdFailedToLoad;
            _interstitialAd.OnPaidEvent += BannerOnAdPaid;

            _interstitialAd.LoadAd(request);
        }

        protected override void DoDispose()
        {
            _interstitialAd.OnAdLoaded -= BannerOnAdLoaded;
            _interstitialAd.OnAdClosed -= BannerOnAdClosed;
            _interstitialAd.OnAdOpening -= BannerOnAdOpening;
            _interstitialAd.OnAdFailedToLoad -= BannerOnAdFailedToLoad;
            _interstitialAd.OnPaidEvent -= BannerOnAdPaid;

            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        protected override void DoShow()
        {
            if (_interstitialAd.IsLoaded())
            {
                _interstitialAd.Show();
            }
            else
            {
                _finishAction?.Invoke();
                _finishAction = null;
            }
        }

        protected override void DoHide()
        {
            throw new NotSupportedException();
        }

        protected override void BannerOnAdClosed(object sender, EventArgs e)
        {
            base.BannerOnAdClosed(sender, e);
            _finishAction?.Invoke();
            _finishAction = null;
        }

        protected override void BannerOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            base.BannerOnAdFailedToLoad(sender, e);
            _finishAction?.Invoke();
            _finishAction = null;
        }
    }
#endif
}