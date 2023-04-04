#if PCSOFT_ADS_ADMOB
using System;
using UnityEngine;

namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components
{
    public abstract partial class AdBase
    {
        public void Show()
        {
            if (IsShown)
                return;

#if PCSOFT_ADS_ADMOB_LOGGING
            Debug.Log("[ADVERTISEMENT] Try to show ad", this);
#endif

#if PCSOFT_ADS_ADMOB && (UNITY_ANDROID || UNITY_IPHONE)
            DoShow();
#endif
            IsShown = true;
        }

        public void Hide()
        {
            if (!SupportHide)
                throw new NotSupportedException("Ad do not support 'Hide'");
            if (!IsReady)
                throw new InvalidOperationException("Ad is not ready yet. please call 'Request' first");
            if (!IsShown)
                return;

#if PCSOFT_ADS_ADMOB_LOGGING
            Debug.Log("[ADVERTISEMENT] Hide ad", this);
#endif

#if PCSOFT_ADS_ADMOB && (UNITY_ANDROID || UNITY_IPHONE)
            DoHide();
#endif
            IsShown = false;
        }

        protected abstract void DoShow();
        protected abstract void DoHide();
    }
}
#endif