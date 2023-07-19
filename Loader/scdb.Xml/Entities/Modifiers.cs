using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class Modifiers
	{
		[XmlElement("ItemWeaponModifiersParams")]
		public ItemWeaponModifiersParams[] ItemWeaponModifiersParams;

		public ItemMiningModifierParams ItemMiningModifierParams;
		public ItemMiningBoosterParams ItemMiningBoosterParams;

	}
}
