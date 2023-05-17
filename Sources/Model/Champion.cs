using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Text;

namespace Model;
public class Champion : IEquatable<Champion>
{
    public string Name
    {
        get => name;
        private init
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                name = "Unknown";
                return;
            }
            name = value;
        }
    }
    private readonly string name = null!;

    public string Bio
    {
        get => bio;
        set
        {
            if(value == null)
            {
                bio = "";
                return;
            }
            bio = value;
        }
    }
    private string bio = "";

    public ChampionClass Class { get; set; }

    public string Icon { get; set; }

    public LargeImage Image { get; set; }

    public Champion(string name, ChampionClass champClass = ChampionClass.Unknown, string icon = "", string image = "", string bio = "")
    {
        Name = name;
        Class = champClass;
        Icon = icon;
        Image = new LargeImage(image);
        Bio = bio;
        Characteristics = new ReadOnlyDictionary<string, int>(characteristics);
        Skins = new ReadOnlyCollection<Skin>(skins);
    }

    public ReadOnlyCollection<Skin> Skins { get; private set; }
    private List<Skin> skins = new ();

    public ReadOnlyDictionary<string, int> Characteristics { get; private set; }
    private readonly Dictionary<string, int> characteristics = new Dictionary<string, int>();

    public ImmutableHashSet<Skill> Skills => skills.ToImmutableHashSet();
    private HashSet<Skill> skills = new HashSet<Skill>();

    internal bool AddSkin(Skin skin)
    {
        if (skins.Contains(skin))
            return false;
        skins.Add(skin);
        return true;
    }

    internal bool RemoveSkin(Skin skin)
        => skins.Remove(skin);

    public bool AddSkill(Skill skill)
        => skills.Add(skill);

    public bool RemoveSkill(Skill skill)
        => skills.Remove(skill);

    public void AddCharacteristics(params Tuple<string, int>[] someCharacteristics)
    {
        foreach(var c in someCharacteristics)
        {
            characteristics[c.Item1] = c.Item2;
        }
    }

    public bool RemoveCharacteristics(string label)
        => characteristics.Remove(label);

    public int? this[string label]
    {
        get
        {
            if(!characteristics.TryGetValue(label, out int value)) return null;
            else return value;
        }
        set
        {
            if(!value.HasValue)
            {
                RemoveCharacteristics(label);
                return;
            }
            characteristics[label] = value.Value;
        }
    }

    public override bool Equals(object? obj)
    {
        if(ReferenceEquals(obj, null)) return false;
        if(ReferenceEquals(obj, this)) return true;
        if(GetType() != obj.GetType()) return false;
        return Equals(obj as Champion);
    }

    public override int GetHashCode()
        => Name.GetHashCode() % 997;

    public bool Equals(Champion? other)
        => Name.Equals(other?.Name);

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder($"{Name} ({Class})");
        if(!string.IsNullOrWhiteSpace(bio))
        {
            sb.AppendLine($"\t{bio}");
        }
        if(characteristics.Any())
        { 
            sb.AppendLine("\tCharacteristics:");
            foreach(var characteristic in characteristics)
            {
                sb.AppendLine($"\t\t{characteristic.Key} - {characteristic.Value}");
            }
        }
        if(skills.Any())
        {
            sb.AppendLine("\tSkills:");
            foreach(var skill in Skills)
            {
                sb.AppendLine($"\t\t{skill.Name} - {skill.Description}");
            }
        }
        return sb.ToString();
    } 
}

