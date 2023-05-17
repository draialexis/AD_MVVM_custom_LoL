using System;

namespace Model
{
	public class Rune : IEquatable<Rune>
	{
		public string Name
		{
			get => name;
			private init
			{
				if(string.IsNullOrWhiteSpace(value))
				{
					throw new ArgumentException("a Rune must have a name");
				}
				name = value;
			}
		}
		private readonly string name = null!;

		public string Description
		{
			get => description;
			set
			{
				if(string.IsNullOrWhiteSpace(value))
				{
					description = "";
					return;
				}
				description = value;
			}
		}
		private string description = "";

		public RuneFamily Family { get; set; }

		public string Icon { get; set; }

		public LargeImage Image { get; set; }

		public Rune(string name, RuneFamily family, string icon = "", string image = "", string description = "")
		{
			Name = name;
			Family = family;
			Icon = icon;
			Image = new LargeImage(image);
			Description = description;
		}

        public override bool Equals(object? obj)
        {
            if(ReferenceEquals(obj, null)) return false;
			if(ReferenceEquals(obj, this)) return true;
			if(GetType() != obj.GetType()) return false;
			return Equals(obj as Rune);
        }

        public bool Equals(Rune? other)
			=> Name.Equals(other?.Name);

        public override int GetHashCode()
			=> Name.GetHashCode() % 281;

        public override string ToString()
			=> $"{Name} ({Family})";
    }
}

