using System;
using System.Collections.Generic;

namespace TarrifComparison
{
    public static class DecimalExtensions
    {
        public static decimal ApplyVAT(this decimal value)
        {
            return value + (Constants.VatPercent * value);
        }

        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            // argument null checking omitted
            foreach (T item in sequence) action(item);
        }
    }
}
