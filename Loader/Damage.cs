using scdb.Xml.Entities;

namespace Loader
{
	public class Damage
	{
		public double physical;
		public double energy;
		public double distortion;
		public double thermal;
		public double biochemical;
		public double stun;

		public static Damage FromDamageInfo(DamageInfo info)
		{
			if (info == null) return null;

			return new Damage
			{
				physical = info.DamagePhysical,
				energy = info.DamageEnergy,
				distortion = info.DamageDistortion,
				thermal = info.DamageThermal,
				biochemical = info.DamageBiochemical,
				stun = info.DamageStun
			};
		}
	}
}
