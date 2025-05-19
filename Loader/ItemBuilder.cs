using System;
using System.Collections.Generic;
using System.Linq;

using scdb.Xml.Entities;

namespace Loader
{
	public class ItemBuilder
	{
		LocalisationService localisationSvc;
		ManufacturerService manufacturerSvc;
		AmmoService ammoSvc;
		EntityService entitySvc;
		InventoryContainerService _inventoryContainerSvc;

		public ItemBuilder(LocalisationService localisationSvc, ManufacturerService manufacturerSvc, AmmoService ammoSvc, EntityService entitySvc, InventoryContainerService inventoryContainerSvc)
		{
			this.localisationSvc = localisationSvc;
			this.manufacturerSvc = manufacturerSvc;
			this.ammoSvc = ammoSvc;
			this.entitySvc = entitySvc;
			this._inventoryContainerSvc = inventoryContainerSvc;
		}

		public StandardisedItem BuildItem(EntityClassDefinition entity)
		{
			if (entity == null) return null;

			var volume =
				ConvertToSCU(entity.Components?.SAttachableComponentParams?.AttachDef?.inventoryOccupancyVolume) ?? 0;

			var stdItem = new StandardisedItem
			{
				UUID = entity.__ref,
				ClassName = entity.ClassName,
				Size = entity.Components?.SAttachableComponentParams?.AttachDef?.Size ?? 0,
				Grade = entity.Components?.SAttachableComponentParams?.AttachDef?.Grade ?? 0,
				Width = entity.Components?.SAttachableComponentParams?.AttachDef?.inventoryOccupancyDimensions?.x ?? 0,
				Length = entity.Components?.SAttachableComponentParams?.AttachDef?.inventoryOccupancyDimensions?.y ?? 0,
				Height = entity.Components?.SAttachableComponentParams?.AttachDef?.inventoryOccupancyDimensions?.z ?? 0,
				Volume = volume,
				Type = BuildTypeName(entity.Components?.SAttachableComponentParams?.AttachDef?.Type, entity.Components?.SAttachableComponentParams?.AttachDef?.SubType),
				Name = localisationSvc.GetText(entity.Components?.SAttachableComponentParams?.AttachDef?.Localization?.Name, entity.ClassName),
				Description = localisationSvc.GetText(entity.Components?.SAttachableComponentParams?.AttachDef?.Localization?.Description),
				Manufacturer = manufacturerSvc.GetManufacturer(entity.Components?.SAttachableComponentParams?.AttachDef?.Manufacturer, entity.ClassName),
				Ports = BuildPortList(entity),
				Tags = BuildTagList(entity)
			};

			stdItem.Shield = BuildShieldInfo(entity);
			stdItem.QuantumDrive = BuildQuantumDriveInfo(entity);
			stdItem.PowerPlant = BuildPowerPlantInfo(entity);
			stdItem.Cooler = BuildCoolerInfo(entity);
			stdItem.Durability = BuildDurabilityInfo(entity);
			stdItem.Thruster = BuildThrusterInfo(entity);
			stdItem.CargoGrid = BuildCargoGridInfo(entity);
			stdItem.QuantumFuelTank = BuildQuantumFuelTankInfo(entity);
			stdItem.HydrogenFuelTank = BuildHydrogenFuelTankInfo(entity);
			stdItem.HydrogenFuelIntake = BuildHydrogenFuelIntakeInfo(entity);
			stdItem.Armour = BuildArmourInfo(entity);
			stdItem.Emp = BuildEmpInfo(entity);
			stdItem.MissileRack = BuildMissileRackInfo(entity);
			stdItem.QuantumInterdiction = BuildQigInfo(entity);
			stdItem.Ifcs = BuildIfcsInfo(entity);
			stdItem.HeatConnection = BuildHeatConnectionInfo(entity);
			stdItem.PowerConnection = BuildPowerConnectionInfo(entity);
			stdItem.Weapon = BuildWeaponInfo(entity);
			stdItem.Ammunition = BuildAmmunitionInfo(entity);
			stdItem.Missile = BuildMissileInfo(entity);
			stdItem.Bomb = BuildBombInfo(entity);
			stdItem.Scanner = BuildScannerInfo(entity);
			stdItem.Radar = BuildRadarInfo(entity);
			stdItem.Ping = BuildPingInfo(entity);
			stdItem.WeaponRegenPool = BuildWeaponRegenInfo(entity);
			stdItem.InventoryContainer = BuildPersonalInventoryInfo(entity);
			stdItem.DimensionOverrides = BuildUiDimensionOverrides(entity);
			stdItem.TemperatureResistance = BuildTemperatureResistance(entity);
			stdItem.Food = BuildFoodInfo(entity);

			return stdItem;
		}

