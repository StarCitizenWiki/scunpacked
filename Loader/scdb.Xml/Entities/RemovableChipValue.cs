using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class RemovableChipValue
	{
		[XmlAttribute] public string name;
		[XmlAttribute] public double value;
	}
}
