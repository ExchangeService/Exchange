using System;
using System.Collections.Generic;
using System.Linq;

namespace Exchange.Shared.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TResult> LeftOuterJoin<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner?, TResult> resultSelector) =>
            outer.GroupJoin(
                    inner,
                    outerKeySelector,
                    innerKeySelector,
                    (a, b) => new
                              {
                                  a,
                                  b
                              })
                .SelectMany(x => x.b.DefaultIfEmpty(), (x, b) => resultSelector(x.a, b));

        public static IEnumerable<T> ToList<T>(this T element, params T[] array) =>
            new List<T>
            {
                new List<T>
                {
                    element
                },
                array
            };
    }
}