using System;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;
using NDesk.Options;
using scdb.Xml.Entities;

namespace Loader
{
	class Program
	{
		static void Main(string[] args)
		{
			string scDataRoot = null;
			string outputRoot = null;
			string itemFile = null;

			var p = new OptionSet
			{
				{ "scdata=", v => scDataRoot = v },
				{ "input=",  v => scDataRoot = v },
				{ "output=",  v => outputRoot = v },
				{ "itemfile=", v => itemFile = v }
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
			var manufacturerLoader = new ManufacturerLoader(new LocalisationService(labels))
			{
				DataRoot = scDataRoot
			};
			var manufacturerIndex = manufacturerLoader.Load();
			File.WriteAllText(Path.Combine(outputRoot, "manufacturers.json"), JsonConvert.SerializeObject(manufacturerIndex));

			// Ships and ground vehicles
			var shipLoader = new ShipLoader
			{
				OutputFolder = Path.Combine(outputRoot, "ships"),
				DataRoot = scDataRoot,
				OnXmlLoadout = path => loadoutLoader.Load(path)
			};
			var shipIndex = shipLoader.Load();

			File.WriteAllText(Path.Combine(outputRoot, "ships.json"), JsonConvert.SerializeObject(shipIndex));

			// Ammunition
			var ammoLoader = new AmmoLoader
			{
				OutputFolder = Path.Combine(outputRoot, "ammo"),
				DataRoot = scDataRoot
			};
			var ammoIndex = ammoLoader.Load();
			File.WriteAllText(Path.Combine(outputRoot, "ammo.json"), JsonConvert.SerializeObject(ammoIndex));

			// Items
			var itemLoader = new ItemLoader
			{
				OutputFolder = Path.Combine(outputRoot, "items"),
				DataRoot = scDataRoot,
				OnXmlLoadout = path => loadoutLoader.Load(path),
				Manufacturers = manufacturerIndex,
				Ammo = ammoIndex
			};
			var itemIndex = itemLoader.Load();
			File.WriteAllText(Path.Combine(outputRoot, "items.json"), JsonConvert.SerializeObject(itemIndex));

			// Create an index file for each different item type
			var typeIndicies = new Dictionary<string, List<ItemIndexEntry>>();
			foreach (var entry in itemIndex)
			{
				if (String.IsNullOrEmpty(entry.classification)) continue;

				var type = entry.classification.Split('.')[0];
				if (!typeIndicies.ContainsKey(type)) typeIndicies.Add(type, new List<ItemIndexEntry>());
				var typeIndex = typeIndicies[type];
				typeIndex.Add(entry);
			}
			foreach (var pair in typeIndicies)
			{
				File.WriteAllText(Path.Combine(outputRoot, pair.Key.ToLower() + "-items.json"), JsonConvert.SerializeObject(pair.Value));
			}

			// Prices
			var shopLoader = new ShopLoader(new LocalisationService(labels))
			{
				DataRoot = scDataRoot
			};
			var shops = shopLoader.Load();
			File.WriteAllText(Path.Combine(outputRoot, "shops.json"), JsonConvert.SerializeObject(shops));

			// Starmap
			var starmapLoader = new StarmapLoader(new LocalisationService(labels))
			{
				DataRoot = scDataRoot
			};
			var starmapIndex = starmapLoader.Load();
			File.WriteAllText(Path.Combine(outputRoot, "starmap.json"), JsonConvert.SerializeObject(starmapIndex));
		}
	}
}
