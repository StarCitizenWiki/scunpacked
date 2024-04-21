using System.Collections.Generic;
using scdb.Xml.Lootgeneration;

namespace Loader;

public class LootService
{
	public Dictionary<string, LootArchetype> archetypes;
	public Dictionary<string, LootTable> tables;

	public LootService(Dictionary<string, LootArchetype> archetypes, Dictionary<string, LootTable> tables)
	{
		this.archetypes = archetypes;
		this.tables = tables;
	}
}
