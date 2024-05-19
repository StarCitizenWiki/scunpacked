using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using Loader.scdb.Xml.Missiongiver;
using Loader.scdb.Xml.Missiontype;
using Newtonsoft.Json;
using scdb.Xml.Missionbroker;

namespace Loader
{
	public class MissionLoader
	{
		public string OutputFolder { get; set; }
		public string DataRoot { get; set; }
		public LocalisationService locService { get; set; }

		public ConcurrentDictionary<string, MissionBrokerEntry> LoadMissions()
		{
			Directory.CreateDirectory(Path.Combine(OutputFolder, "missions"));

			var output = new ConcurrentDictionary<string, MissionBrokerEntry>();
			var parser = new ClassParser<MissionBrokerEntry>();

			var path = Path.Combine(DataRoot, Path.Join("Data", "Libs", "Foundry", "Records", "missionbroker"));

			Parallel.ForEach(Directory.EnumerateFiles(path, "*.xml", SearchOption.AllDirectories), entityFilename =>
			{
				var mission = parser.Parse(entityFilename);
				AddTranslations(mission);
				output.TryAdd(mission.__ref, mission);
				File.WriteAllText(Path.Combine(OutputFolder, "missions", $"{mission.ClassName.ToLower()}.json"), JsonConvert.SerializeObject(mission));
			});

			return output;
		}

		public ConcurrentDictionary<string, MissionType> LoadMissionTypes()
		{
			Directory.CreateDirectory(Path.Combine(OutputFolder, "missions", "types"));

			var output = new ConcurrentDictionary<string, MissionType>();

			var path = Path.Combine(DataRoot, Path.Join("Data", "Libs", "Foundry", "Records", "missiontype"));

			var parser = new ClassParser<MissionType>();

			Parallel.ForEach(Directory.EnumerateFiles(path, "*.xml", SearchOption.AllDirectories), entityFilename =>
			{
				var missionType = parser.Parse(entityFilename);
				AddTypeTranslations(missionType);
				output.TryAdd(missionType.__ref, missionType);
				File.WriteAllText(Path.Combine(OutputFolder, "missions", "types", $"{missionType.ClassName.ToLower()}.json"),JsonConvert.SerializeObject(missionType));
			});

			return output;
		}

		public ConcurrentDictionary<string, MissionGiver> LoadMissionGiver()
		{
			Directory.CreateDirectory(Path.Combine(OutputFolder, "missions", "missiongiver"));

			var output = new ConcurrentDictionary<string, MissionGiver>();

			var path = Path.Combine(DataRoot, Path.Join("Data", "Libs", "Foundry", "Records", "missiongiver"));

			var parser = new ClassParser<MissionGiver>();

			Parallel.ForEach(Directory.EnumerateFiles(path, "*.xml", SearchOption.AllDirectories), entityFilename =>
			{
				var giver = parser.Parse(entityFilename);
				AddGiverTranslations(giver);
				output.TryAdd(giver.__ref, giver);
				File.WriteAllText(Path.Combine(OutputFolder, "missions", "missiongiver", $"{giver.ClassName.ToLower()}.json"), JsonConvert.SerializeObject(giver));
			});

			return output;
		}

		private void AddTypeTranslations(MissionType entity)
		{
			entity.LocalisedTypeName = locService.GetText(entity.LocalisedTypeName, entity.LocalisedTypeName);
		}

		private void AddGiverTranslations(MissionGiver entity)
		{
			entity.description = locService.GetText(entity.description, entity.description);
			entity.headquarters = locService.GetText(entity.headquarters, entity.headquarters);
			entity.displayName = locService.GetText(entity.displayName, entity.displayName);
		}

		private void AddTranslations(MissionBrokerEntry entity)
		{
			entity.title = locService.GetText(entity.title, entity.title);
			entity.titleHUD = locService.GetText(entity.titleHUD, entity.titleHUD);
			entity.description = locService.GetText(entity.description, entity.description);
			entity.commsChannelName = locService.GetText(entity.commsChannelName, entity.commsChannelName);
			entity.missionGiver = locService.GetText(entity.missionGiver, entity.missionGiver);
		}
	}
}
