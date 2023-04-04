using UnityAdvertisementEx.Runtime.ads_ex.Scripts.Runtime.Assets;
using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor.Utils.Extensions;
using UnityEngine;

namespace UnityAdvertisementEx.Editor.ads_ex.Scripts.Editor.Provider.Blocks
{
    public sealed class AdsBannerBlockList : AdsBlockList
    {
        public AdsBannerBlockList(SerializedObject serializedObject, SerializedProperty elements) : base(serializedObject, elements)
        {
            Columns.Add(new FixedColumn { HeaderText = "Position", AbsoluteWidth = 100f, ElementCallback = OnPosition });
            Columns.Add(new FixedColumn { Header = new GUIContent(EditorGUIUtility.IconContent("Loading").image, "Show on load"), AbsoluteWidth = 20f, MaxHeight = 20f, ElementCallback = OnShowOnLoad });
            Columns.Add(new FixedColumn { HeaderText = "Size", AbsoluteWidth = 100f, ElementCallback = OnSize });
            Columns.Add(new FixedColumn { HeaderText = "Width", AbsoluteWidth = 50f, MaxHeight = 20f, ElementCallback = OnWidth });
            Columns.Add(new FixedColumn { HeaderText = "Height", AbsoluteWidth = 50f, MaxHeight = 20f, ElementCallback = OnHeight });
        }

        private void OnPosition(Rect rect, int i, bool isactive, bool isfocused)
        {
            var prop = serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("position");
            EditorGUI.PropertyField(rect, prop, GUIContent.none);
        }

        private void OnShowOnLoad(Rect rect, int i, bool isactive, bool isfocused)
        {
            var prop = serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("showImmediately");
            EditorGUI.PropertyField(rect, prop, GUIContent.none);
        }

        private void OnSize(Rect rect, int i, bool isactive, bool isfocused)
        {
            var prop = serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("size");
            EditorGUI.PropertyField(rect, prop, GUIContent.none);
        }

        private void OnWidth(Rect rect, int i, bool isactive, bool isfocused)
        {
            var sizeProp = serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("size");
            var prop = serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("width");
            EditorGUI.BeginDisabledGroup(sizeProp.GetEnum<AdBannerSize>() is not (AdBannerSize.AnchorLandscape or AdBannerSize.AnchorPortrait or AdBannerSize.AnchorOrientationDepend or AdBannerSize.Custom));
            {
                EditorGUI.PropertyField(rect, prop, GUIContent.none);
            }
            EditorGUI.EndDisabledGroup();
        }

        private void OnHeight(Rect rect, int i, bool isactive, bool isfocused)
        {
            var sizeProp = serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("size");
            var prop = serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("height");
            EditorGUI.BeginDisabledGroup(sizeProp.GetEnum<AdBannerSize>() is not AdBannerSize.Custom);
            {
                EditorGUI.PropertyField(rect, prop, GUIContent.none);
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}