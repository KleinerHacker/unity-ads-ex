#if GOOGLE_ADMOB
using System;
using UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using Object = UnityEngine.Object;

namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime
{
    public static class AdvertisementSystem
    {
        public static void ShowBanner(string identifier) => FindBannerAd(identifier).Show();

        public static void HideBanner(string identifier) => FindBannerAd(identifier).Hide();

        public static void ShowInterstitial(string identifier, Action onFinished = null) => FindInterstitialAd(identifier).Show(onFinished);

        public static void ShowInterstitialWithAward(string identifier, Action<RewardInfo> onFinished = null) => FindInterstitialWithAwardAd(identifier).Show(onFinished);

        private static BannerAd FindBannerAd(string identifier) =>
            FindAd<BannerAd>("banner", identifier, x => x.Preset.Identifier);
        
        private static InterstitialAd FindInterstitialAd(string identifier) =>
            FindAd<InterstitialAd>("interstitial", identifier, x => x.Preset.Identifier);
        
        private static InterstitialWithAwardAd FindInterstitialWithAwardAd(string identifier) =>
            FindAd<InterstitialWithAwardAd>("interstitial with Award", identifier, x => x.Preset.Identifier);

        private static T FindAd<T>(string debugName, string identifier, Func<T, string> identifierExtractor) where T : AdBase =>
            Object.FindObjectsOfType<T>()
                .FirstOrThrow(x => string.Equals(identifierExtractor(x), identifier),
                    () => new ArgumentException("Unable to find any ad " + debugName + " with identifier " + identifier));
    }
}
#endif