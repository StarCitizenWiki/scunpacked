using System.Collections.Generic;
using System.Xml.Serialization;

namespace scdb.Xml.Layer0
{
	public class Subsumption
	{
		public Layer Layer;
	}

	public class Layer
	{
		[XmlAttribute]
		public string ID;

		[XmlAttribute]
		public string Name;

		public List<Variable> Variables;
	}

	public class Variable
	{
		[XmlAttribute]
		public string ID;

		[XmlAttribute]
		public string DefaultValue;

		[XmlAttribute]
		public string EditorName;
	}

}
