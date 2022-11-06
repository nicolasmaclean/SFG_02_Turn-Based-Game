using System.Collections.Generic;
using Game.Level;
using Game.Play.Units;
using Game.Utility;
using UnityEngine;

namespace Game.Play.Weapons
{
    public class Punch : WeaponSO
    {
        [SerializeField]
        int damage = 1;
        
        public override void Activate(Unit user, Map map, int row, int column)
        {
            throw new System.NotImplementedException();
        }
    }
}