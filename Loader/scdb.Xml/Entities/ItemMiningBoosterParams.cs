using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class ItemMiningBoosterParams
	{
		[XmlAttribute]
		public int powerLevelChange;

		[XmlAttribute]
		public bool showInUI;

		[XmlAttribute]
		public bool isGood;
	}
}
