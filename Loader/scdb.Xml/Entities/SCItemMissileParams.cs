using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemMissileParams
	{
		[XmlAttribute]
		public bool requiresLauncher;

		[XmlAttribute]
		public bool enableLifetime;

		[XmlAttribute]
		public double maxLifetime;

		[XmlAttribute]
		public double armTime;

		[XmlAttribute]
		public double safeDistance;

		[XmlAttribute]
		public double igniteTime;

		[XmlAttribute]
		public double collisionDelayTime;

		[XmlAttribute]
		public double explosionSafetyDistance;

		[XmlAttribute]
		public double irSignalMinValue;

		[XmlAttribute]
		public double irSignalMaxValue;

		[XmlAttribute]
		public double irSignalRiseRate;

		[XmlAttribute]
		public double irSignalDecayRate;

		[XmlAttribute]
		public double projectileProximity;

		public targetingParams targetingParams;
		public ExplosionParams explosionParams;
		public GCSParams GCSParams;
	}
}
