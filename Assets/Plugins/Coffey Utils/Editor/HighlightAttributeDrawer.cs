#if UNITY_EDITOR
using UnityEditor;

namespace CoffeyEditor
{
    [CustomPropertyDrawer(typeof(HighlightAttribute))]
    public class HighlightAttributeDrawer : HighlightableAttributeDrawer
    {
        protected override bool ShouldHighlight(SerializedProperty property) => true;
    }
}
#endif