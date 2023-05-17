using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Model;
using StubLib;
using static System.Console;

namespace ConsoleTests
{
	static class Program
	{
		static IDataManager dataManager = null!;

        static async Task Main(string[] args)
		{
			try
			{
				using var servicesProvider = new ServiceCollection()
								.AddSingleton<IDataManager, StubData>()
								.BuildServiceProvider();

				dataManager = servicesProvider.GetRequiredService<IDataManager>();

				await DisplayMainMenu();

				Console.ReadLine();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex, "Stopped program because of exception");
				throw;
			}
		}

		public static async Task DisplayMainMenu()
		{
			Dictionary<int, string> choices = new Dictionary<int, string>()
			{
				[1] = "1- Manage Champions",
				[2] = "2- Manage Skins",
				[3] = "3- Manage Runes",
				[4] = "4- Manage Rune Pages",
				[99] = "99- Quit"
			};

			while(true)
			{ 
				int input = DisplayAMenu(choices);

				switch(input)
				{
					case 1:
						await DisplayChampionsMenu();
						break;
					case 2:
						break;
					case 3:
						break;
					case 4:
						break;
					case 99:
						WriteLine("Bye bye!");
						return;
					default:
						break;
				}
			}
		}

		private static int DisplayAMenu(Dictionary<int, string> choices)
		{
			int input=-1;
			while(true)
			{
				WriteLine("What is your choice?");
				WriteLine("--------------------");
				foreach(var choice in choices.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value))
				{
					WriteLine(choice);
				}
				if(!int.TryParse(ReadLine(), out input) || input == -1)
				{
					WriteLine("I do not understand what your choice is. Please try again.");
					continue;
				}
				break;
			}
			WriteLine($"You have chosen: {choices[input]}");
			WriteLine();
			return input;
		}

		public static async Task DisplayChampionsMenu()
		{
			Dictionary<int, string> choices = new Dictionary<int, string>()
			{
				[0] = "0- Get number of champions",
				[1] = "1- Get champions",
				[2] = "2- Find champions by name",
				[3] = "3- Find champions by characteristic",
				[4] = "4- Find champions by class",
				[5] = "5- Find champions by skill",
				[6] = "6- Add new champion",
				[7] = "7- Delete a champion",
				[8] = "8- Update a champion",
			};

			int input = DisplayAMenu(choices);

			switch(input)
			{
				case 0:
					int nb = await dataManager.ChampionsMgr.GetNbItems();
					WriteLine($"There are {nb} champions");
					WriteLine("**********************");
					break;
				case 1:
					{ 
						int index = ReadAnInt("Please enter the page index");
						int count = ReadAnInt("Please enter the number of elements to display");
						WriteLine($"{count} champions of page {index+1}");
						var champions = await dataManager.ChampionsMgr.GetItems(index, count, nameof(Champion.Name));
						foreach(var champion in champions)
						{
							WriteLine($"\t{champion}");
						}
						WriteLine("**********************");
					}
					break;
				case 2:
					{
						string substring = ReadAString("Please enter the substring to look for in the name of a champion");
						int index = ReadAnInt("Please enter the page index");
						int count = ReadAnInt("Please enter the number of elements to display");
						var champions = await dataManager.ChampionsMgr.GetItemsByName(substring, index, count, nameof(Champion.Name));
						foreach(var champion in champions)
						{
							WriteLine($"\t{champion}");
						}
						WriteLine("**********************");
					}
					break;
				case 3:
					{
						string substring = ReadAString("Please enter the substring to look for in the characteristics of champions");
						int index = ReadAnInt("Please enter the page index");
						int count = ReadAnInt("Please enter the number of elements to display");
						var champions = await dataManager.ChampionsMgr.GetItemsByCharacteristic(substring, index, count, nameof(Champion.Name));
						foreach(var champion in champions)
						{
							WriteLine($"\t{champion}");
						}
						WriteLine("**********************");
					}
					break;
				case 4:
					{
						ChampionClass championClass = ReadAnEnum<ChampionClass>($"Please enter the champion class (possible values are: {Enum.GetNames<ChampionClass>().Aggregate("", (name, chaine) => $"{chaine} {name}")}):");
						int index = ReadAnInt("Please enter the page index");
						int count = ReadAnInt("Please enter the number of elements to display");
						var champions = await dataManager.ChampionsMgr.GetItemsByClass(championClass, index, count, nameof(Champion.Name));
						foreach(var champion in champions)
						{
							WriteLine($"\t{champion}");
						}
						WriteLine("**********************");
					}
					break;
				case 5:
					{
						string substring = ReadAString("Please enter the substring to look for in the skills of champions");
						int index = ReadAnInt("Please enter the page index");
						int count = ReadAnInt("Please enter the number of elements to display");
						var champions = await dataManager.ChampionsMgr.GetItemsBySkill(substring, index, count, nameof(Champion.Name));
						foreach(var champion in champions)
						{
							WriteLine($"\t{champion}");
						}
						WriteLine("**********************");
					}
					break;
				case 6:
					{
						WriteLine("You are going to create a new champion.");
						string name = ReadAString("Please enter the champion name:");
						ChampionClass championClass = ReadAnEnum<ChampionClass>($"Please enter the champion class (possible values are: {Enum.GetNames<ChampionClass>().Aggregate("", (name, chaine) => $"{chaine} {name}")}):");
						string bio = ReadAString("Please enter the champion bio:");
						Champion champion = new Champion(name, championClass, bio: bio);
						DisplayCreationChampionMenu(champion);
						_ = await dataManager.ChampionsMgr.AddItem(champion);
					}	
					break;
				case 7:
					{
						WriteLine("You are going to delete a champion.");
						string name = ReadAString("Please enter the champion name:");
						var somechampions = await dataManager.ChampionsMgr.GetItemsByName(name, 0, 10, nameof(Champion.Name));
						var someChampionNames = somechampions.Select(c => c!.Name);
						var someChampionNamesAsOneString = someChampionNames.Aggregate("", (name, chaine) => $"{chaine} {name}");
						string champName = ReadAStringAmongPossibleValues($"Who do you want to delete among these champions? (type \"Cancel\" to ... cancel) {someChampionNamesAsOneString}",
																			someChampionNames.ToArray());
						if(champName != "Cancel")
						{
							await dataManager.ChampionsMgr.DeleteItem(somechampions.Single(c => c!.Name == champName));
						}
					}	
					break;
				case 8:
					{
						WriteLine("You are going to update a champion.");
						string name = ReadAString("Please enter the champion name:");
						var somechampions = await dataManager.ChampionsMgr.GetItemsByName(name, 0, 10, nameof(Champion.Name));
						var someChampionNames = somechampions.Select(c => c!.Name);
						var someChampionNamesAsOneString = someChampionNames.Aggregate("", (name, chaine) => $"{chaine} {name}");
						string champName = ReadAStringAmongPossibleValues($"Who do you want to update among these champions? (type \"Cancel\" to ... cancel) {someChampionNamesAsOneString}",
																			someChampionNames.ToArray());
						if(champName == "Cancel") break;
						ChampionClass championClass = ReadAnEnum<ChampionClass>($"Please enter the champion class (possible values are: {Enum.GetNames<ChampionClass>().Aggregate("", (name, chaine) => $"{chaine} {name}")}):");
						string bio = ReadAString("Please enter the champion bio:");
						Champion champion = new Champion(champName, championClass, bio: bio);
						DisplayCreationChampionMenu(champion);
						await dataManager.ChampionsMgr.UpdateItem(somechampions.Single(c => c!.Name == champName), champion);
					}	
					break;
				default:
					break;
			}

		}

		public static void DisplayCreationChampionMenu(Champion champion)
		{
			Dictionary<int, string> choices = new Dictionary<int, string>()
			{
				[1] = "1- Add a skill",
				[2] = "2- Add a skin",
				[3] = "3- Add a characteristic",
				[99] = "99- Finish"
			};

			while(true)
			{ 
				int input = DisplayAMenu(choices);

				switch(input)
				{
					case 1:
						string skillName = ReadAString("Please enter the skill name:");
						SkillType skillType = ReadAnEnum<SkillType>($"Please enter the skill type (possible values are: {Enum.GetNames<SkillType>().Aggregate("", (name, chaine) => $"{chaine} {name}")}):");
						string skillDescription = ReadAString("Please enter the skill description:");
						Skill skill = new Skill(skillName, skillType, skillDescription);
						champion.AddSkill(skill);
						break;
					case 2:
						string skinName = ReadAString("Please enter the skin name:");
						string skinDescription = ReadAString("Please enter the skin description:");
						float skinPrice = ReadAFloat("Please enter the price of this skin:");
						Skin skin = new Skin(skinName, champion, skinPrice, description: skinDescription);
						break;
					case 3:
						string characteristic = ReadAString("Please enter the characteristic:");
						int value = ReadAnInt("Please enter the value associated to this characteristic:");
						champion.AddCharacteristics(Tuple.Create(characteristic, value));
						break;
					case 99:
						return;
					default:
						break;
				}
			}
		}

		private static int ReadAnInt(string message)
		{
			while(true)
			{
				WriteLine(message);
				if(!int.TryParse(ReadLine(), out int result))
				{
					continue;
				}
				return result;
			}
		}

		private static float ReadAFloat(string message)
		{
			while(true)
			{
				WriteLine(message);
				if(!float.TryParse(ReadLine(), out float result))
				{
					continue;
				}
				return result;
			}
		}

		private static string ReadAString(string message)
		{
			while(true)
			{
				WriteLine(message);
				string? line = ReadLine();
				if(line == null)
				{
					continue;
				}
				return line!;
			}
		}

		private static TEnum ReadAnEnum<TEnum>(string message) where TEnum :struct
		{
			while(true)
			{
				WriteLine(message);
				if(!Enum.TryParse<TEnum>(ReadLine(), out TEnum result))
				{
					continue;
				}
				return result;
			}
		}

		private static string ReadAStringAmongPossibleValues(string message, params string[] possibleValues)
		{
			while(true)
			{
				WriteLine(message);
				string? result = ReadLine();
				if(result == null) continue;
				if(result != "Cancel" && !possibleValues.Contains(result!)) continue;
				return result!;
			}
		}
	}
}