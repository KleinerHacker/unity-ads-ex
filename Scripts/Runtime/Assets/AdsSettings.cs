using System;
using GoogleMobileAds.Api;
using UnityEditor;
using UnityEditorEx.Runtime.editor_ex.Scripts.Runtime.Assets;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Assets
{
#if GOOGLE_ADMOB
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

        [SerializeField]
        private AdsInterstitialWithAwardItem[] interstitialWithAwardItems = Array.Empty<AdsInterstitialWithAwardItem>();

        #endregion

        #region Properties

        public AdsBannerItem[] BannerItems => bannerItems;

        public AdsInterstitialItem[] InterstitialItems => interstitialItems;

        public AdsInterstitialWithAwardItem[] InterstitialWithAwardItems => interstitialWithAwardItems;

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

        #endregion

        #region Properties

        public AdPosition Position => position;

        public bool ShowImmediately => showImmediately;

        #endregion
    }

    [Serializable]
    public class AdsInterstitialItem : AdsItem
    {
    }

    [Serializable]
    public sealed class AdsInterstitialWithAwardItem : AdsInterstitialItem
    {
    }
#endif
}