		public string BuildTypeName(string major, string minor)
		{
			if (String.IsNullOrEmpty(major)) return "UNKNOWN";
			if (String.IsNullOrWhiteSpace(minor) || minor == "UNKNOWN") return major;
			else return $"{major}.{minor}";
		}

		List<string> BuildTagList(EntityClassDefinition entity)
		{
			var tags = new List<string>();

			if (entity?.Components?.SAttachableComponentParams?.AttachDef?.Tags == null) return tags;

			var tagString = entity.Components?.SAttachableComponentParams?.AttachDef?.Tags;
			if (tagString != null)
			{
				foreach (var tag in tagString.Split(" "))
				{
					if (!String.IsNullOrEmpty(tag)) tags.Add(tag);
				}
			}
			return tags;
		}

		List<StandardisedItemPort> BuildPortList(EntityClassDefinition entity)
		{
			var ports = new List<StandardisedItemPort>();

			if (entity?.Components?.SItemPortContainerComponentParams == null) return ports;

			foreach (var port in entity.Components?.SItemPortContainerComponentParams.Ports)
			{
				var stdPort = new StandardisedItemPort
				{
					PortName = port.Name,
					Size = port.MaxSize,
					Types = BuildPortTypes(port),
					Flags = BuildPortFlags(port)
				};

				stdPort.Uneditable = stdPort.Flags.Contains("$uneditable") || stdPort.Flags.Contains("uneditable");

				ports.Add(stdPort);
			}

			return ports;
		}

		List<string> BuildPortTypes(SItemPortDef port)
		{
			var types = new List<string>();

			foreach (var portType in port.Types)
			{
				var major = portType.Type;
				if (portType.SubTypes.Length == 0) types.Add(BuildTypeName(major, null));
				else
				{
					foreach (var subType in portType.SubTypes)
					{
						var minor = subType.value;
						types.Add(BuildTypeName(major, minor));
					}
				}
			}

			return types;
		}

		List<string> BuildPortFlags(SItemPortDef port)
		{
			var flags = new List<string>();

			if (port.Flags != null)
			{
				foreach (var flag in port.Flags.Split(" "))
				{
					if (!String.IsNullOrEmpty(flag)) flags.Add(flag);
				}
			}

			return flags;
		}

		StandardisedShield BuildShieldInfo(EntityClassDefinition item)
		{
			var shield = item.Components?.SCItemShieldGeneratorParams;
			if (shield == null) return null;

			return new StandardisedShield
			{
				Health = shield.MaxShieldHealth,
				Regeneration = shield.MaxShieldRegen,
				DownedDelay = shield.DownedRegenDelay,
				DamagedDelay = shield.DamagedRegenDelay,
				Absorption = new StandardisedShieldAbsorption
				{
					Physical = new StandardisedShieldAbsorptionRange { Minimum = shield.ShieldAbsorption[0].Min, Maximum = shield.ShieldAbsorption[0].Max },
					Energy = new StandardisedShieldAbsorptionRange { Minimum = shield.ShieldAbsorption[1].Min, Maximum = shield.ShieldAbsorption[1].Max },
					Distortion = new StandardisedShieldAbsorptionRange { Minimum = shield.ShieldAbsorption[2].Min, Maximum = shield.ShieldAbsorption[2].Max },
					Thermal = new StandardisedShieldAbsorptionRange { Minimum = shield.ShieldAbsorption[3].Min, Maximum = shield.ShieldAbsorption[3].Max },
					Biochemical = new StandardisedShieldAbsorptionRange { Minimum = shield.ShieldAbsorption[4].Min, Maximum = shield.ShieldAbsorption[4].Max },
					Stun = new StandardisedShieldAbsorptionRange { Minimum = shield.ShieldAbsorption[5].Min, Maximum = shield.ShieldAbsorption[5].Max }
				}
			};
		}

