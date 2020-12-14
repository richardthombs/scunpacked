using System.Collections.Generic;

using scdb.Xml.Entities;

namespace Loader
{
	public class LoadoutLoader
	{
		XmlLoadoutLoader xmlLoader;
		ManualLoadoutLoader manualLoader;

		public LoadoutLoader(XmlLoadoutLoader xmlLoader, ManualLoadoutLoader manualLoader)
		{
			this.xmlLoader = xmlLoader;
			this.manualLoader = manualLoader;
		}

		public List<StandardisedLoadoutEntry> Load(EntityClassDefinition entity)
		{
			var loadout = new List<StandardisedLoadoutEntry>();

			var loadoutParams = entity.Components.SEntityComponentDefaultLoadoutParams?.loadout;

			if (loadoutParams?.SItemPortLoadoutManualParams != null) loadout.AddRange(LoadManualLoadout(loadoutParams.SItemPortLoadoutManualParams));
			if (loadoutParams?.SItemPortLoadoutXMLParams != null) loadout.AddRange(LoadXmlLoadout(loadoutParams.SItemPortLoadoutXMLParams));

			return loadout;
		}

		List<StandardisedLoadoutEntry> LoadManualLoadout(SItemPortLoadoutManualParams manualParams)
		{
			return manualLoader.BuildLoadout(manualParams);
		}

		List<StandardisedLoadoutEntry> LoadXmlLoadout(SItemPortLoadoutXMLParams xmlParams)
		{
			return xmlLoader.Load(xmlParams.loadoutPath);
		}
	}
}
