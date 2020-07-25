using Newtonsoft.Json;
using scdb.Xml.Entities;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Loader
{
	public class LocationParser
	{
		public SCLocation Parse(string fullXmlPath)
		{
			if (!File.Exists(fullXmlPath))
			{
				Console.WriteLine("Location file does not exist");
				return null;
			}

			return ParseLocation(fullXmlPath);
		}

		SCLocation ParseLocation(string xmlFilename)
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

			var serialiser = new XmlSerializer(typeof(SCLocation), new XmlRootAttribute { ElementName = rootNodeName });
			using var stream = new XmlNodeReader(doc);
			var entity = (SCLocation)serialiser.Deserialize(stream);

			return entity;
		}
	}
}
