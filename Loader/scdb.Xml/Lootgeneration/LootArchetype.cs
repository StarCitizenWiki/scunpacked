using System.Xml;
using System.Xml.Serialization;
using scdb.Xml.Entities;

namespace scdb.Xml.Lootgeneration
{
	public class LootArchetype: ClassBase
	{
		public ExcludedTags excludedTags;

		public PrimaryOrGroup primaryOrGroup;
		public SecondaryOrGroups secondaryOrGroups;
	}


	public class ExcludedTags
	{
		public Reference[] tags;
	}

	public class PrimaryOrGroup
	{
		[XmlAttribute] public string __type;
		public LootArchetypeEntry_Primary[] entries;
	}


	public class SecondaryOrGroups
	{
		public LootArchetypeOrGroup_Secondary LootArchetypeOrGroup_Secondary;
	}

	public class LootArchetypeEntry
	{
		[XmlAttribute] public string name;
		[XmlAttribute] public string groupName;
		[XmlAttribute] public string tag;
		[XmlAttribute] public double weight;
		[XmlAttribute] public string __type;
		public AdditionalTags additionalTags;
		public OptionalData optionalData;
	}

	public class LootArchetypeEntry_Primary : LootArchetypeEntry;

	public class LootArchetypeEntry_Secondary
	{
		[XmlAttribute] public string tag;
		[XmlAttribute] public double weight;
		[XmlAttribute] public string __type;
	};

	public class LootArchetypeOrGroup_Secondary
	{

		public LootArchetypeEntry_Secondary[] entries;
	};

	public class OptionalData
	{
		public EntryOptionalData_StackSize EntryOptionalData_StackSize;
		public EntryOptionalData_SpawnWith EntryOptionalData_SpawnWith;
	}

	public class EntryOptionalData_StackSize
	{
		[XmlAttribute] public double min;
		[XmlAttribute] public double max;
	}

	public class EntryOptionalData_SpawnWith: EntryOptionalData_StackSize
	{
		[XmlAttribute] public string name;
		[XmlAttribute] public string mode;

		public TagsToMatch tagsToMatch;
	}

	public class PnTags
	{
		public Reference[] positiveTags;
		public Reference[] negativeTags;
	}

	public class AdditionalTags : PnTags;

	public class TagsToMatch : PnTags;
}
