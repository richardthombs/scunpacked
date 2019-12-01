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
			Console.WriteLine("Hello World!");

			var s = new Serialise();
			s.Run();

			var parser = new ShipParser
			{
				InputRoot = @"..\all"
			};

			var ship = parser.Parse(@"..\all\Data\Libs\Foundry\Records\entities\spaceships\aegs_avenger_stalker.xml");

			var json = JsonConvert.SerializeObject(ship, Newtonsoft.Json.Formatting.Indented);
			File.WriteAllText("ship.json", json);
		}
	}

	public class Serialise
	{
		public void Run()
		{
			var entity = new EntityClassDefinition
			{
				Components = new Component[]
				{
					new SEntityComponentDefaultLoadoutParams
					{
						loadout = new loadout
						{
							SItemPortLoadoutXMLParams =
						new SItemPortLoadoutXMLParams
						{
							loadoutPath = "Hello, World!"
						}
						}
					}
				}
			};

			XmlSerializer s = new XmlSerializer(typeof(EntityClassDefinition));
			using (TextWriter writer = new StreamWriter("testout.xml"))
			{
				s.Serialize(writer, entity);
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
		readonly String[] allCaps = { "PU", "AI", "CRIM", "CIV", "QIG", "PIR", "SEC", "UEE" };
		public string InputRoot { get; set; }

		public Ship Parse(string shipEntityPath)
		{
			EntityClassDefinition shipEntity;
			Vehicle vehicle = null;
			Loadout loadout = null;

			shipEntity = ParseShipDefinition(shipEntityPath);

			var vehicleComponent = shipEntity.Components.First(x => x.GetType() == typeof(VehicleComponentParams)) as VehicleComponentParams;
			if (vehicleComponent != null)
			{
				var vehiclePath = vehicleComponent.vehicleDefinition;
				vehiclePath = vehiclePath.Replace('/', '\\');
				vehiclePath = Path.Combine(InputRoot, "Data", vehiclePath);
				vehicle = ParseVehicle(vehiclePath, vehicleComponent.modification);
			}

			var loadoutComponent = shipEntity.Components.First(x => x.GetType() == typeof(SEntityComponentDefaultLoadoutParams)) as SEntityComponentDefaultLoadoutParams;
			if (loadoutComponent != null)
			{
				var loadoutPath = loadoutComponent.loadout.SItemPortLoadoutXMLParams.loadoutPath;
				loadoutPath = loadoutPath.Replace('/', '\\');
				loadoutPath = Path.Combine(InputRoot, "Data", loadoutPath);
				loadout = ParseLoadout(loadoutPath);

				var flightControllerItemName = loadout.Items.First(x => x.portName == "hardpoint_controller_flight")?.itemName;
				Console.WriteLine(flightControllerItemName);
			}

			return new Ship
			{
				Entity = shipEntity,
				Vehicle = vehicle,
				Loadout = loadout
			};
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
