using System.Collections.Generic;
using System.Threading.Tasks;
using Loader.Entries;
using Loader.Helper;
using Loader.Loader;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Loader.Services
{
	internal class CommodityService : LoaderService<Commodity>
	{
		private readonly CommodityLoader _commodityLoader;

		private readonly LoadoutLoader _loadoutLoader;

		public CommodityService(ILogger<LoaderService<Commodity>> logger, IOptions<ServiceOptions> options,
		                        IJsonFileReaderWriter jsonFileReaderWriter, LoadoutLoader loadoutLoader,
		                        CommodityLoader commodityLoader)
			: base(logger, options, jsonFileReaderWriter)
		{
			_loadoutLoader = loadoutLoader;
			_commodityLoader = commodityLoader;
		}

		protected override string FileName => "commodities.json";

		protected override Task<List<Commodity>> LoadItems()
		{
			return _commodityLoader.Load(path => _loadoutLoader.Load(path));
		}
	}
}
