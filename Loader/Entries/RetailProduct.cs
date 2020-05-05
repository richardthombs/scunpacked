using System;

namespace Loader.Entries
{
	public class RetailProduct : ILoaderItem
	{
		public string Name { get; set; }

		public Item Item { get; set; }

		public Guid ItemId { get; set; }

		public double BasePrice { get; set; }

		public double MaxDiscountPercentage { get; set; }

		public double MaxPremiumPercentage { get; set; }

		public double ManHours { get; set; }

		public double OutputSPUPerProduction { get; set; }

		public Guid Id { get; set; }
	}
}
