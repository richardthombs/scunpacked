using System;
using System.IO;
using System.Linq;

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

			// Ships and ground vehicles
			var shipLoader = new ShipLoader
			{
				OutputFolder = Path.Combine(outputRoot, "ships"),
				DataRoot = scDataRoot
			};
			var shipIndex = shipLoader.Load();
			File.WriteAllText(Path.Combine(outputRoot, "ships.json"), JsonConvert.SerializeObject(shipIndex, Newtonsoft.Json.Formatting.Indented));

			// Items that go on ships
			var itemLoader = new ItemLoader
			{
				OutputFolder = Path.Combine(outputRoot, "items"),
				DataRoot = scDataRoot
			};
			var itemIndex = itemLoader.Load();
			File.WriteAllText(Path.Combine(outputRoot, "items.json"), JsonConvert.SerializeObject(itemIndex, Newtonsoft.Json.Formatting.Indented));
		}
	}
}
