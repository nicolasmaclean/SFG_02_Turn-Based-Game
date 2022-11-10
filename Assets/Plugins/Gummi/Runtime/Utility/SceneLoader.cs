// https://www.patrykgalach.com/2021/02/15/smooth-scene-loading/
// https://bitbucket.org/gaello/smooth-scene-loading/src/master/Assets/Scripts/SmoothLoader.cs

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Gummi.Utility
{
    /// <summary>
    /// Async scene loading with default loading scene animations
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        AsyncOperation _loadOperation;

        [SerializeField]
        private Image _progressBar;

        float _currentValue;
        float _targetValue;

        [SerializeField]
        [Range(0, 1)]
        float _animationMultiplier = 0.25f;

        IEnumerator Start()
        {
            // Set 0 for progress values.
            _progressBar.fillAmount = _currentValue = _targetValue = 0;

            // load next scene
            var currentScene = SceneManager.GetActiveScene();
            _loadOperation = SceneManager.LoadSceneAsync(currentScene.buildIndex + 1);

            // let the animation finish before the scene loads
            _loadOperation.allowSceneActivation = false;


            while (true)
            {
                _targetValue = _loadOperation.progress / 0.9f;

                // Calculate progress value to display.
                _currentValue = Mathf.MoveTowards(_currentValue, _targetValue, _animationMultiplier * Time.deltaTime);
                _progressBar.value = _currentValue;

                // When the progress reaches 1, allow the process to finish by setting the scene activation flag.
                if (Mathf.Approximately(_currentValue, 1))
                {
                    _loadOperation.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }
}