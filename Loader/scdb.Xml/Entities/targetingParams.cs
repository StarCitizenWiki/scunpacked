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
	}
}
