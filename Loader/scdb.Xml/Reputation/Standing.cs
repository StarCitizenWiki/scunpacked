using System.Xml.Serialization;
using scdb.Xml.Entities;

namespace Loader.scdb.Xml.Reputation;

public class Standing : ClassBase
{
	[XmlAttribute] public string name;
	[XmlAttribute] public string description;
	[XmlAttribute] public string displayName;
	[XmlAttribute] public string perkDescription;
	[XmlAttribute] public int minReputation;
	[XmlAttribute] public int driftReputation;
	[XmlAttribute] public int driftTimeHours;
	[XmlAttribute] public bool gated;
}
