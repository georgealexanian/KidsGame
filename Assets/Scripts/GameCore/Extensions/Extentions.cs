using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GameCore.Extensions
{
    [UnityEngine.Scripting.Preserve]
    public static class Extensions
    {
        public static int GetIndex(this IEnumerable<ValueOfRange> indexRanges, int value)
        {
            return BaseGetIndex(indexRanges, value);
        }
    
        private static int BaseGetIndex(IEnumerable<ValueOfRange> indexRanges, int value)
        {
            foreach (var indexRange in indexRanges)
            {
                if (indexRange.InsideOf(value))
                {
                    return indexRange.value;
                }
            }

            return -1;
        }
        
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}