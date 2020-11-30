using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

namespace Loader
{
	public class LabelsLoader
	{
		public string OutputFolder { get; set; }
		public string DataRoot { get; set; }

		public Dictionary<string, string> Load(string language)
		{
			var labels = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

			try
			{
				using (var ini = new StreamReader(Path.Combine(DataRoot, $@"Data\Localization\{language}\global.ini")))
				{
					for (var line = ini.ReadLine(); line != null; line = ini.ReadLine())
					{
						var split = line.Split('=', 2);
						labels.Add(split[0], split[1]);
					}
				}
				File.WriteAllText(Path.Combine(OutputFolder, "labels.json"), JsonConvert.SerializeObject(labels));
			}
			catch (DirectoryNotFoundException)
			{ }
			catch (FileNotFoundException)
			{ }

			return labels;
		}
	}
}
