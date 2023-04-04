#if PCSOFT_ADS_ADMOB
using GoogleMobileAds.Editor;
using UnityAdvertisementEx.Editor.ads_ex.Scripts.Editor.Provider.Blocks;
using UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Assets;
using UnityEngine;
using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor.Utils;
using UnityEngine.UIElements;

#else
using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor.Utils;
using UnityEngine;
using UnityEngine.UIElements;
#endif

namespace UnityAdvertisementEx.Editor.ads_ex.Scripts.Editor.Provider
{
    public sealed class AdsProvider : SettingsProvider
    {
        #region Static Area

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new AdsProvider();
        }

        #endregion

#if PCSOFT_ADS_ADMOB
        private SerializedObject _serializedObject;
        private SerializedProperty _bannerItemsProperty;
        private SerializedProperty _interstitialItemsProperty;
        private SerializedProperty _rewardedItemsProperty;

        private GoogleMobileAdsSettings _ads;
        private UnityEditor.Editor _adsEditor;
        private AdsBannerBlockList _adsBannerBlockList;
        private AdsInterstitialBlockList _adsInterstitialBlockList;
        private AdsRewardedBlockList _adsRewardedBlockList;

        private bool _foldoutBanner;
        private bool _foldoutInterstitial;
        private bool _foldoutRewarded;
#endif

        public AdsProvider() : base("Project/Services/Ads/AdMobs", SettingsScope.Project, new[] { "Advertisement", "Ad", "Ads", "AdMobs" })
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
#if PCSOFT_ADS_ADMOB
            _ads = GoogleMobileAdsSettings.LoadInstance();

            _serializedObject = AdsSettings.SerializedSingleton;
            if (_serializedObject == null)
                return;

            _bannerItemsProperty = _serializedObject.FindProperty("bannerItems");
            _interstitialItemsProperty = _serializedObject.FindProperty("interstitialItems");
            _rewardedItemsProperty = _serializedObject.FindProperty("rewardedItems");
            _adsBannerBlockList = new AdsBannerBlockList(_serializedObject, _bannerItemsProperty);
            _adsInterstitialBlockList = new AdsInterstitialBlockList(_serializedObject, _interstitialItemsProperty);
            _adsRewardedBlockList = new AdsRewardedBlockList(_serializedObject, _rewardedItemsProperty);
#endif
        }

        public override void OnTitleBarGUI()
        {
            EditorGUILayout.BeginVertical();
            ExtendedEditorGUILayout.SymbolFieldLeft("Activate", "PCSOFT_ADS_ADMOB");
            ExtendedEditorGUILayout.SymbolFieldLeft("Verbose Logging", "PCSOFT_ADS_ADMOB_LOGGING");
            EditorGUILayout.EndVertical();
        }

        public override void OnGUI(string searchContext)
        {
            GUILayout.Space(15f);

#if PCSOFT_ADS_ADMOB
            _serializedObject.Update();

            UnityEditor.Editor.CreateCachedEditor(_ads, null, ref _adsEditor);
            _adsEditor.OnInspectorGUI();

            EditorGUILayout.Space(25f);

            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(5, 5, 0, 0) });
            EditorGUILayout.LabelField("Advertisement Blocks", EditorStyles.boldLabel);

            _foldoutBanner = EditorGUILayout.BeginFoldoutHeaderGroup(_foldoutBanner, "Banner Ads");
            if (_foldoutBanner)
            {
                _adsBannerBlockList.DoLayoutList();
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            _foldoutInterstitial = EditorGUILayout.BeginFoldoutHeaderGroup(_foldoutInterstitial, "Interstitial Ads");
            if (_foldoutInterstitial)
            {
                _adsInterstitialBlockList.DoLayoutList();
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            _foldoutRewarded = EditorGUILayout.BeginFoldoutHeaderGroup(_foldoutRewarded, "Rewarded Ads");
            if (_foldoutRewarded)
            {
                _adsRewardedBlockList.DoLayoutList();
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.EndVertical();

            _serializedObject.ApplyModifiedProperties();
#else
            EditorGUILayout.HelpBox("System is deactivated", MessageType.Info);
#endif
        }
    }
}