using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class MaxAngularVelocity
	{
		[XmlAttribute]
		public double x;

		[XmlAttribute]
		public double y;

		[XmlAttribute]
		public double z;
	}
}
