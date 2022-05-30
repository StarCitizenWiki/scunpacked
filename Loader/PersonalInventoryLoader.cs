using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

namespace Loader
{
	public class PersonalInventoryLoader
	{
		public string DataRoot { get; set; }

		bool verbose = false;

		public List<PersonalInventoryIndexEntry> Load()
		{
			var index = new List<PersonalInventoryIndexEntry>();
			index.AddRange(Load(@"Data\Libs\Foundry\Records\inventorycontainers"));

			return index;
		}

		List<PersonalInventoryIndexEntry> Load(string entityFolder)
		{
			var index = new List<PersonalInventoryIndexEntry>();
			var parser = new PersonalInventoryParser();

			foreach (var entityFilename in Directory.EnumerateFiles(Path.Combine(DataRoot, entityFolder), "*.xml", SearchOption.AllDirectories))
			{
				if (verbose) Console.WriteLine(entityFilename);

				var personalinventory = parser.Parse(entityFilename);
				if (personalinventory == null) continue;

				double scu = 0;

				if (personalinventory.capacity?.SStandardCargoUnit?.standardCargoUnits != null) {
					scu  = personalinventory.capacity.SStandardCargoUnit.standardCargoUnits;
				} else if (personalinventory.capacity?.SCentiCargoUnit?.centiSCU != null) {
					scu = personalinventory.capacity.SCentiCargoUnit.centiSCU * Math.Pow(10, -2);
				} else if (personalinventory.capacity?.SMicroCargoUnit?.microSCU != null) {
					scu = personalinventory.capacity.SMicroCargoUnit.microSCU * Math.Pow(10, -6);
				}

				var indexEntry = new PersonalInventoryIndexEntry
				{
					x = personalinventory?.interiorDimensions?.x ?? 0,
					y = personalinventory?.interiorDimensions?.y ?? 0,
					z = personalinventory?.interiorDimensions?.z ?? 0,
					scu = scu,
					reference = personalinventory?.__ref ?? null
				};

				if (personalinventory?.__ref != null) {
					index.Add(indexEntry);
				}
			}

			return index;
		}
	}
}
