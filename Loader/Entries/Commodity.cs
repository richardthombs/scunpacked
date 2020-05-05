using System;

namespace Loader.Entries
{
	internal class Commodity : ILoaderItem
	{
		public string JsonFilename { get; set; }

		public string ClassName { get; set; }

		public CommodityTypeAndSubType Type { get; set; }

		public Guid TypeId {get; set; }

		public CommodityTypeAndSubType SubType { get; set; }

		public Guid? SubTypeId { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public Guid Id { get; set; }
	}
}
