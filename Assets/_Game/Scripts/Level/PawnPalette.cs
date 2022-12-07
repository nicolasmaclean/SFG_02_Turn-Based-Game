using Game.Play.Units;
using Game.Utility;
using UnityEngine;

namespace Game.Level
{
    public enum PawnType
    {
        Big = 0, Mass = 1,
    }
    
    public class PawnPalette : DynamicSingletonSO<PawnPalette>
    {
        [SerializeField]
        Pawn _bigMech;
        
        [SerializeField]
        Pawn _massMech;

        [SerializeField]
        Healthbar _healthbar;
        
        public static Pawn Get(PawnType type)
        {
            Pawn prefab;
            
            switch (type)
            {
                default:
                case PawnType.Big:
                    prefab = Instance._bigMech;
                    break;
                
                case PawnType.Mass:
                    prefab = Instance._massMech; 
                    break;
            }

            if (!prefab)
            {
                Debug.LogError($"Unable to get Tile instance for '{ type }'");
                return null;
            }
            
            Pawn pawn = Instantiate(prefab);
            Healthbar healthbar = Instantiate(Instance._healthbar, pawn.transform);
            healthbar.Configure(pawn);

            return pawn;
        }

        public static Pawn Get(PawnType type, Transform parent)
        {
            Pawn tile = Get(type);
            tile.transform.SetParent(parent);

            return tile;
        }
    }
}