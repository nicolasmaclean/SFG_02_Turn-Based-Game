using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "UI/Sprite Animation")]
    public class AnimationSO : ScriptableObject
    {
        public Sprite[] Frames;
    }
}
