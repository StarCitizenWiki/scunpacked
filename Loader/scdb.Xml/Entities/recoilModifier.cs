using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class recoilModifier
	{
		[XmlAttribute]
		public double angleRecoilStrengthMultiplier;

		[XmlAttribute]
		public double animatedRecoilMultiplier;

		[XmlAttribute]
		public double decayMultiplier;

		[XmlAttribute]
		public double endDecayMultiplier;

		[XmlAttribute]
		public double fireRecoilStrengthFirstMultiplier;

		[XmlAttribute]
		public double fireRecoilStrengthMultiplier;

		[XmlAttribute]
		public double fireRecoilTimeMultiplier;

		[XmlAttribute]
		public double frontalOscillationDecayMultiplier;

		[XmlAttribute]
		public double frontalOscillationRandomnessMultiplier;

		[XmlAttribute]
		public double frontalOscillationRotationMultiplier;

		[XmlAttribute]
		public double frontalOscillationStrengthMultiplier;

		[XmlAttribute]
		public double randomnessBackPushMultiplier;

		[XmlAttribute]
		public double randomnessMultiplier;
	}
}
