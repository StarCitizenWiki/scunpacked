using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemExplosiveParams
	{
		[XmlAttribute]
		public float maxLifeTime;

		public ExplosionParams explosionParams;
	}
}
