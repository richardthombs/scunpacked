using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Loader.SCDb.Xml.Entities;
using Microsoft.Extensions.Logging;

namespace Loader.Parser
{
	public class ManufacturerParser
	{
		private readonly ILogger<ManufacturerParser> _logger;

		public ManufacturerParser(ILogger<ManufacturerParser> logger)
		{
			_logger = logger;
		}

		public Task<SCItemManufacturer> Parse(string fullXmlPath)
		{
			if (!File.Exists(fullXmlPath))
			{
				_logger.LogWarning("Manufacturer  file does not exist");
				return null;
			}

			return ParseManufacturer(fullXmlPath);
		}

		private async Task<SCItemManufacturer> ParseManufacturer(string xmlFilename)
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

			var serialiser = new XmlSerializer(typeof(SCItemManufacturer),
			                                   new XmlRootAttribute {ElementName = rootNodeName});

			using var stream = new XmlNodeReader(doc);
			var entity = (SCItemManufacturer) serialiser.Deserialize(stream);
			return entity;
		}
	}
}
