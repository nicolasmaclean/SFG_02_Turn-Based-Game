using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Map : MonoBehaviour
    {
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

        [SerializeField]
        [NonReorderable]
        Row[] _tiles = new Row[8];

        void Awake()
        {
            PopulateTiles();
        }

        [Button(Label = "Create Tiles")]
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
            
            ClearTiles();
            
            // create tile instances
            _runtimeTiles = new Tile[8, 8];
            for (int r = 0; r < 8; r++)
            {
                Row row = _tiles[r];
                for (int c = 0; c < 8; c++)
                {
                    TileType type = row.Data[c];
                    var tile = _runtimeTiles[r, c] = TilePalette.Get(type, transform);
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

        [Button(Label = "Clear Tiles")]
        void ClearTiles()
        {
            // exit, there are no tiles to clear
            if (_runtimeTiles != null)
            {
                foreach (Tile t in _runtimeTiles)
                {
                    DestroyImmediate(t);
                }
                _runtimeTiles = null;
            }
            
            // delete extra children
            for (int i = transform.childCount-1; i >= 0; i--)
            {
                var child = transform.GetChild(i);
                DestroyImmediate(child.gameObject);
            }
        }

        Tile _hoverCurrent;
        public void UpdateHover(Tile tile)
        {
            // if (_hoverCurrent)
            // {
            //     _hoverCurrent.GetComponentInChildren<Image>().color = Color.white;
            // }
            //
            // _hoverCurrent = tile;
            // _hoverCurrent.GetComponentInChildren<Image>().color = Color.red;
        }

        Tile _selectionCurrent;
        public void UpdateSelection(Tile tile)
        {
            
        }
    }

    [System.Serializable]
    public class Row
    {
        public TileType[] Data = new TileType[8];
    }
}