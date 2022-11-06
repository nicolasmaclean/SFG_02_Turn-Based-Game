using System.Collections.Generic;
using System.Linq;
using Game.Level;
using Game.Play.Units;
using Game.Utility;
using UnityEngine;

namespace Game.Play.Weapons
{
    public abstract class SkillSO : ScriptableObject
    {
        public Sprite Icon => _icon;
        
        [SerializeField]
        Sprite _icon;

        [Header("Stats")]
        [SerializeField]
        protected int _range;

        public abstract void Activate(Pawn user, Board board, int row, int column);

        public List<Tile> GetTilesInRange(Board board, Vector2Int position) => board.GetTilesInRange(position, _range);
    }
}