using System.Collections.Generic;
using Game.Level;
using Game.Play.Units;
using Game.Utility;
using UnityEngine;

namespace Game.Play.Weapons
{
    public class Punch : SkillSO
    {
        [SerializeField]
        int damage = 1;
        
        public override void Activate(Pawn user, Board board, int row, int column)
        {
            throw new System.NotImplementedException();
        }
    }
}