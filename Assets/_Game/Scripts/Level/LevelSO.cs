using System.Collections.Generic;
using UnityEngine;

namespace Game.Level
{
    [CreateAssetMenu(menuName = "Level/Level")]
    public class LevelSO : ScriptableObject
    {
        [SerializeField]
        Row[] _tiles = new Row[8];
    }

    [System.Serializable]
    public class Row
    {
        public TileSO[] Data = new TileSO[8];
    }
}