using System.Collections.Generic;
using System.IO;
using Loader.scdb.Xml.Factions;
using Newtonsoft.Json;

namespace Loader
{
	public class FactionLoader
	{
		public string OutputFolder { get; set; }
		public string DataRoot { get; set; }

		public LocalisationService locService { get; set; }

		public Dictionary<string, Faction> LoadFactions()
		{
			Directory.CreateDirectory(Path.Combine(OutputFolder, "factions"));

			var output = new Dictionary<string, Faction>();

			var path = Path.Combine(DataRoot, Path.Join("Data", "Libs", "Foundry", "Records", "factions"));
			var parser = new ClassParser<Faction>();

			foreach (var entityFilename in Directory.EnumerateFiles(path, "*.xml"))
			{
				var faction = parser.Parse(entityFilename);
				AddTranslations(faction);
				output.Add(faction.__ref, faction);
				File.WriteAllText(Path.Combine(OutputFolder, "factions", $"{faction.ClassName.ToLower()}.json"), JsonConvert.SerializeObject(faction));
			}

			return output;
		}

		private void AddTranslations(Faction faction)
		{
			faction.displayName = locService.GetText(faction.displayName, faction.displayName);
			faction.description = locService.GetText(faction.description, faction.description);
		}
	}
}
