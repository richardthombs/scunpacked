using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Loader.Entries;
using Loader.Parser;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Loader.Loader
{
	public class ManufacturerLoader
	{
		private readonly ILogger<ManufacturerLoader> _logger;
		private readonly ManufacturerParser _manufacturerParser;
		private readonly ServiceOptions _options;

		public ManufacturerLoader(ILogger<ManufacturerLoader> logger, IOptions<ServiceOptions> options,
		                          ManufacturerParser manufacturerParser)
		{
			_logger = logger;
			_manufacturerParser = manufacturerParser;
			_options = options.Value;
		}

		private string DataRoot => _options.SCData;

		public async Task<List<Manufacturer>> Load()
		{
			var index = new List<Manufacturer>();

			await foreach (var item in Load(@"Data\Libs\Foundry\Records\scitemmanufacturer"))
			{
				index.Add(item);
			}

			return index;
		}

		private async IAsyncEnumerable<Manufacturer> Load(string entityFolder)
		{
			foreach (var entityFilename in Directory.EnumerateFiles(Path.Combine(DataRoot, entityFolder), "*.xml"))
			{
				_logger.LogInformation(entityFilename);

				var entity = await _manufacturerParser.Parse(entityFilename);
				if (entity == null)
				{
					continue;
				}

				var indexEntry = new Manufacturer
				                 {
					                 Name = entity.Localization.Name,
					                 Code = entity.Code,
					                 Reference = entity.__ref
				                 };

				yield return indexEntry;
			}
		}
	}
}
