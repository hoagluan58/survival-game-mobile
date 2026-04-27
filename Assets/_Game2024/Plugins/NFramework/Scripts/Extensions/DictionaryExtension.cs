using System;
using System.Collections.Generic;

namespace NFramework
{
    public static class DictionaryExtension
    {
        /// <summary>
        /// Finds first key (if there's one) that matches the value set in parameters
        /// </summary>
        public static bool TryGetKeyByValue<T, W>(this Dictionary<T, W> dictionary, W value, out T key)
        {
            key = default;
            foreach (KeyValuePair<T, W> pair in dictionary)
            {
                if (pair.Value.Equals(value))
                {
                    key = pair.Key;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Finds first key (if there's one) that matches the value set in parameters
        /// </summary>
        public static List<T> GetKeysByValue<T, W>(this Dictionary<T, W> dictionary, W value)
        {
            List<T> keys = new List<T>();
            foreach (KeyValuePair<T, W> pair in dictionary)
            {
                if (pair.Value.Equals(value))
                    keys.Add(pair.Key);
            }
            return keys;
        }

        public static bool IsNullOrEmpty<T, K>(this Dictionary<T, K> dictionary)
        {
            return dictionary == null ? true : dictionary.Count == 0;
        }

        public static Dictionary<TKey, TValue> MergeDictionaries<TKey, TValue>(this Dictionary<TKey, TValue> dict1,
            Dictionary<TKey, TValue> dict2, Func<TValue, TValue, TValue> mergeFunction)
        {
            var mergedDictionary = new Dictionary<TKey, TValue>(dict1);

            foreach (var kvp in dict2)
            {
                if (mergedDictionary.ContainsKey(kvp.Key))
                {
                    mergedDictionary[kvp.Key] = mergeFunction(mergedDictionary[kvp.Key], kvp.Value);
                }
                else
                {
                    mergedDictionary.Add(kvp.Key, kvp.Value);
                }
            }

            return mergedDictionary;
        }

        public static Dictionary<TKey, TValue> MoveKeyToFirst<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey keyToMove)
        {
            var newDict = new Dictionary<TKey, TValue>();

            if (dict.ContainsKey(keyToMove))
            {
                newDict.Add(keyToMove, dict[keyToMove]);
            }

            foreach (var kvp in dict)
            {
                if (!kvp.Key.Equals(keyToMove))
                {
                    newDict.Add(kvp.Key, kvp.Value);
                }
            }

            return newDict;
        }
    }
}

