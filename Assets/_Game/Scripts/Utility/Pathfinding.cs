using System.Collections.Generic;
using Gummi;
using UnityEngine;

namespace Game.Utility
{
    public static class Pathfinding
    {
        public static IEnumerable<Vector2Int> GetTilesInRange(bool[,] map, Vector2Int position, int range)
        {
            List<Vector2Int> tiles = new List<Vector2Int>();

            // short-circuiting
            if (range == 0)
            {
                if (!map[position.x, position.y])
                {
                    tiles.Add(position);
                }
                
                return tiles;
            }
            
            // get positions from distance matrix
            int[,] distances = Dijkstra(map, position);
            for (int r = 0; r < distances.GetLength(0); r++)
            {
                for (int c = 0; c < distances.GetLength(1); c++)
                {
                    if (distances[r, c] < 0 || range < distances[r, c]) continue;
                    tiles.Add(new Vector2Int(r, c));
                }

            }
            
            return tiles;
        }

        public static int GetDistance(bool[,] map, Vector2Int from, Vector2Int to)
        {
            int[,] distances = Dijkstra(map, from);
            return distances[to.x, to.y];
        }

        static int[,] Dijkstra(bool[,] map, Vector2Int position)
        {
            // construct default distances matrix
            int[,] distances = new int[map.GetLength(0), map.GetLength(1)];
            for (int r = 0; r < distances.GetLength(0); r++)
            {
                for (int c = 0; c < distances.GetLength(1); c++)
                {
                    distances[r, c] = -1;
                }
            }
            
            return Dijkstra(map, distances, position);
        }

        public static readonly Vector2Int[] DIRECTIONS = { Vector2Int.down, Vector2Int.left, Vector2Int.up, Vector2Int.right };
        static int[,] Dijkstra(bool[,] map, int[,] distances, Vector2Int position)
        {
            Queue<Vector2Int> toVisit = new Queue<Vector2Int>();
            toVisit.Enqueue(position);
            distances[position.x, position.y] = 0;

            // flood fill
            while (toVisit.TryDequeue(out Vector2Int point))
            {
                int newDistance = distances[point.x, point.y] + 1;

                // visit neighbors
                foreach (Vector2Int offset in DIRECTIONS)
                {
                    Vector2Int nextPosition = point + offset;
                    
                    // skip, this position is out of bounds 
                    if (!map.IsInBounds(nextPosition)) continue;
                    
                    // skip, this position is not navigable
                    if (map[nextPosition.x, nextPosition.y]) continue;

                    // skip, this path is longer than one that has already been found
                    int nextDist = distances[nextPosition.x, nextPosition.y];
                    if (0 <= nextDist && nextDist <= newDistance) continue;

                    // save onto path
                    distances[nextPosition.x, nextPosition.y] = newDistance;
                    toVisit.Enqueue(nextPosition);
                }
            }

            return distances;
        }
    }
}
