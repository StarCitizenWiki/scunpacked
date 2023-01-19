using System;
using System.Collections.Generic;
using System.Linq;

namespace Loader
{
	public class InventoryContainerService
	{
		List<InventoryContainerIndexEntry> inventories;

		public InventoryContainerService(List<InventoryContainerIndexEntry> inventories)
		{
			this.inventories = inventories;
		}

		public StandardisedInventoryContainer GetInventoryContainer(string guid)
		{
			// Try and find by the reference guid
			var found = inventories.FirstOrDefault(x => x.reference == guid);

			// If that didn't work, then give up
			if (found == null) return null;

			return new StandardisedInventoryContainer
			{
				SCU = found.scu,
				x = found.x,
				y = found.y,
				z = found.z
			};
		}
	}
}
