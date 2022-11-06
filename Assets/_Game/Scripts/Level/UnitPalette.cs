using Game.Play.Units;
using Game.Utility;
using UnityEngine;

namespace Game.Level
{
    public enum UnitType
    {
        Big = 0, Mass = 1,
    }
    
    [CreateAssetMenu(menuName="Level/Unit Palette")]
    public class UnitPalette : DynamicSingletonSO<UnitPalette>
    {
        [SerializeField]
        Unit _bigMech;
        
        [SerializeField]
        Unit _massMech;
        
        public static Unit Get(UnitType type)
        {
            Unit prefab;
            
            switch (type)
            {
                default:
                case UnitType.Big:
                    prefab = Instance._bigMech;
                    break;
                
                case UnitType.Mass:
                    prefab = Instance._massMech; 
                    break;
            }

            if (!prefab)
            {
                Debug.LogError($"Unable to get Tile instance for '{ type }'");
                return null;
            }

            return Instantiate(prefab);
        }

        public static Unit Get(UnitType type, Transform parent)
        {
            Unit tile = Get(type);
            tile.transform.SetParent(parent);

            return tile;
        }
    }
}