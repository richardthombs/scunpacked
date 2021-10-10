using System;
using System.Collections.Generic;
using System.IO;

using scdb.Xml.DefaultLoadouts;

namespace Loader
{
	public class XmlLoadoutLoader
	{
		public string DataRoot { get; set; }

		public List<StandardisedLoadoutEntry> Load(string loadoutXmlPath)
		{
			if (String.IsNullOrWhiteSpace(loadoutXmlPath)) return new List<StandardisedLoadoutEntry>();

			var windowsPath = loadoutXmlPath.Replace("/", "\\");
			Console.WriteLine(windowsPath);

			var loadoutParser = new DefaultLoadoutParser();
			var defaultLoadout = loadoutParser.Parse(Path.Combine(DataRoot, "Data", windowsPath));

			return BuildStandardLoadout(defaultLoadout?.Items);
		}

		List<StandardisedLoadoutEntry> BuildStandardLoadout(Item[] loadoutItems)
		{
			var stdLoadout = new List<StandardisedLoadoutEntry>();

			if (loadoutItems != null)
			{
				foreach (var entry in loadoutItems)
				{
					stdLoadout.Add(new StandardisedLoadoutEntry
					{
						ClassName = entry.itemName,
						PortName = entry.portName,
						Entries = BuildStandardLoadout(entry.Items)
					});
				}
			}

			return stdLoadout;
		}
	}
}
