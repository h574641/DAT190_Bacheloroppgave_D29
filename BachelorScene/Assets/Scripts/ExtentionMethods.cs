
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ExtentionMethods
{
    public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue def)
    {
        TValue result;

        if (dict.TryGetValue(key, out result))
        {
            return result;
        }

        return def;
    }

    public static void ForEach<T>(this IEnumerable<T> value, Action<T> action)
    {
        foreach (T item in value)
        {
            action(item);
        }
    }

    public static T LastOrDefault<T>(this IList<T> list)
    {
        return list.ElementAtOrDefault(list.Count - 1);
    }
}