using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using scdb.Xml.Entities;

namespace Loader
{
	public class AmmoParser
	{
		static Dictionary<string, string> filenameToClassMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		static Dictionary<string, AmmoParams> cache = new Dictionary<string, AmmoParams>(StringComparer.OrdinalIgnoreCase);

		public AmmoParams Parse(string fullXmlPath)
		{
			if (filenameToClassMap.ContainsKey(fullXmlPath))
			{
				var className = filenameToClassMap[fullXmlPath];
				var cached = cache[className];
				Console.WriteLine("Cached " + className);
				return cached;
			}

			if (!File.Exists(fullXmlPath))
			{
				Console.WriteLine("Entity definition file does not exist");
				return null;
			}

			var entity = ParseEntityDefinition(fullXmlPath);
			filenameToClassMap.Add(fullXmlPath, entity.ClassName);
			cache.Add(entity.ClassName, entity);

			return entity;
		}

		AmmoParams ParseEntityDefinition(string entityPath)
		{
			string rootNodeName;
			using (var reader = XmlReader.Create(new StreamReader(entityPath)))
			{
				reader.MoveToContent();
				rootNodeName = reader.Name;
			}

			var split = rootNodeName.Split('.');
			string className = split[split.Length - 1];

			var xml = File.ReadAllText(entityPath);
			var doc = new XmlDocument();
			doc.LoadXml(xml);

			var serialiser = new XmlSerializer(typeof(AmmoParams), new XmlRootAttribute { ElementName = rootNodeName });
			using (var stream = new XmlNodeReader(doc))
			{
				var entity = (AmmoParams)serialiser.Deserialize(stream);
				entity.ClassName = className;
				return entity;
			}
		}
	}
}
