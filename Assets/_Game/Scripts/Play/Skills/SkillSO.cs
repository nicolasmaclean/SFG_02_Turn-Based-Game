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

        public abstract void Activate(Pawn user, Board board, int row, int column);

        // public List<Tile> GetTilesInRange(Board board, Vector2Int position) => board.GetTilesInRange(position, _range);
    }
}