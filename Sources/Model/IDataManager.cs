using System;
using Shared;

namespace Model
{
	public interface IDataManager
	{
		IChampionsManager ChampionsMgr { get; }
		ISkinsManager SkinsMgr { get; }
	}

	public interface IChampionsManager : IGenericDataManager<Champion?>
	{
		Task<int> GetNbItemsByCharacteristic(string charName);
		Task<IEnumerable<Champion?>> GetItemsByCharacteristic(string charName, int index, int count, string? orderingPropertyName = null, bool descending = false);

		Task<int> GetNbItemsByClass(ChampionClass championClass);
		Task<IEnumerable<Champion?>> GetItemsByClass(ChampionClass championClass, int index, int count, string? orderingPropertyName = null, bool descending = false);

		Task<int> GetNbItemsBySkill(Skill? skill);
		Task<IEnumerable<Champion?>> GetItemsBySkill(Skill? skill, int index, int count, string? orderingPropertyName = null, bool descending = false);

		Task<int> GetNbItemsBySkill(string skill);
		Task<IEnumerable<Champion?>> GetItemsBySkill(string skill, int index, int count, string? orderingPropertyName = null, bool descending = false);
	}

	public interface ISkinsManager : IGenericDataManager<Skin?>
	{
		Task<int> GetNbItemsByChampion(Champion? champion);
		Task<IEnumerable<Skin?>> GetItemsByChampion(Champion? champion, int index, int count, string? orderingPropertyName = null, bool descending = false);
	}
}

