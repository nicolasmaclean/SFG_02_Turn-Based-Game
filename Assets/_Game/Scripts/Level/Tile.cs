using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Tile : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler
    {
        static readonly Vector2 SPACING = new(80, -40);
        [SerializeField, ReadOnly]
        Vector2Int _position;

        Map _map;

        void Start()
        {
            _map = transform.parent.GetComponent<Map>();
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
        
        public void OnPointerEnter()//(PointerEventData eventData)
        {
            // eventData.Use();
            // _map.UpdateHover(this);
            GetComponentInChildren<Image>().color = Color.red;
        }

        public void OnPointerExit()//(PointerEventData eventData)
        {
            // eventData.Use();
            GetComponentInChildren<Image>().color = Color.white;
        }
    }
}
