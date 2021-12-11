using System;
using System.Collections.Generic;
using System.Linq;

using scdb.Xml.Entities;

namespace Loader
{
	class ItemMatchRule
	{
		public Predicate<EntityClassDefinition> Matcher { get; set; }
		public Func<string, string, string> Classifier { get; set; }
	}

	public class ItemClassifier
	{
		// This list is used to classify items into a hierarchy that is easier to consume by downstream websites
		// Items are currently split into "FPS" and "Ship" at the highest level, and each of these are split out
		// into their own "<blah>-items.json" file.
		List<ItemMatchRule> matchers = new List<ItemMatchRule>
		{
			// Ship weapons
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponDefensive.CountermeasureLauncher"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponGun.*"), Classifier = (t,s) => $"Ship.Weapon.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponMining.*"), Classifier = (t,s) => $"Ship.Mining.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponAttachment.Barrel") && !TagMatch(item, "FPS_Barrel"), Classifier = (t,s) => $"Ship.{t}.{s}"} ,
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponAttachment.FiringMechanism"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponAttachment.PowerArray"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponAttachment.Ventilation"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "MissileLauncher.*"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Missile.*"), Classifier = (t,s) => $"Ship.{t}.{s}" },

			// Ship components
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Armor.*"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Cooler.*"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "EMP.*"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "PowerPlant.*"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "QuantumDrive.*"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "QuantumInterdictionGenerator.*"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Radar.*"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Scanner.*"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Ping.*"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Transponder.*"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Shield.*"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Paints.*"), Classifier = (t,s) => $"Ship.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponRegenPool.*"), Classifier = (t,s) => $"Ship.{t}.{s}" },

			// FPS weapons
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponPersonal.*"), Classifier = (t,s) => $"FPS.Weapon.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponAttachment.Barrel") && TagMatch(item, "FPS_Barrel"), Classifier = (t,s) => $"FPS.WeaponAttachment.BarrelAttachment" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponAttachment.IronSight"), Classifier = (t,s) => $"FPS.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponAttachment.Magazine"), Classifier = (t,s) => $"FPS.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponAttachment.Utility"), Classifier = (t,s) => $"FPS.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponAttachment.BottomAttachment"), Classifier = (t,s) => $"FPS.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "WeaponAttachment.Missile"), Classifier = (t,s) => $"FPS.{t}.{s}" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Light.Weapon"), Classifier = (t,s) => $"FPS.WeaponAttachment.Light" },

			// FPS armor
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Char_Armor_Arms.*"), Classifier = (t,s) => $"FPS.Armor.Arms" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Char_Armor_Helmet.*"), Classifier = (t,s) => $"FPS.Armor.Helmet" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Char_Armor_Legs.*"), Classifier = (t,s) => $"FPS.Armor.Legs" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Char_Armor_Torso.*"), Classifier = (t,s) => $"FPS.Armor.Torso" },
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "Char_Armor_Undersuit.*"), Classifier = (t,s) => $"FPS.Armor.Undersuit" },

			// Default catch all
			new ItemMatchRule { Matcher = (item) => TypeMatch(item, "*.*"), Classifier = (t,s) => null }
		};

		static bool TypeMatch(EntityClassDefinition entity, string typePattern)
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

		static bool TagMatch(EntityClassDefinition entity, string tag)
		{
			var tagList = entity?.Components?.SAttachableComponentParams?.AttachDef?.Tags ?? "";
			var split = tagList.Split(' ');
			return split.Contains(tag, StringComparer.OrdinalIgnoreCase);
		}

		public string Classify(EntityClassDefinition entity)
		{
			foreach (var match in matchers)
			{
				if (!match.Matcher(entity)) continue;

				var classification = match.Classifier(entity.Components?.SAttachableComponentParams?.AttachDef.Type, entity.Components?.SAttachableComponentParams.AttachDef.SubType);
				if (classification?.EndsWith(".UNDEFINED") ?? false) classification = classification.Substring(0, classification.Length - 10);

				return classification;
			}

			return null;
		}
	}
}
