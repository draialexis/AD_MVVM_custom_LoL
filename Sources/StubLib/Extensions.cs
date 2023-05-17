using System;
using Model;

namespace StubLib
{
	static class Extensions
    { 
        internal static Task<IEnumerable<T?>> GetItemsWithFilterAndOrdering<T>(this IEnumerable<T> collection,
            Func<T, bool> filter, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            IEnumerable<T> temp = collection;
            temp = temp.Where(item => filter(item));
            if(orderingPropertyName != null)
            {
                var prop = typeof(T).GetProperty(orderingPropertyName!);
                if (prop != null)
                {
                    temp = descending ? temp.OrderByDescending(item => prop.GetValue(item))
                                        : temp.OrderBy(item => prop.GetValue(item));
                }
            }
            return Task.FromResult<IEnumerable<T?>>(temp.Skip(index*count).Take(count));
        }

        internal static Task<int> GetNbItemsWithFilter<T>(this IEnumerable<T> collection, Func<T, bool> filter)
        {
            return Task.FromResult(collection.Count(item => filter(item)));
        }

        internal static Task<T?> AddItem<T>(this IList<T> collection, T? item)
        {
            if(item == null || collection.Contains(item))
            {
                return Task.FromResult<T?>(default(T));
            }
            collection.Add(item);
            return Task.FromResult<T?>(item);
        }

        internal static Task<bool> DeleteItem<T>(this IList<T> collection, T? item)
        {
            if(item == null)
            {
                return Task.FromResult(false);
            }
            bool result = collection.Remove(item!);
            return Task.FromResult(result);
        }

        internal static Task<T?> UpdateItem<T>(this IList<T> collection, T? oldItem, T? newItem)
        {
            if(oldItem == null || newItem == null) return Task.FromResult<T?>(default(T));

            if(!collection.Contains(oldItem))
            {
                return Task.FromResult<T?>(default(T));
            }

            collection.Remove(oldItem!);
            collection.Add(newItem!);
            return Task.FromResult<T?>(newItem);
        }
    }
}

