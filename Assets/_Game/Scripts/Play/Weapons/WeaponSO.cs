using System.Collections.Generic;
using System.Linq;
using Game.Level;
using Game.Play.Units;
using Game.Utility;
using UnityEngine;

namespace Game.Play.Weapons
{
    public abstract class WeaponSO : ScriptableObject
    {
        public Sprite Icon => _icon;
        
        [SerializeField]
        Sprite _icon;

        [Header("Stats")]
        [SerializeField]
        protected int _range;

        public abstract void Activate(Unit user, Map map, int row, int column);

        public List<Tile> GetTilesInRange(Map map, Vector2Int position) => map.GetTilesInRange(position, _range);
    }
}