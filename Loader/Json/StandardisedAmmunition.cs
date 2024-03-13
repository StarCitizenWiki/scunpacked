namespace Loader
{
	public class StandardisedAmmunition
	{
		public string UUID { get; set; }
		public string Type { get; set; }
		public double Speed { get; set; }
		public double Range { get; set; }
		public double Size { get; set; }
		public double? Capacity { get; set; }
		public StandardisedDamage ImpactDamage { get; set; }
		public StandardisedDamage DetonationDamage { get; set; }
		public StandardisedBulletImpulseFalloff BulletImpulseFalloff { get; set; }
		public StandardisedBulletPierceability BulletPierceability { get; set; }
		public StandardisedBulletElectron BulletElectron { get; set; }

		public StandardisedDamage DamageDropMinDistance { get; set; }
		public StandardisedDamage DamageDropPerMeter { get; set; }
		public StandardisedDamage DamageDropMinDamage { get; set; }
	}
}
