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
using System.Threading.Tasks;
using Loader.Entries;
using Loader.Parser;
using Loader.SCDb.Xml.Entities;
using Loader.SCDb.Xml.Vehicles;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Loader.Loader
{
	public class ShipLoader
	{
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

		private readonly EntityParser _entityParser;
		private readonly ILogger<ShipLoader> _logger;
		private readonly ServiceOptions _options;

		public ShipLoader(ILogger<ShipLoader> logger, EntityParser entityParser, IOptions<ServiceOptions> options)
		{
			_logger = logger;
			_entityParser = entityParser;
			_options = options.Value;
		}

		public string OutputFolder => Path.Combine(_options.Output, "ships");

		private string DataRoot => _options.SCData;

		private Func<string, Task<string>> OnXmlLoadout { get; set; }

		public async Task<List<Ship>> Load(Func<string, Task<string>> onXmlLoadout)
		{
			OnXmlLoadout = onXmlLoadout;
			Directory.CreateDirectory(OutputFolder);

			var index = new List<Ship>();
			await foreach (var item in LoadAndWriteAsJsonFile(@"Data\Libs\Foundry\Records\entities\spaceships"))
			{
				index.Add(item);
			}

			await foreach (var item in LoadAndWriteAsJsonFile(@"Data\Libs\Foundry\Records\entities\groundvehicles"))
			{
				index.Add(item);
			}

			return index;
		}

		private async IAsyncEnumerable<Ship> LoadAndWriteAsJsonFile(string entityFolder)
		{
			var index = new List<Ship>();

			foreach (var entityFilename in Directory.EnumerateFiles(Path.Combine(DataRoot, entityFolder), "*.xml"))
			{
				if (AvoidFile(entityFilename))
				{
					continue;
				}

				EntityClassDefinition entity = null;
				Vehicle vehicle = null;

				_logger.LogInformation(entityFilename);

				entity = await _entityParser.Parse(entityFilename, OnXmlLoadout);
				if (entity == null)
				{
					continue;
				}

				if (entity.Components.VehicleComponentParams == null)
				{
					_logger.LogWarning("This doesn't seem to be a vehicle");
					continue;
				}

				var vehicleFilename = entity.Components?.VehicleComponentParams?.vehicleDefinition;
				if (vehicleFilename != null)
				{
					vehicleFilename = Path.Combine(DataRoot, "Data", vehicleFilename.Replace('/', '\\'));
					var vehicleModification = entity.Components?.VehicleComponentParams?.modification;
					_logger.LogInformation(vehicleFilename);

					var vehicleParser = new VehicleParser();
					vehicle = vehicleParser.Parse(vehicleFilename, vehicleModification);
				}

				var jsonFilename = Path.Combine(OutputFolder, $"{entity.ClassName.ToLower()}.json");
				var json = JsonConvert.SerializeObject(new
				                                       {
					                                       Raw = new
					                                             {
						                                             Entity = entity,
						                                             Vehicle = vehicle
					                                             }
				                                       });
				await File.WriteAllTextAsync(jsonFilename, json);

				var isGroundVehicle =
					entity.Components?.VehicleComponentParams.vehicleCareer == "@vehicle_focus_ground";
				var isGravlevVehicle = entity.Components?.VehicleComponentParams.isGravlevVehicle ?? false;
				var isSpaceship = !(isGroundVehicle || isGravlevVehicle);
				var indexEntry = new Ship
				                 {
					                 JsonFilename =
						                 Path.GetRelativePath(Path.GetDirectoryName(OutputFolder), jsonFilename),
					                 ClassName = entity.ClassName,
					                 Type = entity.Components?.SAttachableComponentParams?.AttachDef.Type,
					                 SubType = entity.Components?.SAttachableComponentParams?.AttachDef.SubType,
					                 Name = entity.Components.VehicleComponentParams.vehicleName,
					                 Career = entity.Components.VehicleComponentParams.vehicleCareer,
					                 Role = entity.Components.VehicleComponentParams.vehicleRole,
					                 DogFightEnabled =
						                 Convert.ToBoolean(entity.Components.VehicleComponentParams.dogfightEnabled),
					                 Size = vehicle?.size,
					                 IsGroundVehicle = isGroundVehicle,
					                 IsGravlevVehicle = isGravlevVehicle,
					                 IsSpaceship = isSpaceship,
					                 NoParts = vehicle?.Parts == null || vehicle.Parts.Length == 0
				                 };

				yield return indexEntry;
			}
		}

		private bool AvoidFile(string filename)
		{
			var fileSplit = Path.GetFileNameWithoutExtension(filename).Split('_');
			return fileSplit.Any(part => _avoids.Contains(part));
		}
	}
}