		StandardisedQuantumDrive BuildQuantumDriveInfo(EntityClassDefinition item)
		{
			var qdComponent = item.Components?.SCItemQuantumDriveParams;
			if (qdComponent == null) return null;

			return new StandardisedQuantumDrive
			{
				JumpRange = qdComponent.jumpRange,
				FuelRate = qdComponent.quantumFuelRequirement / 1e6,
				StandardJump = new StandardisedJumpPerformance
				{
					Speed = qdComponent.@params.driveSpeed,
					SpoolUpTime = qdComponent.@params.spoolUpTime,
					Stage1AccelerationRate = qdComponent.@params.stageOneAccelRate,
					State2AccelerationRate = qdComponent.@params.stageTwoAccelRate,
					Cooldown = qdComponent.@params.cooldownTime
				},
				SplineJump = new StandardisedJumpPerformance
				{
					Speed = qdComponent.splineJumpParams.driveSpeed,
					SpoolUpTime = qdComponent.splineJumpParams.spoolUpTime,
					Stage1AccelerationRate = qdComponent.splineJumpParams.stageOneAccelRate,
					State2AccelerationRate = qdComponent.splineJumpParams.stageTwoAccelRate,
					Cooldown = qdComponent.splineJumpParams.cooldownTime
				}
			};
		}

		StandardisedDurability BuildDurabilityInfo(EntityClassDefinition item)
		{
			var degradation = item.Components?.SDegradationParams;
			var health = item.Components?.SHealthComponentParams;

			if (degradation == null && health == null) return null;

			return new StandardisedDurability
			{
				Lifetime = degradation?.MaxLifetimeHours,
				Health = health?.Health
			};
		}

		StandardisedPowerPlant BuildPowerPlantInfo(EntityClassDefinition item)
		{
			if (item.Components?.SAttachableComponentParams?.AttachDef.Type != "PowerPlant") return null;
			var powerPlant = item.Components?.EntityComponentPowerConnection;
			if (powerPlant == null) return null;

			return new StandardisedPowerPlant
			{
				Output = powerPlant.PowerDraw
			};
		}

		StandardisedCooler BuildCoolerInfo(EntityClassDefinition item)
		{
			var cooler = item.Components?.SCItemCoolerParams;
			if (cooler == null) return null;

			return new StandardisedCooler
			{
				Rate = cooler.CoolingRate
			};
		}

		StandardisedThruster BuildThrusterInfo(EntityClassDefinition item)
		{
			var thruster = item.Components?.SCItemThrusterParams;
			if (thruster == null) return null;

			return new StandardisedThruster
			{
				ThrustCapacity = thruster.thrustCapacity,
				FuelRate = thruster.fuelBurnRatePer10KNewton / 1e4,
				MaxThrustFuelRate = thruster.fuelBurnRatePer10KNewton / 1e4 * thruster.thrustCapacity,
				Type = thruster.thrusterType
			};
		}

		StandardisedCargoGrid BuildCargoGridInfo(EntityClassDefinition item)
		{
			var cargo = item.Components?.SCItemCargoGridParams;
			if (cargo == null) return null;

			return new StandardisedCargoGrid
			{
				Width = cargo.dimensions.x,
				Height = cargo.dimensions.z,
				Depth = cargo.dimensions.y,
				Capacity = Math.Floor(cargo.dimensions.x / 1.25) * Math.Floor(cargo.dimensions.y / 1.25) * Math.Floor(cargo.dimensions.z / 1.25),
				MiningOnly = cargo.miningOnly
			};
		}

		StandardisedFuelTank BuildQuantumFuelTankInfo(EntityClassDefinition item)
		{
			var tank = item.Components?.SCItemFuelTankParams;
			if (tank == null) return null;

			if (item.Components?.SAttachableComponentParams.AttachDef.Type != "QuantumFuelTank") return null;

			return new StandardisedFuelTank
			{
				Capacity = tank.capacity
			};
		}

