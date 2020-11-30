using System;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Loader
{
	public class StarmapLoader
	{
		public string OutputFolder { get; set; }
		public string DataRoot { get; set; }

		LocalisationService localisationService;
		bool verbose = false;

		public StarmapLoader(LocalisationService localisationService)
		{
			this.localisationService = localisationService;
		}

		public List<StarmapIndexEntry> Load()
		{
			var index = new List<StarmapIndexEntry>();
			index.AddRange(Load(@"Data\Libs\Foundry\Records\starmap\pu"));

			File.WriteAllText(Path.Combine(OutputFolder, "starmap.json"), JsonConvert.SerializeObject(index));

			return index;
		}

		List<StarmapIndexEntry> Load(string entityFolder)
		{
			var index = new List<StarmapIndexEntry>();

			foreach (var entityFilename in Directory.EnumerateFiles(Path.Combine(DataRoot, entityFolder), "*.xml", SearchOption.AllDirectories))
			{
				if (verbose) Console.WriteLine($"StarmapLoader: {entityFilename}");

				var parser = new StarmapParser();
				var entity = parser.Parse(entityFilename);
				if (entity == null) continue;

				var indexEntry = new StarmapIndexEntry
				{
					name = localisationService.GetText(entity.name),
					description = localisationService.GetText(entity.description),
					callout1 = localisationService.GetText(entity.callout1),
					callout2 = localisationService.GetText(entity.callout2),
					callout3 = localisationService.GetText(entity.callout3),
					type = entity.type,
					navIcon = entity.navIcon,
					hideInStarmap = entity.hideInStarmap,
					jurisdiction = entity.jurisdiction,
					parent = entity.parent,
					size = entity.size,
					reference = entity.__ref,
					path = entity.__path
				};

				index.Add(indexEntry);
			}

			return index;
		}
	}
}
