using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Loader.SCDb.Xml.Entities;
using Newtonsoft.Json;

namespace Loader
{
	public class ItemLoader
	{
		public string OutputFolder { get; set; }
		public string DataRoot { get; set; }
		public Func<string, string> OnXmlLoadout { get; set; }
		public List<ManufacturerIndexEntry> Manufacturers { get; set; }

		string[] include = new string[] {
			"ships",
			"vehicles",
			"doors"
		};

		string[] avoids =
		{
			// CIG tags
			"test",
			"template",
			"s42"
		};

		public List<ItemIndexEntry> Load()
		{
			Directory.CreateDirectory(OutputFolder);

			var index = new List<ItemIndexEntry>();
			foreach (var folder in include)
			{
				index.AddRange(Load(Path.Combine(@"Data\Libs\Foundry\Records\entities\scitem", folder)));
			}
			return index;
		}

		List<ItemIndexEntry> Load(string itemsFolder)
		{
			var folderPath = Path.Combine(DataRoot, itemsFolder);
			var index = new List<ItemIndexEntry>();

			foreach (var entityFilename in Directory.EnumerateFiles(folderPath, "*.xml", SearchOption.AllDirectories))
			{
				if (avoidFile(entityFilename)) continue;

				EntityClassDefinition entity = null;

				// Entity
				Console.WriteLine(entityFilename);
				var entityParser = new EntityParser();
				entity = entityParser.Parse(entityFilename, OnXmlLoadout);
				if (entity == null) continue;

				var jsonFilename = Path.Combine(OutputFolder, $"{entity.ClassName.ToLower()}.json");
				var json = JsonConvert.SerializeObject(new
				{
					Raw = new
					{
						Entity = entity,
					}
				});
				File.WriteAllText(jsonFilename, json);

				index.Add(new ItemIndexEntry
				{
					jsonFilename = Path.GetRelativePath(Path.GetDirectoryName(OutputFolder), jsonFilename),
					className = entity.ClassName,
					itemName = entity.ClassName.ToLower(),
					type = entity.Components?.SAttachableComponentParams?.AttachDef.Type,
					subType = entity.Components?.SAttachableComponentParams?.AttachDef.SubType,
					size = entity.Components?.SAttachableComponentParams?.AttachDef.Size,
					grade = entity.Components?.SAttachableComponentParams?.AttachDef.Grade,
					name = entity.Components?.SAttachableComponentParams?.AttachDef.Localization.Name,
					manufacturer = FindManufacturer(entity.Components?.SAttachableComponentParams?.AttachDef.Manufacturer)?.code
				});
			}
			return index;
		}

		ManufacturerIndexEntry FindManufacturer(string reference)
		{
			var manufacturer = Manufacturers.FirstOrDefault(x => x.reference == reference);
			return manufacturer;
		}

		bool avoidFile(string filename)
		{
			var fileSplit = Path.GetFileNameWithoutExtension(filename).Split('_');
			return fileSplit.Any(part => avoids.Contains(part));
		}
	}
}
