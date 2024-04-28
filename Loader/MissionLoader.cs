using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using scdb.Xml.Entities;
using scdb.Xml.Missionbroker;

namespace Loader
{
	public class MissionLoader
	{
		public string OutputFolder { get; set; }
		public string DataRoot { get; set; }
		public LocalisationService locService { get; set; }

		public Dictionary<string, MissionBrokerEntry> LoadMissions()
		{
			Directory.CreateDirectory(Path.Combine(OutputFolder, "missions"));

			var output = new Dictionary<string, MissionBrokerEntry>();


			var path = Path.Combine(DataRoot, Path.Join("Data", "Libs", "Foundry", "Records", "missionbroker"));

			foreach (var entityFilename in Directory.EnumerateFiles(path, "*.xml", SearchOption.AllDirectories))
			{
				var mission = Parse<MissionBrokerEntry>(entityFilename);
				AddTranslations(mission);
				output.Add(mission.__ref, mission);
				File.WriteAllText(Path.Combine(OutputFolder, "missions", $"{mission.ClassName.ToLower()}.json"), JsonConvert.SerializeObject(mission));
			}

			return output;
		}

		T Parse<T>(string xmlFilename) where T : ClassBase
		{
			string rootNodeName;
			using (var reader = XmlReader.Create(new StreamReader(xmlFilename)))
			{
				reader.MoveToContent();
				rootNodeName = reader.Name;
			}

			var split = rootNodeName.Split('.');
			string className = split[split.Length - 1];

			var xml = File.ReadAllText(xmlFilename);
			var doc = new XmlDocument();
			doc.LoadXml(xml);

			var serialiser = new XmlSerializer(typeof(T), new XmlRootAttribute { ElementName = rootNodeName });
			using (var stream = new XmlNodeReader(doc))
			{
				var entity = (T)serialiser.Deserialize(stream);
				entity.ClassName = className;
				return entity;
			}
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
