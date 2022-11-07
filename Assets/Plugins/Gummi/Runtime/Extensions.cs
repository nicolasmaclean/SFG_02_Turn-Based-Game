using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Gummi
{
    public static class IntExtensions
    {
        /// <summary>
        /// Returns the number of digits <paramref name="val"/> has.
        /// Note the negative sign will not be counted as a sign.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int CountDigits(this int val)
        {
            if (val == 0)
            {
                return 1;
            }
            else
            {
                return Mathf.FloorToInt(Mathf.Log10(Mathf.Abs(val))) + 1;
            }
        }
    }
    
    public static class ListExtensions
    {
        /// <summary>
        /// Prints <paramref name="list"/> in a human-readable format.
        /// It prints with each element on its own line as "## : item"
        /// such that ## is the index, zero padded, and item is item.ToString().
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string PrettyPrint(this IList list)
        {
            if (list == null)
            {
                return "null";
            }

            string output = "";

            int indexLength = (list.Count - 1).CountDigits() + 1;
            for (int i = 0; i < list.Count; i++)
            {
                output += $"{i.ToString("D" + indexLength)} : {list[i]}\n";
            }

            return output;
        }

        public static T PickRandom<T>(this List<T> list)
        {
            return list.Count switch
            {
                0 => throw new System.Exception("List is empty. Unable to pick an element at random."),
                1 => list[0],
                _ => list[Random.Range(0, list.Count)]
            };
        }
    }

    public static class ArrayExtensions
    {
        public static T[] ConvertTo1D<T>(this T[,] arr)
        {
            int rows = arr.GetLength(0);
            int cols = arr.GetLength(1);
            T[] result = new T[rows * cols];

            for (int r = 0, k = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    result[k++] = arr[r, c];
                }
            }

            return result;
        }

        public static bool IsInBounds<T>(this T[,] arr, Vector2Int pos)
        {
            return pos.x >= 0 && pos.y >= 0 && pos.x < arr.GetLength(0) && pos.y < arr.GetLength(1);
        }
    }

    public static class IEnumerableExtensions
    {
        public static T PickRandom<T>(this IEnumerable<T> enumerable)
        {
            List<T> list = enumerable.ToList();
            return list.PickRandom();
        }
    }

    public static class TransformExtensions
    {
        public static void DestroyChildrenImmediate(this Transform transform)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Transform child = transform.GetChild(i);
                Object.DestroyImmediate(child.gameObject);
            }
        }
    }
}