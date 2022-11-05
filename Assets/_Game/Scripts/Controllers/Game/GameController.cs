using Gummi.MVC;
using UnityEngine;
using UnityEngine.InputSystem;
// ReSharper disable Unity.IncorrectMethodSignature

namespace Game.Controllers.Game
{
    public class GameController : SubController<GameState, GameView>
    {
        [SerializeField]
        PlayerInput _input;
        InputAction _mouse;
        
        [Header("Game State")]
        [SerializeField, ReadOnly]
        Tile _hovering;
        
        [SerializeField, ReadOnly]
        Tile _selected;

        void Start()
        {
            _mouse = _input.actions["Point"];
        }
        
        public void OnMouseMove(InputAction.CallbackContext context)
        {
            Vector2 mouse = GetMousePosition();
            Tile tile = GetTileFromMouse(mouse);
            
            // exit, we are not looking at a tile
            if (!tile) return;

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

        public void OnMouseDown(InputAction.CallbackContext context)
        {
            // exit, was not mouse down
            if (!context.performed) return;
            
            Vector2 mouse = GetMousePosition();
            Tile tile = _selected = GetTileFromMouse(mouse);
            
            // deselect tile
            if (!tile)
            {
                UI.Deselect();
                return;
            }
            
            // select new tile
            UI.Select(tile);
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
    }
}