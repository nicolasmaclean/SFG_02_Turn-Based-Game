using System;
using Game.Level;
using Game.Play.Skills;
using Game.Play.Units;
using Gummi.MVC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Controllers.Game
{
    public class GameView : UIView
    {
        [Header("Player Turn")]
        [SerializeField]
        Button _endTurn;

        [SerializeField]
        GameObject _playerInterface;
        
        [SerializeField]
        GameObject _btn_attack;

        [SerializeField]
        GameObject _btn_attack_cancel;

        [SerializeField]
        GameObject _btn_attack_confirm;
        
        [SerializeField]
        TMP_Text _txt_turn;
        
        [Header("Selection")]
        [SerializeField]
        TMP_Text _txt_selection;
        
        [SerializeField]
        Transform _selectionCursor;

        void Start()
        {
            HideSelectionCursor();
            HidePlayerTurn();
            _playerInterface.SetActive(false);
        }

        public void UpdateTurn(int turn)
        {
            _txt_turn.text = turn.ToString();
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

        public void UpdatePawnView(Pawn pawn=null, SkillSO skill=null)
        {
            if (!pawn)
            {
                _btn_attack.gameObject.SetActive(false);
                _btn_attack_cancel.gameObject.SetActive(false);
                _btn_attack_confirm.gameObject.SetActive(false);
                return;
            } 
            
            _btn_attack.gameObject.SetActive(true);

            if (!skill) return;
            _btn_attack_cancel.gameObject.SetActive(true);
            _btn_attack_confirm.gameObject.SetActive(true);
        }
        
        public void ShowPlayerTurn()
        {
            _endTurn.gameObject.SetActive(true);
            _playerInterface.SetActive(true);

            _btn_attack.SetActive(false);
            _btn_attack_cancel.SetActive(false);
            _btn_attack_confirm.SetActive(false);
        }

        public void HidePlayerTurn()
        {
            _endTurn.gameObject.SetActive(false);
            _playerInterface.SetActive(false);
        }
    }
}