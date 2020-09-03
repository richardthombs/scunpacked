using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using scdb.Xml.Entities;

namespace Loader
{
	public class ItemLoader
	{
		public string OutputFolder { get; set; }
		public string DataRoot { get; set; }
		public Func<string, string> OnXmlLoadout { get; set; }
		public List<ManufacturerIndexEntry> Manufacturers { get; set; }
		public List<AmmoIndexEntry> Ammo { get; set; }

		string[] include = new string[] {
			"ships",
			"vehicles",
			"doors",
			"weapons"
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

			foreach (var entry in index)
			{
				var entity = ClassParser<EntityClassDefinition>.ClassByRefCache[entry.reference];

				// If the entity has a loadout file, then load it
				if (entity.Components?.SEntityComponentDefaultLoadoutParams?.loadout?.SItemPortLoadoutXMLParams != null)
				{
					entity.Components.SEntityComponentDefaultLoadoutParams.loadout.SItemPortLoadoutXMLParams.loadoutPath = OnXmlLoadout(entity.Components.SEntityComponentDefaultLoadoutParams.loadout.SItemPortLoadoutXMLParams.loadoutPath);
				}

				// If it is a weapon magazine, then load it
				EntityClassDefinition magazine = null;
				if (!String.IsNullOrEmpty(entity.Components?.SCItemWeaponComponentParams?.ammoContainerRecord))
				{
					magazine = ClassParser<EntityClassDefinition>.ClassByRefCache.GetValueOrDefault(entity.Components?.SCItemWeaponComponentParams.ammoContainerRecord);
				}

				// If it is an ammo container, then load the ammo properties
				AmmoIndexEntry ammoEntry = null;
				var ammoRef = magazine?.Components?.SAmmoContainerComponentParams?.ammoParamsRecord ?? entity.Components?.SAmmoContainerComponentParams?.ammoParamsRecord;
				if (!String.IsNullOrEmpty(ammoRef))
				{
					ammoEntry = Ammo.FirstOrDefault(x => x.reference == ammoRef);
				}

				var jsonFilename = Path.Combine(OutputFolder, $"{entity.ClassName.ToLower()}.json");
				var json = JsonConvert.SerializeObject(new
				{
					magazine = magazine,
					ammo = ammoEntry,
					Raw = new
					{
						Entity = entity,
					}
				});
				File.WriteAllText(jsonFilename, json);
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
				var entityParser = new ClassParser<EntityClassDefinition>();
				entity = entityParser.Parse(entityFilename);
				if (entity == null) continue;

				index.Add(new ItemIndexEntry
				{
					className = entity.ClassName,
					reference = entity.__ref,
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
