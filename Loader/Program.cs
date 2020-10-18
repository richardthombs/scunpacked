using System;
using System.Collections.Generic;
using System.IO;

using NDesk.Options;
using Newtonsoft.Json;

namespace Loader
{
	class Program
	{
		static void Main(string[] args)
		{
			string scDataRoot = null;
			string outputRoot = null;
			string itemFile = null;
			bool shipsOnly = false;

			var p = new OptionSet
			{
				{ "scdata=", v => scDataRoot = v },
				{ "input=",  v => scDataRoot = v },
				{ "output=",  v => outputRoot = v },
				{ "itemfile=", v => itemFile = v },
				{ "shipsonly", v => shipsOnly = true }
			};

			var extra = p.Parse(args);

			var badArgs = false;
			if (extra.Count > 0) badArgs = true;
			else if (!String.IsNullOrEmpty(itemFile) && (!String.IsNullOrEmpty(scDataRoot) || !String.IsNullOrEmpty(outputRoot))) badArgs = true;
			else if (String.IsNullOrEmpty(itemFile) && (String.IsNullOrEmpty(scDataRoot) || String.IsNullOrEmpty(outputRoot))) badArgs = true;

			if (badArgs)
			{
				Console.WriteLine("Usage:");
				Console.WriteLine("    Loader.exe -input=<path to extracted Star Citizen data> -output=<path to JSON output folder>");
				Console.WriteLine(" or Loader.exe -itemfile=<path to an SCItem XML file>");
				Console.WriteLine();
				return;
			}

			JsonConvert.DefaultSettings = () => new JsonSerializerSettings
			{
				Formatting = Formatting.Indented,
				NullValueHandling = NullValueHandling.Ignore
			};

			if (itemFile != null)
			{
				var entityParser = new ClassParser<scdb.Xml.Entities.EntityClassDefinition>();
				var entity = entityParser.Parse(itemFile);
				var json = JsonConvert.SerializeObject(entity);
				Console.WriteLine(json);
				return;
			}

			// Prep the output folder
			if (Directory.Exists(outputRoot) && !shipsOnly)
			{
				var info = new DirectoryInfo(outputRoot);
				foreach (var file in info.GetFiles()) file.Delete();
				foreach (var dir in info.GetDirectories()) dir.Delete(true);
			}
			else Directory.CreateDirectory(outputRoot);

			// A loadout loader to help with any XML loadouts we encounter while parsing entities
			var loadoutLoader = new LoadoutLoader
			{
				OutputFolder = outputRoot,
				DataRoot = scDataRoot
			};

			// Localisation
			Console.WriteLine("Load Localisation");
			var labelLoader = new LabelsLoader
			{
				OutputFolder = outputRoot,
				DataRoot = scDataRoot
			};
			var labels = labelLoader.Load("english");
			var localisationSvc = new LocalisationService(labels);

			// Manufacturers
			Console.WriteLine("Load Manufacturers");
			var manufacturerLoader = new ManufacturerLoader(localisationSvc)
			{
				OutputFolder = outputRoot,
				DataRoot = scDataRoot
			};
			var manufacturerIndex = manufacturerLoader.Load();

			// Ammunition
			Console.WriteLine("Load Ammunition");
			var ammoLoader = new AmmoLoader
			{
				OutputFolder = outputRoot,
				DataRoot = scDataRoot
			};
			var ammoIndex = ammoLoader.Load();

			// Items
			if (!shipsOnly)
			{
				Console.WriteLine("Load Items");
				var itemLoader = new ItemLoader
				{
					OutputFolder = outputRoot,
					DataRoot = scDataRoot,
					OnXmlLoadout = path => loadoutLoader.Load(path),
					Manufacturers = manufacturerIndex,
					Ammo = ammoIndex
				};
				itemLoader.Load();
			}

			// Ships and vehicles
			Console.WriteLine("Load Ships and Vehicles");
			var shipLoader = new ShipLoader
			{
				OutputFolder = outputRoot,
				DataRoot = scDataRoot,
				OnXmlLoadout = path => loadoutLoader.Load(path),
				Manufacturers = manufacturerIndex
			};
			shipLoader.Load();

			// Prices
			if (!shipsOnly)
			{
				Console.WriteLine("Load Shops");
				var shopLoader = new ShopLoader(localisationSvc)
				{
					OutputFolder = outputRoot,
					DataRoot = scDataRoot
				};
				shopLoader.Load();
			}

			// Starmap
			if (!shipsOnly)
			{
				Console.WriteLine("Load Starmap");
				var starmapLoader = new StarmapLoader(localisationSvc)
				{
					OutputFolder = outputRoot,
					DataRoot = scDataRoot
				};
				starmapLoader.Load();
			}

			Console.WriteLine("Finished!");
		}
	}
}
