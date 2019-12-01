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
			var parser = new ShipParser
			{
				InputRoot = @"c:\dev\scdata\3.7.2"
			};

			//var entityName = "aegs_sabre.xml";
			var entityName = "anvl_c8x_pisces_expedition.xml";
			if (args.Length == 1) entityName = args[0];

			var ship = parser.Parse(Path.Combine(@"Data\Libs\Foundry\Records\entities\spaceships", entityName));
			var json = JsonConvert.SerializeObject(ship, Newtonsoft.Json.Formatting.Indented);
			File.WriteAllText("ship.json", json);
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

		public Ship Parse(string shipEntityPath)
		{
			EntityClassDefinition shipEntity;
			Vehicle vehicle = null;
			Loadout loadout = null;

			Console.WriteLine($"Ship entity file: {shipEntityPath}");

			shipEntity = ParseShipDefinition(Path.Combine(InputRoot, shipEntityPath));

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
					throw new NotImplementedException();
				}

				var flightControllerItemName = loadout.Items.First(x => x.portName == "hardpoint_controller_flight")?.itemName;
				Console.WriteLine(flightControllerItemName);
				Console.WriteLine($"{CountLoadout(loadout.Items):n0} items in the loadout");
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

		EntityClassDefinition ParseShipDefinition(string shipEntityPath)
		{
			var textInfo = new CultureInfo("en-US").TextInfo;

			// Try and guess the root entity name from the filename
			var shipEntityName = Path.GetFileNameWithoutExtension(shipEntityPath);
			var parts = shipEntityName.Split("_");

			for (int p = 0; p < parts.Length; p++)
			{
				var value = parts[p];
				if (p == 0) value = value.ToUpper();
				else
				{
					if (allCaps.Contains(value.ToUpper())) value = value.ToUpper();
					else value = textInfo.ToTitleCase(value);
				}
				parts[p] = value;
			}

			shipEntityName = $"EntityClassDefinition.{String.Join('_', parts)}";

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
