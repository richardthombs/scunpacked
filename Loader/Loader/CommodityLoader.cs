using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Loader.Entries;
using Loader.Helper;
using Loader.Parser;
using Loader.SCDb.Xml.Entities;
using Loader.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Loader.Loader
{
	internal class CommodityLoader
	{
		private readonly string[] _avoids =
		{
			// CIG tags
			"test",
			"template",
			"s42"
		};

		private readonly EntityParser _entityParser;

		private readonly IJsonFileReaderWriter _jsonFileReaderWriter;

		private readonly LocalisationService _localisationService;

		private readonly ILogger<CommodityLoader> _logger;

		private readonly ServiceOptions _options;

		public CommodityLoader(ILogger<CommodityLoader> logger, EntityParser entityParser,
		                       IJsonFileReaderWriter jsonFileReaderWriter,
		                       LoaderService<CommodityTypeAndSubType> commodityTypeService,
		                       IOptions<ServiceOptions> options, LocalisationService localisationService)
		{
			_logger = logger;
			_entityParser = entityParser;
			_jsonFileReaderWriter = jsonFileReaderWriter;
			_localisationService = localisationService;
			CommodityTypes = commodityTypeService.Items;
			_options = options.Value;
		}

		private Dictionary<string, CommodityTypeAndSubType> CommodityTypes { get; }

		private string OutputFolder => Path.Combine(_options.Output, "commodities");

		private string DataRoot => _options.SCData;

		private Func<string, Task<string>> OnXmlLoadout { get; set; }

		public async Task<List<Commodity>> Load(Func<string, Task<string>> onXmlLoadout)
		{
			OnXmlLoadout = onXmlLoadout;
			Directory.CreateDirectory(OutputFolder);

			var index = new List<Commodity>();

			var path = Path.Combine(@"Data\Libs\Foundry\Records\entities\commodities");
			await foreach (var item in LoadAndWriteJsonFiles(path))
			{
				index.Add(item);
			}

			return index;
		}

		private async IAsyncEnumerable<Commodity> LoadAndWriteJsonFiles(string itemsFolder)
		{
			var folderPath = Path.Combine(DataRoot, itemsFolder);
			var index = new List<Item>();

			foreach (var entityFilename in Directory.EnumerateFiles(folderPath, "*.xml", SearchOption.AllDirectories))
			{
				if (AvoidFile(entityFilename))
				{
					continue;
				}

				EntityClassDefinition entity = null;

				// Entity
				_logger.LogInformation(entityFilename);
				entity = await _entityParser.Parse(entityFilename, OnXmlLoadout);
				if (entity == null)
				{
					continue;
				}

				var jsonFilename = Path.Combine(OutputFolder, $"{entity.ClassName.ToLower()}.json");
				_ = _jsonFileReaderWriter.WriteFile(jsonFilename, () => new {Raw = new {Entity = entity}});

				var etd = GetLocalizedDataFromEntity(entity);

				var type = CommodityTypes[etd.Type];
				var subType = CommodityTypes.GetValueOrDefault(etd.SubType);
				var description = string.IsNullOrWhiteSpace(etd.Description) ? subType?.Description : etd.Description;

				yield return new Commodity
				             {
					             Id = new Guid(entity.Id),
					             JsonFilename =
						             Path.GetRelativePath(Path.GetDirectoryName(OutputFolder), jsonFilename),
					             ClassName = entity.ClassName,
					             Type = type,
					             TypeId = type.Id,
					             SubType = subType,
					             SubTypeId = subType?.Id,
					             Name = etd.Name,
					             Description = description
				             };
			}
		}

		private EntityTextData GetLocalizedDataFromEntity(EntityClassDefinition entity)
		{
			var etd = new EntityTextData();

			var e = entity.Components.CommodityComponentParams;
			etd.Type = e.type;
			etd.SubType = e.subtype;
			etd.Name = _localisationService.GetText(e.name);
			etd.Description = _localisationService.GetText(e.description);

			return etd;
		}

		private bool AvoidFile(string filename)
		{
			var fileSplit = Path.GetFileNameWithoutExtension(filename).Split('_');
			return fileSplit.Any(part => _avoids.Contains(part));
		}
	}
}
