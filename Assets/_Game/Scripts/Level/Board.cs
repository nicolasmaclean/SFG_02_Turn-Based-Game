using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Play.Units;
using Game.Utility;
using Gummi;
using Gummi.Singletons;
using Gummi.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Level
{
    public class Board : Singleton<Board>
    {
        public Space[,] Spaces { get; private set; }

        [Header("Layout")]
        [SerializeField]
        MapSO _tileData;

        [SerializeField, FormerlySerializedAs("_units")]
        [NonReorderable]
        PlacedPawn[] _pawns;

        protected override void Awake()
        {
            base.Awake();
            PopulateTiles();
            PopulatePawns();
        }
        
        #region Combat
        public void Hit(Vector2Int pos, Vector2Int from, int damage)
        {
            Space space = Spaces[pos.x, pos.y];
            Tile tile   = space.Tile;
            Pawn pawn   = space.Pawn;
            
            // damage tile
            tile.Damage(damage);
            
            // there is no pawn to damage/push
            if (!pawn) return;

            // hurt pawn
            if (pawn.Hurt(damage))
            {
                space.RemovePawn();
                pawn = null;
            }
                
            // get next position
            Vector2Int to = pos - from;
            to.Clamp(-Vector2Int.one, Vector2Int.one);
            to += pos;
            
            // case: push off edge
            // exit, this isn't allowed
            if (!InBounds(to))
            {
                return;
            }
                
            // case: move to the next tile
            Space next = Spaces[to.x, to.y];
            if (pawn && next.Tile.CanBeOccupied)
            {
                space.RemovePawn();
                next.AddPawn(pawn);
                return;
            }
                
            // case: tile is occupied
            // damage the next tile and the pawn
            Hit(next);
        }

        static void Hit(Space space)
        {
            Tile tile      = space.Tile;
            Pawn pawn      = space.Pawn;

            if (pawn)
            {
                if (pawn.Hurt(1)) space.RemovePawn();
            }
            else if (tile)
            {
                tile.Damage(1);
            }
        }

        public bool TryMove(Pawn pawn, Vector2Int to)
        {
            Vector2Int from = pawn.Position;
            
            // validate move is in range
            int distance = GetDistance(from, to);
            if (distance > pawn.Movement) return false;
            
            Spaces[from.x, from.y].RemovePawn();
            Spaces[to.x,   to.y].AddPawn(pawn);
            return true;
        }
        #endregion

        #region Generation
        [Button(Label = "Create Tiles", Mode = ButtonMode.NotInPlayMode)]
        void PopulateTiles()
        {
            Clear();
            
            // create tile instances
            Spaces = new Space[8, 8];
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    TileType type = _tileData[r, c];
                    
                    Tile tile = TilePalette.Get(type, transform);
                    var space = Spaces[r, c] = new Space(tile, new Vector2Int(r, c));
                    
                    tile.Configure(this, space);
                }
            }

            SortTiles();
        }

        void SortTiles()
        {
            // convert to 1D list
            List<Tile> tilesLong = Spaces.ConvertTo1D().Select(s => s.Tile).ToList();
            
            // sort
            tilesLong.Sort((a, b) =>
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
            for (int i = 0; i < tilesLong.Count; i++)
            {
                Tile tile = tilesLong[i];
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
                pawn.Team = data.team;
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
        
        #region Feedback
        public void Highlight(HashSet<Vector2Int> tiles)
        {
            foreach (var vector in tiles)
            {
                Spaces[vector.x, vector.y].Tile.Highlight();
            }
        }

        public void Unhighlight(HashSet<Vector2Int> tiles)
        {
            foreach (var vector in tiles)
            {
                Spaces[vector.x, vector.y].Tile.Unhighlight();
            }
        }
        #endregion

        #region Utility
        public static bool InBounds(Vector2Int pos)
        {
            return pos.x is > 0 and < 8 && pos.y is > 0 and < 8;
        }

        /// <summary>
        /// Get <see cref="bool"/> matrix of tile positions that are un-navigable.
        /// </summary>
        /// <returns></returns>
        public bool[,] GetNavigableMap(bool includePawns = true)
        {
            Space[,] spaces = Spaces;
            bool[,] res = new bool[spaces.GetLength(0), spaces.GetLength(1)];

            for (int r = 0; r < spaces.GetLength(0); r++)
            {
                for (int c = 0; c < spaces.GetLength(1); c++)
                {
                    res[r, c] = !spaces[r, c].Tile.IsNavigable || (includePawns && spaces[r, c].Pawn);
                }
            }

            return res;
        }

        public int GetDistance(Vector2Int from, Vector2Int to)
        {
            return Pathfinding.GetDistance(GetNavigableMap(), from, to);
        }

        public HashSet<Vector2Int> GetTilesInRange(Vector2Int position, int range)
        {
            IEnumerable<Vector2Int> tilePositions = Pathfinding.GetTilesInRange(GetNavigableMap(), position, range);
            return tilePositions.ToHashSet();
            // return tilePositions.Select(pos => Spaces[pos.x, pos.y].Tile).ToList();
        }

        public List<Vector2Int> GetAdjacentBuildings(Vector2Int position)
        {
            List<Vector2Int> buildings = new List<Vector2Int>();
            SearchNeighbors(position, space =>
            {
                if (space.Tile && space.Tile.Type == TileType.Building)
                {
                    buildings.Add(space.Position);
                }
                return false;
            });

            return buildings;
        }
        
        public Optional<Vector2Int> GetAdjacentBuilding(Vector2Int position)
        {
            Optional<Vector2Int> found = new Optional<Vector2Int>(Vector2Int.zero);
            SearchNeighbors(position, space =>
            {
                if (space.Tile.Type == TileType.Building)
                {
                    found.Value = space.Position;
                    found.Enabled = true;
                }
                
                return found.Enabled;
            });

            return found;
        }
        
        public bool HasAdjacentBuilding(Vector2Int position)
        {
            return GetAdjacentBuilding(position).Enabled;
        }

        public List<Vector2Int> GetAdjacentPawns(Vector2Int position, Team team)
        {
            List<Vector2Int> pawns = new List<Vector2Int>();
            SearchNeighbors(position, space =>
            {
                if (space.Pawn && space.Pawn.Team == team)
                {
                    pawns.Add(space.Position);
                }
                return false;
            });

            return pawns;
        }

        public Optional<Vector2Int> GetAdjacentPawn(Vector2Int position, Team team)
        {
            Optional<Vector2Int> found = new Optional<Vector2Int>(Vector2Int.zero);
            SearchNeighbors(position, space =>
            {
                if (space.Pawn && space.Pawn.Team == team)
                {
                    found.Value = space.Position;
                    found.Enabled = true;
                }
                
                return found.Enabled;
            });

            return found;
        }
        
        public bool HasAdjacentPawn(Vector2Int position, Team team)
        {
            return GetAdjacentPawn(position, team).Enabled;
        }

        public List<Pawn> Pawns(Team team)
        {
            List<Space> spaces = Spaces.ConvertTo1D().ToList();
            
            List<Pawn> pawns = spaces.Select(space => space.Pawn).ToList();
            pawns.RemoveAll(pawn => pawn == null || pawn.Team != team);
            
            return pawns;
        }

        public HashSet<Vector2Int> GetDropZone()
        {
            var dropzone = from space in Spaces.ConvertTo1D().ToArray()
                where space.Tile.IsDropZone
                select space.Position;
            return dropzone.ToHashSet();
        }

        void SearchNeighbors(Vector2Int position, System.Func<Space, bool> callback)
        {
            foreach (var offset in Pathfinding.DIRECTIONS)
            {
                Vector2Int neighborPosition = position + offset;
                if (!Spaces.IsInBounds(neighborPosition)) continue;;

                Space neighbor = Spaces[neighborPosition.x, neighborPosition.y];
                if (callback.Invoke(neighbor)) break;
            }
        }
        #endregion
    }

    [System.Serializable]
    public class PlacedPawn
    {
        public PawnType type;
        public Vector2Int Position;
        public Team team;
    }
}