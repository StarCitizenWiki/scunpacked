using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class InventoryOpenContainerType
	{
		[XmlAttribute] public float gridCellSize;
		[XmlAttribute] public bool isExternalContainer;
		[XmlAttribute] public int maxPercentageErasedOnParentDestruction;
		[XmlAttribute] public float randomDestructionDistributionExponent;

		public Vec3 minPermittedItemSize;
		public Vec3 maxPermittedItemSize;
	}
}
