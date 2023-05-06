using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class Afterburner
	{
		[XmlAttribute]
		public double afterburnerSpoolUpTime;

		[XmlAttribute]
		public double afterburnerCapacitorThresholdRatio;

		[XmlAttribute]
		public double capacitorMax;

		[XmlAttribute]
		public double capacitorAfterburnerIdleCost;

		[XmlAttribute]
		public double capacitorAfterburnerLinearCost;

		[XmlAttribute]
		public double capacitorAfterburnerAngularCost;

		[XmlAttribute]
		public double capacitorRegenDelayAfterUse;

		[XmlAttribute]
		public double capacitorRegenPerSec;
	}
}
