#if PCSOFT_ADS_ADMOB
#if PCSOFT_ADS_ADMOB && (UNITY_ANDROID || UNITY_IPHONE)
using System.Collections.Generic;
using GoogleMobileAds.Api;
#endif
using UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Assets;
using UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components;
using UnityAssetLoader.Runtime.asset_loader.Scripts.Runtime;
using InterstitialAd = UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components.InterstitialAd;
using UnityEngine;
using RewardedAd = UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components.RewardedAd;

namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime
{
    internal static class UnityAdvertisementExEvents
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        public static void Initialize()
        {
            Debug.Log("Load ads...");
            AssetResourcesLoader.LoadFromResources<AdsSettings>("");

#if PCSOFT_ADS_ADMOB && (UNITY_ANDROID || UNITY_IPHONE)
            Debug.Log("> Ads");

            var requestConfiguration = new RequestConfiguration.Builder()
                .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.True)
                .SetTagForUnderAgeOfConsent(TagForUnderAgeOfConsent.True)
                .SetTestDeviceIds(new List<string> { AdRequest.TestDeviceSimulator })
                .SetSameAppKeyEnabled(true)
                .build();
            MobileAds.SetRequestConfiguration(requestConfiguration);
            MobileAds.Initialize(_ => Debug.Log("Initialize Ads"));

            MobileAds.RaiseAdEventsOnUnityMainThread = true;
#endif
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void InitializeLate()
        {
            Debug.Log("Initialize ads...");
            foreach (var item in AdsSettings.Singleton.BannerItems)
            {
                var go = new GameObject("Banner (" + item.Identifier + ")");
                Object.DontDestroyOnLoad(go);
                var bannerAd = go.AddComponent<BannerAd>();
                bannerAd.Preset = item;
            }

            foreach (var item in AdsSettings.Singleton.InterstitialItems)
            {
                var go = new GameObject("Interstitial (" + item.Identifier + ")");
                Object.DontDestroyOnLoad(go);
                var interstitialAd = go.AddComponent<InterstitialAd>();
                interstitialAd.Preset = item;
            }

            foreach (var item in AdsSettings.Singleton.RewardedItems)
            {
                var go = new GameObject("Rewarded (" + item.Identifier + ")");
                Object.DontDestroyOnLoad(go);
                var awardAd = go.AddComponent<RewardedAd>();
                awardAd.Preset = item;
            }
        }
    }
}
#endif