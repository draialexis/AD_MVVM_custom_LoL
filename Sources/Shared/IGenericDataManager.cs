namespace Shared;
public interface IGenericDataManager<T>
{
    Task<int> GetNbItems();
	Task<IEnumerable<T>> GetItems(int index, int count, string? orderingPropertyName = null, bool descending = false);
	Task<int> GetNbItemsByName(string substring);
	Task<IEnumerable<T>> GetItemsByName(string substring, int index, int count, string? orderingPropertyName = null, bool descending = false);
	Task<T> UpdateItem(T oldItem, T newItem);
	Task<T> AddItem(T item);
	Task<bool> DeleteItem(T item);
}
