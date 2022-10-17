using UnityEditor;

namespace GummiEditor.Attributes.Buttons
{
    using Object = UnityEngine.Object;

    [CustomEditor(typeof(Object), true)]
    [CanEditMultipleObjects]
    internal class ButtonObjectEditor : UnityEditor.Editor
    {
        ButtonsDrawer _buttonsDrawer;

        void OnEnable()
        {
            _buttonsDrawer = new ButtonsDrawer(target);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            _buttonsDrawer.DrawButtons(targets);
        }
    }
}