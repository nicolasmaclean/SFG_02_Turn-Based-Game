using System.Collections.Generic;
using System.Linq;
using Game.Level;
using Game.Play.Units;
using Game.Utility;
using UnityEngine;

namespace Game.Play.Skills
{
    [System.Flags]
    public enum SkillTag
    {
        Attack = 1, Healing = 2, Movement = 4, Tactical = 8,
    }

    public enum Effective
    {
        Kill = 0, DamageUnit = 1, DamageBuilding = 2, Move = 3, None = 4,
    }
    
    public abstract class SkillSO : ScriptableObject
    {
        
        public Sprite Icon => _icon;
        public SkillTag Tags => _tags;
        
        [SerializeField]
        Sprite _icon;

        [SerializeField]
        SkillTag _tags;

        [Header("Stats")]
        [SerializeField]
        protected int _range;

        public virtual void Activate(Pawn user, Board board, Vector2Int target)
        {
            Vector2Int delta = target - user.Position;
            float distance = delta.x + delta.y;

            if (distance > _range)
            {
                throw new System.Exception($"{name} was activated with a target that is out of range.");
            }
        }

        public Vector2Int Evaluate(Board board, Pawn user, Vector2Int position)
        {
            return Evaluate(board, user, position, out _);
        }
        
        public abstract Vector2Int Evaluate(Board board, Pawn user, Vector2Int pos, out Effective effectiveness);
    }
    
    public static class EffectiveExtensions
    {
        public static bool Compare(this Effective a, Effective b)
        {
            // pick random, if equal in priority
            if  (a == b ||
                 (a == Effective.DamageBuilding && b == Effective.DamageUnit) ||
                 (b == Effective.DamageBuilding && a == Effective.DamageUnit))
            {
                return Random.Range(0, 2) == 1;
            }

            return a < b;
        }
    }
}