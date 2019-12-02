using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Globalization;
using System.Linq;

using Newtonsoft.Json;

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
	}

	public class Ship
	{
		public EntityClassDefinition Entity { get; set; }
		public Vehicle Vehicle { get; set; }
		public Loadout Loadout { get; set; }
	}

	public class ShipParser
	{
		readonly String[] allCaps = { "PU", "AI", "CRIM", "CIV", "QIG", "PIR", "SEC", "UEE", "C8X" };
		public string InputRoot { get; set; }

		public Ship Parse(string shipEntityPath, string shipEntityClass)
		{
			EntityClassDefinition shipEntity;
			Vehicle vehicle = null;
			Loadout loadout = null;

			Console.WriteLine($"Ship entity file: {shipEntityPath}");

			shipEntity = ParseShipDefinition(shipEntityPath, shipEntityClass);
			if (shipEntity == null) return null;

			var vehicleComponent = shipEntity.Components.First(x => x.GetType() == typeof(VehicleComponentParams)) as VehicleComponentParams;
			if (vehicleComponent != null)
			{
				Console.WriteLine($"Vehicle definition: {vehicleComponent.vehicleDefinition}");

				var vehiclePath = vehicleComponent.vehicleDefinition;
				vehiclePath = vehiclePath.Replace('/', '\\');
				vehiclePath = Path.Combine(InputRoot, "Data", vehiclePath);
				vehicle = ParseVehicle(vehiclePath, vehicleComponent.modification);
			}

			var loadoutComponent = shipEntity.Components.First(x => x.GetType() == typeof(SEntityComponentDefaultLoadoutParams)) as SEntityComponentDefaultLoadoutParams;
			if (loadoutComponent != null)
			{
				if (loadoutComponent.loadout.SItemPortLoadoutXMLParams != null)
				{

					Console.WriteLine($"Loadout XML file: {loadoutComponent.loadout.SItemPortLoadoutXMLParams.loadoutPath}");
					var loadoutPath = loadoutComponent.loadout.SItemPortLoadoutXMLParams.loadoutPath;
					loadoutPath = loadoutPath.Replace('/', '\\');
					loadoutPath = Path.Combine(InputRoot, "Data", loadoutPath);
					loadout = ParseLoadout(loadoutPath);
				}
				if (loadoutComponent.loadout.SItemPortLoadoutManualParams != null)
				{
					Console.WriteLine($"Loadout hardcoded in entity definition");
					//throw new NotImplementedException();
				}

				if (loadout != null)
				{
					Console.WriteLine($"{CountLoadout(loadout.Items):n0} items in the loadout");
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
				Loadout = loadout
			};
		}

		int CountLoadout(Item[] items)
		{
			if (items == null) return 0;
			var count = items.Length;
			foreach (var item in items)
			{
				if (item.Items != null) count += CountLoadout(item.Items);
			}
			return count;
		}

		EntityClassDefinition ParseShipDefinition(string shipEntityPath, string shipEntityClass)
		{
			if (!File.Exists(shipEntityPath))
			{
				Console.WriteLine("Ship entity definition file does not exist");
				return null;
			}

			var shipEntityName = $"EntityClassDefinition.{shipEntityClass}";
			Console.WriteLine($"Expecting ship entity root node to be: {shipEntityName}");

			var xml = File.ReadAllText(shipEntityPath);
			var doc = new XmlDocument();
			doc.LoadXml(xml);

			var serialiser = new XmlSerializer(typeof(EntityClassDefinition), new XmlRootAttribute { ElementName = shipEntityName });
			using (var stream = new XmlNodeReader(doc))
			{
				var entity = (EntityClassDefinition)serialiser.Deserialize(stream);
				return entity;
			}
		}

		Vehicle ParseVehicle(string vehiclePath, string modification)
		{
			if (!File.Exists(vehiclePath))
			{
				Console.WriteLine("Vehicle implementation file does not exist");
				return null;
			}

			var xml = File.ReadAllText(vehiclePath);
			var doc = new XmlDocument();
			doc.LoadXml(xml);

			var serialiser = new XmlSerializer(typeof(Vehicle));
			using (var stream = new XmlNodeReader(doc))
			{
				var vehicle = (Vehicle)serialiser.Deserialize(stream);
				return vehicle;
			}
		}

		Loadout ParseLoadout(string loadoutPath)
		{
			if (!File.Exists(loadoutPath))
			{
				Console.WriteLine("Loadout file does not exist");
				return null;
			}

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
