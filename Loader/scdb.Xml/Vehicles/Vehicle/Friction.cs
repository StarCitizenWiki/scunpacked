using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class Friction
	{
		[XmlAttribute] public double back;
		[XmlAttribute] public double front;
		[XmlAttribute] public double offset;
		[XmlAttribute] public double offsetBraking;
		[XmlAttribute] public double offsetAccel;
	}
}
