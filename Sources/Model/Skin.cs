using System;
using System.Diagnostics.CodeAnalysis;

namespace Model
{
	public class Skin : IEquatable<Skin>
	{
		public string Name
		{
			get => name;
			private init
			{
				if(string.IsNullOrWhiteSpace(value))
				{
					throw new ArgumentException("A skin must have a name");
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
				if (string.IsNullOrWhiteSpace(value))
				{
					description = "";
					return;
				}
				description = value;
			}
		}
		private string description = "";

		public string Icon { get; set; }
		public LargeImage Image { get; set; }

		public float Price { get; set; }

		public Champion Champion
		{
			get => champion;
			private init
			{
				if (value == null)
					throw new ArgumentNullException("A skill can't have a null champion");
				champion = value;
			}
		}
		private readonly Champion champion = null!;

		public Skin(string name, Champion champion, float price = 0.0f, string icon = "", string image = "", string description = "")
		{
			Name = name;
			Champion = champion;
			Champion.AddSkin(this);
			Price = price;
			Icon = icon;
			Image = new LargeImage(image);
			Description = description;
		}

        public override bool Equals(object? obj)
        {
            if(ReferenceEquals(obj, null)) return false;
			if(ReferenceEquals(obj, this)) return true;
			if(GetType() != obj.GetType()) return false;
			return Equals(obj as Skin);
        }

        public bool Equals(Skin? other)
			=> Name.Equals(other?.Name);

        public override int GetHashCode()
			=> Name.GetHashCode() % 997;

        public override string ToString()
			=> $"{Name}";
    }
}

