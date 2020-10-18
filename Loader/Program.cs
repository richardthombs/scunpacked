using NDesk.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Loader
{
	class Program
	{
		static void Main(string[] args)
		{
			string scDataRoot = null;
			string outputRoot = null;
			string itemFile = null;
			bool loadItems = true;
			bool loadShips = true;
			bool loadShops = true;
			bool loadStarmap = true;

			var p = new OptionSet
			{
				{ "scdata=", v => scDataRoot = v },
				{ "input=",  v => scDataRoot = v },
				{ "output=",  v => outputRoot = v },
				{ "itemfile=", v => itemFile = v },
				{ "noitems", v => loadItems = false },
				{ "noships", v=> loadShips = false },
				{ "noshops", v => loadShops = false },
				{ "nostarmap", v => loadStarmap = false }
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
				Console.WriteLine(" or Loader.exe -itemfile=<path to an SCItem XML file");
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
			if (Directory.Exists(outputRoot))
			{
				var info = new DirectoryInfo(outputRoot);
				foreach (var file in info.GetFiles()) file.Delete();
				foreach (var dir in info.GetDirectories()) dir.Delete(true);
			}
			else Directory.CreateDirectory(outputRoot);

			// A loadout loader to help with any XML loadouts we encounter while parsing entities
			var loadoutLoader = new LoadoutLoader
			{
				OutputFolder = Path.Combine(outputRoot, "loadouts"),
				DataRoot = scDataRoot
			};

			// Localisation
			Console.WriteLine("Load Localisation");
			var labels = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			using (var ini = new StreamReader(Path.Combine(scDataRoot, @"Data\Localization\english\global.ini")))
			{
				for (var line = ini.ReadLine(); line != null; line = ini.ReadLine())
				{
					var split = line.Split('=', 2);
					labels.Add(split[0], split[1]);
				}
			}
			File.WriteAllText(Path.Combine(outputRoot, "labels.json"), JsonConvert.SerializeObject(labels));

			// Manufacturers
			Console.WriteLine("Load Manufacturers");
			var manufacturerLoader = new ManufacturerLoader(new LocalisationService(labels))
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
			if (loadItems)
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
				var itemIndex = itemLoader.Load();
			}

			// Ships and ground vehicles
			if (loadShips)
			{
				Console.WriteLine("Load Ships and ground vehicles");
				var shipLoader = new ShipLoader
				{
					OutputFolder = outputRoot,
					DataRoot = scDataRoot,
					OnXmlLoadout = path => loadoutLoader.Load(path),
					Manufacturers = manufacturerIndex
				};
				var shipIndex = shipLoader.Load();
			}

			// Prices
			if (loadShops)
			{
				Console.WriteLine("Load Shops");
				var shopLoader = new ShopLoader(new LocalisationService(labels))
				{
					OutputFolder = outputRoot,
					DataRoot = scDataRoot
				};
				var shops = shopLoader.Load();
			}

			// Starmap
			if (loadStarmap)
			{
				Console.WriteLine("Load Starmap");
				var starmapLoader = new StarmapLoader(new LocalisationService(labels))
				{
					OutputFolder = outputRoot,
					DataRoot = scDataRoot
				};
				var starmapIndex = starmapLoader.Load();
			}
		}
	}
}
