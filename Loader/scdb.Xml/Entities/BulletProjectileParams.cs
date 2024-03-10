using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class BulletProjectileParams
	{
		[XmlAttribute] public double impactRadius;
		[XmlAttribute] public double minImpactRadius;
		[XmlAttribute] public double ignitionChanceOverride;
		[XmlAttribute] public double keepAliveOnZeroDamage;
		[XmlAttribute] public string hitType;

		public DamageInfo[] damage;
		public DamageDropParams damageDropParams;
		public DetonationParams detonationParams;
		public ImpulseFalloffParams impulseFalloffParams;
		public PierceabilityParams pierceabilityParams;
		public ElectronParams electronParams;
	}

	public class DetonationParams
	{
		public ProjectileDetonationParams ProjectileDetonationParams;
	}

	public class ImpulseFalloffParams
	{
		public BulletImpulseFalloffParams BulletImpulseFalloffParams;
	}

	public class ElectronParams
	{
		public BulletElectronParams BulletElectronParams;
	}

	public class DamageDropParams
	{
		public BulletDamageDropParams BulletDamageDropParams;
	}

	public class BulletDamageDropParams
	{
		public DamageDropMinDistance damageDropMinDistance;
		public DamageDropPerMeter damageDropPerMeter;
		public DamageDropMinDamage damageDropMinDamage;
	}

	public class DamageDropMinDistance
	{
		public DamageInfo DamageInfo;
	}

	public class DamageDropPerMeter
	{
		public DamageInfo DamageInfo;
	}

	public class DamageDropMinDamage
	{
		public DamageInfo DamageInfo;
	}

	public class ProjectileDetonationParams
	{
		[XmlAttribute]
		double armTime;

		[XmlAttribute]
		double safeDistance;

		[XmlAttribute]
		double destructDelay;

		[XmlAttribute]
		bool explodeOnImpact;

		[XmlAttribute]
		bool explodeOnFinalImpact;

		[XmlAttribute]
		bool explodeOnExpire;

		[XmlAttribute]
		bool explodeOnTargetRange;

		public ExplosionParams explosionParams;
	}

	public class BulletImpulseFalloffParams
	{
		[XmlAttribute]
		public double minDistance;

		[XmlAttribute]
		public double dropFalloff;

		[XmlAttribute]
		public double maxFalloff;
	}

	public class PierceabilityParams
	{
		[XmlAttribute]
		public double damageFalloffLevel1;

		[XmlAttribute]
		public double damageFalloffLevel2;

		[XmlAttribute]
		public double damageFalloffLevel3;

		[XmlAttribute]
		public double maxPenetrationThickness;
	}

	public class BulletElectronParams
	{
		[XmlAttribute]
		public double residualChargeMultiplier;

		[XmlAttribute]
		public double maximumJumps;

		[XmlAttribute]
		public double jumpRange;
	}
}
