using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class Stabilisation
	{
		[XmlAttribute] public double upDamping;
		[XmlAttribute] public double angDamping;
		[XmlAttribute] public double rollDamping;
		[XmlAttribute] public double rollfixAir;
		[XmlAttribute] public double maxTiltAngleAir;
	}
}
