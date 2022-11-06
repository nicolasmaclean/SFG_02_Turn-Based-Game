using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Play.Units
{
    public enum Owner
    {
        Enemy = 0, Player = 1,
    }
    
    public class Unit : MonoBehaviour
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
        string _unitName = "Unit Name...";

        public Owner Owner;

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
