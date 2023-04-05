#if PCSOFT_ADS_ADMOB
using System;
using GoogleMobileAds.Api;
using UnityEngine;

namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components
{
    public sealed partial class InterstitialAd
    {
        protected override void OnAdClosed()
        {
            base.OnAdClosed();

            try
            {
                _finishAction?.Invoke();
            }
            finally
            {
                _finishAction = null;
            }
        }

        protected override void OnAdFailedToLoad(string id, LoadAdError error, bool runNewRequest)
        {
            base.OnAdFailedToLoad(id, error, runNewRequest);

            try
            {
                _finishAction?.Invoke();
            }
            finally
            {
                _finishAction = null;
            }
        }

        private void OnAdFailedToShow(AdError error)
        {
            Debug.LogError("[ADVERTISEMENT] Ad show failure: " + error.GetMessage(), this);

            try
            {
                OnShowingFailed?.Invoke(this, new AdErrorEventArgs { AdError = error });
                _finishAction?.Invoke();
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
    }
}
#endif