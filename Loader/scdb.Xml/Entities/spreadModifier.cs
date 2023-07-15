using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class spreadModifier
	{
		[XmlAttribute]
		public double minMultiplier;

		[XmlAttribute]

		public double maxMultiplier;

		[XmlAttribute]
		public double firstAttackMultiplier;

		[XmlAttribute]
		public double attackMultiplier;

		[XmlAttribute]
		public double decayMultiplier;

		[XmlAttribute]
		public double additiveModifier;
	}
}
