using System.Collections.Generic;
using Game.Level;
using Game.Play.Units;
using UnityEngine;

namespace Game.Play.Weapons
{
    [CreateAssetMenu(menuName = "Weapons/Repair")]
    public class Repair : WeaponSO
    {
        [SerializeField]
        int _healAmount;
        
        public override void Activate(Unit user, Map map, int row, int column)
        {
            user.Heal(_healAmount);
        }
    }
}