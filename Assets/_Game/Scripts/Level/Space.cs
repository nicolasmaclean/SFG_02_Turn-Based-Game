using UnityEngine;
using Game.Play.Units;

namespace Game.Level
{
    public class Space
    {
        public Tile Tile { get; private set; }
        public Pawn Pawn { get; private set; }
        
        public Space(Tile tile)
        {
            Tile = tile;
        }

        public void AddPawn(Pawn pawn)
        {
            Pawn = pawn;
            Transform trans = pawn.transform;
            
            trans.SetParent(Tile.transform);
            trans.localPosition = Vector3.zero;
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