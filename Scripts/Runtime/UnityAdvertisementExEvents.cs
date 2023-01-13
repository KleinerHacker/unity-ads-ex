#if !UNITY_EDITOR && GOOGLE_ADMOB && (UNITY_ANDROID || UNITY_IPHONE)
using System.Collections.Generic;
using GoogleMobileAds.Api;
#endif

#if GOOGLE_ADMOB
using UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Assets;
using UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components;
using UnityAssetLoader.Runtime.asset_loader.Scripts.Runtime;
using InterstitialAd = UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components.InterstitialAd;
#endif

using UnityEngine;

namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime
{
    internal static class UnityAdvertisementExEvents
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        public static void Initialize()
        {
#if GOOGLE_ADMOB
            Debug.Log("Load ads...");
            AssetResourcesLoader.LoadFromResources<AdsSettings>("");
#endif
            
#if !UNITY_EDITOR && GOOGLE_ADMOB && (UNITY_ANDROID || UNITY_IPHONE)
            Debug.Log("> Ads");
            var requestConfiguration = new RequestConfiguration.Builder()
                .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.True)
                .SetTagForUnderAgeOfConsent(TagForUnderAgeOfConsent.True)
                .SetTestDeviceIds(new List<string> { AdRequest.TestDeviceSimulator })
                .build();
            MobileAds.SetRequestConfiguration(requestConfiguration);
            MobileAds.Initialize(initStatus => Debug.Log("Initialize Ads"));
#endif
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void InitializeLate()
        {
#if GOOGLE_ADMOB
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
            
            foreach (var item in AdsSettings.Singleton.InterstitialWithAwardItems)
            {
                var go = new GameObject("Interstitial with Award (" + item.Identifier + ")");
                Object.DontDestroyOnLoad(go);
                var interstitialWithAwardAd = go.AddComponent<InterstitialWithAwardAd>();
                interstitialWithAwardAd.Preset = item;
            }
#endif
        }
    }
}