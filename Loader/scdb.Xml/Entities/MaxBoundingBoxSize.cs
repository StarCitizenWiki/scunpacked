using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class MaxBoundingBoxSize
	{
		[XmlAttribute]
		public double x;

		[XmlAttribute]
		public double y;

		[XmlAttribute]
		public double z;
	}
}
