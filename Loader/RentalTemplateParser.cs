using scdb.Xml.Entities;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Loader
{
	class RentalTemplateParser
	{
		public RentalTemplate Parse(string fullXmlPath)
		{
			if (!File.Exists(fullXmlPath))
			{
				Console.WriteLine("Location file does not exist");
				return null;
			}

			return ParseLocation(fullXmlPath);
		}

		RentalTemplate ParseLocation(string xmlFilename)
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

			var serialiser = new XmlSerializer(typeof(RentalTemplate), new XmlRootAttribute { ElementName = rootNodeName });
			using var stream = new XmlNodeReader(doc);
			var entity = (RentalTemplate)serialiser.Deserialize(stream);

			return entity;
		}
	}
}
