using System.Collections.Generic;
using Game.Utility;
using UnityEngine;

namespace Game.Level
{
    public enum TileType
    {
        Grass = 0, Building = 1, Mountain = 2,
    }
    
    public class TilePalette : DynamicSingletonSO<TilePalette>
    {
        [SerializeField]
        Tile[] _grassVariants;
        
        [SerializeField]
        Tile[] _buildingVariants;
        
        [SerializeField]
        Tile[] _mountainVariants;

        public static Tile Get(TileType type)
        {
            Tile prefab;
            
            switch (type)
            {
                default:
                case TileType.Grass:
                    prefab = PickFromList(Instance._grassVariants);
                    break;
                
                case TileType.Building:
                    prefab = PickFromList(Instance._buildingVariants); 
                    break;
                
                case TileType.Mountain:
                    prefab = PickFromList(Instance._mountainVariants); 
                    break;
            }

            if (!prefab)
            {
                Debug.LogError($"Unable to get Tile instance for '{ type }'");
                return null;
            }

            return Instantiate(prefab);
        }

        public static Tile Get(TileType type, Transform parent)
        {
            Tile tile = Get(type);
            tile.transform.SetParent(parent);

            return tile;
        }

        static Tile PickFromList(IReadOnlyList<Tile> tiles)
        {
            // exit, no tiles were provided
            if (tiles == null || tiles.Count == 0) return null;
            
            int i = Random.Range(0, tiles.Count);
            return tiles[i];
        }
    }
}