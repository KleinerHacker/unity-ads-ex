#if PCSOFT_ADS_ADMOB
using System;
using GoogleMobileAds.Api;
using UnityEngine;

namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components
{
    public abstract partial class AdBase
    {
        #region Load and Failure

        protected virtual void OnAdLoaded(string id)
        {
#if PCSOFT_ADS_ADMOB_LOGGING
            Debug.Log("[ADVERTISEMENT] Ad loaded: " + id, this);
#endif
            OnLoaded?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnAdFailedToLoad(string id, LoadAdError error, bool runNewRequest)
        {
            Debug.LogWarning("[ADVERTISEMENT] Ad failure (" + id + "): " + error, this);
            try
            {
                OnFailed?.Invoke(this, new AdFailedToLoadEventArgs { LoadAdError = error });
            }
            catch (Exception e)
            {
                Debug.LogError("[ADVERTISEMENT] Failure while event callback (" + id + "): " + e.Message);
            }

            if (IsShown && SupportHide)
            {
                Hide();
            }

            IsShown = false;

            Dispose();
            if (runNewRequest)
            {
                Request();
            }
        }

        #endregion

        #region Open and Close

        protected virtual void OnAdOpening()
        {
#if PCSOFT_ADS_ADMOB_LOGGING
            Debug.Log("[ADVERTISEMENT] Ad opened", this);
#endif
            OnOpened?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnAdClosed()
        {
#if PCSOFT_ADS_ADMOB_LOGGING
            Debug.Log("[ADVERTISEMENT] Ad closed", this);
#endif
            try
            {
                OnClosed?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                Debug.LogError("[ADVERTISEMENT] Failure while event callback: " + e.Message);
            }

            if (IsShown && SupportHide)
            {
                Hide();
            }

            IsShown = false;

            Dispose();
            Request();
        }

        #endregion

        #region Impression Events

        protected virtual void OnAdPaid(AdValue adValue)
        {
#if PCSOFT_ADS_ADMOB_LOGGING
            Debug.Log("[ADVERTISEMENT] Ad paid: " + adValue.Precision + ", " + adValue.Value + ", " + adValue.CurrencyCode);
#endif
            OnPaid?.Invoke(this, new AdValueEventArgs { AdValue = adValue });
        }

        protected virtual void OnAdClicked()
        {
#if PCSOFT_ADS_ADMOB_LOGGING
            Debug.Log("[ADVERTISEMENT] Ad clicked");
#endif
            OnClicked?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnAdImpressionRecorded()
        {
#if PCSOFT_ADS_ADMOB_LOGGING
            Debug.Log("[ADVERTISEMENT] Ad impression recorded");
#endif
            OnImpressionRecorded?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
#endif