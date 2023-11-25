using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemWeaponComponentParams
	{
		[XmlAttribute]
		public string ammoContainerRecord;

		public SWeaponConnectionParams connectionParams;

		[XmlArray]
		[XmlArrayItem(typeof(SWeaponActionSequenceParams))]
		[XmlArrayItem(typeof(SWeaponActionFireSingleParams))]
		[XmlArrayItem(typeof(SWeaponActionFireRapidParams))]
		[XmlArrayItem(typeof(SWeaponActionFireBeamParams))]
		[XmlArrayItem(typeof(SWeaponActionFireChargedParams))]
		[XmlArrayItem(typeof(SWeaponActionFireBurstParams))]
		[XmlArrayItem(typeof(SWeaponActionFireHealingBeamParams))]
		[XmlArrayItem(typeof(SWeaponActionFireSalvageRepairParams))]
		[XmlArrayItem(typeof(SWeaponActionGatheringBeamParams))]
		[XmlArrayItem(typeof(SWeaponActionFireTractorBeamParams))]
		public SWeaponActionParams[] fireActions;
		public SWeaponRegenConsumerParams[] weaponRegenConsumerParams;
	}

	public class SWeaponActionParams
	{
		[XmlAttribute]
		public string name;

		[XmlAttribute]
		public string localisedName;

		[XmlAttribute]
		public string aiShootingMode;
	}

	public class SWeaponActionFireSingleParams : SWeaponActionParams
	{
		[XmlAttribute]
		public double heatPerShot;

		[XmlAttribute]
		public double fireRate;

		public launchParams launchParams;
	}

	public class SWeaponActionFireRapidParams : SWeaponActionParams
	{
		[XmlAttribute]
		public double spinUpTime;

		[XmlAttribute]
		public double spinDownTime;

		[XmlAttribute]
		public string spinParam;

		[XmlAttribute]
		public double heatPerShot;

		[XmlAttribute]
		public double fireRate;

		public launchParams launchParams;
	}

	public class SWeaponActionFireBurstParams : SWeaponActionParams
	{
		[XmlAttribute]
		public double shotCount;

		[XmlAttribute]
		public double heatPerShot;

		[XmlAttribute]
		public double fireRate;

		public launchParams launchParams;
	}

	public class SWeaponActionFireBeamParams : SWeaponActionParams
	{
		[XmlAttribute]
		public string fireHelper;

		[XmlAttribute]
		public double toggle;

		[XmlAttribute]
		public double energyRate;

		[XmlAttribute]
		public double fullDamageRange;

		[XmlAttribute]
		public double zeroDamageRange;

		[XmlAttribute]
		public double heatPerSecond;

		[XmlAttribute]
		public string hitType;

		[XmlAttribute]
		public double hitRadius;

		public DamagePerSecond damagePerSecond;
	}

	public class SWeaponActionFireChargedParams : SWeaponActionParams
	{
		[XmlAttribute]
		public double chargeTime;

		[XmlAttribute]
		public double overchargeTime;

		[XmlAttribute]
		public double overchargedTime;

		[XmlAttribute]
		public double cooldownTime;

		public weaponAction weaponAction;
	}

	public class SWeaponActionFireHealingBeamParams : SWeaponActionParams
	{
		[XmlAttribute]
		public string healingMode;

		[XmlAttribute]
		public double maxDistance;

		[XmlAttribute]
		public double maxSensorDistance;

		[XmlAttribute]
		public string medicalAmmoType;

		[XmlAttribute]
		public double mSCUPerSec;

		[XmlAttribute]
		public double ammoPerMSCU;

		[XmlAttribute]
		public double wearPerSec;

		[XmlAttribute]
		public string batteryAmmoType;

		[XmlAttribute]
		public double batteryDrainPerSec;

		[XmlAttribute]
		public double autoDosageTargetBDLModifier;

		[XmlAttribute]
		public double healingBreakTime;

		[XmlAttribute]
		public double maxDoseForAutoAdjustment;
	}


	public class SWeaponActionFireSalvageRepairParams : SWeaponActionParams
	{
		[XmlAttribute]
		public string salvageRepairMode;

		[XmlAttribute]
		public bool toggle;

		[XmlAttribute]
		public bool salvageCanFireOnFull;

		[XmlAttribute]
		public string salvageRepairAmmoType;

		[XmlAttribute]
		public string batteryAmmoType;

		[XmlAttribute]
		public double minEnergyDraw;

		[XmlAttribute]
		public double maxEnergyDraw;

		[XmlAttribute]
		public double materialEfficiency;

		[XmlAttribute]
		public double maxVehicleDamageRatio;

		[XmlAttribute]
		public double repairedMaterialRatio;

		[XmlAttribute]
		public double maxHealthRepairRate;

		[XmlAttribute]
		public double healthToAmmoRatio;

		[XmlAttribute]
		public double heatPerSecond;

		[XmlAttribute]
		public double wearPerSecond;

		[XmlAttribute]
		public double hitRadius;

		[XmlAttribute]
		public double materialGatheringGracePeriod;

		[XmlAttribute]
		public double aimPointThicknessDamageThreshold;

		[XmlAttribute]
		public double rampUpTime;

		[XmlAttribute]
		public double rampDownTime;

		[XmlAttribute]
		public double damageThreshold;
	}


	public class SWeaponActionGatheringBeamParams : SWeaponActionParams
	{
		[XmlAttribute]
		public string inventoryType;

		[XmlAttribute]
		public double minimumDistance;

		[XmlAttribute]
		public double maximumDistance;

		[XmlAttribute]
		public double beamRadius;

		[XmlAttribute]
		public double collectionRate;
	}


	public class SWeaponActionFireTractorBeamParams : SWeaponActionParams
	{
		[XmlAttribute]
		public bool toggle;

		[XmlAttribute]
		public double minForce;

		[XmlAttribute]
		public double maxForce;

		[XmlAttribute]
		public double additionalForceDuringZeroGHandholding;

		[XmlAttribute]
		public double minDistance;

		[XmlAttribute]
		public double maxDistance;

		[XmlAttribute]
		public double fullStrengthDistance;

		[XmlAttribute]
		public double maxAngle;

		[XmlAttribute]
		public double maxVolume;

		[XmlAttribute]
		public double volumeForceCoefficient;

		[XmlAttribute]
		public double tetherBreakTime;

		[XmlAttribute]
		public double safeRangeValueFactor;

		[XmlAttribute]
		public double maxPlayerLookRotationScale;

		[XmlAttribute]
		public bool allowScrollingIntoBreakingRange;

		[XmlAttribute]
		public bool shouldDryFireInGreenZones;

		[XmlAttribute]
		public bool shouldFireInHangars;

		[XmlAttribute]
		public bool shouldTractorSelf;

		[XmlAttribute]
		public double heatPerSecond;

		[XmlAttribute]
		public double wearPerSecond;

		[XmlAttribute]
		public string ammoType;

		[XmlAttribute]
		public double minEnergyDraw;

		[XmlAttribute]
		public double maxEnergyDraw;
	}

	public enum SWeaponActionSequenceMode
	{
		Looping,
		Automatically,
		Individually
	}

	public class SWeaponActionSequenceParams : SWeaponActionParams
	{
		[XmlAttribute]
		public SWeaponActionSequenceMode mode;

		public SWeaponSequenceEntryParams[] sequenceEntries;
	}

	public class SWeaponSequenceEntryParams
	{
		[XmlAttribute]
		public double delay;

		[XmlAttribute]
		public string unit;

		public weaponAction weaponAction;
	}

	public class weaponAction
	{
		public SWeaponActionFireSingleParams SWeaponActionFireSingleParams;
		public SWeaponActionFireRapidParams SWeaponActionFireRapidParams;
		public SWeaponActionFireBeamParams SWeaponActionFireBeamParams;
		public SWeaponActionFireChargedParams SWeaponActionFireChargedParams;
		public SWeaponActionFireBurstParams SWeaponActionFireBurstParams;
	}

	public class launchParams
	{
		public SProjectileLauncher SProjectileLauncher;
		public SMissileLauncher SMissileLauncher;
	}

	public class SProjectileLauncher
	{
		[XmlAttribute]
		public double ammoCost;

		[XmlAttribute]
		public double pelletCount;

		public SSpreadParams spreadParams;
	}

	public class SMissileLauncher
	{
		[XmlAttribute]
		public double detachVelocityRight;

		[XmlAttribute]
		public double detachVelocityForward;

		[XmlAttribute]
		public double detachVelocityUp;

		[XmlAttribute]
		public double detachVelocityRoll;

		[XmlAttribute]
		public double canFireWithoutLock;

		[XmlAttribute]
		public double showHitIndicator;

		[XmlAttribute]
		public bool igniteOnPylon;
	}

	public class SSpreadParams
	{
		[XmlAttribute]
		public double min;

		[XmlAttribute]
		public double max;

		[XmlAttribute]
		public double firstAttack;

		[XmlAttribute]
		public double attack;

		[XmlAttribute]
		public double decay;
	}

	public class DamagePerSecond
	{
		public DamageInfo DamageInfo;
	}

	public class SWeaponRegenConsumerParams
	{
		[XmlAttribute]
		public double requestedRegenPerSec { get; set; }

		[XmlAttribute]
		public double regenerationCooldown { get; set; }

		[XmlAttribute]
		public double regenerationCostPerBullet { get; set; }

		[XmlAttribute]
		public double requestedAmmoLoad { get; set; }
	}
}
