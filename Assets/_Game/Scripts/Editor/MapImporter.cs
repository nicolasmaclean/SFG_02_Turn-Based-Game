using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using Game.Level;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace GameEditor
{
    [ScriptedImporter(1, "mapdata")]
    public class MapImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            string text = File.ReadAllText(ctx.assetPath);
            TileType[,] tiles = ReadTileTypes(text);

            MapSO map = ScriptableObject.CreateInstance<MapSO>();
            map.Load(tiles);
            
            ctx.AddObjectToAsset("Map Data", map);
        }
        
        public static TileType[,] ReadTileTypes(string text)
        {
            string[] lines    = text.Split("\n");
            TileType[,] tiles = new TileType[8, 8];

            for (int r = 0; r < 8; r++)
            {
                string[] line = lines[r].Split(" ");
                for (int c = 0; c < 8; c++)
                {
                    // validate input
                    int val = int.Parse(line[c]);
                    if (!System.Enum.IsDefined(typeof(TileType), val))
                    {
                        Debug.LogError($"Invalid tile type of { val } at ({r}, {c}).");
                        val = 0;
                    }
                    
                    tiles[r, c] = (TileType) val;
                }

            }
            return tiles;
        }
    }
}
