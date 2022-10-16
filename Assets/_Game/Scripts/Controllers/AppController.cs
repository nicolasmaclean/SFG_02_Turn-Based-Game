using System;
using System.Collections;
using System.Collections.Generic;
using Game.Controllers.Game;
using UnityEngine;
using Gummi.MVC;

namespace Game.Controllers
{
    public class AppController : RootController<GameState>
    {
        [SerializeField] StartController _start;
        [SerializeField] GameController _gameLoop;
        [SerializeField] WinController _win;
        [SerializeField] LoseController _lose;

        protected override SubController<GameState> GetController(GameState state)
        {
            return state switch
            {
                GameState.StartMenu => _start,
                GameState.Game => _gameLoop,
                GameState.Win => _win,
                GameState.Lose => _lose,
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null),
            };
        }
    }
    
    public enum GameState
    {
        StartMenu = 0, Game = 1, Win = 2, Lose = 3,
    }
}
