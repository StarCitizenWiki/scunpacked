using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Loader.scdb.Xml.Factions;
using Newtonsoft.Json;
using scdb.Xml.Entities;

namespace Loader
{
	public class FactionLoader
	{
		public string OutputFolder { get; set; }
		public string DataRoot { get; set; }

		public Dictionary<string, Faction> LoadFactions()
		{
			Directory.CreateDirectory(Path.Combine(OutputFolder, "factions"));

			var output = new Dictionary<string, Faction>();


			var path = Path.Combine(DataRoot, Path.Join("Data", "Libs", "Foundry", "Records", "factions"));

			foreach (var entityFilename in Directory.EnumerateFiles(path, "*.xml"))
			{
				var faction = Parse<Faction>(entityFilename);
				output.Add(faction.__ref, faction);
				File.WriteAllText(Path.Combine(OutputFolder, "factions", $"{faction.ClassName.ToLower()}.json"), JsonConvert.SerializeObject(faction));
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
	}
}
