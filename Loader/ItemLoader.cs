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

		string[] include = new string[] {
			"ships",
			"vehicles",
			"doors"
		};

		public List<ItemIndexEntry> Load()
		{
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
				Loadout defaultLoadout = null;
				ItemPort[] normalisedLoadout = null;

				// Entity
				Console.WriteLine(entityFilename);
				var entityParser = new EntityParser();
				entity = entityParser.Parse(entityFilename);
				if (entity == null) continue;

				// Default loadout
				var defaultLoadoutFilename = entity.Components?.SEntityComponentDefaultLoadoutParams?.loadout?.SItemPortLoadoutXMLParams?.loadoutPath;
				if (!String.IsNullOrEmpty(defaultLoadoutFilename))
				{
					defaultLoadoutFilename = Path.Combine(DataRoot, "Data", defaultLoadoutFilename.Replace('/', '\\'));
					Console.WriteLine(defaultLoadoutFilename);

					var loadoutParser = new DefaultLoadoutParser();
					defaultLoadout = loadoutParser.Parse(defaultLoadoutFilename);
				}

				// Normalise loadout
				var normaliser = new LoadoutNormaliser();
				if (defaultLoadout != null) normalisedLoadout = normaliser.NormaliseLoadout(defaultLoadout.Items, null);
				if (normalisedLoadout == null)
				{
					var manualLoadout = entity.Components?.SEntityComponentDefaultLoadoutParams?.loadout?.SItemPortLoadoutManualParams;
					normalisedLoadout = normaliser.NormaliseLoadout(manualLoadout, null);
				}

				var jsonFilename = Path.Combine(OutputFolder, $"{entity.ClassName.ToLower()}.json");
				var json = JsonConvert.SerializeObject(new
				{
					Raw = new
					{
						Entity = entity,
						DefaultLoadout = defaultLoadout
					},
					NormalisedLoadout = normalisedLoadout
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
					DefaultLoadoutFilename = defaultLoadout != null ? Path.GetRelativePath(DataRoot, defaultLoadoutFilename) : null,
					Entity = entity,
					DefaultLoadout = defaultLoadout
				});
			}
			return index;
		}
	}
}
