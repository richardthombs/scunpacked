using scdb.Xml.Entities;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Loader
{
	class DamageResistanceMacroParser
	{
		public DamageResistanceMacro Parse(string fullXmlPath)
		{
			if (!File.Exists(fullXmlPath))
			{
				Console.WriteLine("Location file does not exist");
				return null;
			}

			return ParseDamageResistanceMacro(fullXmlPath);
		}

		DamageResistanceMacro ParseDamageResistanceMacro(string xmlFilename)
		{
			string rootNodeName;
			using (var reader = XmlReader.Create(new StreamReader(xmlFilename)))
			{
				reader.MoveToContent();
				rootNodeName = reader.Name;
			}

			var doc = new XmlDocument();
			doc.Load(xmlFilename);

			var serialiser = new XmlSerializer(typeof(DamageResistanceMacro), new XmlRootAttribute { ElementName = rootNodeName });
			using var stream = new XmlNodeReader(doc);
			var entity = (DamageResistanceMacro)serialiser.Deserialize(stream);

			return entity;
		}
	}
}
