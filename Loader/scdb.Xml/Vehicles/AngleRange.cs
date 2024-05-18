using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class AngleRange
	{
		[XmlAttribute]
		public string min;

		[XmlAttribute]
		public string max;
	}
}
