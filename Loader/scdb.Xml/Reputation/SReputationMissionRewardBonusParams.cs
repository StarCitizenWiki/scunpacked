using System.Xml.Serialization;
using scdb.Xml.Entities;

namespace Loader.scdb.Xml.Reputation;

public class SReputationMissionRewardBonusParams : ClassBase
{
	public SReputationMissionGiverRewardBonusParams[] missionGiverBonuses;
}

public class SReputationMissionGiverRewardBonusParams
{
	[XmlAttribute] public string missionGiverEntityClass;
	[XmlAttribute] public string reputationScope;
	public SReputationStandingRewardBonusParams[] rewardBonuses;
}

public class SReputationStandingRewardBonusParams
{
	[XmlAttribute] public string standing;
	[XmlAttribute] public double bonusFraction;

}
