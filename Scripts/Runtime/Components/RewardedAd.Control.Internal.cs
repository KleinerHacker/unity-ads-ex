#if PCSOFT_ADS_ADMOB
using GoogleMobileAds.Api;
using UnityEngine;

namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components
{
    public sealed partial class RewardedAd
    {
        protected override void DoRequest(string id, AdRequest request)
        {
            GoogleMobileAds.Api.RewardedAd.Load(id, request, (ad, error) =>
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
                    DestroyAd(_rewardedAd, id);
                    _rewardedAd = ad;
                    InitAd(_rewardedAd, id);
                }
            });
        }

        protected override void DoDispose()
        {
            DestroyAd(_rewardedAd, "");
            _rewardedAd = null;
        }

        private void InitAd(GoogleMobileAds.Api.RewardedAd ad, string id)
        {
#if PCSOFT_ADS_ADMOB_LOGGING
            Debug.Log("[ADVERTISEMENT] Initialize Interstitial Ad With Award: " + id);
#endif

            ad.OnAdFullScreenContentOpened += OnAdOpening;
            ad.OnAdFullScreenContentClosed += OnAdClosed;
            ad.OnAdFullScreenContentFailed += OnAdFailedToShow;

            ad.OnAdPaid += OnAdPaid;
            ad.OnAdClicked += OnAdClicked;
            ad.OnAdImpressionRecorded += OnAdImpressionRecorded;
        }

        private void DestroyAd(GoogleMobileAds.Api.RewardedAd ad, string id)
        {
#if PCSOFT_ADS_ADMOB_LOGGING
            Debug.Log("[ADVERTISEMENT] Destroy Interstitial Ad With Award: " + id);
#endif

            ad.OnAdFullScreenContentOpened -= OnAdOpening;
            ad.OnAdFullScreenContentClosed -= OnAdClosed;
            ad.OnAdFullScreenContentFailed -= OnAdFailedToShow;

            ad.OnAdPaid -= OnAdPaid;
            ad.OnAdClicked -= OnAdClicked;
            ad.OnAdImpressionRecorded -= OnAdImpressionRecorded;

            _rewardedAd.Destroy();
        }
    }
}
#endif