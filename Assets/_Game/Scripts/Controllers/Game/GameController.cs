using System.Collections.Generic;
using Game.Level;
using Game.Play.Skills;
using Game.Play.Units;
using Gummi.MVC;
using Gummi;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Space = Game.Level.Space;

#if UNITY_EDITOR
using UnityEditor;
#endif

// ReSharper disable Unity.IncorrectMethodSignature

namespace Game.Controllers.Game
{
    public enum SubGameState
    {
        Deployment = 0, EnemyMove = 1, Player = 2, EnemyAttack = 3,
    }
    
    public class GameController : SubController<GameState, GameView>
    {
        #region Variables
        [SerializeField]
        PlayerInput _input;
        InputAction _mouse;

        [Header("Game State")]
        [SerializeField, Readonly(AppMode.Editor)]
        SubGameState _state = SubGameState.Deployment;

        [Header("Deployment")]
        [SerializeField]
        Vector3 _deployOffset = Vector3.up * 50;

        [SerializeField]
        PawnType[] _playerPawns;

        [SerializeField, Readonly]
        List<Pawn> _pawnsToDeploy;

        [Header("Player Turn")]
        [SerializeField, Readonly(AppMode.Editor)]
        int _turnsLeft = 5;
        
        [Header("Debug")]
        [SerializeField, Readonly]
        Tile _hovering;
        
        [SerializeField, Readonly]
        Pawn _selected;

        [SerializeField, ReadOnly]
        SkillSO _selectedSkill;

        static Board _board => Board.Instance;
        Dictionary<Pawn, TurnData> _enemyTurns = new();
        #endregion

        void Start()
        {
            _mouse = _input.actions["Point"];
            EnterState();
            UI.UpdateTurn(_turnsLeft);
        }

        #region State Management
        public void GoToNextSubState()
        {
            // cycle to the next state
            var state = _state + 1;
            if (!System.Enum.IsDefined(typeof(SubGameState), state))
            {
                state = SubGameState.EnemyMove;
            }
            
            GoToSubState(state);
        }

        public void GoToSubState(SubGameState state)
        {
            if (_state == state) return;
            
            _state = state;
            EnterState();
        }

        void EnterState()
        {
            switch (_state)
            {
                case SubGameState.Deployment:
                    DeployEnter();
                    break;
                
                case SubGameState.EnemyMove:
                    MoveEnter();
                    break;
                
                case SubGameState.Player: 
                    PlayerEnter();
                    break;
                
                case SubGameState.EnemyAttack:
                    if (PlayerDone()) break;
                    AttackEnter(); break;
                
                default:
                    break;
            }
        }
        #endregion
        
        #region Deployment
        void DeployEnter()
        {
            _board.Highlight(_board.GetDropZone());
            
            // create/hide pawns
            _pawnsToDeploy = new List<Pawn>();
            foreach (PawnType type in _playerPawns)
            {
                Pawn p = PawnPalette.Get(type);
                _pawnsToDeploy.Add(p);
                
                p.gameObject.SetActive(false);
                p.Team = Team.Player;
            }
        }

        void DeployHover(Tile target)
        {
            // TODO: update tile/unit selection
            Space space = target.Space;
            
            // TODO: what do we do after pawns have been deployed??
            if (_pawnsToDeploy.Count == 0) return;
            
            // exit, we want to deploy unit in the drop zone only
            Pawn pawn = _pawnsToDeploy[0];
            if (!target.IsDropZone || (space.Pawn && space.Position != pawn.Position) )
            {
                return;
            }

            // show pawn, if hidden
            if (!pawn.gameObject.activeInHierarchy)
            {
                pawn.gameObject.SetActive(true);
            }
            else
            {
                _board.Spaces[pawn.Position.x, pawn.Position.y].RemovePawn();
            }
            
            // move pawn
            target.Space.AddPawn(pawn, _deployOffset);
        }

        void DeployClick(Tile target)
        {
            // exit, we can't deploy to this tile
            Space space = target.Space;
            Pawn pawn = _pawnsToDeploy[0];
            if (!target.IsDropZone || space.Position != pawn.Position) return;
            
            pawn.transform.localPosition = Vector3.zero;
            _pawnsToDeploy.RemoveAt(0);

            if (_pawnsToDeploy.Count == 0)
            {
                // TODO: show confirm button
                GoToNextSubState();
            }
        }
        #endregion
        
        #region Enemy Move
        void MoveEnter()
        {
            _enemyTurns.Clear();
            foreach (Pawn enemy in _board.Pawns(Team.Enemy))
            {
                // calculate next turn
                TurnData turn = EnemyAI.PlanTurn(_board, enemy);
                _enemyTurns.Add(enemy, turn);
                
                // move enemy
                Vector2Int move = enemy.Position;
                _board.Spaces[move.x, move.y].RemovePawn();
                
                move = turn.Move;
                _board.Spaces[move.x, move.y].AddPawn(enemy);
                
                // TODO: animate the movement
                // show the enemy in the pawn selection
            }
            
            GoToNextSubState();
        }
        #endregion
        
