using System.Collections.Generic;
using Game.Level;
using Game.Play.Units;
using UnityEngine;

namespace Game.Play.Skills
{
    [CreateAssetMenu(menuName = "Skills/Repair")]
    public class Repair : SkillSO
    {
        [SerializeField]
        int _healAmount;
        
        public override bool Activate(Pawn user, Board board, Vector2Int target)
        {
            if (!base.Activate(user, board, target)) return false;
            
            user.Heal(_healAmount);
            return true;
        }

        public override Vector2Int Evaluate(Board board, Pawn user, Vector2Int pos, out Effective effectiveness)
        {
            effectiveness = Effective.None;
            return user.Position;
        }
    }
}