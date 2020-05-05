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
		private readonly Dictionary<string, Item> _items;

		private readonly ILogger<RetailProductLoader> _logger;

		private readonly ServiceOptions _options;

		public RetailProductLoader(ILogger<RetailProductLoader> logger, IOptions<ServiceOptions> options,
		                           LoaderService<Item> itemService)
		{
			_options = options.Value;
			_logger = logger;
			_items = itemService.Items;
		}

		private string DataRoot => _options.SCData;

		public async Task<List<RetailProduct>> Load()
		{
			var productList =
				await GenericParser.Parse<Node>(Path.Combine(DataRoot,
				                                             @"Data\Libs\Subsumption\Shops\RetailProductPrices.xml"));

			return GetNodes(productList).Select(GetRetailProductFromNode).Where(i => i != null).ToList();
		}

		private RetailProduct GetRetailProductFromNode(Node node)
		{
			if (node.Id == null)
			{
				_logger.LogWarning("This is a node (name = {0}) with id is null", node.Name);
				return null;
			}

			var item = _items.GetValueOrDefault(node.Id);

			if (item == null)
			{
				_logger.LogWarning("Can't find item (name = {0}) with id {1}", node.Name, node.Id);
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
			yield return node;

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
