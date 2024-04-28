using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Loader.scdb.Xml.Reputation;
using Newtonsoft.Json;
using scdb.Xml.Entities;

namespace Loader
{
	public class RewardLoader
	{
		public string OutputFolder { get; set; }
		public string DataRoot { get; set; }
		public LocalisationService locService { get; set; }

		public Dictionary<string, Scope> LoadScopes()
		{
			Directory.CreateDirectory(Path.Combine(OutputFolder, "reputation", "scopes"));

			var output = new Dictionary<string, Scope>();


			var path = Path.Combine(DataRoot, Path.Join("Data", "Libs", "Foundry", "Records", "reputation", "scopes"));

			foreach (var entityFilename in Directory.EnumerateFiles(path, "*.xml", SearchOption.AllDirectories))
			{
				var scope = Parse<Scope>(entityFilename);
				AddScopeTranslations(scope);
				output.Add(scope.__ref, scope);
				File.WriteAllText(Path.Combine(OutputFolder, "reputation", "scopes", $"{scope.ClassName.ToLower()}.json"), JsonConvert.SerializeObject(scope));
			}

			return output;
		}

		public Dictionary<string, Standing> LoadStandings()
		{
			Directory.CreateDirectory(Path.Combine(OutputFolder, "reputation", "standings"));

			var output = new Dictionary<string, Standing>();


			var path = Path.Combine(DataRoot, Path.Join("Data", "Libs", "Foundry", "Records", "reputation", "standings"));

			foreach (var entityFilename in Directory.EnumerateFiles(path, "*.xml", SearchOption.AllDirectories))
			{
				var standing = Parse<Standing>(entityFilename);
				AddStandingTranslations(standing);
				output.Add(standing.__ref, standing);
				File.WriteAllText(Path.Combine(OutputFolder, "reputation", "standings", $"{standing.ClassName.ToLower()}.json"), JsonConvert.SerializeObject(standing));
			}

			return output;
		}

		public void LoadRewards()
		{
			Directory.CreateDirectory(Path.Combine(OutputFolder, "reputation", "rewards"));

			var path = Path.Combine(DataRoot, Path.Join("Data", "Libs", "Foundry", "Records", "reputation", "rewards"));

			foreach (var entityFilename in Directory.EnumerateFiles(Path.Join(path, "missionrewards_reputation"), "*.xml", SearchOption.AllDirectories))
			{
				var reward = Parse<SReputationRewardAmount>(entityFilename);
				File.WriteAllText(Path.Combine(OutputFolder, "reputation", "rewards", $"{reward.ClassName.ToLower()}.json"), JsonConvert.SerializeObject(reward));
			}

			foreach (var entityFilename in Directory.EnumerateFiles(Path.Join(path, "missionrewards_bonusuec"), "*.xml", SearchOption.AllDirectories))
			{
				var reward = Parse<SReputationMissionRewardBonusParams>(entityFilename);
				File.WriteAllText(Path.Combine(OutputFolder, "reputation", "rewards", $"{reward.ClassName.ToLower()}.json"), JsonConvert.SerializeObject(reward));
			}
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

		private void AddScopeTranslations(Scope entity)
		{
			entity.displayName = locService.GetText(entity.displayName, entity.displayName);
			entity.description = locService.GetText(entity.description, entity.description);
		}

		private void AddStandingTranslations(Standing entity)
		{
			entity.displayName = locService.GetText(entity.displayName, entity.displayName);
			entity.perkDescription = locService.GetText(entity.perkDescription, entity.perkDescription);
		}
	}
}
