using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class ResourceContainer
	{
		public Capacity capacity;

		public Reference[] inclusiveResources;
		public Reference[] exclusiveResources;

		public Reference[] inclusiveGroups;
		public Reference[] exclusiveGroups;
	}
}
