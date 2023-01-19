using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using scdb.Xml.Entities;

namespace Loader
{
	public class InventoryContainerParser
	{
		public InventoryContainer Parse(string fullXmlPath)
		{
			if (!File.Exists(fullXmlPath))
			{
				Console.WriteLine("InventoryContainer file does not exist");
				return null;
			}

			return ParseInventoryContainer(fullXmlPath);
		}

		InventoryContainer ParseInventoryContainer(string xmlFilename)
		{
			string rootNodeName;
			using (var reader = XmlReader.Create(new StreamReader(xmlFilename)))
			{
				reader.MoveToContent();
				rootNodeName = reader.Name;
			}

			var xml = File.ReadAllText(xmlFilename);
			var doc = new XmlDocument();
			doc.LoadXml(xml);

			var serialiser = new XmlSerializer(typeof(InventoryContainer), new XmlRootAttribute { ElementName = rootNodeName });
			using (var stream = new XmlNodeReader(doc))
			{
				var entity = (InventoryContainer)serialiser.Deserialize(stream);
				return entity;
			}
		}
	}
}
