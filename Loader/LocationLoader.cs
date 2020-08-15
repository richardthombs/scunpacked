using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Loader
{
	public class LocationLoader
	{
		public string DataRoot { get; set; }

		public List<LocationIndexEntry> Load()
		{
			//Console.WriteLine(JsonConvert.SerializeObject(Directory.GetDirectories(Path.Combine(DataRoot, @"Data\Libs\Foundry\Records\starmap\pu"))));

			var index = new List<LocationIndexEntry>();
			index.AddRange(Load(@"Data\Libs\Foundry\Records\starmap\pu"));

			return index;
		}

		List<LocationIndexEntry> Load(string entityFolder)
		{
			var index = new List<LocationIndexEntry>();

			foreach (var entityFilename in Directory.EnumerateFiles(Path.Combine(DataRoot, entityFolder), "*.xml", SearchOption.AllDirectories))
			{
				Console.WriteLine(entityFilename);

				var parser = new LocationParser();
				var entity = parser.Parse(entityFilename);
				if (entity == null) continue;

				var indexEntry = new LocationIndexEntry
				{
					name = entity.name,
					description = entity.description,
					type = entity.type,
					navIcon = entity.navIcon,
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
