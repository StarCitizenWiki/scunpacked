using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class InventoryContainer: ClassBase
	{
		public InteriorDimensions interiorDimensions;
		public InventoryType inventoryType;
	}
}
