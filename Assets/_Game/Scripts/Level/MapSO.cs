using System.Collections.Generic;
using Gummi;
using UnityEngine;

namespace Game.Level
{
    [CreateAssetMenu(menuName = "map")]
    public class MapSO : ScriptableObject
    {
        [SerializeField]
        [NonReorderable]
        SerializedArray<TileType>[] _tiles;

        public TileType this[int r, int c] => _tiles[r][c];

        public void Load(TileType[,] tiles)
        {
            int rows = tiles.GetLength(0);
            int cols = tiles.GetLength(1);
            
            _tiles = new SerializedArray<TileType>[rows];
            for (int r = 0; r < rows; r++)
            {
                SerializedArray<TileType> row = new SerializedArray<TileType>();
                for (int c = 0; c < cols; c++)
                {
                    row[c] = tiles[r, c];
                }

                _tiles[r] = row;
            }
        }
    }

    [System.Serializable]
    class SerializedArray<T>
    {
        [SerializeField]
        T[] _array = new T[8];

        public T this[int i]
        {
            get => _array[i];
            set => _array[i] = value;
        }
    }
}
