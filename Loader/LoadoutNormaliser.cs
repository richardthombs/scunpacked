using System;
using System.Collections.Generic;
using System.Linq;

using scdb.Xml.DefaultLoadouts;
using scdb.Xml.Entities;
using scdb.Xml.Vehicles;
using scdb.Models;

namespace Loader
{
	public class LoadoutNormaliser
	{
		public scdb.Models.ItemPort[] NormaliseLoadout(Item[] defaultLoadoutItems, Part[] parts)
		{
			var modelLoadout = new List<scdb.Models.ItemPort>();

			foreach (var part in GetParts(parts))
			{
				if (part.@class == "ItemPort")
				{
					var portName = part.name;
					var itemName = FindItemByPort(defaultLoadoutItems, portName);

					var itemPort = new scdb.Models.ItemPort
					{
						port = portName,
						item = itemName,
						minsize = part.ItemPort?.minsize,
						maxsize = part.ItemPort?.maxsize,
						flags = GetItemPortFlags(part.ItemPort?.flags),
						types = GetItemPortTypes(part.ItemPort?.Types)
					};

					modelLoadout.Add(itemPort);
				}
			}

			return modelLoadout.ToArray();
		}

		public scdb.Models.ItemPort[] NormaliseLoadout(SItemPortLoadoutManualParams manualLoadout, Part[] parts)
		{
			var modelLoadout = new List<scdb.Models.ItemPort>();

			foreach (var part in GetParts(parts))
			{
				if (part.@class == "ItemPort")
				{
					var portName = part.name;
					var itemName = FindItemByPort(manualLoadout.entries, portName);

					var itemPort = new scdb.Models.ItemPort
					{
						port = portName,
						item = itemName,
						minsize = part.ItemPort?.minsize,
						maxsize = part.ItemPort?.maxsize,
						flags = GetItemPortFlags(part.ItemPort?.flags),
						types = GetItemPortTypes(part.ItemPort?.Types)
					};

					modelLoadout.Add(itemPort);

				}
			}

			return modelLoadout.ToArray();
		}

		public IEnumerable<Part> GetParts(Part[] parts)
		{
			if (parts == null) yield break;

			foreach (var part in parts)
			{
				yield return part;

				if (part.Parts != null)
				{
					foreach (var subPart in GetParts(part.Parts))
					{
						yield return subPart;
					}
				}
			}
		}

		public string FindItemByPort(Item[] defaultLoadoutItems, string portName)
		{
			foreach (var item in defaultLoadoutItems)
			{
				if (String.Compare(item.portName, portName, true) == 0) return item.itemName;
				if (item.Items != null)
				{
					var found = FindItemByPort(item.Items, portName);
					if (found != null) return found;
				}
			}

			return null;
		}

		public string FindItemByPort(SItemPortLoadoutEntryParams[] entries, string portName)
		{
			if (entries == null) return null;

			foreach (var entry in entries)
			{
				if (String.Compare(entry.itemPortName, portName, true) == 0) return entry.entityClassName;

				var found = FindItemByPort(entry.loadout?.SItemPortLoadoutManualParams?.entries, portName);
				if (found != null) return found;
			}

			return null;
		}

		Dictionary<string, string[]> GetItemPortTypes(scdb.Xml.Vehicles.Type[] vehicleItemPortTypes)
		{
			if (vehicleItemPortTypes == null) return null;

			var dict = new Dictionary<string, string[]>();
			foreach (var vehicleItemPortType in vehicleItemPortTypes)
			{
				var type = vehicleItemPortType.type;
				var subtypes = vehicleItemPortType.subtypes?.Split(",").Where(x => !String.IsNullOrEmpty(x)).ToArray();

				if (!dict.ContainsKey(type)) dict.Add(type, new string[0]);
				if (subtypes != null) dict[type] = dict[type].Concat(subtypes).ToArray();
			}
			return dict;
		}

		Dictionary<string, bool> GetItemPortFlags(string flags)
		{
			if (String.IsNullOrWhiteSpace(flags)) return null;

			return flags.Split(" ").Where(x => !String.IsNullOrEmpty(x)).ToDictionary(x => x, x => true);
		}
	}
}
