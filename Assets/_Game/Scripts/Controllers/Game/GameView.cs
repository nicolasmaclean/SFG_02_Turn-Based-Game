using System;
using Game.Level;
using Gummi.MVC;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Controllers.Game
{
    public class GameView : UIView
    {
        [Header("Player Turn")]
        [SerializeField]
        Button _endTurn;
        
        [Header("Selection")]
        [SerializeField]
        TMP_Text _txt_selection;
        
        [SerializeField]
        Transform _selectionCursor;

        void Start()
        {
            HideSelectionCursor();
            HidePlayerTurn();
        }

        public void Deselect()
        {
            HideSelectionCursor();
        }
        
        public void Select(Tile tile)
        {
            // show the cursor, if not already
            if (!_selectionCursor.gameObject.activeInHierarchy)
            {
                _selectionCursor.gameObject.SetActive(true);
            }

            _selectionCursor.position = tile.transform.position;
            _txt_selection.text = tile.DisplayName;
        }
        
        void HideSelectionCursor()
        {
            _selectionCursor.gameObject.SetActive(false);
            _txt_selection.text = string.Empty;
        }

        public void ShowPlayerTurn()
        {
            _endTurn.gameObject.SetActive(true);
        }

        public void HidePlayerTurn()
        {
            _endTurn.gameObject.SetActive(false);
        }
    }
}