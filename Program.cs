using System.IO;
using Newtonsoft.Json;

namespace shipparser
{
	class Program
	{
		static void Main(string[] args)
		{
			var scDataRoot = @"c:\dev\scdata\3.7.2";

			var shipLoader = new ShipLoader
			{
				OutputFolder = @".\json\ships",
				DataRoot = scDataRoot
			};
			var shipIndex = shipLoader.Load();
			File.WriteAllText(Path.Combine(@".\json", "ships.json"), JsonConvert.SerializeObject(shipIndex, Newtonsoft.Json.Formatting.Indented));

			var itemLoader = new ItemLoader
			{
				OutputFolder = @".\json\items",
				DataRoot = scDataRoot
			};
			var itemIndex = itemLoader.Load();
			File.WriteAllText(Path.Combine(@".\json", "items.json"), JsonConvert.SerializeObject(itemIndex, Newtonsoft.Json.Formatting.Indented));
		}
	}
}
