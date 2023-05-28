using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class Power
	{
		[XmlAttribute] public double acceleration;
		[XmlAttribute] public double decceleration;
		[XmlAttribute] public double topSpeed;
		[XmlAttribute] public double reverseSpeed;
	}
}
