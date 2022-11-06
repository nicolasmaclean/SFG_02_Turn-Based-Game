using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Play.Units
{
    public enum Team
    {
        Enemy = 0, Player = 1,
    }
    
    public class Pawn : MonoBehaviour
    {
        public int InitialHealth => _initHealth;
        public int Health => _health;
        public int Movement => _movement;
        
        [Header("Stats")]
        [SerializeField]
        int _initHealth = 1;

        [SerializeField]
        int _movement = 3;

        [SerializeField, ReadOnly]
        int _health;

        [ReadOnly]
        public Vector2Int Position;
        
        [Space]
        
        [Header("Meta Data")]
        [SerializeField]
        public string Name = "Pawn Name...";

        public Team team;

        public void Hurt(int amount)
        {
            _health -= amount;
        }

        public void Heal(int amount)
        {
            _health += amount;
        }
    }
}
