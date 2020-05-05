using System.Collections.Generic;
using System.Threading.Tasks;
using Loader.Entries;
using Loader.Helper;
using Loader.Loader;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Loader.Services
{
	internal class CommodityTypeService : LoaderService<CommodityTypeAndSubType>
	{
		private readonly CommodityTypeLoader _commodityTypeLoader;

		private readonly LoadoutLoader _loadoutLoader;

		public CommodityTypeService(ILogger<LoaderService<CommodityTypeAndSubType>> logger,
		                            IOptions<ServiceOptions> options, IJsonFileReaderWriter jsonFileReaderWriter,
		                            LoadoutLoader loadoutLoader, CommodityTypeLoader commodityTypeLoader)
			: base(logger, options, jsonFileReaderWriter)
		{
			_loadoutLoader = loadoutLoader;
			_commodityTypeLoader = commodityTypeLoader;
		}

		protected override string FileName => "commoditytypes.json";

		protected override Task<List<CommodityTypeAndSubType>> LoadItems()
		{
			return _commodityTypeLoader.Load();
		}
	}
}
