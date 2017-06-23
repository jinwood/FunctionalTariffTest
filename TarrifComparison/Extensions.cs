using System;
using System.Collections.Generic;

namespace TarrifComparison
{
    public static class DecimalExtensions
    {
        //"this" keyword denotes an extension method and allows
        //you to chain this function call against variables
        //of this type, ie myDecimalVar.ApplyVAT();
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
