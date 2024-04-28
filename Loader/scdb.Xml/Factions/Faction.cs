using System.Xml.Serialization;
using scdb.Xml.Entities;

namespace Loader.scdb.Xml.Factions;

public class Faction : ClassBase
{
	[XmlAttribute] public string displayName;
	[XmlAttribute] public string description;
	[XmlAttribute] public string gameToken;
	[XmlAttribute] public string defaultReaction;

	public FriendlyFireReactionOverride[] friendlyFireReactionOverrides;
	public FactionRelationship[] factionRelationships;
}

public class FriendlyFireReactionOverride
{
	[XmlAttribute] public string reactionType;
	[XmlAttribute] public bool shouldAllowFriendlyFire;
}

public class FactionRelationship
{
	[XmlAttribute] public string faction;
	[XmlAttribute] public string reactionType;
}
