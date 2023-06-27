using Newtonsoft.Json;
using scdb.Xml.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using scdb.Xml.Layer0;

namespace Loader
{
	public class Layer0Parser
	{


		public Layer GetLayers(string xmlFilename)
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

			var serialiser = new XmlSerializer(typeof(Subsumption), new XmlRootAttribute { ElementName = rootNodeName });
			using var stream = new XmlNodeReader(doc);
			var entity = (Subsumption)serialiser.Deserialize(stream);

			return entity.Layer;
		}

		// public List<Variable> GetVariables(string xmlFilename)
		// {
		// 	string rootNodeName;
		// 	using (var reader = XmlReader.Create(new StreamReader(xmlFilename)))
		// 	{
		// 		reader.MoveToContent();
		// 		rootNodeName = reader.Name;
		// 	}
		//
		// 	var xml = File.ReadAllText(xmlFilename);
		// 	var doc = new XmlDocument();
		// 	doc.LoadXml(xml);
		//
		// 	var serialiser = new XmlSerializer(typeof(Subsumption), new XmlRootAttribute { ElementName = rootNodeName });
		// 	using var stream = new XmlNodeReader(doc);
		// 	var entity = (Subsumption)serialiser.Deserialize(stream);
		//
		// 	return entity.Layer.Variables;
		// }
	}
}
