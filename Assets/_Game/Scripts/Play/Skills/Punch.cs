using System.Collections.Generic;
using Game.Level;
using Game.Play.Units;
using Game.Utility;
using UnityEngine;

namespace Game.Play.Skills
{
    public class Punch : SkillSO
    {
        [SerializeField]
        int _damage = 1;
        
        public override void Activate(Pawn user, Board board, int row, int column)
        {
            Pawn target = board.Spaces[row, column].Pawn;
            if (target)
            {
                target.Hurt(_damage);
            }
        }
    }
}