using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;
using NDesk.Options;

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
				var entityParser = new EntityParser();
				var entity = entityParser.Parse(itemFile, x => x);
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
			var manufacturerLoader = new ManufacturerLoader
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

			// Items that go on ships
			var itemLoader = new ItemLoader
			{
				OutputFolder = Path.Combine(outputRoot, "items"),
				DataRoot = scDataRoot,
				OnXmlLoadout = path => loadoutLoader.Load(path),
				Manufacturers = manufacturerIndex
			};
			var itemIndex = itemLoader.Load();
			File.WriteAllText(Path.Combine(outputRoot, "items.json"), JsonConvert.SerializeObject(itemIndex));

			// Prices
			var shopLoader = new ShopLoader(new LocalisationService(labels))
			{
				DataRoot = scDataRoot,
				OnXmlLoadout = path => loadoutLoader.Load(path)
			};
			var shops = shopLoader.Load();
			File.WriteAllText(Path.Combine(outputRoot, "shops.json"), JsonConvert.SerializeObject(shops));
		}
	}
}
