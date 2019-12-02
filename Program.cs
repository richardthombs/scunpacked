using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

using scdb.Xml.Entities;
using scdb.Xml.Loadouts;
using scdb.Xml.Vehicles;
using scdb.Xml.Turbulent;

namespace shipparser
{
	class Program
	{
		static void Main(string[] args)
		{
			var outputFolder = @".\json";

			var scDataRoot = @"c:\dev\scdata\3.7.2";
			var turbulentFolder = Path.Combine(scDataRoot, @"Data\Libs\Foundry\Records\turbulent\vehicles");
			var spaceshipsFolder = Path.Combine(scDataRoot, @"Data\Libs\Foundry\Records\entities\spaceships");
			var vehiclesFolder = Path.Combine(scDataRoot, @"Data\Libs\Foundry\Records\entities\groundvehicles");

			string[] UselessEntities =
			{
				"AEGS_Javelin",
				"ANVL_Hornet_F7A",
				"DefaultSpaceShips.AEGS.AEGS_Idris",
				"does_not_exist",
				"Krig_P72_Archimedes",
				"MISC_Hull_C",
				"RSI_IR1337_Weapon_Mount",
				"TNGS_AEGS_Redeemer",
				"TNGS_ORIG_AX114"
			};

			var parser = new ShipParser { InputRoot = scDataRoot };
			Directory.CreateDirectory(outputFolder);

			foreach (var filename in Directory.EnumerateFiles(turbulentFolder, "*.xml"))
			{
				var entry = GetTurbulentEntry(filename);
				Console.WriteLine($"{filename}: {entry.turbulentName}, {entry.itemClass}");
				if (UselessEntities.Contains(entry.itemClass)) continue;

				var entityFilename = Path.ChangeExtension(Path.Combine(spaceshipsFolder, entry.itemClass.ToLower()), ".xml");
				if (!File.Exists(entityFilename)) entityFilename = Path.ChangeExtension(Path.Combine(vehiclesFolder, entry.itemClass.ToLower()), ".xml");

				var entityClassName = entry.itemClass;

				var ship = parser.Parse(entityFilename, entityClassName);

				if (ship.DefaultLoadout != null) ship.loadout = AddDefaultLoadout(ship.DefaultLoadout.Items);
				else if (ship.Entity.Components.SEntityComponentDefaultLoadoutParams.loadout != null) ship.loadout = AddManualLoadout(ship.Entity.Components.SEntityComponentDefaultLoadoutParams.loadout.SItemPortLoadoutManualParams);

				var json = JsonConvert.SerializeObject(ship, Newtonsoft.Json.Formatting.Indented);
				File.WriteAllText(Path.Combine(outputFolder, $"{entityClassName}.json"), json);
			}
		}

		public static TurbulentEntry GetTurbulentEntry(string turbulentXmlFile)
		{
			var rootNode = Path.GetFileNameWithoutExtension(turbulentXmlFile).ToUpper();
			rootNode = rootNode.Replace("-", "_");
			rootNode = rootNode.Replace("TURBULENTENTRY", "TurbulentEntry");
			if (!rootNode.StartsWith("TurbulentEntry")) rootNode = $"TurbulentEntry.{rootNode}";

			var xml = File.ReadAllText(turbulentXmlFile);
			var doc = new XmlDocument();
			doc.LoadXml(xml);

			var serialiser = new XmlSerializer(typeof(TurbulentEntry), new XmlRootAttribute { ElementName = rootNode });
			using (var stream = new XmlNodeReader(doc))
			{
				var entry = (TurbulentEntry)serialiser.Deserialize(stream);
				return entry;
			}
		}

		public static scdb.Models.loadout[] AddDefaultLoadout(scdb.Xml.Loadouts.Item[] defaultLoadoutItems)
		{
			var modelLoadout = new List<scdb.Models.loadout>();

			if (defaultLoadoutItems != null)
			{
				foreach (var item in defaultLoadoutItems)
				{
					modelLoadout.Add(new scdb.Models.loadout { item = item.itemName, port = item.portName });
					if (item.Items != null) modelLoadout.AddRange(AddDefaultLoadout(item.Items));
				}
			}

			return modelLoadout.ToArray();
		}

		public static scdb.Models.loadout[] AddManualLoadout(scdb.Xml.Entities.SItemPortLoadoutManualParams manual)
		{
			var modelLoadout = new List<scdb.Models.loadout>();

			if (manual != null)
			{
				foreach (var item in manual.entries)
				{
					modelLoadout.Add(new scdb.Models.loadout { item = item.entityClassName, port = item.itemPortName });
					if (item.loadout.SItemPortLoadoutManualParams != null) modelLoadout.AddRange(AddManualLoadout(item.loadout.SItemPortLoadoutManualParams));
				}
			}
			return modelLoadout.ToArray();
		}
	}

	public class Ship
	{
		public EntityClassDefinition Entity { get; set; }
		public Vehicle Vehicle { get; set; }
		public Loadout DefaultLoadout { get; set; }
		public scdb.Models.loadout[] loadout { get; set; }
	}
}
