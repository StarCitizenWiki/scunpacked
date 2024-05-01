using System.Collections.Generic;
using System.Xml.Serialization;
using scdb.Xml.Entities;

namespace scdb.Xml.Missionbroker
{
	public class MissionBrokerEntry : ClassBase
	{
		[XmlAttribute] public bool notForRelease;
		[XmlAttribute] public string owner;
		[XmlAttribute] public string missionModule;
		[XmlAttribute] public string title;
		[XmlAttribute] public string titleHUD;
		[XmlAttribute] public string description;
		[XmlAttribute] public string missionGiver;
		[XmlAttribute] public string commsChannelName;
		[XmlAttribute] public string type;
		[XmlAttribute] public string localityAvailable;
		[XmlAttribute] public string locationMissionAvailable;
		[XmlAttribute] public bool initiallyActive;
		[XmlAttribute] public bool notifyOnAvailable;
		[XmlAttribute] public bool showAsOffer;
		[XmlAttribute] public int missionBuyInAmount;
		[XmlAttribute] public int refundBuyInOnWithdraw;
		[XmlAttribute] public bool hasCompleteButton;
		[XmlAttribute] public bool handlesAbandonRequest;
		[XmlAttribute] public int missionModulePerPlayer;
		[XmlAttribute] public int maxInstances;
		[XmlAttribute] public int maxPlayersPerInstance;
		[XmlAttribute] public int maxInstancesPerPlayer;
		[XmlAttribute] public bool canBeShared;
		[XmlAttribute] public bool onceOnly;
		[XmlAttribute] public bool tutorial;
		[XmlAttribute] public bool displayAlliedMarkers;
		[XmlAttribute] public bool availableInPrison;
		[XmlAttribute] public bool failIfSentToPrison;
		[XmlAttribute] public bool failIfBecameCriminal;
		[XmlAttribute] public bool failIfLeavePrison;
		[XmlAttribute] public bool requestOnly;
		[XmlAttribute] public int respawnTime;
		[XmlAttribute] public double respawnTimeVariation;
		[XmlAttribute] public bool instanceHasLifeTime;
		[XmlAttribute] public bool showLifeTimeInMobiGlas;
		[XmlAttribute] public int instanceLifeTime;
		[XmlAttribute] public double instanceLifeTimeVariation;
		[XmlAttribute] public bool canReacceptAfterAbandoning;
		[XmlAttribute] public int abandonedCooldownTime;
		[XmlAttribute] public double abandonedCooldownTimeVariation;
		[XmlAttribute] public bool canReacceptAfterFailing;
		[XmlAttribute] public bool hasPersonalCooldown;
		[XmlAttribute] public int personalCooldownTime;
		[XmlAttribute] public double personalCooldownTimeVariation;
		[XmlAttribute] public bool moduleHandlesOwnShutdown;
		[XmlAttribute] public string linkedMission;
		[XmlAttribute] public bool lawfulMission;
		[XmlAttribute] public string missionGiverRecord;
		[XmlAttribute] public string invitationMission;
		[XmlAttribute] public string missionGiverFragmentTags;

		public Reference[] associatedMissions;
		public MissionReward missionReward;
		public SReputationAmountListParams[] missionResultReputationRewards;
		public MissionDeadline missionDeadline;
		public CompletionTags completionTags;
		public Modifiers modifiers;
		public Reference[] missionTags;
		public ReputationPrerequisites reputationPrerequisites;
		public Reference[] requiredMissions;
		public RequiredCompletedMissionTags requiredCompletedMissionTags;
	}

	public class MissionReward
	{
		[XmlAttribute] public int reward;
		[XmlAttribute] public int max;
		[XmlAttribute] public int plusBonusses;
		[XmlAttribute] public string currencyType;
		[XmlAttribute] public string reputationBonus;
	}

	public class SReputationAmountListParams
	{
		public SReputationAmountParams[] reputationAmounts;
	}

	public class SReputationAmountParams
	{
		[XmlAttribute] public string uniqueEntityClass;
		[XmlAttribute] public string reputationScope;
		[XmlAttribute] public string reward;
	}

	public class MissionDeadline
	{
		[XmlAttribute] public int missionCompletionTime;
		[XmlAttribute] public bool missionAutoEnd;
		[XmlAttribute] public string missionResultAfterTimerEnd;
		[XmlAttribute] public int remainingTimeToShowTimer;
		[XmlAttribute] public string missionEndReason;
	}

	public class CompletionTags
	{
		public Reference[] tags;
	}

	[XmlRoot(ElementName = "modifiers")]
	public class Modifiers
	{
		[XmlElement(Type = typeof(MissionModifier_LawLicense), ElementName = "MissionModifier_LawLicense")]
		[XmlElement(Type = typeof(MissionModifier_SecurityClearance), ElementName = "MissionModifier_SecurityClearance")]
		[XmlElement(Type = typeof(MissionModifier_HostileMission), ElementName = "MissionModifier_HostileMission")]
		[XmlElement(Type = typeof(MissionModifier_FactionHostility), ElementName = "MissionModifier_FactionHostility")]
		public List<BaseMissionModifier> modifiers { get; set; }
	}

	public class BaseMissionModifier
	{
		[XmlAttribute] public string __polymorphicType;
	}

	public class MissionModifier_SecurityClearance : BaseMissionModifier
	{
		[XmlAttribute] public string clearanceToken;
		[XmlAttribute] public string locationProperty;
	}

	public class MissionModifier_LawLicense : BaseMissionModifier
	{
		[XmlAttribute] public string licenseType;
	}

	public class MissionModifier_HostileMission : BaseMissionModifier
	{
		[XmlAttribute] public string missionBrokerEntry;
		[XmlAttribute] public bool legalToAttack;
	}

	public class MissionModifier_FactionHostility : BaseMissionModifier
	{
		[XmlAttribute] public string faction;
		[XmlAttribute] public string myReaction;
		[XmlAttribute] public string theirReaction;
		[XmlAttribute] public bool ignoreCriminalHostility;
	}

	public class ReputationPrerequisites
	{
		[XmlAttribute] public string wantedLevelJurisdictionOverride;
		public WantedLevel wantedLevel;
	}

	public class WantedLevel
	{
		[XmlAttribute] public int minValue;
		[XmlAttribute] public int maxValue;
	}

	public class ReputationRequirements
	{
		public SReputationAmountListParams SReputationAmountListParams;
	}

	public class SReputationMissionRequirementsParams
	{
		public SReputationMissionRequirementExpressionElement[] expression;
	}

	public class SReputationMissionRequirementExpressionElement{}

	public class SReputationMissionRequirementExpression_LeftParenthesis: SReputationMissionRequirementExpressionElement {}
	public class SReputationMissionRequirementExpression_And: SReputationMissionRequirementExpressionElement {}
	public class SReputationMissionRequirementExpression_RightParenthesis: SReputationMissionRequirementExpressionElement {}

	public class SReputationMissionGiverRequirementParams : SReputationMissionRequirementExpressionElement
	{
		[XmlAttribute] public string missionGiverEntityClass;
		[XmlAttribute] public string reputationScope;
		[XmlAttribute] public string comparison;
		[XmlAttribute] public string standing;
	}

	public class RequiredCompletedMissionTags
	{
		public TagSearchTerm TagSearchTerm;
	}

	public class TagSearchTerm
	{
		public Reference[] positiveTags;
		public Reference[] negativeTags;
	}
}
