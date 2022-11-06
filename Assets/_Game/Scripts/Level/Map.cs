using System;
using System.Collections.Generic;
using System.Linq;
using Game.Play.Units;
using Game.Utility;
using UnityEngine;

namespace Game.Level
{
    public class Map : MonoBehaviour
    {
        public Transform GrpTiles
        {
            get
            {
                if (!_grpTiles)
                {
                    _grpTiles = new GameObject("GRP_Tiles").transform;
                    _grpTiles.SetParent(transform, worldPositionStays: false);
                }

                return _grpTiles;
            }
        }
        Transform _grpTiles;
        
        public Transform GrpUnits
        {
            get
            {
                if (!_grpUnits)
                {
                    _grpUnits = new GameObject("GRP_Units").transform;
                    _grpUnits.SetParent(transform, worldPositionStays: false);
                }

                return _grpUnits;
            }
        }
        Transform _grpUnits;
        
        public Tile[,] Tiles
        {
            get
            {
                if (_runtimeTiles == null)
                {
                    PopulateTiles();
                }

                return _runtimeTiles;
            }
        }
        Tile[,] _runtimeTiles;

        public Dictionary<Vector2Int, Unit> Units => _runtimeUnits;
        public Dictionary<Vector2Int, Unit> _runtimeUnits;

        [Header("Layout")]
        [SerializeField]
        [NonReorderable]
        TileRow[] _tiles = new TileRow[8];

        [SerializeField]
        [NonReorderable]
        PlacedUnit[] _units;

        void Awake()
        {
            PopulateTiles();
            // PopulateUnits();
        }

        #region Generation
        [Button(Label = "Create Tiles", Mode = ButtonMode.NotInPlayMode)]
        void PopulateTiles()
        {
            // validate data
            bool hasRows = _tiles.Length != 8;
            bool hasColumns = _tiles.Any(r => r.Data.Length != 8);
            if (hasRows || hasColumns)
            {
                Debug.LogError($"\"{ name }\" needs 8 rows of 8 tiles.");
                return;
            }
            
            Clear();
            
            // create tile instances
            _runtimeTiles = new Tile[8, 8];
            for (int r = 0; r < 8; r++)
            {
                TileRow tileRow = _tiles[r];
                for (int c = 0; c < 8; c++)
                {
                    TileType type = tileRow.Data[c];
                    var tile = _runtimeTiles[r, c] = TilePalette.Get(type, GrpTiles);
                    tile.Configure(r, c);
                }
            }

            SortTiles();
        }

        void SortTiles()
        {
            // convert to 1D list
            List<Tile> _tilesLong = _runtimeTiles.Cast<Tile>().ToList();
            
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

        [Button(Label = "Create Units", Mode = ButtonMode.NotInPlayMode)]
        void PopulateUnits()
        {
            // lazy-generate tiles, units are dependent on the tiles
            if (Tiles == null)
            {
                PopulateTiles();
            }
            else if (_runtimeUnits != null)
            {
                ClearUnits();
            }

            _runtimeUnits = new Dictionary<Vector2Int, Unit>();
            foreach (PlacedUnit data in _units)
            {
                Unit unit = UnitPalette.Get(data.type, GrpUnits);
                unit.Owner = data.Owner;
                
                MoveUnit(unit, data.Position);
            }
        }
        
        [Button(Label = "Clear", Mode = ButtonMode.NotInPlayMode)]
        void Clear()
        {
            // ignore, there are no tiles to clear
            if (_runtimeTiles != null)
            {
                foreach (Tile t in _runtimeTiles)
                {
                    DestroyImmediate(t);
                }
                _runtimeTiles = null;
            }
            
            ClearUnits();
            
            // delete extra children
            for (int i = transform.childCount-1; i >= 0; i--)
            {
                var child = transform.GetChild(i);
                DestroyImmediate(child.gameObject);
            }
        }

        void ClearUnits()
        {
            if (_runtimeUnits == null) return;

            foreach (var placedUnit in _runtimeUnits)
            {
                DestroyImmediate(placedUnit.Value.gameObject);
            }
            _runtimeUnits = null;
        }
        #endregion

        /// <summary>
        /// Get <see cref="bool"/> matrix of tile positions that are un-navigable.
        /// </summary>
        /// <returns></returns>
        public bool[,] GetNavigableMap()
        {
            Tile[,] tiles = Tiles;
            bool[,] res = new bool[tiles.GetLength(0), tiles.GetLength(1)];

            for (int r = 0; r < tiles.GetLength(0); r++)
            {
                for (int c = 0; c < tiles.GetLength(1); c++)
                {
                    res[r, c] = !tiles[r, c].IsNavigable;
                }
            }

            return res;
        }

        public List<Tile> GetTilesInRange(Vector2Int position, int range)
        {
            IEnumerable<Vector2Int> tilePositions = Pathfinding.GetTilesInRange(GetNavigableMap(), position, range);
            return tilePositions.Select(pos => Tiles[pos.x, pos.y]).ToList();
        }

        public void MoveUnit(Vector2Int from, Vector2Int to)
        {
            Unit unit = Units[from];
            _runtimeUnits.Remove(from);
        }

        public void MoveUnit(Unit unit, Vector2Int to)
        {
            _runtimeUnits.Add(to, unit);
            unit.transform.position = Tiles[to.x, to.y].transform.position;
            unit.Position = to;
        }
    }

    [System.Serializable]
    public class TileRow
    {
        public TileType[] Data = new TileType[8];
    }

    [System.Serializable]
    public class PlacedUnit
    {
        public UnitType type;
        public Vector2Int Position;
        public Owner Owner;
    }
}