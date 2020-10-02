using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using scdb.Xml.Entities;

namespace Loader
{
	class ItemMatchRule
	{
		public Predicate<EntityClassDefinition> Matcher { get; set; }
		public Func<string, string, string> Classifier { get; set; }
	}

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
			"airtrafficcontroller",
			"bottle",
			"button",
			"char_accessory_head",
			"char_body",
			"char_head",
			"char_hair_color",
			"char_head_eyebrow",
			"char_head_eyelash",
			"char_head_eyes",
			"char_head_hair",
			"char_lens",
			"char_skin_color",
			"cloth",
			"debris",
			"decal",
			"display",
			"drink",
			"elevator",
			"flair_floor",
			"flair_surface",
			"flair_wall",
			"removablechip",
			"shopdisplay"
		};

		// This list is used to classify items into a hierarchy that is easier to consume by downstream websites
		// Items are currently split into "FPS" and "Ship" at the highest level, and each of these are split out
		// into their own "<blah>-items.json" file.
		List<ItemMatchRule> matchers = new List<ItemMatchRule>
		{
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponPersonal.*"), Classifier = (t,s) => $"FPS.Weapon.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponAttachment.Barrel") && TagMatch(item, "FPS_Barrel"), Classifier = (t,s) => $"FPS.WeaponAttachment.BarrelAttachment" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponAttachment.Barrel"), Classifier = (t,s) => $"Ship.{t}.{s}"} ,
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponAttachment.FiringMechanism"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponAttachment.PowerArray"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponAttachment.Ventilation"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponAttachment.IronSight"), Classifier = (t,s) => $"FPS.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponAttachment.Magazine"), Classifier = (t,s) => $"FPS.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponAttachment.Utility"), Classifier = (t,s) => $"FPS.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponAttachment.BottomAttachment"), Classifier = (t,s) => $"FPS.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponAttachment.Missile"), Classifier = (t,s) => $"FPS.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Light.Weapon"), Classifier = (t,s) => $"FPS.WeaponAttachment.Light" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Armor.*"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Cooler.*"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "EMP.*"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Missile.*"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "PowerPlant.*"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "QuantumDrive.*"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "QuantumInterdictionGenerator.*"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Radar.MidRangeRadar"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Scanner.Scanner"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Shield.*"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponDefensive.CountermeasureLauncher"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponGun.*"), Classifier = (t,s) => $"Ship.Weapon.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponMining.*"), Classifier = (t,s) => $"Ship.Mining.{s}" },

			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Char_Armor_Arms.*"), Classifier = (t,s) => $"FPS.Armor.Arms" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Char_Armor_Helmet.*"), Classifier = (t,s) => $"FPS.Armor.Helmet" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Char_Armor_Legs.*"), Classifier = (t,s) => $"FPS.Armor.Legs" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Char_Armor_Torso.*"), Classifier = (t,s) => $"FPS.Armor.Torso" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Char_Armor_Undersuit.*"), Classifier = (t,s) => $"FPS.Armor.Undersuit" },

			// Default catch all
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "*.*"), Classifier = (t,s) => null }
		};

		public static bool TypeMatch(EntityClassDefinition entity, string typePattern)
		{
			var patternSplit = typePattern.Split('.', 2);

			var type = patternSplit[0];
			if (type == "*") type = null;

			var subType = patternSplit.Length > 1 ? patternSplit[1] : null;
			if (subType == "*") subType = null;

			var entityType = entity?.Components?.SAttachableComponentParams?.AttachDef?.Type;
			var entitySubType = entity?.Components?.SAttachableComponentParams?.AttachDef?.SubType;

			if (!String.IsNullOrEmpty(type) && !String.Equals(type, entityType, StringComparison.OrdinalIgnoreCase)) return false;
			if (!String.IsNullOrEmpty(subType) && !String.Equals(subType, entitySubType, StringComparison.OrdinalIgnoreCase)) return false;

			return true;
		}

		public static bool TagMatch(EntityClassDefinition entity, string tag)
		{
			var tagList = entity?.Components?.SAttachableComponentParams?.AttachDef?.Tags ?? "";
			var split = tagList.Split(' ');
			return split.Contains(tag, StringComparer.OrdinalIgnoreCase);
		}

		public List<ItemIndexEntry> Load()
		{
			Directory.CreateDirectory(OutputFolder);

			var index = new List<ItemIndexEntry>();
			index.AddRange(Load(@"Data\Libs\Foundry\Records\entities\scitem"));

			// Once all the items have been loaded, we have to spin through them again looking for
			// any that use ammunition magazines so we can load the magazine and then load the ammunition it uses
			foreach (var item in index)
			{
				var entity = ClassParser<EntityClassDefinition>.ClassByNameCache[item.className];

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
				if (avoidType(entity.Components?.SAttachableComponentParams?.AttachDef.Type)) continue;

				// If the entity has a loadout file, then load it
				if (entity.Components?.SEntityComponentDefaultLoadoutParams?.loadout?.SItemPortLoadoutXMLParams != null)
				{
					entity.Components.SEntityComponentDefaultLoadoutParams.loadout.SItemPortLoadoutXMLParams.loadoutPath = OnXmlLoadout(entity.Components.SEntityComponentDefaultLoadoutParams.loadout.SItemPortLoadoutXMLParams.loadoutPath);
				}

				var indexEntry = CreateIndexEntry(entity);
				indexEntry.jsonUrl = $"/api/items/{entity.ClassName.ToLower()}.json";
				indexEntry.xmlSource = Path.GetRelativePath(DataRoot, entityFilename);

				// Add it to the item index
				index.Add(indexEntry);
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

		ItemIndexEntry CreateIndexEntry(EntityClassDefinition entity)
		{
			foreach (var match in matchers)
			{
				if (match.Matcher(entity))
				{
					var classification = match.Classifier(entity.Components?.SAttachableComponentParams?.AttachDef.Type, entity.Components?.SAttachableComponentParams.AttachDef.SubType);
					var indexEntry = new ItemIndexEntry
					{
						className = entity.ClassName,
						reference = entity.__ref,
						itemName = entity.ClassName.ToLower(),
						type = entity.Components?.SAttachableComponentParams?.AttachDef.Type,
						subType = entity.Components?.SAttachableComponentParams?.AttachDef.SubType,
						size = entity.Components?.SAttachableComponentParams?.AttachDef.Size,
						grade = entity.Components?.SAttachableComponentParams?.AttachDef.Grade,
						name = entity.Components?.SAttachableComponentParams?.AttachDef.Localization.Name,
						tags = entity.Components?.SAttachableComponentParams?.AttachDef.Tags,
						manufacturer = FindManufacturer(entity.Components?.SAttachableComponentParams?.AttachDef.Manufacturer)?.code,
						classification = classification
					};
					return indexEntry;
				}
			}

			throw new ApplicationException("Item didn't get picked up by the default match for some reason");
		}

		public void LoadAmmunitionIfNeeded(EntityClassDefinition entity)
		{

		}
	}
}
