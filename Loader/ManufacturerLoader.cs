using System;
using System.Collections.Generic;
using System.IO;

namespace Loader
{
	public class ManufacturerLoader
	{
		public string DataRoot { get; set; }

		public List<ManufacturerIndexEntry> Load()
		{
			var index = new List<ManufacturerIndexEntry>();
			index.AddRange(Load(@"Data\Libs\Foundry\Records\scitemmanufacturer"));

			return index;
		}

		List<ManufacturerIndexEntry> Load(string entityFolder)
		{
			var index = new List<ManufacturerIndexEntry>();

			foreach (var entityFilename in Directory.EnumerateFiles(Path.Combine(DataRoot, entityFolder), "*.xml"))
			{
				Console.WriteLine(entityFilename);

				var parser = new ManufacturerParser();
				var entity = parser.Parse(entityFilename);
				if (entity == null) continue;

				var indexEntry = new ManufacturerIndexEntry
				{
					name = entity.Localization.Name,
					code = entity.Code,
					reference = entity.__ref
				};

				index.Add(indexEntry);
			}

			return index;
		}
	}
}
