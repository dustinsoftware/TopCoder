using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TopCoder
{
	public static class Utility
	{
		public static TValue GetOrAddValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
			where TValue : new()
		{
			if (!dictionary.ContainsKey(key))
			{
				dictionary.Add(key, new TValue());
			}
			return dictionary[key];
		}

		public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> source)
		{
			return source == null ? null : new ReadOnlyCollection<T>(source.ToList());
		}
	}
}
