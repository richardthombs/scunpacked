using Loader.SCDb.Xml.Entities;

namespace Loader
{
	public class Shop
	{
		public string name;
		public string containerPath;
		public bool acceptsStolenGoods;
		public double profitMargin;

		public ShopItem[] inventory;
	}

	public class ShopItem
	{
		public string name;
		public string displayName;
		public double basePrice;
		public double basePriceOffsetPercentage;
		public double maxDiscountPercentage;
		public double maxPremiumPercentage;
		public double inventory;
		public double optimalInventoryLevel;
		public double maxInventory;
		public bool autoRestock;
		public bool autoConsume;
		public double refreshRatePercentagePerMinute;
		public bool shopBuysThis;
		public bool shopSellsThis;
		public string filename;
		public string type;
		public string subType;
		public string[] tags;

		public EntityClassDefinition item;
	}
}
