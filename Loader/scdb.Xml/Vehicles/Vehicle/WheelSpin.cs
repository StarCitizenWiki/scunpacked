using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class WheelSpin
	{
		[XmlAttribute] public double grip1;
		[XmlAttribute] public double grip2;
		[XmlAttribute] public double gripRecoverSpeed;
		[XmlAttribute] public double accelMultiplier1;
		[XmlAttribute] public double accelMultiplier2;
	}
}
