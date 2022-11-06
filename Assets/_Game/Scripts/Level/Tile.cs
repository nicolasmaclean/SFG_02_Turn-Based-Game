using System;
using System.Collections;
using System.Collections.Generic;
using Game.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Tile : MonoBehaviour
    {
        static readonly Vector2 SPACING = new(64, -42);
        const float ANIMAION_LENGTH     = 0.5f;
        const float VERTICAL_OFFSET     = 100f;
        const float COLUMN_DELAY        = 0.2f;

        public string DisplayName => _displayName;
        public string Description => _description;
        public bool IsNavigable => _isNavigable;
        
        [SerializeField]
        bool _isNavigable = true;
        
        [Header("Display Data")]
        [SerializeField]
        string _displayName = "Display Name...";
        
        [SerializeField]
        [TextArea(3, 8)]
        string _description = "Description...";
        
        [Header("Debug")]
        [SerializeField, ReadOnly]
        Vector2Int _position;

        CanvasGroup _group;
        
        void Start()
        {
            _group = GetComponent<CanvasGroup>();
            AnimateIn();
        }

        public void Configure(int row, int column)
        {
            name = $"Tile ({ row }, { column })";
            _position = new Vector2Int(row, column);
            
            // calculate positions
            int sum  = row + column;
            int diff = column - row;

            float vertical   = sum  * SPACING.y;
            float horizontal = diff * SPACING.x;
                    
            // set position
            transform.localPosition = new Vector3(horizontal, vertical, 0);
        }
        
        public void OnHoverEnter()
        {
            GetComponentInChildren<Image>().color = Color.red;
        }

        public void OnHoverExit()
        {
            GetComponentInChildren<Image>().color = Color.white;
        }

        void AnimateIn()
        {
            _group.alpha = 0;
            Vector3 initialPosition = transform.position;
            transform.position += Vector3.up * VERTICAL_OFFSET;
            
            StartCoroutine(Animation());

            IEnumerator Animation()
            {
                yield return new WaitForSeconds(COLUMN_DELAY * _position.y);
                
                // concurrently fade in
                StartCoroutine(Coroutines.AlphaLerp(_group, 1, ANIMAION_LENGTH / 2));
                yield return StartCoroutine(Coroutines.PositionLerp(transform, initialPosition, ANIMAION_LENGTH));
            }
        }
    }
}
