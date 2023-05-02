using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemClothingParams
	{
		public TemperatureResistance TemperatureResistance;
	}

	public class TemperatureResistance
	{
		[XmlAttribute]
		public double MinResistance;

		[XmlAttribute]
		public double MaxResistance;
	}
}
