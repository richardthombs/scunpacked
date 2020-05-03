using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Loader.SCDb.Xml.DefaultLoadouts;
using Microsoft.Extensions.Logging;

namespace Loader.Parser
{
	public class DefaultLoadoutParser
	{
		private readonly ILogger<DefaultLoadoutParser> _logger;

		public DefaultLoadoutParser(ILogger<DefaultLoadoutParser> logger)
		{
			_logger = logger;
		}

		public Loadout Parse(string fullXmlPath)
		{
			if (!File.Exists(fullXmlPath))
			{
				_logger.LogWarning("Loadout file does not exist");
				return null;
			}

			return ParseLoadout(fullXmlPath);
		}

		private Loadout ParseLoadout(string loadoutPath)
		{
			var xml = File.ReadAllText(loadoutPath);
			var doc = new XmlDocument();
			doc.LoadXml(xml);

			var serialiser = new XmlSerializer(typeof(Loadout));

			using var stream = new XmlNodeReader(doc);
			var loadout = (Loadout) serialiser.Deserialize(stream);
			return loadout;
		}
	}
}
