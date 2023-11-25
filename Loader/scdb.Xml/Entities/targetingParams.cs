using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class targetingParams
	{
		[XmlAttribute]
		public string trackingSignalType;

		[XmlAttribute]
		public double trackingSignalMin;

		[XmlAttribute]
		public double trackingDistanceMax;

		[XmlAttribute]
		public double trackingSignalAmplifier;

		[XmlAttribute]
		public double trackingSignalAmplifierSeeking;

		[XmlAttribute]
		public double trackingAngle;

		[XmlAttribute]
		public double trackingNoiseAmplifier;

		[XmlAttribute]
		public double lockTime;

		[XmlAttribute]
		public double lockSignalAmplifier;

		[XmlAttribute]
		public double lockingAngle;

		[XmlAttribute]
		public double minRatioForLock;

		[XmlAttribute]
		public double lockIncreaseRate;

		[XmlAttribute]
		public double lockDecreaseRate;

		[XmlAttribute]
		public double lockRangeMin;

		[XmlAttribute]
		public double lockRangeMax;

		[XmlAttribute]
		public double lockResolutionRadius;

		[XmlAttribute]
		public double signalResilienceMin;

		[XmlAttribute]
		public double signalResilienceMax;
	}
}
