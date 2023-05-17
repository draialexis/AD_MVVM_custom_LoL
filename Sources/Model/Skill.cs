using System;

namespace Model
{
	public class Skill : IEquatable<Skill>
	{
		public SkillType Type { get; private set; }

		public string Name
		{
			get => name;
			private init
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					throw new ArgumentException("a Skill needs a name");
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

		public Skill(string name, SkillType type, string description = "")
		{
			Name = name;
			Type = type;
			Description = description ?? "";
		}

        public override bool Equals(object? obj)
        {
            if(ReferenceEquals(obj, null)) return false;
			if(ReferenceEquals(obj, this)) return true;
			if(GetType() != obj.GetType()) return false;
			return Equals(obj as Skill);
        }

        public bool Equals(Skill? other)
			=> Name.Equals(other?.Name) && Type == other.Type;

        public override int GetHashCode()
			=> Name.GetHashCode() % 281;

        public override string ToString()
			=> $"{Name} ({Type})";
    }
}

