using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SHealthComponentParams
	{
		[XmlAttribute] public double Health;

		[XmlAttribute] public bool DetachFromItemPortOnDeath;
		[XmlAttribute] public bool DetachFromEntityOnDeath;
		[XmlAttribute] public bool PropagateExplosionDamageToChildren;
		[XmlAttribute] public bool IsSalvagable;
		[XmlAttribute] public bool IsRepairable;

		public DamageResistances DamageResistances;
	}
}
