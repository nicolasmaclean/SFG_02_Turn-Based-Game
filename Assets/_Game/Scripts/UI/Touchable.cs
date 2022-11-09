using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.UI
{
    public class Touchable : Graphic
    {
        protected override void UpdateGeometry() { }
    }
    
    #if UNITY_EDITOR
    [CustomEditor(typeof(Touchable))]
    public class Touchable_Editor : Editor
    {
        public override void OnInspectorGUI() { }
    }
    #endif
}
