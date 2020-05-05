using System.Collections.Generic;
using System.Threading.Tasks;
using Loader.Entries;
using Loader.Helper;
using Loader.Loader;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Loader.Services
{
	internal class ShipsService : LoaderService<Ship>
	{
		private readonly LoadoutLoader _loadoutLoader;

		private readonly ShipLoader _shipLoader;

		public ShipsService(ILogger<LoaderService<Ship>> logger, IOptions<ServiceOptions> options,
		                    IJsonFileReaderWriter jsonFileReaderWriter, LoadoutLoader loadoutLoader,
		                    ShipLoader shipLoader)
			: base(logger, options, jsonFileReaderWriter)
		{
			_loadoutLoader = loadoutLoader;
			_shipLoader = shipLoader;
		}

		protected override string FileName => "ships.json";

		protected override Task<List<Ship>> LoadItems()
		{
			return _shipLoader.Load(path => _loadoutLoader.Load(path));
		}
	}
}
