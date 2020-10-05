using scdb.Xml.Entities;
using System.Collections.Generic;

namespace Loader
{
	public class Shop
	{
		public string name;
		public string containerPath;
		public bool acceptsStolenGoods;
		public double profitMargin;
		public string reference;
		public string parent;

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
		public bool shopRentThis;
		public string filename;
		public string type;
		public string subType;
		public string[] tags;
		public List<ShopRentalTemplate> rentalTemplates;
		public string node_reference;
		public string item_reference;

		public EntityClassDefinition item;
	}
}
