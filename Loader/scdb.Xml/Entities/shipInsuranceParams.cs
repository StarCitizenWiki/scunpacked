using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class shipInsuranceParams
	{
		[XmlAttribute]
		public string shipEntityClassName;

		[XmlAttribute]
		public float baseWaitTimeMinutes;

		[XmlAttribute]
		public float mandatoryWaitTimeMinutes;

		[XmlAttribute]
		public float baseExpeditingFee;
	}
}
