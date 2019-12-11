using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

using scdb.Xml.Entities;
using scdb.Xml.Vehicles;
using scdb.Xml.DefaultLoadouts;
using scdb.Xml.Turbulent;
using scdb.Models;

namespace Loader
{
	public class ShipLoader
	{
		public string OutputFolder { get; set; }
		public string DataRoot { get; set; }
		public Func<string, string> OnXmlLoadout { get; set; }


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
				EntityClassDefinition entity = null;
				Vehicle vehicle = null;
				string vehicleModification = null;

				var entry = GetTurbulentEntry(filename);
				if (UselessEntities.Contains(entry.itemClass)) continue;

				var entityFilename = Path.ChangeExtension(Path.Combine(spaceshipsFolder, entry.itemClass.ToLower()), ".xml");
				if (!File.Exists(entityFilename)) entityFilename = Path.ChangeExtension(Path.Combine(vehiclesFolder, entry.itemClass.ToLower()), ".xml");
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

				var indexEntry = new ShipIndexEntry
				{
					JsonFilename = Path.GetRelativePath(Path.GetDirectoryName(OutputFolder), jsonFilename),
					ClassName = entity.ClassName,
					ItemName = entity.ClassName.ToLower(),
					Type = entity.Components?.SAttachableComponentParams?.AttachDef.Type,
					SubType = entity.Components?.SAttachableComponentParams?.AttachDef.SubType,
					EntityFilename = Path.GetRelativePath(DataRoot, entityFilename),
					VehicleFilename = vehicleFilename != null ? Path.GetRelativePath(DataRoot, vehicleFilename) : null,
					Entity = entity,
					Vehicle = vehicle
				};

				index.Add(indexEntry);
			}

			return index;
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
	}
}
