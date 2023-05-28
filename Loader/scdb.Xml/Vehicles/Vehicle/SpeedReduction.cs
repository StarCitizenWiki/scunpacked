using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class SpeedReduction
	{
		[XmlAttribute] public double reductionAmount;
		[XmlAttribute] public double reductionRate;
	}
}
