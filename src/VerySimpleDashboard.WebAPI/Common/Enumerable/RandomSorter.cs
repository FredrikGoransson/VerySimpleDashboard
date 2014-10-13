using System;
using System.Collections.Generic;
using System.Linq;

namespace VerySimpleDashboard.WebAPI.Common.Enumerable
{
    public static class RandomSorter
    {
        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> enumerable)
        {
            var random = new Random(Environment.TickCount);
            return enumerable.Select(o => new {Item = o, Index = random.NextDouble()})
                .OrderBy(o => o.Index)
                .Select(o => o.Item);
        }
    }
}