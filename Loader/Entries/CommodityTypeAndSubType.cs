using System;

namespace Loader.Entries
{
	internal class CommodityTypeAndSubType : ILoaderItem
	{
		public string TypeName { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public string Type { get; set; }

		public string Symbol { get; set; }

		public Guid Id { get; set; }
	}
}
