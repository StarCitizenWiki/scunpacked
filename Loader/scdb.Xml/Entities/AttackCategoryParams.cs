using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class AttackCategoryParams
	{
		[XmlAttribute]
		public string actionCategory;

		[XmlAttribute]
		public double stunRecoveryModifier;

		[XmlAttribute]
		public double blockStunReductionModifier;

		[XmlAttribute]
		public double blockStunStaminaModifier;

		[XmlAttribute]
		public double attackImpulse;

		[XmlAttribute]
		public bool ignoreBodyPartImpulseScale;

		[XmlAttribute]
		public bool fullbodyAnimation;

		public DamageInfo damageInfo;
	}
}
