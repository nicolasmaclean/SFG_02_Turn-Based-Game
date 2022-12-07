using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Arrows : MonoBehaviour
    {
        [SerializeField]
        GameObject[] _arrows;
        
        public void Show(int direction)
        {
            for (int i = 0; i < _arrows.Length; i++)
            {
                _arrows[i].SetActive(direction == i);
            }
        }
    }
}
