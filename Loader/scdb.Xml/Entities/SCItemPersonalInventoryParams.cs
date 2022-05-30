using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemPersonalInventoryParams
	{
		[XmlAttribute]
		public string containerParams;
		public Capacity capacity;
	}
}
