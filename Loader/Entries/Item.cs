using System;

namespace Loader.Entries
{
	public class Item : ILoaderItem
	{
		public string ClassName{ get; set; }
		public int? Grade{ get; set; }
		public string ItemName{ get; set; }
		public string JsonFilename{ get; set; }
		public Manufacturer Manufacturer{ get; set; }
		public string Name{ get; set; }
		public int? Size{ get; set; }
		public string SubType{ get; set; }
		public string Type{ get; set; }

		public string Description { get; set; }

		public Guid Id {get; set;}

		public Guid ManufacturerId {get; set;}
	}
}
