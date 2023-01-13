using UnityEditor;
using UnityEngine;

namespace UnityAdvertisementEx.Editor.ads_ex.Scripts.Editor.Provider.Blocks
{
    public sealed class AdsBannerBlockList : AdsBlockList
    {
        public AdsBannerBlockList(SerializedObject serializedObject, SerializedProperty elements) : base(serializedObject, elements)
        {
            CustomColumnsSize = 155f + 55f;
            
            drawHeaderCustomColumnsCallback += DrawHeaderCustomColumnsCallback;
            drawElementCustomColumnsCallback += DrawElementCustomColumnsCallback;
        }

        private void DrawHeaderCustomColumnsCallback(Rect rect)
        {
            GUI.Label(new Rect(rect.x, rect.y, 150f, 20f), "Position", EditorStyles.boldLabel);
            GUI.Label(new Rect(rect.x + 155f, rect.y, 50f, 20f), new GUIContent("Show", "Show if scene was loaded"), EditorStyles.boldLabel);
        }

        private void DrawElementCustomColumnsCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var itemProperty = serializedProperty.GetArrayElementAtIndex(i);
            var positionProperty = itemProperty.FindPropertyRelative("position");
            var immediatelyProperty = itemProperty.FindPropertyRelative("showImmediately");

            EditorGUI.PropertyField(new Rect(rect.x, rect.y + 3f, 150f, 20f), positionProperty, GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + 155f, rect.y + 2f, 50f, 20f), immediatelyProperty, GUIContent.none);
        }
    }
}