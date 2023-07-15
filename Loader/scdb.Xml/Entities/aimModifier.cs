using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class aimModifier
	{
		[XmlAttribute]
		public double zoomScale;

		[XmlAttribute]
		public double zoomTimeScale;

		[XmlAttribute]
		public bool hideWeaponInADS;
	}
}
