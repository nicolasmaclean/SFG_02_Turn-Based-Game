using UnityEngine;

namespace Game.Level
{
    [System.Serializable, CreateAssetMenu(menuName = "Level/Tile")]
    public class TileSO : ScriptableObject
    {
        public string DisplayName;
        public GameObject Prefab;
        [TextArea(3, 8)] public string Description;
    }

    public enum TileType
    {
        Flat = 0, Building = 1, Block = 2,
    }
}