using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Play.Skills;
using Gummi;
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

        [SerializeField]
        SkillSO[] _skills;

        [SerializeField, Readonly]
        int _health;

        [Readonly]
        public Vector2Int Position;
        
        [Space]
        
        [Header("Meta Data")]
        [SerializeField]
        public string Name = "Pawn Name...";

        public Team Team;

        void Start()
        {
            _health = InitialHealth;
        }

        public void Hurt(int amount)
        {
            _health -= amount;
        }

        public void Heal(int amount)
        {
            _health += amount;
        }

        public SkillSO GetSkill(SkillTag skillTag)
        {
            List<SkillSO> skills = _skills.ToList();
            return skills.FindAll(skill => skill.Tags.HasFlag(skillTag)).PickRandom();
        }
    }
}
