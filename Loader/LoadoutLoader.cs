using System;
using System.IO;

using Newtonsoft.Json;

namespace Loader
{
	public class LoadoutLoader
	{
		public string OutputFolder { get; set; }
		public string DataRoot { get; set; }

		public string Load(string loadoutXmlPath)
		{
			Directory.CreateDirectory(OutputFolder);

			if (String.IsNullOrWhiteSpace(loadoutXmlPath)) return "";

			var windowsPath = loadoutXmlPath.Replace("/", "\\");
			Console.WriteLine(windowsPath);

			var loadoutParser = new DefaultLoadoutParser();
			var defaultLoadout = loadoutParser.Parse(Path.Combine(DataRoot, "Data", windowsPath));

			var jsonFilename = Path.Combine(OutputFolder, $"{Path.GetFileNameWithoutExtension(loadoutXmlPath)}.json");
			var json = JsonConvert.SerializeObject(defaultLoadout);
			File.WriteAllText(jsonFilename, json);

			return Path.GetRelativePath(OutputFolder, jsonFilename);
		}
	}
}
