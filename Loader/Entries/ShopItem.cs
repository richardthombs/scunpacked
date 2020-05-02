using Loader.SCDb.Xml.Entities;

namespace Loader.Entries
{
	public class ShopItem
	{
		public bool AutoConsume{ get; set; }
		public bool AutoRestock{ get; set; }
		public double BasePrice{ get; set; }
		public double BasePriceOffsetPercentage{ get; set; }
		public string DisplayName{ get; set; }
		public string Filename{ get; set; }
		public double Inventory{ get; set; }

		public EntityClassDefinition Item{ get; set; }
		public double MaxDiscountPercentage{ get; set; }
		public double MaxInventory{ get; set; }
		public double MaxPremiumPercentage{ get; set; }
		public string Name{ get; set; }
		public double OptimalInventoryLevel{ get; set; }
		public double RefreshRatePercentagePerMinute{ get; set; }
		public bool ShopBuysThis{ get; set; }
		public bool ShopSellsThis{ get; set; }
		public string SubType{ get; set; }
		public string[] Tags{ get; set; }
		public string Type{ get; set; }
	}
}
