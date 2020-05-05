using System;
using System.Collections.Generic;

namespace Loader.Entries
{
	public class Shop : ILoaderItem
	{
		public bool AcceptsStolenGoods { get; set; }

		public string ContainerPath { get; set; }

		public List<ShopItem> Inventory { get; set; }

		public string Name { get; set; }

		public double ProfitMargin { get; set; }

		public Guid Id { get; set; }
	}
}
