using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;
using scdb.Xml.Entities;

namespace Loader
{
	public class InventoryContainerLoader
	{
		public string DataRoot { get; set; }

		bool verbose = false;

		public List<InventoryContainerIndexEntry> Load()
		{
			var index = new List<InventoryContainerIndexEntry>();
			index.AddRange(Load(Path.Join("Data", "Libs", "Foundry", "Records", "inventorycontainers")));

			return index;
		}

		List<InventoryContainerIndexEntry> Load(string entityFolder)
		{
			var index = new List<InventoryContainerIndexEntry>();
			var parser = new ClassParser<InventoryContainer>();

			foreach (var entityFilename in Directory.EnumerateFiles(Path.Combine(DataRoot, entityFolder), "*.xml", SearchOption.AllDirectories))
			{
				if (verbose) Console.WriteLine(entityFilename);

				var inventoryContainer = parser.Parse(entityFilename);
				if (inventoryContainer == null) continue;

				double scu = 0;
				var capacity = inventoryContainer.inventoryType?.InventoryClosedContainerType?.capacity;

				double unit = 0;
				if (capacity?.SStandardCargoUnit?.standardCargoUnits != null)
				{
					unit = 0;
					scu  = capacity.SStandardCargoUnit.standardCargoUnits;
				} else if (capacity?.SCentiCargoUnit?.centiSCU != null)
				{
					unit = 2;
					scu = capacity.SCentiCargoUnit.centiSCU * Math.Pow(10, -unit);
				} else if (capacity?.SMicroCargoUnit?.microSCU != null)
				{
					unit = 6;
					scu = capacity.SMicroCargoUnit.microSCU * Math.Pow(10, -unit);
				}

				var indexEntry = new InventoryContainerIndexEntry
				{
					x = inventoryContainer?.interiorDimensions?.x ?? 0,
					y = inventoryContainer?.interiorDimensions?.y ?? 0,
					z = inventoryContainer?.interiorDimensions?.z ?? 0,
					scu = scu,
					unit = unit,
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
