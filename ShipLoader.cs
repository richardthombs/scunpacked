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

namespace shipparser
{
	public class ShipLoader
	{
		public string OutputFolder { get; set; }
		public string DataRoot { get; set; }

		public void Load()
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

			var shipList = new List<ShipIndex>();

			foreach (var filename in Directory.EnumerateFiles(turbulentFolder, "*.xml"))
			{
				var entry = GetTurbulentEntry(filename);
				Console.WriteLine($"{filename}: {entry.turbulentName}, {entry.itemClass}");
				if (UselessEntities.Contains(entry.itemClass)) continue;

				var entityFilename = Path.ChangeExtension(Path.Combine(spaceshipsFolder, entry.itemClass.ToLower()), ".xml");
				if (!File.Exists(entityFilename)) entityFilename = Path.ChangeExtension(Path.Combine(vehiclesFolder, entry.itemClass.ToLower()), ".xml");

				var entityClassName = entry.itemClass;

				var ship = LoadShip(entityFilename, entityClassName);

				if (ship.DefaultLoadout != null) ship.loadout = AddDefaultLoadout(ship.DefaultLoadout.Items);
				else if (ship.Entity.Components.SEntityComponentDefaultLoadoutParams.loadout != null) ship.loadout = AddManualLoadout(ship.Entity.Components.SEntityComponentDefaultLoadoutParams.loadout.SItemPortLoadoutManualParams);

				var json = JsonConvert.SerializeObject(ship, Newtonsoft.Json.Formatting.Indented);
				File.WriteAllText(Path.Combine(OutputFolder, $"{entityClassName}.json"), json);

				shipList.Add(new ShipIndex
				{
					filename = Path.GetFileNameWithoutExtension(filename),
					itemClass = entry.itemClass,
					turbulentName = entry.turbulentName
				});
			}

			File.WriteAllText(Path.Combine(OutputFolder, "ships.json"), JsonConvert.SerializeObject(shipList, Newtonsoft.Json.Formatting.Indented));
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

		Ship LoadShip(string shipEntityPath, string shipEntityClass)
		{
			EntityClassDefinition shipEntity;
			Vehicle vehicle = null;
			Loadout loadout = null;

			Console.WriteLine($"Ship entity file: {shipEntityPath}");

			var entityParser = new EntityParser();
			shipEntity = entityParser.Parse(shipEntityPath, shipEntityClass);
			if (shipEntity == null) return null;

			var vehicleComponent = shipEntity.Components.VehicleComponentParams;
			if (vehicleComponent != null)
			{
				Console.WriteLine($"Vehicle definition: {vehicleComponent.vehicleDefinition}");

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

					Console.WriteLine($"Loadout XML file: {loadoutComponent.loadout.SItemPortLoadoutXMLParams.loadoutPath}");
					var loadoutPath = loadoutComponent.loadout.SItemPortLoadoutXMLParams.loadoutPath;
					loadoutPath = loadoutPath.Replace('/', '\\');
					loadoutPath = Path.Combine(DataRoot, "Data", loadoutPath);

					var loadoutParser = new LoadoutParser();
					loadout = loadoutParser.Parse(loadoutPath);
				}
				if (loadoutComponent.loadout.SItemPortLoadoutManualParams != null)
				{
					Console.WriteLine($"Loadout hardcoded in entity definition");
				}

				if (loadout != null)
				{
					var flightControllerItemName = loadout.Items.FirstOrDefault(x => x.portName == "hardpoint_controller_flight")?.itemName;
					if (flightControllerItemName != null) Console.WriteLine(flightControllerItemName);
					else Console.WriteLine("No flight controller!");
				}
				else
				{
					Console.WriteLine("No loadout");
				}
			}
			else
			{
				throw new ApplicationException("No loadout");
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
