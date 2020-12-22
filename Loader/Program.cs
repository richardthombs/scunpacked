using System;
using System.Collections.Generic;
using System.IO;

using NDesk.Options;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Loader
{
	class Program
	{
		static void Main(string[] args)
		{
			string scDataRoot = null;
			string outputRoot = null;
			bool doShips = true;
			bool doItems = true;
			bool doShops = true;
			bool doStarmap = true;
			bool noCache = false;
			string typeFilter = null;
			string shipFilter = null;

			var p = new OptionSet
			{
				{ "scdata=", v => scDataRoot = v },
				{ "input=",  v => scDataRoot = v },
				{ "output=",  v => outputRoot = v },
				{ "noships", v => doShips = false },
				{ "noitems", v => doItems = false },
				{ "noshops", v => doShops = false },
				{ "nomap", v => doStarmap = false },
				{ "nocache", v => noCache = true },
				{ "types=", v => typeFilter = v },
				{ "ships=", v=> shipFilter = v }
			};

			var extra = p.Parse(args);

			var badArgs = extra.Count > 0;
			if (badArgs)
			{
				Console.WriteLine("Usage:");
				Console.WriteLine("    Loader.exe -input=<path to extracted Star Citizen data> -output=<path to JSON output folder>");
				Console.WriteLine();
				return;
			}

			JsonConvert.DefaultSettings = () => new JsonSerializerSettings
			{
				Formatting = Formatting.Indented,
				NullValueHandling = NullValueHandling.Ignore,
				Converters = new List<JsonConverter> { new StringEnumConverter() }
			};

			bool incremental = !doShips || !doItems || !doShops || !doStarmap;

			// Prep the output folder
			if (Directory.Exists(outputRoot) && !incremental)
			{
				var info = new DirectoryInfo(outputRoot);
				foreach (var file in info.GetFiles()) file.Delete();
				foreach (var dir in info.GetDirectories()) dir.Delete(true);
			}
			else Directory.CreateDirectory(outputRoot);

			var entitySvc = new EntityService
			{
				OutputFolder = outputRoot,
				DataRoot = scDataRoot
			};
			entitySvc.Initialise(noCache);

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
			var manufacturerSvc = new ManufacturerService(manufacturerIndex);

			// Ammunition
			Console.WriteLine("Load Ammunition");
			var ammoLoader = new AmmoLoader
			{
				OutputFolder = outputRoot,
				DataRoot = scDataRoot
			};
			var ammoIndex = ammoLoader.Load();
			var ammoSvc = new AmmoService(ammoIndex);

			// Insurance
			Console.WriteLine("Load Insurance");
			var insuranceLoader = new InsuranceLoader()
			{
				DataRoot = scDataRoot
			};
			var insurancePrices = insuranceLoader.Load();
			var insuranceSvc = new InsuranceService(insurancePrices);

			var xmlLoadoutLoader = new XmlLoadoutLoader { DataRoot = scDataRoot };
			var manualLoadoutLoader = new ManualLoadoutLoader();
			var loadoutLoader = new LoadoutLoader(xmlLoadoutLoader, manualLoadoutLoader);
			var itemBuilder = new ItemBuilder(localisationSvc, manufacturerSvc, ammoSvc, entitySvc);
			var itemInstaller = new ItemInstaller(entitySvc, loadoutLoader, itemBuilder);

			// Items
			if (doItems)
			{
				Console.WriteLine("Load Items");
				var itemLoader = new ItemLoader(itemBuilder, manufacturerSvc, entitySvc, ammoSvc, itemInstaller, loadoutLoader)
				{
					OutputFolder = outputRoot,
					DataRoot = scDataRoot,
				};
				itemLoader.Load(typeFilter);
			}

			// Ships and vehicles
			if (doShips)
			{
				Console.WriteLine("Load Ships and Vehicles");
				var shipLoader = new ShipLoader(itemBuilder, manufacturerSvc, localisationSvc, entitySvc, itemInstaller, loadoutLoader, insuranceSvc)
				{
					OutputFolder = outputRoot,
					DataRoot = scDataRoot,
				};
				shipLoader.Load(shipFilter);
			}

			// Prices
			if (doShops)
			{
				Console.WriteLine("Load Shops");
				var shopLoader = new ShopLoader(localisationSvc, entitySvc)
				{
					OutputFolder = outputRoot,
					DataRoot = scDataRoot
				};
				shopLoader.Load();
			}

			// Starmap
			if (doStarmap)
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
