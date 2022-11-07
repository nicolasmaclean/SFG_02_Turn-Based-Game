using UnityEngine;
using Game.Play.Units;
using UnityEngine.InputSystem.Interactions;

namespace Game.Level
{
    public class Space
    {
        public Tile Tile { get; private set; }
        public Pawn Pawn { get; private set; }
        
        public Vector2Int Position { get; }
        
        public Space(Tile tile, Vector2Int position)
        {
            Tile = tile;
            Position = position;
        }

        public void AddPawn(Pawn pawn, Vector3 offset = default)
        {
            Pawn = pawn;
            pawn.Position = Position;
            Transform trans = pawn.transform;
            
            trans.SetParent(Tile.transform);
            trans.localPosition = offset;
        }

        public void RemovePawn()
        {
            if (!Pawn)
            {
                Debug.LogError("There was no pawn to remove.");
                return;
            }

            Pawn = null;
        }

        public void DestroyPawn()
        {
            Object.DestroyImmediate(Pawn.gameObject);
            Pawn = null;
        }

        public void DestroyTile()
        {
            Object.DestroyImmediate(Tile.gameObject);
            Tile = null;
        }
    }
}