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

		// Avoid filenames that have these endings
		string[] file_avoids =
		{
			"test",
			"template",
			"s42",
			"tow"
		};

		// Avoid these folders
		string[] folder_avoids =
		{
			"environments",
			"hangar",
			"holoui",
			"innerthought_dummies",
			"lootables",
			"mission_entities",
			"missionstorage",
			"placeholder",
			"prop",
			"shopdisplays",
			"spawning",
			"starmarine",
			"template",
		};

		// Avoid items with these types
		string[] type_avoids =
		{
			"UNDEFINED",
			"bottle",
			"button",
			"char_accessory_eyes",
			"char_accessory_head",
			"char_body",
			"char_clothing_feet",
			"char_clothing_hands",
			"char_clothing_hat",
			"char_clothing_legs",
			"char_clothing_torso_0",
			"char_clothing_torso_1",
			"char_clothing_torso_2",
			"char_flair",
			"char_head",
			"char_hair_color",
			"char_head_eyebrow",
			"char_head_eyelash",
			"char_head_eyes",
			"char_head_hair",
			"char_skin_color",
			"cloth",
			"debris",
			"drink",
			"flair_floor",
			"flair_surface",
			"flair_wall",
			"food",
			"removablechip",
			"shopdisplay"
		};

		public List<ItemIndexEntry> Load()
		{
			Directory.CreateDirectory(OutputFolder);

			var index = new List<ItemIndexEntry>();
			index.AddRange(Load(@"Data\Libs\Foundry\Records\entities\scitem"));

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
				if (avoidType(entity.Components?.SAttachableComponentParams?.AttachDef.Type)) continue;

				// If the entity has a loadout file, then load it
				if (entity.Components?.SEntityComponentDefaultLoadoutParams?.loadout?.SItemPortLoadoutXMLParams != null)
				{
					entity.Components.SEntityComponentDefaultLoadoutParams.loadout.SItemPortLoadoutXMLParams.loadoutPath = OnXmlLoadout(entity.Components.SEntityComponentDefaultLoadoutParams.loadout.SItemPortLoadoutXMLParams.loadoutPath);
				}

				// If uses an ammunition magazine, then load it
				EntityClassDefinition magazine = null;
				if (!String.IsNullOrEmpty(entity.Components?.SCItemWeaponComponentParams?.ammoContainerRecord))
				{
					magazine = ClassParser<EntityClassDefinition>.ClassByRefCache.GetValueOrDefault(entity.Components?.SCItemWeaponComponentParams.ammoContainerRecord);
				}

				// If it is an ammo container or if it has a magazine then load the ammo properties
				AmmoIndexEntry ammoEntry = null;
				var ammoRef = magazine?.Components?.SAmmoContainerComponentParams?.ammoParamsRecord ?? entity.Components?.SAmmoContainerComponentParams?.ammoParamsRecord;
				if (!String.IsNullOrEmpty(ammoRef))
				{
					ammoEntry = Ammo.FirstOrDefault(x => x.reference == ammoRef);
				}

				// Write the JSON of this entity to its own file
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

				// Add it to the item index
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
					manufacturer = FindManufacturer(entity.Components?.SAttachableComponentParams?.AttachDef.Manufacturer)?.code,
					jsonFilename = jsonFilename,
					xmlFilename = entityFilename
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
			var avoidFile = fileSplit.Any(part => file_avoids.Contains(part));
			if (avoidFile) return true;

			var folderSplit = Path.GetDirectoryName(filename).Split('\\');
			var avoidFolder = folderSplit.Any(part => folder_avoids.Contains(part));
			if (avoidFolder) return true;

			return false;
		}

		bool avoidType(string type)
		{
			if (type == null) return true;
			return type_avoids.Contains(type, StringComparer.OrdinalIgnoreCase);
		}
	}
}
