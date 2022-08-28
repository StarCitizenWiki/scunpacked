using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemConsumableParams
	{
		[XmlAttribute]
		public string containerTypeTag;
		[XmlAttribute]
		public bool oneShotConsume;
		[XmlAttribute]
		public bool canBeReclosed;
		[XmlAttribute]
		public bool discardWhenConsumed;

		public consumableVolume consumableVolume;
	}
}
