using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class Compression
	{
		[XmlAttribute] public double frictionBoost;
		[XmlAttribute] public double frictionBoostHandBrake;
	}
}
