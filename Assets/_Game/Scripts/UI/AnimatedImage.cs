using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [RequireComponent(typeof(Image))]
    public class AnimatedImage : MonoBehaviour
    {
        [SerializeField]
        AnimationSO _anim;
        
        [SerializeField]
        float _framesPerSecond = 2f;
        
        [SerializeField, ReadOnly]
        int _currentFrame;
        
        Image _img;

        float _secondsPerFrame => 1 / _framesPerSecond;
        
        IEnumerator Start()
        {
            if (!_anim)
            {
                Debug.LogError($"No animation data was provided to { name }");
                yield break;
            }
            
            _img = GetComponent<Image>();

            while (true)
            {
                _img.sprite = _anim.Frames[_currentFrame];
                _currentFrame = (_currentFrame + 1) % _anim.Frames.Length;
                
                yield return new WaitForSeconds(_secondsPerFrame);
            }
        }
    }
}
