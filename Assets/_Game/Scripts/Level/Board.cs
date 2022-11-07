using System;
using System.Collections.Generic;
using System.Linq;
using Game.Play.Units;
using Game.Utility;
using Gummi;
using Gummi.Singletons;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Level
{
    public class Board : Singleton<Board>
    {
        public Space[,] Spaces { get; private set; }

        [Header("Layout")]
        [SerializeField]
        TextAsset _tileData;

        [SerializeField, FormerlySerializedAs("_units")]
        [NonReorderable]
        PlacedPawn[] _pawns;

        protected override void Awake()
        {
            PopulateTiles();
            PopulatePawns();
        }

        #region Generation
        [Button(Label = "Create Tiles", Mode = ButtonMode.NotInPlayMode)]
        void PopulateTiles()
        {
            TileType[,] tiles = MapData.ReadTileTypes(_tileData);
            
            Clear();
            
            // create tile instances
            Spaces = new Space[8, 8];
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    TileType type = tiles[r, c];
                    Tile tile = TilePalette.Get(type, transform);
                    tile.Configure(r, c);

                    Spaces[r, c] = new Space(tile);
                }
            }

            SortTiles();
        }

        void SortTiles()
        {
            // convert to 1D list
            List<Tile> _tilesLong = Spaces.ConvertTo1D().Select(s => s.Tile).ToList();
            
            // sort
            _tilesLong.Sort((a, b) =>
            {
                Vector3 aPos = a.transform.position;
                Vector3 bPos = b.transform.position;

                // bottom to top
                int compare = aPos.y.CompareTo(bPos.y);
                if (compare != 0)
                {
                    return -compare;
                }
                
                // left to right
                return aPos.x.CompareTo(bPos.x);
            });
            
            // update hierarchy
            for (int i = 0; i < _tilesLong.Count; i++)
            {
                Tile tile = _tilesLong[i];
                tile.transform.SetSiblingIndex(i);
            }
        }

        [Button(Label = "Create Pawns", Mode = ButtonMode.NotInPlayMode)]
        void PopulatePawns()
        {
            // lazy-generate tiles, units are dependent on the tiles
            if (Spaces == null)
            {
                PopulateTiles();
            }
            
            Clear(includeTiles: false);

            foreach (PlacedPawn data in _pawns)
            {
                Pawn pawn = PawnPalette.Get(data.type);
                pawn.team = data.team;
                Spaces[data.Position.x, data.Position.y].AddPawn(pawn);
            }
        }
        
        [Button(Label = "Clear", Mode = ButtonMode.NotInPlayMode)]
        void Clear(bool includeTiles = true, bool includePawns = true)
        {
            // clean up children, in case the Spaces pointer is broken
            if (Spaces == null)
            {
                transform.DestroyChildrenImmediate();
                return;
            }
            
            // short-circuit clear
            if (includeTiles && includePawns)
            {
                transform.DestroyChildrenImmediate();
                Spaces = null;
                return;
            }

            bool isEmpty = true;
            foreach (Space space in Spaces)
            {
                if (includeTiles && space.Tile)
                {
                    space.DestroyTile();
                }
                if (includePawns && space.Pawn)
                {
                    space.DestroyPawn();
                }

                if (space.Tile || space.Pawn)
                {
                    isEmpty = false;
                }
            }

            if (isEmpty)
            {
                Spaces = null;
            }
        }
        #endregion

        /// <summary>
        /// Get <see cref="bool"/> matrix of tile positions that are un-navigable.
        /// </summary>
        /// <returns></returns>
        public bool[,] GetNavigableMap()
        {
            Space[,] spaces = Spaces;
            bool[,] res = new bool[spaces.GetLength(0), spaces.GetLength(1)];

            for (int r = 0; r < spaces.GetLength(0); r++)
            {
                for (int c = 0; c < spaces.GetLength(1); c++)
                {
                    res[r, c] = !spaces[r, c].Tile.IsNavigable;
                }
            }

            return res;
        }

        public List<Tile> GetTilesInRange(Vector2Int position, int range)
        {
            IEnumerable<Vector2Int> tilePositions = Pathfinding.GetTilesInRange(GetNavigableMap(), position, range);
            return tilePositions.Select(pos => Spaces[pos.x, pos.y].Tile).ToList();
        }
    }

    [System.Serializable]
    public class PlacedPawn
    {
        public PawnType type;
        public Vector2Int Position;
        public Team team;
    }
}