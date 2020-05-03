using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Loader.Entries;
using Loader.Parser;
using Loader.SCDb.Xml.Shops;
using Loader.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Loader.Loader
{
	internal class RetailProductLoader
	{
		private LocalisationService _localisationService;
		private EntityParser _entityParser;
		private readonly ServiceOptions _options;
		private readonly ILogger<RetailProductLoader> _logger;
		private readonly Dictionary<string, Item> _items;

		public RetailProductLoader(ILogger<RetailProductLoader> logger, LocalisationService localisationService,
		                           EntityParser entityParser, IOptions<ServiceOptions> options, LoaderService<Item> itemService)
		{
			_localisationService = localisationService;
			_entityParser = entityParser;
			_options = options.Value;
			_logger = logger;
			_items = itemService.Items.ToDictionary(item => item.Id.ToString());
		}

		private string DataRoot => _options.SCData;

		private Func<string, Task<string>> OnXmlLoadout { get; set; }

		public async Task<List<RetailProduct>> Load()
		{
			var productList =
				await GenericParser.Parse<Node>(Path.Combine(DataRoot, @"Data\Libs\Subsumption\Shops\RetailProductPrices.xml"));

			return GetNodes(productList).Select(node => GetRetailProductFromNode(node)).Where(i => i != null).ToList();
		}

		private RetailProduct GetRetailProductFromNode(Node node)
		{
			var item = _items.GetValueOrDefault(node.Id);

			if (item == null)
			{
				_logger.LogWarning("Can't find item with id {0}", node.Id);
				return null;
			}

			return new RetailProduct
			       {
				       Id = item.Id,
				       BasePrice = node.BasePrice,
				       Item = item,
				       ItemId = item.Id,
				       ManHours = node.ManHours,
				       MaxDiscountPercentage = node.MaxDiscountPercentage,
				       MaxPremiumPercentage = node.MaxPremiumPercentage,
				       Name = item.Name,
				       OutputSPUPerProduction = node.OutputSPUPerProduction
			       };

		}

		private IEnumerable<Node> GetNodes(Node node)
		{
			if (node.RetailProducts?.Length > 0)
			{
				yield return node;
			}

			if (!(node.RetailProducts?.Length > 0))
			{
				yield break;
			}

			foreach (var subNode in node.RetailProducts)
			{
				foreach (var subProduct in GetNodes(subNode))
				{
					yield return subProduct;
				}
			}
		}
	}
}