		StandardisedFuelTank BuildHydrogenFuelTankInfo(EntityClassDefinition item)
		{
			var tank = item.Components?.SCItemFuelTankParams;
			if (tank == null) return null;

			if (item.Components?.SAttachableComponentParams.AttachDef.Type != "FuelTank") return null;

			return new StandardisedFuelTank
			{
				Capacity = tank.capacity
			};
		}

		StandardisedFuelIntake BuildHydrogenFuelIntakeInfo(EntityClassDefinition item)
		{
			var intake = item.Components?.SCItemFuelIntakeParams;
			if (intake == null) return null;

			return new StandardisedFuelIntake
			{
				Rate = intake.fuelPushRate
			};
		}

		StandardisedArmour BuildArmourInfo(EntityClassDefinition item)
		{
			var armour = item.Components?.SCItemVehicleArmorParams;
			if (armour == null) return null;

			return new StandardisedArmour
			{
				DamageMultipliers = new StandardisedDamage
				{
					Physical = armour.damageMultiplier.DamageInfo.DamagePhysical,
					Energy = armour.damageMultiplier.DamageInfo.DamageEnergy,
					Distortion = armour.damageMultiplier.DamageInfo.DamageDistortion,
					Thermal = armour.damageMultiplier.DamageInfo.DamageThermal,
					Biochemical = armour.damageMultiplier.DamageInfo.DamageBiochemical,
					Stun = armour.damageMultiplier.DamageInfo.DamageStun
				},

				SignalMultipliers = new StandardisedSignature
				{
					CrossSection = armour.signalCrossSection,
					Infrared = armour.signalInfrared,
					Electromagnetic = armour.signalElectromagnetic
				}
			};
		}

		StandardisedEmp BuildEmpInfo(EntityClassDefinition item)
		{
			var emp = item.Components?.SCItemEMPParams;
			if (emp == null) return null;

			return new StandardisedEmp
			{
				ChargeTime = emp.chargeTime,
				CooldownTime = emp.cooldownTime,
				Damage = emp.distortionDamage,
				Radius = emp.empRadius
			};
		}

		StandardisedMissileRack BuildMissileRackInfo(EntityClassDefinition item)
		{
			if (item.Components?.SAttachableComponentParams?.AttachDef.Type != "MissileLauncher") return null;
			if (item.Components?.SAttachableComponentParams?.AttachDef.SubType != "MissileRack") return null;

			var rootPort = item.Components?.SItemPortContainerComponentParams;
			if (rootPort == null) return null;

			var rackPorts = rootPort.Ports;
			if (rackPorts == null || rackPorts.Length == 0) return null;

			return new StandardisedMissileRack
			{
				Count = rackPorts.Length,
				Size = rackPorts[0].MaxSize
			};
		}

		StandardisedQig BuildQigInfo(EntityClassDefinition item)
		{
			var qig = item.Components?.SCItemQuantumInterdictionGeneratorParams;
			if (qig == null) return null;

			return new StandardisedQig
			{
				JammingRange = qig.jammerSettings[0].jammerRange,
				InterdictionRange = qig.quantumInterdictionPulseSettings[0].radiusMeters
			};
		}

		StandardisedIfcs BuildIfcsInfo(EntityClassDefinition item)
		{
			var ifcs = item.Components?.IFCSParams;
			if (ifcs == null) return null;

			return new StandardisedIfcs
			{
				MaxSpeed = ifcs.maxSpeed,
				ScmSpeed = ifcs.scmSpeed,
				Pitch = ifcs.maxAngularVelocity.x,
                Yaw = ifcs.maxAngularVelocity.z,
                Roll = ifcs.maxAngularVelocity.y,
                Afterburner = new StandardisedAfterburner
                {
	                SpoolUpTime = ifcs.afterburner.afterburnerSpoolUpTime,
	                CapacitorThresholdRatio = ifcs.afterburner.afterburnerCapacitorThresholdRatio,
	                CapacitorMax = ifcs.afterburner.capacitorMax,
	                CapacitorAfterburnerIdleCost = ifcs.afterburner.capacitorAfterburnerIdleCost,
	                CapacitorAfterburnerLinearCost = ifcs.afterburner.capacitorAfterburnerLinearCost,
	                CapacitorAfterburnerAngularCost = ifcs.afterburner.capacitorAfterburnerAngularCost,
	                CapacitorRegenDelayAfterUse = ifcs.afterburner.capacitorRegenDelayAfterUse,
	                CapacitorRegenPerSec = ifcs.afterburner.capacitorRegenPerSec,
                }
			};
		}

