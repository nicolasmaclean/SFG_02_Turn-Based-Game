using System.Collections.Generic;
using System.Reflection;
using Gummi;
using UnityEditor;
using UnityEngine;

namespace GummiEditor.Attributes.Buttons
{
    public abstract class ButtonBase
    {
        protected readonly MethodInfo Method;
        protected readonly string DisplayName;
        
        readonly bool _disabled;
        readonly int _spacing;
        readonly bool _shouldColor;
        readonly Color _color;

        internal static ButtonBase Create(MethodInfo method, ButtonAttribute buttonAttribute)
        {
            return new ButtonWithoutParameters(method, buttonAttribute);
        }

        protected ButtonBase(MethodInfo method, ButtonAttribute buttonAttribute)
        {
            Method = method;
            DisplayName = string.IsNullOrEmpty(buttonAttribute.Label) ? ObjectNames.NicifyVariableName(method.Name) : buttonAttribute.Label;

            _spacing = buttonAttribute.Spacing;
            _disabled = buttonAttribute.Mode switch
            {
                ButtonMode.Always => false,
                ButtonMode.InPlayMode => !EditorApplication.isPlaying,
                ButtonMode.NotInPlayMode => EditorApplication.isPlaying,
                _ => true
            };
        }

        public void Draw(IEnumerable<object> targets)
        {
            if (_spacing > 0) GUILayout.Space(_spacing);
            
            EditorGUI.BeginDisabledGroup(_disabled);
            
            var backgroundColor = GUI.backgroundColor;
            if (_shouldColor) GUI.backgroundColor = _color;
            
            DrawInternal(targets);
            
            GUI.backgroundColor = backgroundColor;
            
            EditorGUI.EndDisabledGroup();
        }

        protected abstract void DrawInternal(IEnumerable<object> targets);
    }
}