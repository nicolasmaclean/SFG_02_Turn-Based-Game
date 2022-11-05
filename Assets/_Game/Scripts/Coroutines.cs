using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    public static class Coroutines
    {
        public static IEnumerator AlphaLerp(CanvasGroup group, float to, float duration, Action OnComplete=null)
        {
            float from = group.alpha;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                group.alpha = Mathf.Lerp(from, to, elapsedTime / duration);
                
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            group.alpha = to;
            OnComplete?.Invoke();
        }

        public static IEnumerator PositionLerp(Transform trans, Vector3 to, float duration, Action OnComplete=null)
        {
            Vector3 from = trans.position;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                trans.position = Vector3.Lerp(from, to, elapsedTime / duration);
                
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            trans.position = to;
            OnComplete?.Invoke();
        }
    }
}