using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class HandBrake
	{
		[XmlAttribute] public double deccelerationPowerLock;
		[XmlAttribute] public double decceleration;
		[XmlAttribute] public double lockFront;
		[XmlAttribute] public double lockBack;
		[XmlAttribute] public double frontFrictionScale;
		[XmlAttribute] public double backFrictionScale;
		[XmlAttribute] public double angCorrectionScale;
	}
}
