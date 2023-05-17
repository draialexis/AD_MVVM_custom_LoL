using System;
using Model;

namespace StubLib
{
	public partial class StubData
	{
		private readonly List<RunePage> runePages = new();

        private void InitRunePages()
        {
            var runePage1 = new RunePage("rune page 1");
            runePage1[RunePage.Category.Major] = runes[0];
            runePage1[RunePage.Category.Minor1] = runes[1];
            runePage1[RunePage.Category.Minor2] = runes[2];
            runePage1[RunePage.Category.Minor3] = runes[3];
            runePage1[RunePage.Category.OtherMinor1] = runes[4];
            runePage1[RunePage.Category.OtherMinor2] = runes[5];
            runePages.Add(runePage1);
        }

		public class RunePagesManager : IRunePagesManager
		{
			private readonly StubData parent;

			public RunePagesManager(StubData parent)
				=> this.parent = parent;

            private static Func<RunePage, string, bool> filterByName
                = (rp, substring) => rp.Name.Contains(substring, StringComparison.InvariantCultureIgnoreCase);

            private static Func<RunePage, Rune?, bool> filterByRune
                = (rp, rune) => rune != null && rp.Runes.Values.Contains(rune!);

            public Task<RunePage?> AddItem(RunePage? item)
                => parent.runePages.AddItem(item);

            public Task<bool> DeleteItem(RunePage? item)
                => parent.runePages.DeleteItem(item);

            public Task<IEnumerable<RunePage?>> GetItems(int index, int count, string? orderingPropertyName = null, bool descending = false)
                => parent.runePages.GetItemsWithFilterAndOrdering(
                    rp => true,
                    index, count, orderingPropertyName, descending);

            public Task<IEnumerable<RunePage?>> GetItemsByChampion(Champion? champion, int index, int count, string? orderingPropertyName = null, bool descending = false)
                => Task.FromResult<IEnumerable<RunePage?>>(
                    parent.championsAndRunePages
                        .Where(tuple => tuple.Item1.Equals(champion))
                        .Select(tuple => tuple.Item2)
                        .Skip(index*count).Take(count));

            public Task<IEnumerable<RunePage?>> GetItemsByName(string substring, int index, int count, string? orderingPropertyName = null, bool descending = false)
                => parent.runePages.GetItemsWithFilterAndOrdering(
                    rp => filterByName(rp, substring),
                    index, count, orderingPropertyName, descending);

            public Task<IEnumerable<RunePage?>> GetItemsByRune(Rune? rune, int index, int count, string? orderingPropertyName = null, bool descending = false)
                => parent.runePages.GetItemsWithFilterAndOrdering(
                    rp => filterByRune(rp, rune),
                    index, count, orderingPropertyName, descending);

            public Task<int> GetNbItems()
                => parent.runePages.GetNbItemsWithFilter(
                    rp => true);

            public Task<int> GetNbItemsByChampion(Champion? champion)
                => Task.FromResult(parent.championsAndRunePages.Count(tuple => tuple.Item1.Equals(champion)));

            public Task<int> GetNbItemsByName(string substring)
                => parent.runePages.GetNbItemsWithFilter(
                    rp => filterByName(rp, substring));

            public Task<int> GetNbItemsByRune(Rune? rune)
                => parent.runePages.GetNbItemsWithFilter(
                    rp => filterByRune(rp, rune));

            public Task<RunePage?> UpdateItem(RunePage? oldItem, RunePage? newItem)
                => parent.runePages.UpdateItem(oldItem, newItem);
        }
	}
}

