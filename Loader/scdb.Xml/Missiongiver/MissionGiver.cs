using System.Xml.Serialization;
using scdb.Xml.Entities;

namespace Loader.scdb.Xml.Missiongiver;

public class MissionGiver : ClassBase
{
	[XmlAttribute] public string entityClass;
	[XmlAttribute] public string displayName;
	[XmlAttribute] public string description;
	[XmlAttribute] public string headquarters;
	[XmlAttribute] public double invitationTimeout;
	[XmlAttribute] public double visitTimeout;
	[XmlAttribute] public double shortCooldown;
	[XmlAttribute] public double mediumCooldown;
	[XmlAttribute] public double longCooldown;
}
