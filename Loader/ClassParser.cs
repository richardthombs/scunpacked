using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using scdb.Xml.Entities;

namespace Loader
{
	public class ClassParser<T> where T : ClassBase
	{
		static Dictionary<string, string> filenameToClassMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		public static Dictionary<string, T> ClassByNameCache = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
		public static Dictionary<string, T> ClassByRefCache = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);

		public virtual T Parse(string path)
		{
			if (filenameToClassMap.ContainsKey(path))
			{
				var className = filenameToClassMap[path];
				var cached = ClassByNameCache[className];
				Console.WriteLine("Cached " + className);
				return cached;
			}

			if (!File.Exists(path))
			{
				Console.WriteLine($"ClassParser: File \"{path}\" does not exist");
				return null;
			}

			var entity = ParseClass(path);
			filenameToClassMap.Add(path, entity.ClassName);
			ClassByNameCache.Add(entity.ClassName, entity);
			ClassByRefCache.Add(entity.__ref, entity);

			return entity;
		}

		T ParseClass(string path)
		{
			string rootNodeName;
			using (var reader = XmlReader.Create(new StreamReader(path)))
			{
				reader.MoveToContent();
				rootNodeName = reader.Name;
			}

			var split = rootNodeName.Split('.');
			string className = split[split.Length - 1];

			var xml = File.ReadAllText(path);
			var doc = new XmlDocument();
			doc.LoadXml(xml);

			var serialiser = new XmlSerializer(typeof(T), new XmlRootAttribute { ElementName = rootNodeName });
			using (var stream = new XmlNodeReader(doc))
			{
				var entity = (T)serialiser.Deserialize(stream);
				entity.ClassName = className;
				return entity;
			}
		}
	}
}
