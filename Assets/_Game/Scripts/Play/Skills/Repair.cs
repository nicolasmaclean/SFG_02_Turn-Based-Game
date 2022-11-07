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
        
        public override void Activate(Pawn user, Board board, int row, int column)
        {
            user.Heal(_healAmount);
        }
    }
}