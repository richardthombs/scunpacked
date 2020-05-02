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
			"ships",
			"vehicles",
			"doors"
		};

		private readonly ILogger<ItemLoader> _logger;
		private readonly ServiceOptions _options;

		public ItemLoader(ILogger<ItemLoader> logger, EntityParser entityParser,
		                  ManufacturersService manufacturersService, IOptions<ServiceOptions> options)
		{
			_logger = logger;
			_entityParser = entityParser;
			Manufacturers = manufacturersService.Items;
			_options = options.Value;
		}

		private string OutputFolder => Path.Combine(_options.Output, "items");
		private string DataRoot => _options.SCData;
		private Func<string, Task<string>> OnXmlLoadout { get; set; }
		private List<Manufacturer> Manufacturers { get; }

		public async Task<List<Item>> Load(Func<string, Task<string>> onXmlLoadout)
		{
			OnXmlLoadout = onXmlLoadout;
			Directory.CreateDirectory(OutputFolder);

			var index = new List<Item>();
			foreach (var folder in _include)
			{
				await foreach (var item in
					LoadAndWriteJsonFile(Path.Combine(@"Data\Libs\Foundry\Records\entities\scitem", folder)))
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
				var json = JsonConvert.SerializeObject(new {Raw = new {Entity = entity}});
				await File.WriteAllTextAsync(jsonFilename, json);

				yield return new Item
				             {
					             JsonFilename = Path.GetRelativePath(Path.GetDirectoryName(OutputFolder), jsonFilename),
					             ClassName = entity.ClassName,
					             ItemName = entity.ClassName.ToLower(),
					             Type = entity.Components?.SAttachableComponentParams?.AttachDef.Type,
					             SubType = entity.Components?.SAttachableComponentParams?.AttachDef.SubType,
					             Size = entity.Components?.SAttachableComponentParams?.AttachDef.Size,
					             Grade = entity.Components?.SAttachableComponentParams?.AttachDef.Grade,
					             Name = entity.Components?.SAttachableComponentParams?.AttachDef.Localization.Name,
					             Manufacturer =
						             FindManufacturer(entity.Components?.SAttachableComponentParams?.AttachDef
						                                    .Manufacturer)
							             ?.Code
				             };
			}
		}

		private Manufacturer FindManufacturer(string reference)
		{
			return Manufacturers.FirstOrDefault(x => x.Reference == reference);
		}

		private bool AvoidFile(string filename)
		{
			var fileSplit = Path.GetFileNameWithoutExtension(filename).Split('_');
			return fileSplit.Any(part => _avoids.Contains(part));
		}
	}
}
