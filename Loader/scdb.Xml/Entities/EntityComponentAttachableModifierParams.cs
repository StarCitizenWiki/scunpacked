using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class EntityComponentAttachableModifierParams
	{
		[XmlAttribute]
		public int charges;

		[XmlAttribute]
		public bool canInterrupt;

		[XmlAttribute]
		public bool isInterruptible;

		public Modifiers modifiers;
	}
}
