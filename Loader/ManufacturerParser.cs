using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using scdb.Xml.Entities;

namespace Loader
{
	public class ManufacturerParser
	{
		public SCItemManufacturer Parse(string fullXmlPath)
		{
			if (!File.Exists(fullXmlPath))
			{
				Console.WriteLine("Manufacturer file does not exist");
				return null;
			}

			return ParseManufacturer(fullXmlPath);
		}

		SCItemManufacturer ParseManufacturer(string xmlFilename)
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

			var serialiser = new XmlSerializer(typeof(SCItemManufacturer), new XmlRootAttribute { ElementName = rootNodeName });
			using (var stream = new XmlNodeReader(doc))
			{
				var entity = (SCItemManufacturer)serialiser.Deserialize(stream);
				return entity;
			}
		}
	}
}
