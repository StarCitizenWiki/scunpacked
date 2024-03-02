using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SAmmoContainerComponentParams
	{
		[XmlAttribute]
		public double initialAmmoCount;

		[XmlAttribute]
		public double maxAmmoCount;

		[XmlAttribute]
		public double maxRestockCount;

		[XmlAttribute]
		public string ammoParamsRecord;
	}
}
