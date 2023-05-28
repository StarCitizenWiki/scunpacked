using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class Correction
	{
		[XmlAttribute] public double lateralSpring;
		[XmlAttribute] public double angSpring;
	}
}