        #region Player
        void PlayerEnter()
        {
            UI.ShowPlayerTurn();
        }
        
        void PlayerHover(Tile tile)
        {
            // exit, hover has not changed
            if (_hovering == tile) return;

            // stop hovering over old tile
            if (_hovering)
            {
                _hovering.OnHoverExit();
            }

            // hover over new tile
            _hovering = tile;
            _hovering.OnHoverEnter();
        }

        void PlayerClick(Tile tile)
        {
            // select new tile
            UI.Select(tile);
            #if UNITY_EDITOR
            Selection.objects = new Object[] { tile.gameObject };
            #endif

            // // take turn for the player's pawn
            // if (_selected && _selected.Team == Team.Player)
            // {
            //     // try to move pawn
            //     if (!_selectedSkill)
            //     {
            //         // tile is occupied
            //         if (!tile.CanBeOccupied)
            //         {
            //             print("tile is occupied");
            //             PlayerDeselectPawn();
            //             return;
            //         }
            //         
            //         // move the pawn 
            //         float distance = _board.GetDistance(_selected.Position, tile.Space.Position);
            //         if (distance <= _selected.Movement)
            //         {
            //             print("move pawn");
            //             return;
            //         }
            //
            //         // move is out of range
            //         PlayerDeselectPawn();
            //         print("move is out of range");
            //         return;
            //     }
            //     
            //     // use skill
            //     float dist = _board.GetDistance(_selected.Position, tile.Space.Position);
            //     if (dist <= _selectedSkill.Range)
            //     {
            //         print("use skill");
            //         return;
            //     }
            //     
            //     // skill is out of range
            //     print("skill is out of range");
            //     PlayerDeselectSkill();
            //     return;
            // }
            //
            // PlayerSelectPawn(tile.Space.Pawn);
        }

        void PlayerDeselectPawn()
        {
            _selected = null;
        }
        
        void PlayerSelectPawn(Pawn pawn)
        {
            _selected = pawn;
        }

        void PlayerDeselectSkill()
        {
            _selectedSkill = null;
        }
        
        void PlayerSelectSkill(SkillSO skill)
        {
            _selectedSkill = skill;
        }

        bool PlayerDone()
        {
            _turnsLeft--;
            UI.UpdateTurn(_turnsLeft);
            if (_turnsLeft > 0)
            {
                return false;
            }

            root.ChangeController(GameState.Win);
            return true;
        }
        #endregion
        
        #region Enemy Attack
        void AttackEnter()
        {
            foreach (var kvp in _enemyTurns)
            {
                Pawn em = kvp.Key;
                TurnData turn = kvp.Value;

                turn.Skill.Activate(em, _board, turn.Target);
                
                // TODO: animate the attacks
                // show the enemy (and skill) active
            }
            
            GoToNextSubState();
        }
        #endregion

        #region Input
        public void OnMouseMove(InputAction.CallbackContext context)
        {
            if (_state is SubGameState.EnemyMove or SubGameState.EnemyAttack) return;
            
            Vector2 mouse = GetMousePosition();
            Tile tile = GetTileFromMouse(mouse);

            if (!tile) return;

            switch (_state)
            {
                case SubGameState.Deployment: DeployHover(tile); return;
                case SubGameState.Player: PlayerHover(tile); return;
                
                case SubGameState.EnemyMove:
                case SubGameState.EnemyAttack:
                default: return;
            }
        }

        public void OnMouseDown(InputAction.CallbackContext context)
        {
            if (_state is SubGameState.EnemyMove or SubGameState.EnemyAttack) return;
            
            // exit, was not mouse down
            if (!context.performed) return;
            
            Vector2 mouse = GetMousePosition();
            Tile tile = GetTileFromMouse(mouse);

            // deselect tile
            if (!tile)
            {
                UI.Deselect();
                return;
            }
            
            switch (_state)
            {
                case SubGameState.Deployment: DeployClick(tile); return;
                case SubGameState.Player: PlayerClick(tile); return;

                case SubGameState.EnemyMove:
                case SubGameState.EnemyAttack:
                default: return;
            }
        }

        Vector2 GetMousePosition()
        {
            Vector2 mouse = _mouse.ReadValue<Vector2>();
            mouse.x = Mathf.Clamp(mouse.x, 0, Screen.width);
            mouse.y = Mathf.Clamp(mouse.y, 0, Screen.height);

            return mouse;
        }
        
        static Tile GetTileFromMouse(Vector2 mouse)
        {
            // exit, we are not hovering over anything
            RaycastHit2D hitinfo = Physics2D.Raycast(mouse, Vector3.forward, 100f);
            if (hitinfo.collider == null) return null;
            
            // exit, we are not looking at a tile
            Tile tile = hitinfo.rigidbody.gameObject.GetComponent<Tile>();
            return tile;
        }
        #endregion
    }
}