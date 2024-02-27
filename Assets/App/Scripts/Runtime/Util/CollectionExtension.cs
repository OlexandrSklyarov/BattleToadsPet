using System;
using System.Collections.Generic;

namespace Util
{
    public static class CollectionExtension
    {
        private static Random Rand = new Random();

        public static T RandomElement<T>(this T[] items)
        {
            return items[Rand.Next(0, items.Length)];
        }

        public static T RandomElement<T>(this List<T> items)
        {
            return items[Rand.Next(0, items.Count)];
        }

        public static int RandomIndex<T>(this T[]items)
        {
            return Rand.Next(0, items.Length);
        }

        public static int RandomIndex<T>(this List<T> items)
        {
            return Rand.Next(0, items.Count);
        }

        private static Random rng = new Random();  

        public static void Shuffle<T>(this IList<T> list)  
        {  
            int n = list.Count;  
            while (n > 1) {  
                n--;  
                int k = rng.Next(n + 1);  
                T value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }
    }
}