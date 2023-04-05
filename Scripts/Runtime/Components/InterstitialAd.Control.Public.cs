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
                Debug.Log("[ADVERTISEMENT] Ad already shown");
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

        protected override void DoShow()
        {
            if (_interstitialAd != null && _interstitialAd.CanShowAd())
            {
                _interstitialAd.Show();
            }
            else
            {
                _finishAction?.Invoke();
                _finishAction = null;

                IsShown = false;
                Request();
            }
        }

        protected override void DoHide()
        {
            throw new NotSupportedException();
        }
    }
}
#endif