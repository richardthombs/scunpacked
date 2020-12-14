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

		ItemBuilder itemBuilder;
		ManufacturerService manufacturerSvc;
		ItemClassifier itemClassifier;
		EntityService entitySvc;
		AmmoService ammoSvc;
		ItemInstaller itemInstaller;
		LoadoutLoader loadoutLoader;

		// Don't dump items with these types
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
			"drink",
			"flair_floor",
			"flair_surface",
			"flair_wall",
			"removablechip",
			"shopdisplay"
		};

		public ItemLoader(ItemBuilder itemBuilder, ManufacturerService manufacturerSvc, EntityService entitySvc, AmmoService ammoSvc, ItemInstaller itemInstaller, LoadoutLoader loadoutLoader)
		{
			this.itemBuilder = itemBuilder;
			this.manufacturerSvc = manufacturerSvc;
			this.itemClassifier = new ItemClassifier();
			this.entitySvc = entitySvc;
			this.ammoSvc = ammoSvc;
			this.itemInstaller = itemInstaller;
			this.loadoutLoader = loadoutLoader;
		}

		public List<ItemIndexEntry> Load(string typeFilter = null)
		{
			Directory.CreateDirectory(Path.Combine(OutputFolder, "items"));
			Directory.CreateDirectory(Path.Combine(OutputFolder, "v2", "items"));

			var damageResistanceMacros = LoadDamageResistanceMacros();

			Console.WriteLine($"ItemLoader: Creating index...");
			var index = CreateIndex(typeFilter);

			// Once all the items have been loaded, we have to spin through them again looking for
			// any that use ammunition magazines so we can load the magazine and then load the ammunition it uses
			Console.WriteLine($"ItemLoader: Creating {index.Count} item files...");
			foreach (var item in index)
			{
				var entity = entitySvc.GetByClassName(item.className);

				// If uses an ammunition magazine, then load it
				EntityClassDefinition magazine = null;
				if (!String.IsNullOrEmpty(entity.Components?.SCItemWeaponComponentParams?.ammoContainerRecord))
				{
					magazine = entitySvc.GetByReference(entity.Components.SCItemWeaponComponentParams.ammoContainerRecord);
				}

				// If it is an ammo container or if it has a magazine then load the ammo properties
				AmmoParams ammoEntry = null;
				var ammoRef = magazine?.Components?.SAmmoContainerComponentParams?.ammoParamsRecord ?? entity.Components?.SAmmoContainerComponentParams?.ammoParamsRecord;
				if (!String.IsNullOrEmpty(ammoRef))
				{
					ammoEntry = ammoSvc.GetByReference(ammoRef);
				}

				DamageResistance damageResistances = null;
				if (!String.IsNullOrEmpty(entity.Components?.SCItemSuitArmorParams?.damageResistance))
				{
					var damageMacro = damageResistanceMacros.Find(y => y.__ref == entity.Components.SCItemSuitArmorParams.damageResistance);
					damageResistances = damageMacro?.damageResistance;
				}

				var stdItem = itemBuilder.BuildItem(entity);
				var loadout = loadoutLoader.Load(entity);
				itemInstaller.InstallLoadout(stdItem, loadout);

				stdItem.Classification = item.classification;
				item.stdItem = stdItem;

				File.WriteAllText(Path.Combine(OutputFolder, "v2", "items", $"{entity.ClassName.ToLower()}.json"), JsonConvert.SerializeObject(stdItem));
				File.WriteAllText(Path.Combine(OutputFolder, "v2", "items", $"{entity.ClassName.ToLower()}-raw.json"), JsonConvert.SerializeObject(entity));

				// Write the JSON of this entity to its own file
				var jsonFilename = Path.Combine(OutputFolder, "items", $"{entity.ClassName.ToLower()}.json");
				var json = JsonConvert.SerializeObject(new
				{
					magazine = magazine,
					ammo = ammoEntry,
					Raw = new
					{
						Entity = entity,
					},
					damageResistances = damageResistances
				});
				File.WriteAllText(jsonFilename, json);
			}

			File.WriteAllText(Path.Combine(OutputFolder, "items.json"), JsonConvert.SerializeObject(index));

			// Create an index file for each different item type
			var typeIndicies = new Dictionary<string, List<ItemIndexEntry>>();
			foreach (var entry in index)
			{
				if (String.IsNullOrEmpty(entry.classification)) continue;

				var type = entry.classification.Split('.')[0];
				if (!typeIndicies.ContainsKey(type)) typeIndicies.Add(type, new List<ItemIndexEntry>());
				var typeIndex = typeIndicies[type];
				typeIndex.Add(entry);
			}
			foreach (var pair in typeIndicies)
			{
				File.WriteAllText(Path.Combine(OutputFolder, pair.Key.ToLower() + "-items.json"), JsonConvert.SerializeObject(pair.Value));
			}

			return index;
		}

		private List<DamageResistanceMacro> LoadDamageResistanceMacros()
		{
			var damageResistanceMacroFolder = @"Data\Libs\Foundry\Records\damage";
			var damageResistanceMacros = new List<DamageResistanceMacro>();

			foreach (var damageMacroFilename in Directory.EnumerateFiles(Path.Combine(DataRoot, damageResistanceMacroFolder), "*.xml", SearchOption.AllDirectories))
			{
				var damageResistanceMacroParser = new DamageResistanceMacroParser();
				DamageResistanceMacro entity = damageResistanceMacroParser.Parse(damageMacroFilename);
				if (entity == null) continue;

				damageResistanceMacros.Add(entity);
			}

			return damageResistanceMacros;
		}

		List<ItemIndexEntry> CreateIndex(string typeFilter)
		{
			var index = new List<ItemIndexEntry>();

			foreach (var entity in entitySvc.GetAll(typeFilter))
			{
				// Skip types that are not very interesting
				if (avoidType(entity.Components?.SAttachableComponentParams?.AttachDef.Type)) continue;

				var indexEntry = CreateIndexEntry(entity);

				// Add it to the item index
				index.Add(indexEntry);
			}

			return index;
		}

		ItemIndexEntry CreateIndexEntry(EntityClassDefinition entity)
		{
			var classification = itemClassifier.Classify(entity);

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
				manufacturer = manufacturerSvc.GetManufacturer(entity.Components?.SAttachableComponentParams?.AttachDef.Manufacturer, entity.ClassName)?.Code,
				classification = classification
			};
			return indexEntry;
		}

		bool avoidType(string type)
		{
			if (type == null) return true;
			return type_avoids.Contains(type, StringComparer.OrdinalIgnoreCase);
		}
	}
}
