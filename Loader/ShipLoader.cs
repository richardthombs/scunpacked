using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

using scdb.Xml.Entities;
using scdb.Xml.Vehicles;

namespace Loader
{
	public class ShipLoader
	{
		public string OutputFolder { get; set; }
		public string DataRoot { get; set; }
		public Func<string, string> OnXmlLoadout { get; set; }

		string[] avoids =
		{
			// CIG tags
			"pu",
			"ai",
			"civ",
			"qig",
			"crim",
			"pir",
			"template",
			"wreck",
			"piano",
			"swarm",
			"nointerior",
			"s42",

			// Skin variants
			"pink",
			"yellow",
			"emerald",
			"dunestalker",
			"snowblind",
			"shipshowdown",
			"showdown",
			"citizencon2018",
			"citizencon",
			"pirate"
		};

		public List<ShipIndexEntry> Load()
		{
			Directory.CreateDirectory(OutputFolder);

			var index = new List<ShipIndexEntry>();
			index.AddRange(Load(@"Data\Libs\Foundry\Records\entities\spaceships"));
			index.AddRange(Load(@"Data\Libs\Foundry\Records\entities\groundvehicles"));
			return index;
		}

		public List<ShipIndexEntry> Load(string entityFolder)
		{
			var index = new List<ShipIndexEntry>();

			foreach (var entityFilename in Directory.EnumerateFiles(Path.Combine(DataRoot, entityFolder), "*.xml"))
			{
				if (avoidFile(entityFilename)) continue;

				EntityClassDefinition entity = null;
				Vehicle vehicle = null;
				string vehicleModification = null;

				Console.WriteLine(entityFilename);

				// Entity
				var entityParser = new EntityParser();
				entity = entityParser.Parse(entityFilename, OnXmlLoadout);
				if (entity == null) continue;

				// Vehicle
				var vehicleFilename = entity.Components?.VehicleComponentParams?.vehicleDefinition;
				if (vehicleFilename != null)
				{
					vehicleFilename = Path.Combine(DataRoot, "Data", vehicleFilename.Replace('/', '\\'));
					vehicleModification = entity.Components?.VehicleComponentParams?.modification;
					Console.WriteLine(vehicleFilename);

					var vehicleParser = new VehicleParser();
					vehicle = vehicleParser.Parse(vehicleFilename, vehicleModification);
				}

				var jsonFilename = Path.Combine(OutputFolder, $"{entity.ClassName.ToLower()}.json");
				var json = JsonConvert.SerializeObject(new
				{
					Raw = new
					{
						Entity = entity,
						Vehicle = vehicle,
					}
				});
				File.WriteAllText(jsonFilename, json);

				bool isGroundVehicle = entity.Components?.VehicleComponentParams.vehicleCareer == "@vehicle_focus_ground";
				bool isGravlevVehicle = entity.Components?.VehicleComponentParams.isGravlevVehicle ?? false;
				bool isSpaceship = !(isGroundVehicle || isGravlevVehicle);
				var indexEntry = new ShipIndexEntry
				{
					jsonFilename = Path.GetRelativePath(Path.GetDirectoryName(OutputFolder), jsonFilename),
					className = entity.ClassName,
					type = entity.Components?.SAttachableComponentParams?.AttachDef.Type,
					subType = entity.Components?.SAttachableComponentParams?.AttachDef.SubType,
					name = entity.Components.VehicleComponentParams.vehicleName,
					career = entity.Components.VehicleComponentParams.vehicleCareer,
					role = entity.Components.VehicleComponentParams.vehicleRole,
					dogFightEnabled = Convert.ToBoolean(entity.Components.VehicleComponentParams.dogfightEnabled),
					size = vehicle?.size,
					isGroundVehicle = isGroundVehicle,
					isGravlevVehicle = isGravlevVehicle,
					isSpaceship = isSpaceship
				};

				index.Add(indexEntry);
			}

			return index;
		}

		bool avoidFile(string filename)
		{
			var fileSplit = Path.GetFileNameWithoutExtension(filename).Split('_');
			return fileSplit.Any(part => avoids.Contains(part));
		}
	}
}
