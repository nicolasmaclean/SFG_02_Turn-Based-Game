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
        [Header("Deployment")]
        [SerializeField]
        GameObject _deploymentTutorial;

        [Header("Enemy")]
        [SerializeField]
        Arrows _arrows;
        
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
            _arrows.gameObject.SetActive(false);
        }

        public void UpdateTurn(int turn)
        {
            _txt_turn.text = turn.ToString();
            Deselect();
        }

        public void Deselect()
        {
            HideSelectionCursor();
        }
        
        public void Select(Tile tile, bool isPlayerTurn)
        {
            // show the cursor, if not already
            if (!_selectionCursor.gameObject.activeInHierarchy)
            {
                _selectionCursor.gameObject.SetActive(true);
            }

            _selectionCursor.SetParent(tile.transform);
            _selectionCursor.localPosition = Vector3.zero;
            _txt_selection.text = tile.DisplayName;
        }

        public void ShowSkills(Pawn pawn)
        {
            if (pawn == null) throw new NullReferenceException();
            
            // display buttons
            _btn_attack.SetActive(true);
            _btn_attack_cancel.SetActive(false);
        }

        public void SkillSelected()
        {
            _btn_attack_cancel.SetActive(true);
        }

        public void SkillCancelled()
        {
            _btn_attack_cancel.SetActive(false);
        }

        public void Attacked()
        {
            _btn_attack.SetActive(false);
            _btn_attack_cancel.SetActive(false);
        }
        
        void HideSelectionCursor()
        {
            _selectionCursor.gameObject.SetActive(false);
            _txt_selection.text = string.Empty;
        }

        public void ShowPlayerTurn()
        {
            _endTurn.gameObject.SetActive(true);
            _playerInterface.SetActive(true);

            _btn_attack.SetActive(false);
            _btn_attack_cancel.SetActive(false);
        }

        public void HidePlayerTurn()
        {
            _endTurn.gameObject.SetActive(false);
            _playerInterface.SetActive(false);
        }

        public void UpdateEnemyPlan(TurnData turn)
        {
            if (turn.Skill == null)
            {
                _arrows.gameObject.SetActive(false);
                return;
            }
            
            if (!_arrows.gameObject.activeInHierarchy) _arrows.gameObject.SetActive(true);
            
            Vector2Int position = turn.Move;
            _arrows.transform.position = Board.Instance.Spaces[position.x, position.y].Tile.transform.position;

            int direction;
            if (turn.Target == new Vector2Int(0, -1)) direction = 3;
            else if (turn.Target == new Vector2Int(0, 1)) direction = 1;
            else if (turn.Target == new Vector2Int(-1, 0)) direction = 0;
            else /* if (turn.Target == new Vector2Int(1, 0)) */ direction = 2;
            
            _arrows.Show(direction);
        }

        public void HideEnemyPlan()
        {
            _arrows.gameObject.SetActive(false);
        }

        public void ShowDeployTutorial() => _deploymentTutorial.SetActive(true);
        public void HideDeployTutorial() => _deploymentTutorial.SetActive(false);
    }
}