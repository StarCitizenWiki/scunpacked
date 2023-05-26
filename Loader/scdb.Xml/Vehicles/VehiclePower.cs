using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class VehiclePower 
    {
    	[XmlAttribute]
		public double acceleration;

		[XmlAttribute]
		public double decceleration;

		[XmlAttribute]
		public double topSpeed;

		[XmlAttribute]
		public double reverseSpeed;
	}
}
