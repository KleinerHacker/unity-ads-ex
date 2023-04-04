#if PCSOFT_ADS_ADMOB
using System;
using GoogleMobileAds.Api;
using UnityEditor;
using UnityEditorEx.Runtime.editor_ex.Scripts.Runtime.Assets;
using UnityEngine;
using UnityEngine.Serialization;
#endif

namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Assets
{
#if PCSOFT_ADS_ADMOB
    public sealed class AdsSettings : ProviderAsset<AdsSettings>
    {
        #region Static Area

        public static AdsSettings Singleton => GetSingleton("Ads", "ads.asset");

#if UNITY_EDITOR
        public static SerializedObject SerializedSingleton => GetSerializedSingleton("Ads", "ads.asset");
#endif

        #endregion

        #region Inspector Data

        [SerializeField]
        private AdsBannerItem[] bannerItems = Array.Empty<AdsBannerItem>();

        [SerializeField]
        private AdsInterstitialItem[] interstitialItems = Array.Empty<AdsInterstitialItem>();

        [FormerlySerializedAs("interstitialWithAwardItems")]
        [SerializeField]
        private AdsRewardedItem[] rewardedItems = Array.Empty<AdsRewardedItem>();

        #endregion

        #region Properties

        public AdsBannerItem[] BannerItems => bannerItems;

        public AdsInterstitialItem[] InterstitialItems => interstitialItems;

        public AdsRewardedItem[] RewardedItems => rewardedItems;

        #endregion
    }

    [Serializable]
    public abstract class AdsItem
    {
        #region Inspector Data

        [FormerlySerializedAs("title")]
        [SerializeField]
        private string identifier;

        [FormerlySerializedAs("id")]
        [SerializeField]
        private string androidId;

        [SerializeField]
        private string iosId;

        #endregion

        #region Properties

        public string Identifier => identifier;

        public string AndroidId => androidId;

        public string IOSId => iosId;

        #endregion
    }

    [Serializable]
    public sealed class AdsBannerItem : AdsItem
    {
        #region Inspector Data

        [SerializeField]
        private AdPosition position = AdPosition.Bottom;

        [SerializeField]
        private bool showImmediately;

        [SerializeField]
        private AdBannerSize size = AdBannerSize.AnchorOrientationDepend;

        [SerializeField]
        private int width = -1;

        [SerializeField]
        private int height;

        #endregion

        #region Properties

        public AdPosition Position => position;

        public bool ShowImmediately => showImmediately;

        public AdBannerSize Size => size;

        public int Width => width;

        public int Height => height;

        #endregion
    }

    [Serializable]
    public class AdsInterstitialItem : AdsItem
    {
    }

    [Serializable]
    public sealed class AdsRewardedItem : AdsInterstitialItem
    {
    }
#endif

    public enum AdBannerSize
    {
        Banner = 0x00,
        IabBanner = 0x10,
        MediumRectangle = 0x20,
        Leaderboard = 0x30,
        AnchorLandscape = 0xA0,
        AnchorPortrait = 0xB0,
        AnchorOrientationDepend = 0xC0,
        Custom = 0xFF,
    }
}