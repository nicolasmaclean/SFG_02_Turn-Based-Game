// https://www.patrykgalach.com/2020/01/20/readonly-attribute-in-unity-editor/

using System.Reflection;
using Gummi;
using UnityEditor;
using UnityEngine;

namespace GummiEditor.Drawer
{
    [CustomPropertyDrawer(typeof(ReadonlyAttribute))]
    public class ReadonlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            AppMode mode = ((ReadonlyAttribute) attribute).Mode;
            
            bool isPlaying = EditorApplication.isPlaying;
            bool ignoreReadonly = (mode == AppMode.Editor && !isPlaying) || (mode == AppMode.Play && isPlaying);
            
            if (ignoreReadonly)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }
            
            // Saving previous GUI enabled value
            var previousGUIState = GUI.enabled;

            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label);
            
            // Setting old GUI enabled value
            GUI.enabled = previousGUIState;
        }
    }
}