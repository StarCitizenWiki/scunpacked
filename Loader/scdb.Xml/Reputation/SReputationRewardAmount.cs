using System.Xml.Serialization;
using scdb.Xml.Entities;

namespace Loader.scdb.Xml.Reputation;

public class SReputationRewardAmount : ClassBase
{
	[XmlAttribute] public string editorName;
	[XmlAttribute] public int reputationAmount;
}
