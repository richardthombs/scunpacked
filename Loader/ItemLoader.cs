using System;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;

using scdb.Xml.Entities;
using scdb.Xml.DefaultLoadouts;
using scdb.Models;

namespace Loader
{
	public class ItemLoader
	{
		public string OutputFolder { get; set; }
		public string DataRoot { get; set; }
		public Func<string, string> OnXmlLoadout { get; set; }

		string[] include = new string[] {
			"ships",
			"vehicles",
			"doors"
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
					json = Path.GetRelativePath(Path.GetDirectoryName(OutputFolder), jsonFilename),
					ClassName = entity.ClassName,
					ItemName = entity.ClassName.ToLower(),
					Type = entity.Components?.SAttachableComponentParams?.AttachDef.Type,
					SubType = entity.Components?.SAttachableComponentParams?.AttachDef.SubType,
					EntityFilename = Path.GetRelativePath(DataRoot, entityFilename),
					Entity = entity
				});
			}
			return index;
		}
	}
}
