using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SWeaponZeroingParams
	{
		[XmlAttribute]
		public double defaultRange;

		[XmlAttribute]
		public double maxRange;

		[XmlAttribute]
		public double rangeIncrement;

		[XmlAttribute]
		public double autoZeroingTime;
	}
}
