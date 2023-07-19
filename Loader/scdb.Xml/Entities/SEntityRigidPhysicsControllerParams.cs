using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SEntityRigidPhysicsControllerParams
	{
		[XmlAttribute] public double Mass;
		[XmlAttribute] public bool PushableByPlayers;
		[XmlAttribute] public bool BlocksNavigation;
	}
}
