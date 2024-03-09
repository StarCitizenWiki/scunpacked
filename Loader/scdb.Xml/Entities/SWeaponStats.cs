using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SWeaponStats
	{
		[XmlAttribute]
		public double fireRate;

		[XmlAttribute]
		public double fireRateMultiplier;

		[XmlAttribute]
		public double damageMultiplier;

		[XmlAttribute]
		public double damageOverTimeMultiplier;

		[XmlAttribute]
		public double projectileSpeedMultiplier;

		[XmlAttribute]
		public double pellets;

		[XmlAttribute]
		public double burstShots;

		[XmlAttribute]
		public double ammoCost;

		[XmlAttribute]
		public double ammoCostMultiplier;

		[XmlAttribute]
		public double heatGenerationMultiplier;

		[XmlAttribute]
		public double soundRadiusMultiplier;

		[XmlAttribute]
		public double chargeTimeMultiplier;

		[XmlAttribute]
		public bool useAlternateProjectileVisuals;

		public recoilModifier recoilModifier;

		public spreadModifier spreadModifier;

		public aimModifier aimModifier;

		public salvageModifier salvageModifier;
	}
}
