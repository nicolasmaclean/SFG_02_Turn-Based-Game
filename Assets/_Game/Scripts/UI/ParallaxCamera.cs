using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Game
{
    [RequireComponent(typeof(PlayerInput))]
    public class ParallaxCamera : MonoBehaviour
    {
        [SerializeField] AnimationCurve _horizontal = AnimationCurve.Linear(0, 0, 1, 100);
        [SerializeField] AnimationCurve _vertical = AnimationCurve.Linear(0, 0, 1, 50);
        
        PlayerInput _input;
        InputAction _parallax;

        void Awake()
        {
            _input = GetComponent<PlayerInput>();
            _parallax = _input.actions["Parallax"];
        }
        
        void Update()
        {
            Vector2 nPos = GetInput();
            
            // normalize screen space
            Vector2 screen = new Vector2(Screen.width, Screen.height);
            nPos.x = Mathf.Clamp(nPos.x, 0, screen.x);
            nPos.y = Mathf.Clamp(nPos.y, 0, screen.y);
            
            nPos -= screen / 2;
            nPos /= screen;
            
            // multiply
            nPos.x = Mathf.Sign(nPos.x) *  _horizontal.Evaluate(Mathf.Abs(nPos.x));
            nPos.y = Mathf.Sign(nPos.y) *  _vertical.Evaluate(Mathf.Abs(nPos.y));
            
            Move(new Vector3(nPos.x, nPos.y, transform.position.z));
        }

        Vector2 GetInput()
        {
            return _parallax.ReadValue<Vector2>();
        }

        void Move(Vector3 position)
        {
            transform.position = position;
        }
    }
}
