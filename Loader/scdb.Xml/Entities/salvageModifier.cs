using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class salvageModifier
	{
		[XmlAttribute]
		public double salvageSpeedMultiplier;

		[XmlAttribute]
		public double radiusMultiplier;

		[XmlAttribute]
		public double extractionEfficiency;
	}
}
