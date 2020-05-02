namespace Loader.Entries
{
	public class Shop
	{
		public bool AcceptsStolenGoods{ get; set; }
		public string ContainerPath{ get; set; }

		public ShopItem[] Inventory{ get; set; }
		public string Name{ get; set; }
		public double ProfitMargin{ get; set; }
	}
}
