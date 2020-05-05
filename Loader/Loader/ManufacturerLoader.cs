using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Loader.Entries;
using Loader.Parser;
using Loader.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Loader.Loader
{
	internal class ManufacturerLoader
	{
		private readonly LocalisationService _localisationService;

		private readonly ILogger<ManufacturerLoader> _logger;

		private readonly ManufacturerParser _manufacturerParser;

		private readonly ServiceOptions _options;

		public ManufacturerLoader(ILogger<ManufacturerLoader> logger, IOptions<ServiceOptions> options,
		                          ManufacturerParser manufacturerParser, LocalisationService localisationService)
		{
			_logger = logger;
			_manufacturerParser = manufacturerParser;
			_localisationService = localisationService;
			_options = options.Value;
		}

		private string DataRoot => _options.SCData;

		public async Task<List<Manufacturer>> Load()
		{
			var manufacturers = new List<Manufacturer>();

			await foreach (var item in Load(@"Data\Libs\Foundry\Records\scitemmanufacturer"))
			{
				manufacturers.Add(item);
			}

			return manufacturers;
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

				yield return new Manufacturer
				             {
					             Id = new Guid(entity.__ref),
					             Name = _localisationService.GetText(entity.Localization.Name),
					             Code = entity.Code
				             };
			}
		}
	}
}
