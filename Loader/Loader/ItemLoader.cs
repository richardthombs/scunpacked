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
			"",
			"characters",
			"consumables",
			"ships",
			"weapons",
			"carryables",
			"human"
		};

		private readonly IJsonFileReaderWriter _jsonFileReaderWriter;

		private readonly LocalisationService _localisationService;

		private readonly ILogger<ItemLoader> _logger;

		private readonly ServiceOptions _options;

		public ItemLoader(ILogger<ItemLoader> logger, EntityParser entityParser,
		                  IJsonFileReaderWriter jsonFileReaderWriter, LoaderService<Manufacturer> manufacturersService,
		                  IOptions<ServiceOptions> options, LocalisationService localisationService,
		                  LoaderService<Ship> shipService, LoaderService<Commodity> commodtiyService)
		{
			_logger = logger;
			_entityParser = entityParser;
			_jsonFileReaderWriter = jsonFileReaderWriter;
			_localisationService = localisationService;
			Manufacturers = manufacturersService.Items;
			Ships = shipService.Items;
			Commodities = commodtiyService.Items;
			_options = options.Value;
		}

		private Dictionary<string, Manufacturer> Manufacturers { get; }

		public Dictionary<string, Commodity> Commodities { get; }

		public Dictionary<string, Ship> Ships { get; }

		private string OutputFolder => Path.Combine(_options.Output, "items");

		private string DataRoot => _options.SCData;

		private Func<string, Task<string>> OnXmlLoadout { get; set; }

		public async Task<List<Item>> Load(Func<string, Task<string>> onXmlLoadout)
		{
			OnXmlLoadout = onXmlLoadout;
			Directory.CreateDirectory(OutputFolder);

			var items = new List<Item>();
			items.AddRange(Ships.Values.Select(ship => new Item
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

			// TODO: Add commodities like ships
			var unknownManufacturer = GetUnknownManufacturer();
			items.AddRange(Commodities.Values.Select(comm => new Item
			                                                 {
				                                                 Id = comm.Id,
				                                                 JsonFilename = comm.JsonFilename,
				                                                 ClassName = comm.ClassName,
				                                                 Type = comm.Type.Name,
				                                                 SubType = comm.SubType?.Name,
				                                                 ItemName = comm.ClassName.ToLower(),
				                                                 Name = comm.Name,
				                                                 Description = comm.Description,
				                                                 Manufacturer = unknownManufacturer,
				                                                 ManufacturerId = unknownManufacturer.Id
			                                                 }));

			foreach (var folder in _include)
			{
				var path = Path.Combine(@"Data\Libs\Foundry\Records\entities\scitem", folder);
				await foreach (var item in LoadAndWriteJsonFiles(path))
				{
					items.Add(item);
				}
			}

			return items;
		}

		private async IAsyncEnumerable<Item> LoadAndWriteJsonFiles(string itemsFolder)
		{
			var folderPath = Path.Combine(DataRoot, itemsFolder);
			var index = new List<Item>();

			var searchOptions = itemsFolder.ToLower().EndsWith("scitem")
				                    ? SearchOption.TopDirectoryOnly
				                    : SearchOption.AllDirectories;
			foreach (var entityFilename in Directory.EnumerateFiles(folderPath, "*.xml", searchOptions))
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

			return etd;
		}

		private Manufacturer FindManufacturer(string reference)
		{
			return Manufacturers.GetValueOrDefault(reference ?? "") ?? GetUnknownManufacturer();
		}

		private Manufacturer GetUnknownManufacturer()
		{
			return Manufacturers.Values.First(x => x.Code == "UNKN");
		}

		private bool AvoidFile(string filename)
		{
			var fileSplit = Path.GetFileNameWithoutExtension(filename).Split('_');
			return fileSplit.Any(part => _avoids.Contains(part));
		}
	}
}
