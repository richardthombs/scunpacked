using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using scdb.Xml.Entities;

namespace Loader
{
	public class AmmoLoader
	{
		public string DataRoot { get; set; }
		public string OutputFolder { get; set; }

		bool verbose;

		public List<AmmoParams> Load()
		{
			var index = new List<AmmoParams>();
			index.AddRange(Load(@"Data\Libs\Foundry\Records\ammoparams"));

			return index;
		}

		List<AmmoParams> Load(string entityFolder)
		{
			var index = new List<AmmoParams>();
			var parser = new ClassParser<AmmoParams>();

			foreach (var filename in Directory.EnumerateFiles(Path.Combine(DataRoot, entityFolder), "*.xml", SearchOption.AllDirectories))
			{
				if (verbose) Console.WriteLine(filename);

				var ammoParams = parser.Parse(filename);
				if (ammoParams == null) continue;

				index.Add(ammoParams);
			}

			return index;
		}
	}
}
