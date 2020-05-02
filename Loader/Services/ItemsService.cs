using System.Collections.Generic;
using System.Threading.Tasks;
using Loader.Entries;
using Loader.Loader;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Loader.Services
{
	internal class ItemsService : LoaderService<Item>
	{
		private readonly ItemLoader _itemLoader;
		private readonly LoadoutLoader _loadoutLoader;

		public ItemsService(ILogger<LoaderService<Item>> logger, IOptions<ServiceOptions> options,
		                    LoadoutLoader loadoutLoader, ItemLoader itemLoader)
			: base(logger, options)
		{
			_loadoutLoader = loadoutLoader;
			_itemLoader = itemLoader;
		}

		protected override string FileName => "items.json";

		protected override Task<List<Item>> LoadItems()
		{
			return _itemLoader.Load(path => _loadoutLoader.Load(path));
		}
	}
}
