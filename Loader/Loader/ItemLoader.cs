using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Loader.Entries;
using Loader.Parser;
using Loader.SCDb.Xml.Entities;
using Loader.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Loader.Loader
{
	internal class ItemLoader
	{
		private readonly string[] _avoids =
		{
			// CIG tags
			"test",
			"template",
			"s42"
		};

		private readonly EntityParser _entityParser;

		private readonly string[] _include =
		{
			"scitem",
			"commodities",
		};

		private readonly LocalisationService _localisationService;

		private readonly ILogger<ItemLoader> _logger;
		private readonly ServiceOptions _options;

		public ItemLoader(ILogger<ItemLoader> logger, EntityParser entityParser,
		                  LoaderService<Manufacturer> manufacturersService, IOptions<ServiceOptions> options,
		                  LocalisationService localisationService, LoaderService<Ship> shipService)
		{
			_logger = logger;
			_entityParser = entityParser;
			_localisationService = localisationService;
			Manufacturers = manufacturersService.Items;
			Ships = shipService.Items;
			_options = options.Value;
		}

		public Dictionary<string, Ship> Ships { get; }

		private string OutputFolder => Path.Combine(_options.Output, "items");
		private string DataRoot => _options.SCData;
		private Func<string, Task<string>> OnXmlLoadout { get; set; }
		private Dictionary<string, Manufacturer> Manufacturers { get; }

		public async Task<List<Item>> Load(Func<string, Task<string>> onXmlLoadout)
		{
			OnXmlLoadout = onXmlLoadout;
			Directory.CreateDirectory(OutputFolder);

			var index = new List<Item>();
			index.AddRange(Ships.Values.Select(ship => new Item
			                                           {
				                                           Id = ship.Id,
				                                           JsonFilename = ship.JsonFilename,
				                                           ClassName = ship.ClassName,
				                                           Type = ship.Type,
				                                           SubType = ship.SubType,
				                                           ItemName = ship.ClassName.ToLower(),
				                                           Name = ship.Name,
				                                           Description = ship.Description,
				                                           Manufacturer = ship.Manufacturer,
				                                           ManufacturerId = ship.ManufacturerId
			                                           }));

			foreach (var folder in _include)
			{
				var path = Path.Combine(@"Data\Libs\Foundry\Records\entities", folder);
				await foreach (var item in LoadAndWriteJsonFile(path))
				{
					index.Add(item);
				}
			}

			return index;
		}

		private async IAsyncEnumerable<Item> LoadAndWriteJsonFile(string itemsFolder)
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
				if (_options.WriteRawJsonFiles)
				{
					var json = JsonConvert.SerializeObject(new {Raw = new {Entity = entity}});
					await File.WriteAllTextAsync(jsonFilename, json);
				}

				var manufacturer =
					FindManufacturer(entity.Components?.SAttachableComponentParams?.AttachDef.Manufacturer);

				var etd = GetLocalizedDataFromEntity(entity);
				yield return new Item
				             {
					             Id = new Guid(entity.Id),
					             JsonFilename =
						             Path.GetRelativePath(Path.GetDirectoryName(OutputFolder), jsonFilename),
					             ClassName = entity.ClassName,
					             Type = etd.Type,
					             SubType = etd.SubType,
					             ItemName = entity.ClassName.ToLower(),
					             Size = entity.Components?.SAttachableComponentParams?.AttachDef.Size,
					             Grade = entity.Components?.SAttachableComponentParams?.AttachDef.Grade,
					             Name = etd.Name,
					             Description = etd.Description,
					             Manufacturer = manufacturer,
					             ManufacturerId = manufacturer.Id
				             };
			}
		}

		private EntityTextData GetLocalizedDataFromEntity(EntityClassDefinition entity)
		{
			var etd = new EntityTextData();

			if (entity.Components?.SAttachableComponentParams != null)
			{
				etd.Type = entity.Components.SAttachableComponentParams.AttachDef.Type;
				etd.SubType = entity.Components.SAttachableComponentParams.AttachDef.SubType;
				etd.Name = _localisationService.GetText(entity.Components.SAttachableComponentParams.AttachDef
				                                              .Localization.Name);
				etd.Description =
					_localisationService.GetText(entity.Components.SAttachableComponentParams.AttachDef.Localization
					                                   .Description);
			}
			else
			{
				if (entity.Components?.CommodityComponentParams != null)
				{
					var e = entity.Components.CommodityComponentParams;
					etd.Type = e.type;
					etd.SubType = e.subtype;
					etd.Name = _localisationService.GetText(e.name);
					etd.Description = _localisationService.GetText(e.description);
				}

				if (entity.Components?.SCItemPurchasableParams != null)
				{
					var e = entity.Components.SCItemPurchasableParams;
					etd.Type = _localisationService.GetText(e.displayType);
				}
			}

			return etd;
		}

		private Manufacturer FindManufacturer(string reference)
		{
			return Manufacturers.GetValueOrDefault(reference ?? "") ?? Manufacturers.Values.First(x => x.Code == "UNKN");
		}

		private bool AvoidFile(string filename)
		{
			var fileSplit = Path.GetFileNameWithoutExtension(filename).Split('_');
			return fileSplit.Any(part => _avoids.Contains(part));
		}

		private struct EntityTextData
		{
			public string Name { get; set; }
			public string Description { get; set; }
			public string Type { get; set; }
			public string SubType { get; set; }
		}
	}
}
