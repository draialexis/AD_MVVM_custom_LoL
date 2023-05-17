using System;
using System.Collections.ObjectModel;

namespace Model
{
	public partial class RunePage
	{
		public string Name
		{
			get => name;
			private init
			{
				if(string.IsNullOrWhiteSpace(value))
				{
					throw new ArgumentException("a Rune Page must have a name");
				}
				name = value;
			}
		}
		private readonly string name = null!;

		public ReadOnlyDictionary<Category, Rune> Runes { get; private set; }
		private Dictionary<Category, Rune> runes = new Dictionary<Category, Rune>();

		public RunePage(string name)
		{
			Name = name;
			Runes = new ReadOnlyDictionary<Category, Rune>(runes);
		}

		public Rune? this[Category category]
		{
			get
			{
				if(runes.TryGetValue(category, out Rune? rune))
				{
					return rune;
				}
				return null;
			}
			set
			{
				if(value == null)
				{
					runes.Remove(category);
					return;
				}
				runes[category] = value!;
				CheckRunes(category);
			}
		}

		private void CheckRunes(Category newRuneCategory)
		{
			switch(newRuneCategory)
			{
				case Category.Major:
					UpdateMajorFamily(Category.Minor1, true);
					UpdateMajorFamily(Category.Minor2, true);
					UpdateMajorFamily(Category.Minor3, true);
					UpdateMajorFamily(Category.OtherMinor1, false);
					UpdateMajorFamily(Category.OtherMinor2, false);
					break;
				case Category.Minor1:
				case Category.Minor2:
				case Category.Minor3:
					UpdateMajorFamily(newRuneCategory, true);
					break;
				case Category.OtherMinor1:
				case Category.OtherMinor2:
					UpdateMajorFamily(newRuneCategory, false);
					break;
			}
		}

		private bool? CheckFamilies(Category cat1, Category cat2)
		{
			runes.TryGetValue(cat1, out Rune? rune1);
			runes.TryGetValue(cat2, out Rune? rune2);
			if(rune1 == null || rune2 == null)
			{
				return null;
			}
			return rune1.Family == rune2.Family;
		}

		private void UpdateMajorFamily(Category cat, bool expectedValue)
		{
			if(CheckFamilies(Category.Major, cat).GetValueOrDefault(expectedValue) != expectedValue)
			{
				runes.Remove(cat);
			}
		}
	}
}

