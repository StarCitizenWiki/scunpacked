using System.Collections.Generic;

namespace Loader
{
	public class StandardisedItem
	{
		public string UUID { get; set; }
		public string ClassName { get; set; }
		public int Size { get; set; }
		public int Grade { get; set; }
		public string Type { get; set; }
		public string Classification { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public StandardisedManufacturer Manufacturer { get; set; }
		public List<string> Tags { get; set; }
		public double Width { get; set; }
		public double Height { get; set; }
		public double Length { get; set; }
		public double Volume { get; set; }
		public StandardisedUiDimensionOverrides DimensionOverrides { get; set; }
		public StandardisedShield Shield { get; set; }
		public StandardisedQuantumDrive QuantumDrive { get; set; }
		public StandardisedPowerPlant PowerPlant { get; set; }
		public StandardisedCooler Cooler { get; set; }
		public StandardisedThruster Thruster { get; set; }
		public StandardisedDurability Durability { get; set; }
		public StandardisedCargoGrid CargoGrid { get; set; }
		public StandardisedFuelTank HydrogenFuelTank { get; set; }
		public StandardisedFuelTank QuantumFuelTank { get; set; }
		public StandardisedFuelIntake HydrogenFuelIntake { get; set; }
		public StandardisedArmour Armour { get; set; }
		public StandardisedEmp Emp { get; set; }
		public StandardisedMissileRack MissileRack { get; set; }
		public StandardisedQig QuantumInterdiction { get; set; }
		public StandardisedIfcs Ifcs { get; set; }
		public StandardisedPowerConnection PowerConnection { get; set; }
		public StandardisedCoolerConnection HeatConnection { get; set; }
		public StandardisedWeapon Weapon { get; set; }
		public StandardisedAmmunition Ammunition { get; set; }
		public StandardisedMissile Missile { get; set; }
		public StandardisedMissile Bomb { get; set; }
		public StandardisedScanner Scanner { get; set; }
		public StandardisedRadar Radar { get; set; }
		public StandardisedPing Ping { get; set; }
		public StandardisedWeaponRegenPool WeaponRegenPool { get; set; }
		public StandardisedInventoryContainer InventoryContainer { get; set; }
		public StandardisedTemperatureResistance TemperatureResistance { get; set; }
		public StandardisedConsumableParams Food { get; set; }

		public List<StandardisedItemPort> Ports { get; set; }

		public override string ToString()
		{
			return Name ?? ClassName ?? "Unknown";
		}

		public bool ShouldSerializePorts()
		{
			return Ports?.Count > 0;
		}

		public bool ShouldSerializeTags()
		{
			return Tags?.Count > 0;
		}
	}
}
