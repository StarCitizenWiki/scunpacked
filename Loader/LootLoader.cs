using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Scdb.Xml;
using scdb.Xml.Entities;
using scdb.Xml.Lootgeneration;

namespace Loader
{
	public class LootLoader
	{
		public string OutputFolder { get; set; }
		public string DataRoot { get; set; }

		public Dictionary<string, LootArchetype> LoadArchetypes()
		{
			Directory.CreateDirectory(Path.Combine(OutputFolder, "loot"));
			Directory.CreateDirectory(Path.Combine(OutputFolder, "loot", "archetypes"));

			var output = new Dictionary<string, LootArchetype>();


			var path = Path.Combine(DataRoot, Path.Join("Data", "Libs", "Foundry", "Records", "lootgeneration"));

			foreach (var entityFilename in Directory.EnumerateFiles(Path.Join(path, "lootarchetypes"), "*.xml"))
			{
				var archetype = Parse<LootArchetype>(entityFilename);
				output.Add(archetype.__ref, archetype);
				File.WriteAllText(Path.Combine(OutputFolder, "loot", "archetypes", $"{archetype.ClassName.ToLower()}.json"), JsonConvert.SerializeObject(archetype));
			}

			return output;
		}

		public Dictionary<string, LootTable> LoadTables()
		{
			Directory.CreateDirectory(Path.Combine(OutputFolder, "loot"));
			Directory.CreateDirectory(Path.Combine(OutputFolder, "loot", "tables"));

			var output = new Dictionary<string, LootTable>();

			var path = Path.Combine(DataRoot, Path.Join("Data", "Libs", "Foundry", "Records", "lootgeneration"));

			foreach (var entityFilename in Directory.EnumerateFiles(Path.Join(path, "loottables"), "*.xml", SearchOption.AllDirectories))
			{
				var lootTable = Parse<LootTable>(entityFilename);
				output.Add(lootTable.__ref, lootTable);
				File.WriteAllText(Path.Combine(OutputFolder, "loot", "tables", $"{lootTable.ClassName.ToLower()}.json"), JsonConvert.SerializeObject(lootTable));
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
