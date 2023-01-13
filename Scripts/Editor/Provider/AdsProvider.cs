#if GOOGLE_ADMOB
using GoogleMobileAds.Editor;
using UnityAdvertisementEx.Editor.ads_ex.Scripts.Editor.Provider.Blocks;
using UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Assets;
using UnityEngine;
#endif
using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor.Utils;
using UnityEngine.UIElements;

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

#if GOOGLE_ADMOB
        private SerializedObject _serializedObject;
        private SerializedProperty _bannerItemsProperty;
        private SerializedProperty _interstitialItemsProperty;
        private SerializedProperty _interstitialWithAwardItemsProperty;

        private GoogleMobileAdsSettings _ads;
        private UnityEditor.Editor _adsEditor;
        private AdsBannerBlockList _adsBannerBlockList;
        private AdsInterstitialBlockList _adsInterstitialBlockList;
        private AdsInterstitialWithAwardBlockList _adsInterstitialWithAwardBlockList;

        private bool _foldoutBanner;
        private bool _foldoutInterstitial;
        private bool _foldoutInterstitialWithAward;
#endif

        public AdsProvider() : base("Project/Player/Advertisement/AdMobs", SettingsScope.Project, new[] { "Advertisement", "Ad", "Ads", "AdMobs" })
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
#if GOOGLE_ADMOB
            _ads = GoogleMobileAdsSettings.LoadInstance();

            _serializedObject = AdsSettings.SerializedSingleton;
            if (_serializedObject == null)
                return;

            _bannerItemsProperty = _serializedObject.FindProperty("bannerItems");
            _interstitialItemsProperty = _serializedObject.FindProperty("interstitialItems");
            _interstitialWithAwardItemsProperty = _serializedObject.FindProperty("interstitialWithAwardItems");
            _adsBannerBlockList = new AdsBannerBlockList(_serializedObject, _bannerItemsProperty);
            _adsInterstitialBlockList = new AdsInterstitialBlockList(_serializedObject, _interstitialItemsProperty);
            _adsInterstitialWithAwardBlockList = new AdsInterstitialWithAwardBlockList(_serializedObject, _interstitialWithAwardItemsProperty);
#endif
        }

        public override void OnTitleBarGUI()
        {
            EditorGUILayout.BeginVertical();
            ExtendedEditorGUILayout.SymbolFieldLeft("Activate", "GOOGLE_ADMOB");
            ExtendedEditorGUILayout.SymbolFieldLeft("Verbose Logging", "LOGGING_ADMOB");
            EditorGUILayout.EndVertical();
        }

        public override void OnGUI(string searchContext)
        {
#if GOOGLE_ADMOB
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

            _foldoutInterstitialWithAward = EditorGUILayout.BeginFoldoutHeaderGroup(_foldoutInterstitialWithAward, "Interstitial Ads with Awards");
            if (_foldoutInterstitialWithAward)
            {
                _adsInterstitialWithAwardBlockList.DoLayoutList();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.EndVertical();

            _serializedObject.ApplyModifiedProperties();
#endif
        }
    }
}