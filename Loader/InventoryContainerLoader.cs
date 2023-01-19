using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

namespace Loader
{
	public class InventoryContainerLoader
	{
		public string DataRoot { get; set; }

		bool verbose = false;

		public List<InventoryContainerIndexEntry> Load()
		{
			var index = new List<InventoryContainerIndexEntry>();
			index.AddRange(Load(@"Data\Libs\Foundry\Records\inventorycontainers"));

			return index;
		}

		List<InventoryContainerIndexEntry> Load(string entityFolder)
		{
			var index = new List<InventoryContainerIndexEntry>();
			var parser = new InventoryContainerParser();

			foreach (var entityFilename in Directory.EnumerateFiles(Path.Combine(DataRoot, entityFolder), "*.xml", SearchOption.AllDirectories))
			{
				if (verbose) Console.WriteLine(entityFilename);

				var inventoryContainer = parser.Parse(entityFilename);
				if (inventoryContainer == null) continue;

				double scu = 0;
				var capacity = inventoryContainer.inventoryType?.InventoryClosedContainerType?.capacity;

				if (capacity?.SStandardCargoUnit?.standardCargoUnits != null) {
					scu  = capacity.SStandardCargoUnit.standardCargoUnits;
				} else if (capacity?.SCentiCargoUnit?.centiSCU != null) {
					scu = capacity.SCentiCargoUnit.centiSCU * Math.Pow(10, -2);
				} else if (capacity?.SMicroCargoUnit?.microSCU != null) {
					scu = capacity.SMicroCargoUnit.microSCU * Math.Pow(10, -6);
				}

				var indexEntry = new InventoryContainerIndexEntry
				{
					x = inventoryContainer?.interiorDimensions?.x ?? 0,
					y = inventoryContainer?.interiorDimensions?.y ?? 0,
					z = inventoryContainer?.interiorDimensions?.z ?? 0,
					scu = scu,
					reference = inventoryContainer?.__ref
				};

				if (inventoryContainer?.__ref != null) {
					index.Add(indexEntry);
				}
			}

			return index;
		}
	}
}
