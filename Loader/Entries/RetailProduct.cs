using System;
using System.Collections.Generic;
using System.Text;

namespace Loader.Entries
{
	public class RetailProduct
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public Item Item {get; set;}

		public Guid ItemId {get; set;}

		public double BasePrice { get; set; }

		public double MaxDiscountPercentage { get; set; }

		public double MaxPremiumPercentage { get; set; }

		public double ManHours { get; set; }

		public double OutputSPUPerProduction { get; set; }
	}
}
