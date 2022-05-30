using System;
using System.Collections.Generic;
using System.Linq;

namespace Loader
{
	public class PersonalInventoryService
	{
		List<PersonalInventoryIndexEntry> inventories;

		public PersonalInventoryService(List<PersonalInventoryIndexEntry> inventories)
		{
			this.inventories = inventories;
		}

		public StandardisedPersonalInventory GetPersonalInventory(string guid)
		{
			// Try and find by the reference guid
			var found = inventories.FirstOrDefault(x => x.reference == guid);

			// If that didn't work, then give up
			if (found == null) return null;

			return new StandardisedPersonalInventory
			{
				SCU = found.scu,
				x = found.x,
				y = found.y,
				z = found.z
			};
		}
	}
}
