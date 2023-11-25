using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class GCSParams
	{
		[XmlAttribute]
		public bool isDumbMissile;

		[XmlAttribute]
		public double linearSpeed;

		[XmlAttribute]
		public double boostPhaseDuration;

		[XmlAttribute]
		public double terminalPhaseEngagementTime;

		[XmlAttribute]
		public double terminalPhaseEngagementAngle;

		[XmlAttribute]
		public double fuelTankSize;
	}
}
