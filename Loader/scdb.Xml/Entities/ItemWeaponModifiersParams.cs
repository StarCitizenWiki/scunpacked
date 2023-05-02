using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class ItemWeaponModifiersParams
	{
		[XmlAttribute]
		public int fireActionIndex;

		[XmlAttribute]
		public bool showInUI;

		public WeaponModifier weaponModifier;
	}
}
