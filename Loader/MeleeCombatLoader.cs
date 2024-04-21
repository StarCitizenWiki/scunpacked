using System;
using System.Collections.Generic;
using System.IO;

using scdb.Xml.Entities;

namespace Loader
{
	public class MeleeCombatLoader
	{
		public string DataRoot { get; set; }
		public string OutputFolder { get; set; }

		bool verbose;

		public List<MeleeCombatConfig> Load()
		{
			var index = new List<MeleeCombatConfig>();
			index.AddRange(Load(Path.Join("Data", "Libs", "Foundry", "Records", "entities", "scitem", "weapons", "melee")));

			return index;
		}

		List<MeleeCombatConfig> Load(string entityFolder)
		{
			var index = new List<MeleeCombatConfig>();
			var parser = new ClassParser<MeleeCombatConfig>();

			foreach (var filename in Directory.EnumerateFiles(Path.Combine(DataRoot, entityFolder), "*.xml", SearchOption.AllDirectories))
			{
				if (verbose) Console.WriteLine(filename);

				var combatConfig = parser.Parse(filename);
				if (combatConfig == null) continue;

				index.Add(combatConfig);
			}

			return index;
		}
	}
}