		StandardisedCoolerConnection BuildHeatConnectionInfo(EntityClassDefinition item)
		{
			var heat = item.Components?.EntityComponentHeatConnection;
			if (heat == null) return null;

			return new StandardisedCoolerConnection
			{
				ThermalEnergyBase = heat.ThermalEnergyBase,
				ThermalEnergyDraw = heat.ThermalEnergyDraw,
				CoolingRate = heat.MaxCoolingRate
			};
		}

		StandardisedPowerConnection BuildPowerConnectionInfo(EntityClassDefinition item)
		{
			var power = item.Components?.EntityComponentPowerConnection;
			if (power == null) return null;

			return new StandardisedPowerConnection
			{
				PowerBase = power.PowerBase,
				PowerDraw = power.PowerDraw
			};
		}

		StandardisedWeapon BuildWeaponInfo(EntityClassDefinition item)
		{
			var weapon = item.Components?.SCItemWeaponComponentParams;
			if (weapon == null) return null;

			var info = new StandardisedWeapon
			{
				Modes = new List<StandardisedWeaponMode>(),
				Ammunition = BuildAmmunitionInfo(item),
				Consumption = BuildWeaponConsumption(weapon)
			};

			foreach (var action in weapon.fireActions)
			{
				var modeInfo = BuildWeaponModeInfo(action);

				modeInfo.DamagePerShot = (info.Ammunition?.ImpactDamage + info.Ammunition?.DetonationDamage) * modeInfo.PelletsPerShot;
				modeInfo.DamagePerSecond = modeInfo.DamagePerShot * (modeInfo.RoundsPerMinute / 60f);

				info.Modes.Add(modeInfo);
			}

			return info;
		}

		StandardisedWeaponMode BuildWeaponModeInfo(SWeaponActionParams fireAction)
		{
			var mode = new StandardisedWeaponMode
			{
				Name = fireAction.name,
				LocalisedName = localisationSvc.GetText(fireAction.localisedName)
			};

			switch (fireAction)
			{
				case SWeaponActionFireSingleParams p:
					mode.RoundsPerMinute = p.fireRate;
					mode.FireType = "single";
					mode.AmmoPerShot = p.launchParams.SProjectileLauncher?.ammoCost ?? 1;
					mode.PelletsPerShot = p.launchParams.SProjectileLauncher?.pelletCount ?? 1;
					break;

				case SWeaponActionFireRapidParams p:
					mode.RoundsPerMinute = p.fireRate;
					mode.FireType = "rapid";
					mode.AmmoPerShot = p.launchParams.SProjectileLauncher?.ammoCost ?? 1;
					mode.PelletsPerShot = p.launchParams.SProjectileLauncher?.pelletCount ?? 1;
					break;

				case SWeaponActionFireBeamParams p:
					mode.FireType = "beam";
					break;

				case SWeaponActionFireChargedParams p:
					mode.FireType = "charged";

					if (p.weaponAction?.SWeaponActionFireSingleParams != null)
					{
						mode.RoundsPerMinute = p.weaponAction.SWeaponActionFireSingleParams.fireRate;
						mode.AmmoPerShot = p.weaponAction.SWeaponActionFireSingleParams.launchParams.SProjectileLauncher?.ammoCost ?? 1;
						mode.PelletsPerShot = p.weaponAction.SWeaponActionFireSingleParams.launchParams.SProjectileLauncher?.pelletCount ?? 1;
					}
					else if (p.weaponAction?.SWeaponActionFireBurstParams != null)
					{
						mode.RoundsPerMinute = p.weaponAction.SWeaponActionFireBurstParams.fireRate;
						mode.AmmoPerShot = p.weaponAction.SWeaponActionFireBurstParams.launchParams.SProjectileLauncher?.ammoCost ?? 1;
						mode.PelletsPerShot = p.weaponAction.SWeaponActionFireBurstParams.launchParams.SProjectileLauncher?.pelletCount ?? 1;
					}
					break;

				case SWeaponActionFireHealingBeamParams p:
					mode.FireType = p.healingMode;
					break;

				case SWeaponActionFireSalvageRepairParams p:
					mode.FireType = p.salvageRepairMode;
					break;

				case SWeaponActionGatheringBeamParams p:
					mode.FireType = "collectionbeam";
					break;

				case SWeaponActionFireTractorBeamParams p:
					mode.FireType = "tractorbeam";
					break;

				case SWeaponActionSequenceParams p:
					if (p.sequenceEntries != null && p.sequenceEntries.Length > 0 && p.sequenceEntries[0].weaponAction != null)
					{
						SWeaponActionParams weaponAction = null;
						if (p.sequenceEntries[0].weaponAction.SWeaponActionFireSingleParams != null)
						{
							weaponAction = p.sequenceEntries[0].weaponAction.SWeaponActionFireSingleParams;
						}
						else if (p.sequenceEntries[0].weaponAction.SWeaponActionFireBurstParams != null)
						{
							weaponAction = p.sequenceEntries[0].weaponAction.SWeaponActionFireBurstParams;
						}

						if (weaponAction != null)
						{
							mode = BuildWeaponModeInfo(weaponAction);
						}
					}
					mode.FireType = "sequence";
					break;

				case SWeaponActionFireBurstParams p:
					mode.RoundsPerMinute = p.fireRate;
					mode.FireType = "burst";
					mode.AmmoPerShot = p.launchParams.SProjectileLauncher?.ammoCost ?? 1;
					mode.PelletsPerShot = p.launchParams.SProjectileLauncher?.pelletCount ?? 1;
					break;

				default:
					throw new ApplicationException("Unknown fireAction");
			}

			return mode;
		}

