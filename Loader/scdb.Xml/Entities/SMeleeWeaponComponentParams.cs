using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SMeleeWeaponComponentParams
	{
		[XmlAttribute] public bool canBeUsedForTakeDown;
		[XmlAttribute] public bool canBlock;
		[XmlAttribute] public bool canBeUsedInProne;
		[XmlAttribute] public bool canDodge;
		[XmlAttribute] public string meleeCombatConfig;
	}
}
