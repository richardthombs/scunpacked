using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

using scdb.Xml.Entities;
using scdb.Xml.Vehicles;
using scdb.Xml.Loadouts;
using scdb.Xml.Turbulent;

namespace Loader
{
	public class ShipLoader
	{
		public string OutputFolder { get; set; }
		public string DataRoot { get; set; }

		public List<ShipIndexEntry> Load()
		{
			var turbulentFolder = Path.Combine(DataRoot, @"Data\Libs\Foundry\Records\turbulent\vehicles");
			var spaceshipsFolder = Path.Combine(DataRoot, @"Data\Libs\Foundry\Records\entities\spaceships");
			var vehiclesFolder = Path.Combine(DataRoot, @"Data\Libs\Foundry\Records\entities\groundvehicles");

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

			Directory.CreateDirectory(OutputFolder);

			var index = new List<ShipIndexEntry>();

			foreach (var filename in Directory.EnumerateFiles(turbulentFolder, "*.xml"))
			{
				var entry = GetTurbulentEntry(filename);
				if (UselessEntities.Contains(entry.itemClass)) continue;

				var entityFilename = Path.ChangeExtension(Path.Combine(spaceshipsFolder, entry.itemClass.ToLower()), ".xml");
				if (!File.Exists(entityFilename)) entityFilename = Path.ChangeExtension(Path.Combine(vehiclesFolder, entry.itemClass.ToLower()), ".xml");
				Console.WriteLine(entityFilename);

				var ship = LoadShip(entityFilename);

				if (ship.DefaultLoadout != null) ship.loadout = AddDefaultLoadout(ship.DefaultLoadout.Items);
				else if (ship.Entity.Components.SEntityComponentDefaultLoadoutParams.loadout != null) ship.loadout = AddManualLoadout(ship.Entity.Components.SEntityComponentDefaultLoadoutParams.loadout.SItemPortLoadoutManualParams);

				var json = JsonConvert.SerializeObject(ship, Newtonsoft.Json.Formatting.Indented);
				var jsonFilename = Path.Combine(OutputFolder, $"{ship.Entity.ClassName.ToLower()}.json");
				File.WriteAllText(jsonFilename, json);

				var headlines = new ShipHeadlines
				{
					Crew = ship.Entity.Components.VehicleComponentParams.crewSize,
					TopSpeed = Convert.ToInt32(ship.Entity.Components.IFCSParams?.maxAfterburnSpeed),
					ManuveringSpeed = Convert.ToInt32(ship.Entity.Components.IFCSParams?.maxSpeed)
				};

				var indexEntry = new ShipIndexEntry
				{
					json = Path.GetRelativePath(Path.GetDirectoryName(OutputFolder), jsonFilename),
					@class = ship.Entity.ClassName,
					item = ship.Entity.ClassName.ToLower(),
					kind = entityFilename.StartsWith(spaceshipsFolder) ? "spaceship" : "groundvehicle",
					Type = ship.Entity.Components?.SAttachableComponentParams?.AttachDef.Type,
					SubType = ship.Entity.Components?.SAttachableComponentParams?.AttachDef.SubType,
					Headlines = headlines
				};

				index.Add(indexEntry);
			}

			return index;
		}

		int CalculateScu(Part part)
		{
			if (part == null) return 0;
			return 0;
		}

		TurbulentEntry GetTurbulentEntry(string turbulentXmlFile)
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

		Ship LoadShip(string shipEntityPath)
		{
			EntityClassDefinition shipEntity;
			Vehicle vehicle = null;
			Loadout loadout = null;

			var entityParser = new EntityParser();
			shipEntity = entityParser.Parse(shipEntityPath);
			if (shipEntity == null) return null;

			var vehicleComponent = shipEntity.Components.VehicleComponentParams;
			if (vehicleComponent != null)
			{
				var vehiclePath = vehicleComponent.vehicleDefinition;
				vehiclePath = vehiclePath.Replace('/', '\\');
				vehiclePath = Path.Combine(DataRoot, "Data", vehiclePath);

				var vehicleParser = new VehicleParser();
				vehicle = vehicleParser.Parse(vehiclePath, vehicleComponent.modification);
			}

			var loadoutComponent = shipEntity.Components.SEntityComponentDefaultLoadoutParams;
			if (loadoutComponent != null)
			{
				if (loadoutComponent.loadout.SItemPortLoadoutXMLParams != null)
				{
					var loadoutPath = loadoutComponent.loadout.SItemPortLoadoutXMLParams.loadoutPath;
					loadoutPath = loadoutPath.Replace('/', '\\');
					loadoutPath = Path.Combine(DataRoot, "Data", loadoutPath);

					var loadoutParser = new LoadoutParser();
					loadout = loadoutParser.Parse(loadoutPath);
				}
			}

			return new Ship
			{
				Entity = shipEntity,
				Vehicle = vehicle,
				DefaultLoadout = loadout
			};
		}

		scdb.Models.loadout[] AddDefaultLoadout(scdb.Xml.Loadouts.Item[] defaultLoadoutItems)
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

		scdb.Models.loadout[] AddManualLoadout(scdb.Xml.Entities.SItemPortLoadoutManualParams manual)
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
}
