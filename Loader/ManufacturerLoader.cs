using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

namespace Loader
{
	public class ManufacturerLoader
	{
		public string DataRoot { get; set; }
		public string OutputFolder { get; set; }

		LocalisationService localisationService;
		bool verbose = false;

		public ManufacturerLoader(LocalisationService localisationService)
		{
			this.localisationService = localisationService;
		}

		public List<ManufacturerIndexEntry> Load()
		{
			var index = new List<ManufacturerIndexEntry>();
			index.AddRange(Load(@"Data\Libs\Foundry\Records\scitemmanufacturer"));

			File.WriteAllText(Path.Combine(OutputFolder, "manufacturers.json"), JsonConvert.SerializeObject(index));

			return index;
		}

		List<ManufacturerIndexEntry> Load(string entityFolder)
		{
			var index = new List<ManufacturerIndexEntry>();
			var parser = new ManufacturerParser();

			foreach (var entityFilename in Directory.EnumerateFiles(Path.Combine(DataRoot, entityFolder), "*.xml", SearchOption.AllDirectories))
			{
				if (verbose) Console.WriteLine(entityFilename);

				var manufacturer = parser.Parse(entityFilename);
				if (manufacturer == null) continue;

				var indexEntry = new ManufacturerIndexEntry
				{
					name = localisationService.GetText(manufacturer.Localization?.Name),
					code = manufacturer.Code,
					reference = manufacturer.__ref
				};

				index.Add(indexEntry);
			}

			return index;
		}
	}
}
