using UnityEngine;

namespace Game.Play
{
    [CreateAssetMenu(menuName = "Level/Pilot")]
    public class PilotSO : ScriptableObject
    {
        public Sprite Normal;
        public Sprite Hurt;
        public AnimationSO Blink;
    }
}