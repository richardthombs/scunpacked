using System;

namespace Loader.Entries
{
	public class Manufacturer : ILoaderItem
	{
		public string Code { get; set; }

		public string Description { get; set; }

		public string Name { get; set; }

		public Guid Id { get; set; }
	}
}
