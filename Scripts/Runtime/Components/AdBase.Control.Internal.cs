#if PCSOFT_ADS_ADMOB
using GoogleMobileAds.Api;
using UnityEngine;

namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components
{
    public abstract partial class AdBase
    {
        protected void Request()
        {
#if PCSOFT_ADS_ADMOB && (UNITY_ANDROID || UNITY_IPHONE)
#if PCSOFT_ADS_ADMOB_LOGGING
            Debug.Log("[ADVERTISEMENT] Try to request ad", this);
#endif

            var request = new AdRequest.Builder().Build();

            DoRequest(AdId, request);
#endif
        }

        protected void Dispose()
        {
#if PCSOFT_ADS_ADMOB_LOGGING
            Debug.Log("[ADVERTISEMENT] Try top destroy ad");
#endif

#if PCSOFT_ADS_ADMOB && (UNITY_ANDROID || UNITY_IPHONE)
            DoDispose();
#endif
        }

        protected abstract void DoRequest(string id, AdRequest request);
        protected abstract void DoDispose();
    }
}
#endif