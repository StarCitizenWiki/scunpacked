using System.Xml.Serialization;
using scdb.Xml.Entities;

namespace Loader.scdb.Xml.Reputation;

public class Scope : ClassBase
{
	[XmlAttribute] public string scopeName;
	[XmlAttribute] public string displayName;
	[XmlAttribute] public string description;
	public StandingMap standingMap;
}

public class StandingMap
{
	[XmlAttribute] public int reputationCeiling;
	[XmlAttribute] public int initialReputation;
	public Reference[] standings;
}
