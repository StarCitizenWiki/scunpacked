using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class InventoryContainer
	{
		[XmlAttribute]
		public string __ref;

		public InteriorDimensions interiorDimensions;
		public Capacity capacity;
	}
}
