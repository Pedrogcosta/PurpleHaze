using System;
using System.Collections.Generic;

public static class DictionaryExtensions
{
    public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueFactory)
    {
        if (dictionary.TryGetValue(key, out TValue existingValue))
        {
            return existingValue;
        }
        else
        {
            TValue newValue = valueFactory();
            dictionary[key] = newValue;
            return newValue;
        }
    }
}
