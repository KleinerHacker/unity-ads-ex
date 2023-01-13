using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityAdvertisementEx.Editor.ads_ex.Scripts.Editor.Provider.Blocks
{
    public abstract class AdsBlockList : ReorderableList
    {
        protected HeaderCallbackDelegate drawHeaderCustomColumnsCallback;
        protected ElementCallbackDelegate drawElementCustomColumnsCallback;
        
        protected AdsBlockList(SerializedObject serializedObject, SerializedProperty elements) : base(serializedObject, elements)
        {
            drawHeaderCallback += DrawHeaderCallback;
            drawElementCallback += DrawElementCallback;
        }

        private void DrawHeaderCallback(Rect rect)
        {
            GUI.Label(new Rect(rect.x + 15f, rect.y, 200f, 20f), "Identifier", EditorStyles.boldLabel);
            drawHeaderCustomColumnsCallback?.Invoke(new Rect(rect.x + 205f + 15f, rect.y, CustomColumnsSize, rect.height));
            
            var preColumnsSize = 205f + CustomColumnsSize;
            var idsWidth = (rect.width - preColumnsSize) / 2f;
            GUI.Label(new Rect(rect.x + preColumnsSize + 15f, rect.y, idsWidth - 1f, 20f), "Android ID", EditorStyles.boldLabel);
            GUI.Label(new Rect(rect.x + preColumnsSize + idsWidth + 4f + 8f, rect.y, idsWidth - 1f, 20f), "IOS ID", EditorStyles.boldLabel);
        }

        private void DrawElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var itemProperty = serializedProperty.GetArrayElementAtIndex(i);
            var identifierProperty = itemProperty.FindPropertyRelative("identifier");
            var androidIdProperty = itemProperty.FindPropertyRelative("androidId");
            var iosIdProperty = itemProperty.FindPropertyRelative("iosId");

            EditorGUI.PropertyField(new Rect(rect.x, rect.y + 2f, 200f, 20f), identifierProperty, new GUIContent());
            drawElementCustomColumnsCallback?.Invoke(new Rect(rect.x + 205f, rect.y, CustomColumnsSize, rect.height), i, isactive, isfocused);
            
            var preColumnsSize = 205f + CustomColumnsSize;
            var idsWidth = (rect.width - preColumnsSize) / 2f;
            EditorGUI.PropertyField(new Rect(rect.x + preColumnsSize, rect.y + 2f, idsWidth - 1f, 20f), androidIdProperty, new GUIContent());
            EditorGUI.PropertyField(new Rect(rect.x + preColumnsSize + idsWidth + 4f, rect.y + 2f, idsWidth - 1f, 20f), iosIdProperty, new GUIContent());
        }

        protected float CustomColumnsSize { get; set; } = 0f;
    }
}