		StandardisedAmmunition BuildAmmunitionInfo(EntityClassDefinition item)
		{
			var ammo = GetAmmoParams(item);
			if (ammo == null) return null;

			var projectiles = ammo.projectileParams.BulletProjectileParams;
			var impactDamage = Damage.FromDamageInfo(projectiles != null && projectiles.damage.Length > 0 ? projectiles.damage[0] : new DamageInfo());
			var detonationDamage = Damage.FromDamageInfo(projectiles?.detonationParams?.ProjectileDetonationParams?.explosionParams?.damage[0]);

			return new StandardisedAmmunition
			{
				UUID = ammo.__ref,
				Type = ammo.__type,
				Speed = ammo.speed,
				Range = ammo.lifetime * ammo.speed,
				Size = ammo.size,
				ImpactDamage = ConvertDamage(impactDamage),
				DetonationDamage = ConvertDamage(detonationDamage),
				Capacity = item.Components?.SAmmoContainerComponentParams?.maxAmmoCount ?? item.Components?.SAmmoContainerComponentParams?.maxRestockCount,
				BulletImpulseFalloff = new StandardisedBulletImpulseFalloff{
					MinDistance = projectiles?.impulseFalloffParams?.BulletImpulseFalloffParams?.minDistance,
					DropFalloff = projectiles?.impulseFalloffParams?.BulletImpulseFalloffParams?.dropFalloff,
					MaxFalloff = projectiles?.impulseFalloffParams?.BulletImpulseFalloffParams?.maxFalloff,
				},
				BulletPierceability = new StandardisedBulletPierceability{
					DamageFalloffLevel1 = projectiles?.pierceabilityParams?.damageFalloffLevel1,
					DamageFalloffLevel2 = projectiles?.pierceabilityParams?.damageFalloffLevel2,
					DamageFalloffLevel3 = projectiles?.pierceabilityParams?.damageFalloffLevel3,
					MaxPenetrationThickness = projectiles?.pierceabilityParams?.maxPenetrationThickness,
				},
				BulletElectron = new StandardisedBulletElectron{
					JumpRange = projectiles?.electronParams?.BulletElectronParams?.jumpRange,
					MaximumJumps = projectiles?.electronParams?.BulletElectronParams?.maximumJumps,
					ResidualChargeMultiplier = projectiles?.electronParams?.BulletElectronParams?.residualChargeMultiplier,
				},
				DamageDropMinDistance = ConvertDamage(Damage.FromDamageInfo(projectiles?.damageDropParams?.BulletDamageDropParams?.damageDropMinDistance?.DamageInfo ?? new DamageInfo())),
				DamageDropPerMeter = ConvertDamage(Damage.FromDamageInfo(projectiles?.damageDropParams?.BulletDamageDropParams?.damageDropPerMeter?.DamageInfo ?? new DamageInfo())),
				DamageDropMinDamage = ConvertDamage(Damage.FromDamageInfo(projectiles?.damageDropParams?.BulletDamageDropParams?.damageDropMinDamage?.DamageInfo ?? new DamageInfo())),
			};
		}

