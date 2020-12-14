using System;
using System.Collections.Generic;

using scdb.Xml.Entities;

namespace Loader
{
	public class ManualLoadoutLoader
	{
		public List<StandardisedLoadoutEntry> BuildLoadout(SItemPortLoadoutManualParams manualParams)
		{
			var entries = new List<StandardisedLoadoutEntry>();

			foreach (var cigEntry in manualParams.entries)
			{
				entries.Add(BuildLoadout(cigEntry));
			}

			return entries;
		}

		StandardisedLoadoutEntry BuildLoadout(SItemPortLoadoutEntryParams cigLoadoutEntry)
		{
			var entry = new StandardisedLoadoutEntry
			{
				PortName = cigLoadoutEntry.itemPortName,
				ClassName = cigLoadoutEntry.entityClassName,
				Entries = new List<StandardisedLoadoutEntry>()
			};

			if (cigLoadoutEntry.loadout.SItemPortLoadoutManualParams != null)
			{
				foreach (var e in cigLoadoutEntry.loadout.SItemPortLoadoutManualParams.entries)
				{
					entry.Entries.Add(BuildLoadout(e));
				}
			}

			return entry;
		}
	}
}
