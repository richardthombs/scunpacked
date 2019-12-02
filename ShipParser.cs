using System;
using System.IO;
using System.Linq;

using scdb.Xml.Entities;
using scdb.Xml.Loadouts;
using scdb.Xml.Vehicles;

namespace shipparser
{
	public class ShipParser
	{
		public string InputRoot { get; set; }

		public Ship Parse(string shipEntityPath, string shipEntityClass)
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
				vehiclePath = Path.Combine(InputRoot, "Data", vehiclePath);

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
					loadoutPath = Path.Combine(InputRoot, "Data", loadoutPath);

					var loadoutParser = new LoadoutParser();
					loadout = loadoutParser.Parse(loadoutPath);
				}
				if (loadoutComponent.loadout.SItemPortLoadoutManualParams != null)
				{
					Console.WriteLine($"Loadout hardcoded in entity definition");
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
				DefaultLoadout = loadout
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

	}
}
