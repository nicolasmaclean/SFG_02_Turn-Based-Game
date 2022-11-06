using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Animation/UI Image")]
    public class AnimationSO : ScriptableObject
    {
        public Sprite[] Frames;
    }
}