		StandardisedWeaponConsumption BuildWeaponConsumption(SCItemWeaponComponentParams weapon)
		{
			var regenParams = weapon?.weaponRegenConsumerParams.SingleOrDefault();
			if (regenParams == null) return null;

			return new StandardisedWeaponConsumption
			{
				RequestedRegenPerSec = regenParams.requestedRegenPerSec,
				RequestedAmmoLoad = regenParams.requestedAmmoLoad,
				Cooldown = regenParams.regenerationCooldown,
				CostPerBullet = regenParams.regenerationCostPerBullet
			};
		}

		StandardisedDamage ConvertDamage(Damage damage)
		{
			if (damage == null) return null;

			return new StandardisedDamage
			{
				Physical = damage.physical,
				Energy = damage.energy,
				Distortion = damage.distortion,
				Thermal = damage.thermal,
				Biochemical = damage.biochemical,
				Stun = damage.stun
			};
		}

		StandardisedDamage ConvertDamage(DamageInfo damage)
		{
			if (damage == null) return null;

			return new StandardisedDamage
			{
				Physical = damage.DamagePhysical,
				Energy = damage.DamageEnergy,
				Distortion = damage.DamageDistortion,
				Thermal = damage.DamageThermal,
				Biochemical = damage.DamageBiochemical,
				Stun = damage.DamageStun
			};
		}

		AmmoParams GetAmmoParams(EntityClassDefinition item)
		{
			// If this a weapon that contains its own ammo, or if it is a magazine, then it will have an SCAmmoContainerComponentParams component.
			var ammoRef = item.Components?.SAmmoContainerComponentParams?.ammoParamsRecord;
			if (ammoRef != null) return ammoSvc.GetByReference(ammoRef);

			// Otherwise if this is a weapon then SCItemWeaponComponentParams.ammoContainerRecord should be the reference of a magazine entity
			var magRef = item.Components?.SCItemWeaponComponentParams?.ammoContainerRecord;
			if (magRef == null) return null;
			var mag = entitySvc.GetByReference(magRef);
			if (mag == null) return null;

			// And the magazine's SAmmoContainerComponentParams will tell us about the ammo
			return ammoSvc.GetByReference(mag.Components?.SAmmoContainerComponentParams.ammoParamsRecord);
		}

		StandardisedMissile BuildMissileInfo(EntityClassDefinition item)
		{
			var missile = item.Components?.SCItemMissileParams;
			if (missile == null) return null;

			var info = new StandardisedMissile
			{
				Damage = ConvertDamage(missile.explosionParams.damage[0])
			};

			return info;
		}

		StandardisedMissile BuildBombInfo(EntityClassDefinition item)
		{
			var missile = item.Components?.SCItemBombParams;
			if (missile == null) return null;

			var info = new StandardisedMissile
			{
				Damage = ConvertDamage(missile.explosionParams.damage[0])
			};

			return info;
		}

		StandardisedScanner BuildScannerInfo(EntityClassDefinition item)
		{
			var scanner = item.Components?.SSCItemScannerComponentParams;
			if (scanner == null) return null;

			var info = new StandardisedScanner
			{
				Range = scanner.scanRange
			};

			return info;
		}

		StandardisedRadar BuildRadarInfo(EntityClassDefinition item)
		{
			var radar = item.Components?.SCItemRadarComponentParams;
			if (radar == null) return null;

			var info = new StandardisedRadar
			{
				// DetectionLifetime = radar.detectionLifetime,
				// AltitudeCeiling = radar.altitudeCeiling,
				// CrossSectionOcclusion = radar.enableCrossSectionOcclusion,
				Signatures = BuildDetectionSignatures(item)
			};

			return info;
		}

