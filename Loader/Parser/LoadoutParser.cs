using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Loader.SCDb.Xml.DefaultLoadouts;

namespace Loader.Parser
{
	public class DefaultLoadoutParser
	{
		public Loadout Parse(string fullXmlPath)
		{
			if (!File.Exists(fullXmlPath))
			{
				Console.WriteLine("Loadout file does not exist");
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
			using (var stream = new XmlNodeReader(doc))
			{
				var loadout = (Loadout) serialiser.Deserialize(stream);
				return loadout;
			}
		}
	}
}
