using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Scripts.Utils
{
    public static class Extensions
    {
        public static T GetRandom<T> (this IEnumerable<T> collection)
        {
            T[] array = collection.ToArray();
            return array[Random.Range (0, array.Length)];
        }
    }
}