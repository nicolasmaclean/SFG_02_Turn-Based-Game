using System;
using System.Collections;
using System.Collections.Generic;
using Game.Play.Units;
using Gummi;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Healthbar : MonoBehaviour
    {
        [SerializeField, Readonly]
        Pawn _target;

        GameObject[] _bars;
        
        public void Configure(Pawn target)
        {
            _target = target;
            
            PopulateBar();
            UpdateBar();
            
            _target.OnHealthChange.AddListener(UpdateBar);
        }

        void PopulateBar()
        {
            _bars = new GameObject[_target.InitialHealth];
            for (int i = 0; i < _target.InitialHealth; i++)
            {
                GameObject bar = _bars[i] = new GameObject("Bar");
                bar.transform.SetParent(transform);
                bar.AddComponent<CanvasRenderer>();
                Image img = bar.AddComponent<Image>();

                img.color = new Color(0.3460306f, 0.9056604f, 0.4197159f, 1f);
            }
        }

        void UpdateBar()
        {
            // target is dead
            if (_target.Health <= 0) return;
            
            LayoutGroup layout = GetComponent<LayoutGroup>();
            if (layout) Destroy(layout);

            for (int i = 0; i < _target.InitialHealth; i++)
            {
                _bars[i].SetActive(i < _target.Health);
            }
            
        }
    }
}
