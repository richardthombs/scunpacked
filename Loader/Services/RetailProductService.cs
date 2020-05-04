using System.Collections.Generic;
using System.Threading.Tasks;
using Loader.Entries;
using Loader.Loader;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Loader.Services
{
	internal class RetailProductService : LoaderService<RetailProduct>
	{
		private readonly RetailProductLoader _retailProductLoader;

		public RetailProductService(ILogger<LoaderService<RetailProduct>> logger, IOptions<ServiceOptions> options, RetailProductLoader retailProductLoader)
			: base(logger, options)
		{
			_retailProductLoader = retailProductLoader;
		}

		protected override string FileName => "retailproducts.json";

		protected override async Task<List<RetailProduct>> LoadItems()
		{
			var items = await _retailProductLoader.Load();
			return items;
		}
	}
}
