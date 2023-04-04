#if PCSOFT_ADS_ADMOB
using GoogleMobileAds.Api;
using UnityEngine;

namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components
{
    public sealed partial class InterstitialAd
    {
        protected override void DoRequest(string id, AdRequest request)
        {
            GoogleMobileAds.Api.InterstitialAd.Load(id, request, (ad, error) =>
            {
                try
                {
                    if (error == null)
                    {
                        OnAdLoaded();
                    }
                    else
                    {
                        OnAdFailedToLoad(error, false);
                    }
                }
                finally
                {
                    DestroyAd(_interstitialAd, id);
                    _interstitialAd = ad;
                    InitAd(_interstitialAd, id);
                }
            });
        }

        protected override void DoDispose()
        {
            DestroyAd(_interstitialAd, "");
            _interstitialAd = null;
        }

        private void InitAd(GoogleMobileAds.Api.InterstitialAd ad, string id)
        {
            if (ad == null)
                return;

#if PCSOFT_ADS_ADMOB_LOGGING
            Debug.Log("[ADVERTISEMENT] Initialize Interstitial Ad: " + id);
#endif

            ad.OnAdFullScreenContentOpened += OnAdOpening;
            ad.OnAdFullScreenContentClosed += OnAdClosed;
            ad.OnAdFullScreenContentFailed += OnAdFailedToShow;

            ad.OnAdPaid += OnAdPaid;
            ad.OnAdClicked += OnAdClicked;
            ad.OnAdImpressionRecorded += OnAdImpressionRecorded;
        }

        private void DestroyAd(GoogleMobileAds.Api.InterstitialAd ad, string id)
        {
            if (ad == null)
                return;

#if PCSOFT_ADS_ADMOB_LOGGING
            Debug.Log("[ADVERTISEMENT] Destroy Interstitial Ad: " + id);
#endif

            ad.OnAdFullScreenContentOpened -= OnAdOpening;
            ad.OnAdFullScreenContentClosed -= OnAdClosed;
            ad.OnAdFullScreenContentFailed -= OnAdFailedToShow;

            ad.OnAdPaid -= OnAdPaid;
            ad.OnAdClicked -= OnAdClicked;
            ad.OnAdImpressionRecorded -= OnAdImpressionRecorded;

            ad.Destroy();
        }
    }
}
#endif