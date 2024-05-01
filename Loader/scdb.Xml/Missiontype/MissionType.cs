using System.Xml.Serialization;
using scdb.Xml.Entities;

namespace Loader.scdb.Xml.Missiontype;

public class MissionType: ClassBase
{
	[XmlAttribute] public string LocalisedTypeName;
}
