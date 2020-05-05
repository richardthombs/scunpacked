using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Loader.Parser
{
	internal static class GenericParser
	{
		internal static async Task<T> Parse<T>(string xmlFilename)
		{
			string rootNodeName;
			using (var reader = XmlReader.Create(new StreamReader(xmlFilename)))
			{
				reader.MoveToContent();
				rootNodeName = reader.Name;
			}

			var xml = await File.ReadAllTextAsync(xmlFilename);
			var doc = new XmlDocument();
			doc.LoadXml(xml);

			var serialiser = new XmlSerializer(typeof(T), new XmlRootAttribute {ElementName = rootNodeName});

			using var stream = new XmlNodeReader(doc);
			var entity = (T) serialiser.Deserialize(stream);
			return entity;
		}
	}
}
