using System;
using System.Collections.Generic;
using System.Linq;

namespace Loader
{
	public class ManufacturerService
	{
		List<ManufacturerIndexEntry> manufacturers;

		public ManufacturerService(List<ManufacturerIndexEntry> manufacturers)
		{
			this.manufacturers = manufacturers;
		}

		public StandardisedManufacturer GetManufacturer(string guid, string fallbackEntityClassName = null)
		{
			// Try and find by the reference guid
			var found = manufacturers.FirstOrDefault(x => x.reference == guid);

			// If that didn't work, try and extract the manufacturer code from the entity class name
			if (found == null && fallbackEntityClassName != null)
			{
				var fallback = fallbackEntityClassName.Split("_");
				var code = fallback[0];
				found = manufacturers.FirstOrDefault(x => x.code == code);
			}

			// If that didn't work, then give up
			if (found == null) return null;

			return new StandardisedManufacturer
			{
				Code = found.code,
				Name = found.name
			};
		}
	}
}
