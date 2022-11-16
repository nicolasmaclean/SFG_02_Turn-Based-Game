using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Level
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
        public bool IsDropZone;
        public TileType Type => _type;
        public bool CanBeOccupied => IsNavigable && !Space.Pawn;
        
        [SerializeField]
        bool _isNavigable = true;
        
        [Header("Display Data")]
        [SerializeField]
        string _displayName = "Display Name...";

        [SerializeField]
        TileType _type = TileType.Grass;
        
        [SerializeField]
        [TextArea(3, 8)]
        string _description = "Description...";

        public Space Space { get; private set; }
        Vector2Int _position => Space.Position;

        public Board Board { get; private set; }
        public CanvasGroup _group;
        Image[] _imgs;
        
        void Start()
        {
            _group = GetComponent<CanvasGroup>();
            _imgs = GetComponentsInChildren<Image>();
            AnimateIn();
        }

        public void Configure(Board board, Space space)
        {
            Board = board;
            Space = space;
            int row = _position.x;
            int column = _position.y;
            
            name = $"Tile ({ row }, { column })";
            IsDropZone &= 0 < column && column < 4 && 0 < row && row < 7;
            
            // calculate positions
            int sum  = row + column;
            int diff = column - row;

            float vertical   = sum  * SPACING.y;
            float horizontal = diff * SPACING.x;
                    
            // set position
            transform.localPosition = new Vector3(horizontal, vertical, 0);
        }

        public void Damage(int amount) { }

        public void Highlight() => SetColor(Color.yellow);
        public void Unhighlight() => SetColor(Color.white);

        public void OnHoverEnter() => SetColor(Color.red);
        public void OnHoverExit() => SetColor(Color.white);

        void SetColor(Color color)
        {
            foreach (Image img in _imgs)
            {
                img.color = color;
            }
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
