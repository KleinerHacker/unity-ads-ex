#if PCSOFT_ADS_ADMOB
using System;
using GoogleMobileAds.Api;
using UnityEngine;

namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components
{
    public sealed partial class RewardedAd
    {
        protected override void OnAdClosed()
        {
            base.OnAdClosed();

            try
            {
                _finishAction?.Invoke(new RewardInfo(null, RewardResult.RewardCanceled));
            }
            finally
            {
                _finishAction = null;
            }
        }

        private void OnAdFailedToShow(AdError error)
        {
            Debug.LogWarning("[ADVERTISEMENT] Ad show failure: " + error.GetMessage(), this);

            try
            {
                OnShowingFailed?.Invoke(this, new AdErrorEventArgs { AdError = error });
                _finishAction?.Invoke(new RewardInfo(null, RewardResult.NoAdToShow));
            }
            catch (Exception e)
            {
                Debug.LogError("[ADVERTISEMENT] Failure while event callback: " + e.Message);
            }

            _finishAction = null;
            IsShown = false;

            Dispose();
            Request();
        }

        private void RewardedAdOnUserEarnedReward(Reward e)
        {
#if PCSOFT_ADS_ADMOB_LOGGING
            Debug.Log("[ADVERTISEMENT] Ad earned reward: " + e.Type + ", " + e.Amount);
#endif

            try
            {
                _finishAction?.Invoke(new RewardInfo(e, RewardResult.Success));
            }
            finally
            {
                _finishAction = null;
                IsShown = false;
            }
        }
    }
}
#endif