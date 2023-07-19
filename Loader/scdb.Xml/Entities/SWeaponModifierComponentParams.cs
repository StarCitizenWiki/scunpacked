using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SWeaponModifierComponentParams
	{
		[XmlAttribute]
		public string tag;

		[XmlAttribute]
		public string uiIndex;

		[XmlAttribute]
		public string aimHelperYOffset;

		[XmlAttribute]
		public string barrelEffectsStrength;

		[XmlAttribute]
		public string activateOnAttach;

		public modifier modifier;
		public zeroingParams zeroingParams;
	}
}
