using System;

namespace Loader.Entries
{
	public class ShopItem : ILoaderItem
	{
		public bool AutoConsume { get; set; }
		public bool AutoRestock { get; set; }
		public double BasePriceOffsetPercentage { get; set; }
		public string DisplayName { get; set; }
		public string Filename { get; set; }
		public double Inventory { get; set; }
		public RetailProduct RetailProduct { get; set; }
		public Guid RetailProductId { get; set; }
		public double MaxDiscountPercentage { get; set; }
		public double MaxInventory { get; set; }
		public double MaxPremiumPercentage { get; set; }
		public double OptimalInventoryLevel { get; set; }
		public double RefreshRatePercentagePerMinute { get; set; }
		public bool ShopBuysThis { get; set; }
		public bool ShopSellsThis { get; set; }
		public Guid Id { get; set; }
	}
}
