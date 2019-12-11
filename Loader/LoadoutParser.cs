using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using scdb.Xml.DefaultLoadouts;

namespace Loader
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

		Loadout ParseLoadout(string loadoutPath)
		{

			var xml = File.ReadAllText(loadoutPath);
			var doc = new XmlDocument();
			doc.LoadXml(xml);

			var serialiser = new XmlSerializer(typeof(Loadout));
			using (var stream = new XmlNodeReader(doc))
			{
				var loadout = (Loadout)serialiser.Deserialize(stream);
				return loadout;
			}
		}
	}
}
