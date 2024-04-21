using System.Xml;
using System.Xml.Serialization;
using scdb.Xml.Entities;

namespace scdb.Xml.Lootgeneration
{
	public class LootTable: ClassBase
	{
		public WeightedLootArchetype[] lootArchetypes;
	}

	public class WeightedLootArchetype
	{
		[XmlAttribute] public string archetype;
		[XmlAttribute] public double weight;
		public NumberOfResultsConstraints numberOfResultsConstraints;
	}

	public class NumberOfResultsConstraints
	{
		[XmlAttribute] public double minResults;
		[XmlAttribute] public double maxResults;

	}
}
