using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using scdb.Xml.Entities;

namespace shipparser
{
	public class EntityParser
	{
		public EntityClassDefinition Parse(string fullXmlPath, string shipEntityClass)
		{
			if (!File.Exists(fullXmlPath))
			{
				Console.WriteLine("Ship entity definition file does not exist");
				return null;
			}

			return ParseShipDefinition(fullXmlPath, shipEntityClass);
		}

		EntityClassDefinition ParseShipDefinition(string shipEntityPath, string shipEntityClass)
		{
			var shipEntityName = $"EntityClassDefinition.{shipEntityClass}";

			var xml = File.ReadAllText(shipEntityPath);
			var doc = new XmlDocument();
			doc.LoadXml(xml);

			var serialiser = new XmlSerializer(typeof(EntityClassDefinition), new XmlRootAttribute { ElementName = shipEntityName });
			using (var stream = new XmlNodeReader(doc))
			{
				var entity = (EntityClassDefinition)serialiser.Deserialize(stream);
				return entity;
			}
		}
	}
}
