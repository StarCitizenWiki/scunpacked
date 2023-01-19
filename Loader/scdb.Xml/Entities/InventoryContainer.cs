using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class InventoryContainer
	{
		[XmlAttribute]
		public string __ref;

		public InteriorDimensions interiorDimensions;
		public InventoryType inventoryType;
	}
}
