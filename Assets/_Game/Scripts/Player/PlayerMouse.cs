using UnityEngine;
using UnityEngine.InputSystem;
// ReSharper disable Unity.IncorrectMethodSignature

namespace Game.Player
{
    public class PlayerMouse : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        Tile _hovering;
        
        [SerializeField, ReadOnly]
        Tile _selected;

        public void OnMouseMove(InputAction.CallbackContext context)
        {
            // poll input
            Vector2 mouse = context.ReadValue<Vector2>();
            mouse.x = Mathf.Clamp(mouse.x, 0, Screen.width);
            mouse.y = Mathf.Clamp(mouse.y, 0, Screen.height);
            
            
            // exit, we are not hovering over anything
            RaycastHit2D hitinfo = Physics2D.Raycast(mouse, Vector3.forward, 100f);
            if (hitinfo.collider == null) return;
            
            // exit, we are not looking at a tile
            Tile tile = hitinfo.rigidbody.gameObject.GetComponent<Tile>();
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
            
            print("Mouse Down");
        }
    }
}