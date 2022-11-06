using System;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Interactions;

namespace Game.Level
{
    public static class MapData
    {
        public static TileType[,] ReadTileTypes(TextAsset text)
        {
            string[] lines    = text.text.Split("\n");
            TileType[,] tiles = new TileType[8, 8];

            for (int r = 0; r < 8; r++)
            {
                string[] line = lines[r].Split(" ");
                for (int c = 0; c < 8; c++)
                {
                    // validate input
                    int val = int.Parse(line[c]);
                    if (!Enum.IsDefined(typeof(TileType), val))
                    {
                        Debug.LogError($"{ text.name } has an invalid tile type of { val } at ({r}, {c}).");
                        val = 0;
                    }
                    
                    tiles[r, c] = (TileType) val;
                }

            }
            return tiles;
        }
    }
}