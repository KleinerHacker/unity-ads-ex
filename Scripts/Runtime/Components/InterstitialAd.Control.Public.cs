#if PCSOFT_ADS_ADMOB
using System;
using UnityEngine;

namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components
{
    public sealed partial class InterstitialAd
    {
        public void Show(Action onFinished)
        {
            if (IsShown)
            {
#if PCSOFT_ADS_ADMOB_LOGGING
                Debug.LogWarning("[ADVERTISEMENT] Interstitial Ad already shown");
#endif

                onFinished?.Invoke();
                return;
            }

#if PCSOFT_ADS_ADMOB_LOGGING
            Debug.Log("[ADVERTISEMENT] Show Interstitial Ad");
#endif

#if PCSOFT_ADS_ADMOB && (UNITY_ANDROID || UNITY_IPHONE)
            _finishAction = onFinished;
            base.Show();
#else
            onFinished?.Invoke();
#endif
        }

        protected override bool DoShow()
        {
            if (_interstitialAd != null && _interstitialAd.CanShowAd())
            {
#if PCSOFT_ADS_ADMOB_LOGGING
                Debug.Log("[ADVERTISEMENT] Show interstitial ad now");
#endif
                _interstitialAd.Show();
                return true;
            }

#if PCSOFT_ADS_ADMOB_LOGGING
            Debug.Log("[ADVERTISEMENT] Unable to show interstitial ad, not ready");
#endif

            _finishAction?.Invoke();
            _finishAction = null;

            Request();
            return false;
        }

        protected override bool DoHide()
        {
            throw new NotSupportedException();
        }
    }
}
#endif