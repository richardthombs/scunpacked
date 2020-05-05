using System.Collections.Generic;
using System.Threading.Tasks;
using Loader.Entries;
using Loader.Helper;
using Loader.Loader;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Loader.Services
{
	internal class ShopService : LoaderService<Shop>
	{
		private readonly ShopLoader _shopLoader;

		public ShopService(ILogger<ShopService> logger, IOptions<ServiceOptions> options,
		                   IJsonFileReaderWriter jsonFileReaderWriter, ShopLoader shopLoader)
			: base(logger, options, jsonFileReaderWriter)
		{
			_shopLoader = shopLoader;
		}

		protected override string FileName => "shops.json";

		protected override Task<List<Shop>> LoadItems()
		{
			return _shopLoader.Load();
		}
	}
}
