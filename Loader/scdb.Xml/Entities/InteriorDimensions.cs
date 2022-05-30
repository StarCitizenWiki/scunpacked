using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class InteriorDimensions
	{
		[XmlAttribute]
		public double x;

		[XmlAttribute]
		public double y;

		[XmlAttribute]
		public double z;
	}
}
