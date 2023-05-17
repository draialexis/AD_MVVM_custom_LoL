using System;
using Model;

namespace StubLib
{
	public partial class StubData
	{
		private readonly List<Rune> runes = new()
		{
			new Rune("Conqueror", RuneFamily.Precision),
			new Rune("Triumph", RuneFamily.Precision),
			new Rune("Legend: Alacrity", RuneFamily.Precision),
			new Rune("Legend: Tenacity", RuneFamily.Precision),
			new Rune("last stand", RuneFamily.Domination),
			new Rune("last stand 2", RuneFamily.Domination),
		};

		public class RunesManager : IRunesManager
		{
			private readonly StubData parent;

			public RunesManager(StubData parent)
				=> this.parent = parent;

            public Task<Rune?> AddItem(Rune? item)
                => parent.runes.AddItem(item);

            public Task<bool> DeleteItem(Rune? item)
                => parent.runes.DeleteItem(item);

            public Task<IEnumerable<Rune?>> GetItems(int index, int count, string? orderingPropertyName = null, bool descending = false)
                => parent.runes.GetItemsWithFilterAndOrdering(
                    r => true,
                    index, count, orderingPropertyName, descending);

            private static Func<Rune, RuneFamily, bool> filterByRuneFamily
                = (rune, family) => rune.Family == family;

            private static Func<Rune, string, bool> filterByName
                = (rune, substring) => rune.Name.Contains(substring, StringComparison.InvariantCultureIgnoreCase);

            public Task<IEnumerable<Rune?>> GetItemsByFamily(RuneFamily family, int index, int count, string? orderingPropertyName = null, bool descending = false)
                => parent.runes.GetItemsWithFilterAndOrdering(
                    rune => filterByRuneFamily(rune, family),
                    index, count, orderingPropertyName, descending);

            public Task<IEnumerable<Rune?>> GetItemsByName(string substring, int index, int count, string? orderingPropertyName = null, bool descending = false)
                => parent.runes.GetItemsWithFilterAndOrdering(
                    rune => filterByName(rune, substring),
                    index, count, orderingPropertyName, descending);

            public Task<int> GetNbItems()
                => parent.runes.GetNbItemsWithFilter(
                    rune => true);

            public Task<int> GetNbItemsByFamily(RuneFamily family)
                => parent.runes.GetNbItemsWithFilter(
                    rune => filterByRuneFamily(rune, family));

            public Task<int> GetNbItemsByName(string substring)
                => parent.runes.GetNbItemsWithFilter(
                    rune => filterByName(rune, substring));

            public Task<Rune?> UpdateItem(Rune? oldItem, Rune? newItem)
                => parent.runes.UpdateItem(oldItem, newItem);
        }
	}
}

