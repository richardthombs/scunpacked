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
			string outputRoot = null; ;

			var p = new OptionSet
			{
				{ "scdata=", v => { scDataRoot = v; } },
				{ "output=",  v => { outputRoot = v; } }
			};

			var extra = p.Parse(args);

			if (extra.Count > 0 || String.IsNullOrWhiteSpace(scDataRoot) || String.IsNullOrWhiteSpace(outputRoot))
			{
				Console.WriteLine("Usage: Loader.exe -scdata=<path to extracted Star Citizen data> -output=<path to JSON output folder>");
				return;
			}

			JsonConvert.DefaultSettings = () => new JsonSerializerSettings
			{
				Formatting = Formatting.Indented,
				NullValueHandling = NullValueHandling.Ignore
			};

			// Prep the output folder
			if (Directory.Exists(outputRoot))
			{
				var info = new DirectoryInfo(outputRoot);
				foreach (var file in info.GetFiles()) file.Delete();
				foreach (var dir in info.GetDirectories()) dir.Delete(true);
			}
			else Directory.CreateDirectory(outputRoot);

			// Prices
			var priceLoader = new ShopLoader
			{
				DataRoot = scDataRoot
			};

			priceLoader.Load();
			// Localisation
			var labels = new Dictionary<string, string>();
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

			// A loadout loader to help with any XML loadouts we encounter while parsing ships
			var loadoutLoader = new LoadoutLoader
			{
				OutputFolder = Path.Combine(outputRoot, "loadouts"),
				DataRoot = scDataRoot
			};

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

		}
	}
}
