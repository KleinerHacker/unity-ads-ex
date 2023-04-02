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

        #region Events

        public event EventHandler<AdErrorEventArgs> OnShowingFailed;

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
            GoogleMobileAds.Api.InterstitialAd.Load(id, request, (ad, error) =>
            {
                if (error == null)
                {
                    OnAdLoaded();
                }
                else
                {
                    OnAdFailedToLoad(error);
                }

                DestroyAd(_interstitialAd);
                _interstitialAd = ad;
                InitAd(_interstitialAd);
            });
        }

        protected override void DoDispose() => DestroyAd(_interstitialAd);

        private void InitAd(GoogleMobileAds.Api.InterstitialAd ad)
        {
            if (ad == null)
                return;

            ad.OnAdFullScreenContentClosed += OnAdClosed;
            ad.OnAdFullScreenContentOpened += OnAdOpening;
            ad.OnAdFullScreenContentFailed += OnAdFailedToShow;
            ad.OnAdPaid += OnAdPaid;
        }

        private void DestroyAd(GoogleMobileAds.Api.InterstitialAd ad)
        {
            if (ad == null)
                return;

            ad.OnAdFullScreenContentClosed -= OnAdClosed;
            ad.OnAdFullScreenContentOpened -= OnAdOpening;
            ad.OnAdFullScreenContentFailed -= OnAdFailedToShow;
            ad.OnAdPaid -= OnAdPaid;

            _interstitialAd.Destroy();
        }

        protected override void DoShow()
        {
            if (_interstitialAd.CanShowAd())
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

        protected override void OnAdClosed()
        {
            base.OnAdClosed();
            _finishAction?.Invoke();
            _finishAction = null;
        }

        protected override void OnAdFailedToLoad(LoadAdError error)
        {
            base.OnAdFailedToLoad(error);
            _finishAction?.Invoke();
            _finishAction = null;
        }

        private void OnAdFailedToShow(AdError error)
        {
            Debug.LogError("[ADVERTISEMENT] Ad show failure: " + error.GetMessage(), this);
            OnShowingFailed?.Invoke(this, new AdErrorEventArgs { AdError = error });
            _finishAction?.Invoke();
            _finishAction = null;
        }
    }
#endif
}