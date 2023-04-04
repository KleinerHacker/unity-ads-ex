using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor;
using UnityEngine;

namespace UnityAdvertisementEx.Editor.ads_ex.Scripts.Editor.Provider.Blocks
{
    public abstract class AdsBlockList : TableReorderableList
    {
        protected AdsBlockList(SerializedObject serializedObject, SerializedProperty elements) : base(serializedObject, elements)
        {
            Columns.Add(new FlexibleColumn { HeaderText = "Identifier", MaxHeight = 20f, ElementCallback = OnIdentifier });
            Columns.Add(new FixedColumn { HeaderText = "Android ID", AbsoluteWidth = 150f, MaxHeight = 20f, ElementCallback = OnAndroidID });
            Columns.Add(new FixedColumn { HeaderText = "IOS ID", AbsoluteWidth = 150f, MaxHeight = 20f, ElementCallback = OnIOSID });
        }

        private void OnIdentifier(Rect rect, int i, bool isactive, bool isfocused)
        {
            var prop = serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("identifier");
            EditorGUI.PropertyField(rect, prop, GUIContent.none);
        }

        private void OnAndroidID(Rect rect, int i, bool isactive, bool isfocused)
        {
            var prop = serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("androidId");
            EditorGUI.PropertyField(rect, prop, GUIContent.none);
        }

        private void OnIOSID(Rect rect, int i, bool isactive, bool isfocused)
        {
            var prop = serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("iosId");
            EditorGUI.PropertyField(rect, prop, GUIContent.none);
        }
    }
}