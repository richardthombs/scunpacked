using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Loader.Entries;
using Loader.Helper;
using Loader.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Loader.Loader
{
	internal class CommodityTypeLoader
	{
		private readonly IJsonFileReaderWriter _jsonFileReaderWriter;

		private readonly LocalisationService _localisationService;

		private readonly ILogger<CommodityTypeLoader> _logger;

		private readonly ServiceOptions _options;

		public CommodityTypeLoader(ILogger<CommodityTypeLoader> logger, IOptions<ServiceOptions> options,
		                           IJsonFileReaderWriter jsonFileReaderWriter, LocalisationService localisationService)
		{
			_logger = logger;
			_jsonFileReaderWriter = jsonFileReaderWriter;
			_localisationService = localisationService;
			_options = options.Value;
		}

		private string OutputFolder => Path.Combine(_options.Output, "commoditytypes");

		private string DataRoot => _options.SCData;

		public async Task<List<CommodityTypeAndSubType>> Load()
		{
			Directory.CreateDirectory(OutputFolder);

			var index = new List<CommodityTypeAndSubType>();

			var path = Path.Combine(DataRoot, "Data", "Game.xml");
			await foreach (var item in LoadAndWriteJsonFiles(path))
			{
				index.Add(item);
			}

			return index;
		}

		private async IAsyncEnumerable<CommodityTypeAndSubType> LoadAndWriteJsonFiles(string xmlFilename)
		{
			_logger.LogInformation("Read in game.xml. Please wait...");

			var document =
				await XElement.LoadAsync(new StreamReader(xmlFilename), LoadOptions.None, CancellationToken.None);

			var commodityTypes = document.Descendants()
			                             .Where(e => e.Attribute("__type")?.Value == "CommodityType" ||
			                                         e.Attribute("__type")?.Value == "CommoditySubtype");
			foreach (var type in commodityTypes)
			{
				var commodityType = new CommodityTypeAndSubType
				                    {
					                    Id = new Guid(type.Attribute("__ref").Value),
					                    TypeName = type.Attribute("typeName")?.Value,
					                    Name =
						                    _localisationService.GetText(type.Attribute("name")
						                                                     ?.Value),
					                    Description =
						                    _localisationService
							                    .GetText(type.Attribute("description")?.Value),
					                    Symbol = type.Attribute("symbol")?.Value,
					                    Type = type.Attribute("__type")?.Value
				                    };

				_logger.LogInformation($"Handle CommodityType {commodityType.TypeName}");

				var jsonfilename = Path.Combine(OutputFolder, $"{commodityType.TypeName}.json");
				_ = _jsonFileReaderWriter.WriteFile(jsonfilename, () => commodityType);

				yield return commodityType;
			}
		}
	}
}
