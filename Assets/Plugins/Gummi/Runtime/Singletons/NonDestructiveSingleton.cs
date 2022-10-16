using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gummi.Singletons
{
    public class NonDestructiveSingleton<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        public static T Instance => _instance;
        static T _instance;

        protected virtual void Awake()
        {
            // enforce single instance rule
            if (_instance != null)
            {
                Destroy(_instance.gameObject);
            }

            _instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            if (_instance != this) return;

            _instance = null;
        }
    }
}
