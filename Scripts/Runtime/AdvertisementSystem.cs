using System;
using UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Components;
#if PCSOFT_ADS_ADMOB
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using Object = UnityEngine.Object;
#endif

namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime
{
    public static class AdvertisementSystem
    {
#if PCSOFT_ADS_ADMOB
        public static void ShowBanner(string identifier) => FindBannerAd(identifier).Show();

        public static void HideBanner(string identifier) => FindBannerAd(identifier).Hide();

        public static void ShowInterstitial(string identifier, Action onFinished = null) => FindInterstitialAd(identifier).Show(onFinished);

        public static void ShowRewarded(string identifier, Action<RewardInfo> onFinished = null) => FindRewardedAd(identifier).Show(onFinished);

        private static BannerAd FindBannerAd(string identifier) =>
            FindAd<BannerAd>("banner", identifier, x => x.Preset.Identifier);

        private static InterstitialAd FindInterstitialAd(string identifier) =>
            FindAd<InterstitialAd>("interstitial", identifier, x => x.Preset.Identifier);

        private static RewardedAd FindRewardedAd(string identifier) =>
            FindAd<RewardedAd>("rewarded", identifier, x => x.Preset.Identifier);

        private static T FindAd<T>(string debugName, string identifier, Func<T, string> identifierExtractor) where T : AdBase =>
            Object.FindObjectsOfType<T>()
                .FirstOrThrow(x => string.Equals(identifierExtractor(x), identifier),
                    () => new ArgumentException("Unable to find any ad " + debugName + " with identifier " + identifier));
#else
        public static void ShowBanner(string identifier)
        {
        }

        public static void HideBanner(string identifier)
        {
        }

        public static void ShowInterstitial(string identifier, Action onFinished = null) => onFinished?.Invoke();

        public static void ShowRewarded(string identifier, Action<RewardInfo> onFinished = null) => onFinished?.Invoke(new RewardInfo());
#endif
    }
}