//-----------------------------------------------------------------------
// <copyright file="D:\projekte\scunpacked\Loader\ShipLoader.cs" company="primsoft.NET">
// Author: Joerg Primke
// Copyright (c) primsoft.NET. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Loader.SCDb.Xml.Entities;
using Loader.SCDb.Xml.Vehicles;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Loader
{
	public class ShipLoader
	{
		private readonly ILogger<ShipLoader> _logger;
		private readonly EntityParser _entityParser;

		private readonly string[] _avoids =
		{
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
			"hijacked",
			"comms",
			"pink",
			"yellow",
			"emerald",
			"dunestalker",
			"snowblind",
			"shipshowdown",
			"showdown",
			"citizencon2018",
			"citizencon",
			"pirate",
			"talus",
			"carbon"
		};

		public string OutputFolder { get; set; }

		public string DataRoot { get; set; }

		public Func<string, string> OnXmlLoadout { get; set; }

		public ShipLoader(ILogger<ShipLoader> logger, EntityParser entityParser)
		{
			_logger = logger;
			_entityParser = entityParser;
		}

		public List<ShipIndexEntry> Load()
		{
			Directory.CreateDirectory(OutputFolder);

			var index = new List<ShipIndexEntry>();
			index.AddRange(Load(@"Data\Libs\Foundry\Records\entities\spaceships"));
			index.AddRange(Load(@"Data\Libs\Foundry\Records\entities\groundvehicles"));
			return index;
		}

		private List<ShipIndexEntry> Load(string entityFolder)
		{
			var index = new List<ShipIndexEntry>();

			foreach (var entityFilename in Directory.EnumerateFiles(Path.Combine(DataRoot, entityFolder), "*.xml"))
			{
				if (AvoidFile(entityFilename))
					continue;

				EntityClassDefinition entity = null;
				Vehicle vehicle = null;

				Console.WriteLine(entityFilename);

				entity = _entityParser.Parse(entityFilename, OnXmlLoadout);
				if (entity == null)
					continue;

				if (entity.Components.VehicleComponentParams == null)
				{
					Console.WriteLine("This doesn't seem to be a vehicle");
					continue;
				}

				var vehicleFilename = entity.Components?.VehicleComponentParams?.vehicleDefinition;
				if (vehicleFilename != null)
				{
					vehicleFilename = Path.Combine(DataRoot, "Data", vehicleFilename.Replace('/', '\\'));
					var vehicleModification = entity.Components?.VehicleComponentParams?.modification;
					Console.WriteLine(vehicleFilename);

					var vehicleParser = new VehicleParser();
					vehicle = vehicleParser.Parse(vehicleFilename, vehicleModification);
				}

				var jsonFilename = Path.Combine(OutputFolder, $"{entity.ClassName.ToLower()}.json");
				var json = JsonConvert.SerializeObject(new {Raw = new {Entity = entity, Vehicle = vehicle}});
				File.WriteAllText(jsonFilename, json);

				var isGroundVehicle =
					entity.Components?.VehicleComponentParams.vehicleCareer == "@vehicle_focus_ground";
				var isGravlevVehicle = entity.Components?.VehicleComponentParams.isGravlevVehicle ?? false;
				var isSpaceship = !(isGroundVehicle || isGravlevVehicle);
				var indexEntry = new ShipIndexEntry
				                 {
					                 jsonFilename =
						                 Path.GetRelativePath(Path.GetDirectoryName(OutputFolder), jsonFilename),
					                 className = entity.ClassName,
					                 type = entity.Components?.SAttachableComponentParams?.AttachDef.Type,
					                 subType = entity.Components?.SAttachableComponentParams?.AttachDef.SubType,
					                 name = entity.Components.VehicleComponentParams.vehicleName,
					                 career = entity.Components.VehicleComponentParams.vehicleCareer,
					                 role = entity.Components.VehicleComponentParams.vehicleRole,
					                 dogFightEnabled =
						                 Convert.ToBoolean(entity.Components.VehicleComponentParams.dogfightEnabled),
					                 size = vehicle?.size,
					                 isGroundVehicle = isGroundVehicle,
					                 isGravlevVehicle = isGravlevVehicle,
					                 isSpaceship = isSpaceship,
					                 noParts = vehicle?.Parts == null || vehicle.Parts.Length == 0
				                 };

				index.Add(indexEntry);
			}

			return index;
		}

		private bool AvoidFile(string filename)
		{
			var fileSplit = Path.GetFileNameWithoutExtension(filename).Split('_');
			return fileSplit.Any(part => _avoids.Contains(part));
		}
	}
}
