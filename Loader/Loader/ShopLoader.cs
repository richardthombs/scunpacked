//-----------------------------------------------------------------------
// <copyright file="D:\projekte\scunpacked\Loader\ShopLoader.cs" company="primsoft.NET">
// Author: Joerg Primke
// Copyright (c) primsoft.NET. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Loader.Entries;
using Loader.SCDb.Xml.Shops;
using Loader.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Loader.Loader
{
	internal class ShopLoader
	{
		private readonly string[] _avoids =
		{
			"NovemberAnniversarySale2019",
			"NovemberAnniversarySale2018",
			"AC_Inventory",
			"TestInventories"
		};

		private readonly ILogger<ShopLoader> _logger;
		private readonly ServiceOptions _options;

		private readonly Dictionary<string, RetailProduct> _retailProducts;

		public ShopLoader(ILogger<ShopLoader> logger, IOptions<ServiceOptions> options, LoaderService<RetailProduct> retailProductService)
		{
			_options = options.Value;
			_logger = logger;
			_retailProducts = retailProductService.Items.ToDictionary(x => x.Id.ToString());
		}

		private string DataRoot => _options.SCData;

		public async Task<List<Shop>> Load()
		{
			var shopRootNode =
				await GenericParser.Parse<ShopLayoutNode>(Path.Combine(DataRoot, @"Data\Libs\Subsumption\Shops\ShopLayouts.xml"));
			return GetShops(shopRootNode).Select(node => GetShop(node)).ToList();
		}

		private Shop GetShop((ShopLayoutNode, string) node)
		{
			var (shopNode, p) = node;

			var shop = new Shop
			           {
				           Name = shopNode.Name,
				           ProfitMargin = shopNode.ProfitMargin,
				           AcceptsStolenGoods = Convert.ToBoolean(shopNode.AcceptsStolenGoods),
				           ContainerPath = p,
				           Inventory = GetInventory(shopNode).ToArray()
			           };

			_logger.LogInformation($"{shop.ContainerPath}: {shop.Name}, {shop.Inventory.Length} items");

			return shop;
		}

		private IEnumerable<(ShopLayoutNode, string)> GetShops(ShopLayoutNode node, string path = "")
		{
			if (_avoids.Any(x => node.Name == x))
			{
				yield break;
			}

			if (node.ShopInventoryNodes?.Length > 0)
			{
				yield return (node, path);
			}

			if (!(node.ShopLayoutNodes?.Length > 0))
			{
				yield break;
			}

			foreach (var layout in node.ShopLayoutNodes)
			{
				foreach (var (shop, p) in GetShops(layout, Path.Combine(path, layout.Name)))
				{
					yield return (shop, p);
				}
			}
		}

		private List<ShopItem> GetInventory(ShopLayoutNode shopNode)
		{
			var items = new List<ShopItem>();

			if (shopNode?.ShopInventoryNodes.Length > 0)
			{
				items = shopNode.ShopInventoryNodes
				                .Select(itemNode => GetItemNode(itemNode))
				                .Where(i => i != null)
				                .ToList();
			}

			return items;
		}

		private ShopItem GetItemNode(ShopInventoryNode itemNode)
		{
			var product = FindInventoryNode(itemNode.InventoryId);
			if (product == null)
			{
				_logger.LogWarning($"Can't find product {itemNode.Name} ({itemNode.InventoryId}) ");
				return null;
			}

			try
			{
				var item = new ShopItem
				           {
					           BasePriceOffsetPercentage = itemNode.BasePriceOffsetPercentage,
					           MaxDiscountPercentage = itemNode.MaxDiscountPercentage,
					           MaxPremiumPercentage = itemNode.MaxPremiumPercentage,
					           Inventory = itemNode.Inventory,
					           OptimalInventoryLevel = itemNode.OptimalInventoryLevel,
					           MaxInventory = itemNode.MaxInventory,
					           AutoRestock = Convert.ToBoolean(itemNode.AutoRestock),
					           AutoConsume = Convert.ToBoolean(itemNode.AutoConsume),
					           RefreshRatePercentagePerMinute = itemNode.RefreshRatePercentagePerMinute,
					           ShopBuysThis = itemNode.TransactionTypes.Any(x => x.Data == "Sell"),
					           ShopSellsThis = itemNode.TransactionTypes.Any(x => x.Data == "Buy"),
					           RetailProduct = product,
							   RetailProductId = product.Id,
					           Id = new Guid(itemNode.Id),
							   DisplayName = product.Item.Name
				           };

				return item;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "InventoryId = {0}", itemNode.InventoryId);
				throw;
			}
		}

		private RetailProduct FindInventoryNode(string inventoryId)
		{
			return _retailProducts.GetValueOrDefault(inventoryId);
		}
	}

	internal static class GenericParser
	{
		internal static async Task<T> Parse<T>(string xmlFilename)
		{
			string rootNodeName;
			using (var reader = XmlReader.Create(new StreamReader(xmlFilename)))
			{
				reader.MoveToContent();
				rootNodeName = reader.Name;
			}

			var xml = await File.ReadAllTextAsync(xmlFilename);
			var doc = new XmlDocument();
			doc.LoadXml(xml);

			var serialiser = new XmlSerializer(typeof(T), new XmlRootAttribute { ElementName = rootNodeName });

			using var stream = new XmlNodeReader(doc);
			var entity = (T)serialiser.Deserialize(stream);
			return entity;
		}

	}
}
