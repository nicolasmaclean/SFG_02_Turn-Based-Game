using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Tile : MonoBehaviour
    {
        static readonly Vector2 SPACING = new(80, -40);

        [Header("Display Data")]
        [SerializeField]
        string _displayName = "Display Name...";
        
        [SerializeField]
        [TextArea(3, 8)]
        string _description = "Description...";
        
        [Header("Debug")]
        [SerializeField, ReadOnly]
        Vector2Int _position;

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
    }
}