		List<StandardisedSignatureDetection> BuildDetectionSignatures(EntityClassDefinition item)
		{
			var detections = new List<StandardisedSignatureDetection>();
			var signatureDetection = item.Components?.SCItemRadarComponentParams?.signatureDetection;

			if (signatureDetection == null) return null;

			foreach (var detection in item.Components?.SCItemRadarComponentParams.signatureDetection)
			{
				detections.Add(new StandardisedSignatureDetection
				{
					// Detectable = detection.detectable,
					Sensitivity = detection.sensitivity,
					// AmbientPiercing = detection.ambientPiercing
				});
			}

			return detections;
		}

		StandardisedPing BuildPingInfo(EntityClassDefinition item)
		{
			var ping = item.Components?.SSCItemPingComponentParams;
			if (ping == null) return null;

			var info = new StandardisedPing
			{
				ChargeTime = ping.maximumChargeTime,
				CooldownTime = ping.maximumCooldownTime
			};

			return info;
		}

		StandardisedWeaponRegenPool BuildWeaponRegenInfo(EntityClassDefinition item)
		{
			var regen = item.Components?.SCItemWeaponRegenPoolComponentParams;
			if (regen == null) return null;

			var info = new StandardisedWeaponRegenPool
			{
				RegenFillRate = regen.regenFillRate,
				AmmoLoad = regen.ammoLoad,
				RespectsCapacitorAssignments = regen.respectsCapacitorAssignments,
			};

			return info;
		}

		StandardisedInventoryContainer BuildPersonalInventoryInfo(EntityClassDefinition item)
		{
			var inventoryRef = item.Components?.SCItemInventoryContainerComponentParams?.containerParams;
			if (inventoryRef != null) return _inventoryContainerSvc.GetInventoryContainer(inventoryRef);

			return null;
		}

		StandardisedUiDimensionOverrides BuildUiDimensionOverrides(EntityClassDefinition item)
		{
			var uiOverride = item.Components?.SAttachableComponentParams?.AttachDef?.inventoryOccupancyDimensionsUIOverride?.Vec3;
			if (uiOverride != null)
			{
				return new StandardisedUiDimensionOverrides
				{
					Width = uiOverride.x,
					Height = uiOverride.z,
					Depth = uiOverride.y,
				};
			};

			return null;
		}

		StandardisedTemperatureResistance BuildTemperatureResistance(EntityClassDefinition item)
		{
			var resistance = item.Components?.SCItemClothingParams?.TemperatureResistance;
			if (resistance != null)
			{
				return new StandardisedTemperatureResistance
				{
					Min = resistance.MinResistance,
					Max = resistance.MaxResistance
				};
			};

			return null;
		}

		StandardisedConsumableParams BuildFoodInfo(EntityClassDefinition item)
		{
			var consumableParams = item.Components?.SCItemConsumableParams;
			if (consumableParams != null)
			{
				var consumeVolume = consumableParams.Volume;
				var scu = ConvertToSCU(consumeVolume) ?? 1;

				return new StandardisedConsumableParams
				{
					oneShotConsume = consumableParams.oneShotConsume,
					consumeVolume = scu,
					containerClosed = consumableParams.containerClosed,
					canBeReclosed = consumableParams.canBeReclosed,
					discardWhenConsumed = consumableParams.discardWhenConsumed,
					containerTypeTag = consumableParams.containerTypeTag
				};
			}

			return null;
		}

		private double? ConvertToSCU(dynamic volume)
		{
			double? scu = null;

			if (volume?.SStandardCargoUnit?.standardCargoUnits != null) {
				scu  = volume.SStandardCargoUnit.standardCargoUnits;
			} else if (volume?.SCentiCargoUnit?.centiSCU != null) {
				scu = volume.SCentiCargoUnit.centiSCU * Math.Pow(10, -2);
			} else if (volume?.SMicroCargoUnit?.microSCU != null) {
				scu = volume.SMicroCargoUnit.microSCU * Math.Pow(10, -6);
			}

			return scu;
		}
	}
}
