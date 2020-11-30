using System;
using System.Collections.Generic;
using System.Linq;

using scdb.Xml.Entities;

namespace Loader
{
	public class AmmoService
	{
		List<AmmoParams> ammoIndex;

		public AmmoService(List<AmmoParams> ammoIndex)
		{
			this.ammoIndex = ammoIndex;
		}

		public AmmoParams GetByReference(string reference)
		{
			return ammoIndex.FirstOrDefault(x => x.__ref == reference);
		}
	}
}
