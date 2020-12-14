using System;
using System.Collections.Generic;
using System.Linq;

namespace Loader
{
	public class ItemInstaller
	{
		EntityService entitySvc;
		LoadoutLoader loadoutLoader;
		ItemBuilder itemBuilder;

		public ItemInstaller(EntityService entitySvc, LoadoutLoader loadoutLoader, ItemBuilder itemBuilder)
		{
			this.entitySvc = entitySvc;
			this.loadoutLoader = loadoutLoader;
			this.itemBuilder = itemBuilder;
		}

		public void InstallLoadout(StandardisedItem item, List<StandardisedLoadoutEntry> loadout)
		{
			foreach (var port in item.Ports)
			{
				InstallLoadout(port, loadout);
			}
		}

		public void InstallLoadout(List<StandardisedPart> parts, List<StandardisedLoadoutEntry> loadout)
		{
			foreach (var part in parts)
			{
				InstallLoadout(part, loadout);
			}
		}

		public void InstallLoadout(StandardisedPart part, List<StandardisedLoadoutEntry> loadout)
		{
			if (part.Port != null) InstallLoadout(part.Port, loadout);
			InstallLoadout(part.Parts, loadout);
		}

		public void InstallLoadout(List<StandardisedItemPort> ports, List<StandardisedLoadoutEntry> loadout)
		{
			foreach (var port in ports)
			{
				InstallLoadout(port, loadout);
			}
		}

		public void InstallLoadout(StandardisedItemPort port, List<StandardisedLoadoutEntry> loadout)
		{
			var loadoutEntry = FindLoadoutEntry(port.PortName, loadout);
			if (String.IsNullOrEmpty(loadoutEntry?.ClassName)) return;

			port.Loadout = loadoutEntry.ClassName;

			var item = entitySvc.GetByClassName(loadoutEntry.ClassName);
			if (item == null) return;

			var standardisedItem = itemBuilder.BuildItem(item);
			port.InstalledItem = standardisedItem;

			// Update the loadout with anything this item brings with it
			var newLoadout = loadoutLoader.Load(item);
			loadoutEntry.Entries.AddRange(newLoadout);

			InstallLoadout(standardisedItem.Ports, loadoutEntry.Entries);
		}

		StandardisedLoadoutEntry FindLoadoutEntry(string portName, List<StandardisedLoadoutEntry> loadout)
		{
			var loadoutEntry = loadout.FirstOrDefault(x => String.Equals(x.PortName, portName, StringComparison.OrdinalIgnoreCase));
			return loadoutEntry;
		}
	}
}
