namespace Loader
{
	public class Damage
	{
		public double physical;
		public double energy;
		public double distortion;
		public double Thermal;
		public double biochemical;
		public double stun;

		public static Damage FromDamageInfo(scdb.Xml.Entities.DamageInfo info)
		{
			if (info == null) return null;

			return new Damage
			{
				physical = info.DamagePhysical,
				energy = info.DamageEnergy,
				distortion = info.DamageDistortion,
				Thermal = info.DamageThermal,
				biochemical = info.DamageBiochemical,
				stun = info.DamageStun
			};
		}
	}
}
