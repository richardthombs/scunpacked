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
using Loader.Parser;
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

		private readonly EntityParser _entityParser;

		private readonly LocalisationService _localisationService;
		private readonly ILogger<ShopLoader> _logger;
		private readonly ServiceOptions _options;

		public ShopLoader(ILogger<ShopLoader> logger, LocalisationService localisationService,
		                  EntityParser entityParser, IOptions<ServiceOptions> options)
		{
			_localisationService = localisationService;
			_entityParser = entityParser;
			_options = options.Value;
			_logger = logger;
		}

		private string DataRoot => _options.SCData;

		private Func<string, Task<string>> OnXmlLoadout { get; set; }

		public async Task<List<Shop>> Load(Func<string, Task<string>> onXmlLoadout)
		{
			OnXmlLoadout = onXmlLoadout;

			var shopRootNode =
				await Parse<ShopLayoutNode>(Path.Combine(DataRoot, @"Data\Libs\Subsumption\Shops\ShopLayouts.xml"));
			var productList =
				await Parse<Node>(Path.Combine(DataRoot, @"Data\Libs\Subsumption\Shops\RetailProductPrices.xml"));

			return GetShops(shopRootNode).Select(node => GetShop(node, productList)).ToList();
		}

		private Shop GetShop((ShopLayoutNode, string) node, Node productList)
		{
			var (shopNode, p) = node;

			var shop = new Shop
			           {
				           Name = shopNode.Name,
				           ProfitMargin = shopNode.ProfitMargin,
				           AcceptsStolenGoods = Convert.ToBoolean(shopNode.AcceptsStolenGoods),
				           ContainerPath = p,
				           Inventory = GetInventory(productList, shopNode).ToArray()
			           };

			_logger.LogInformation($"{shop.ContainerPath}: {shop.Name}, {shop.Inventory.Length} items");
			foreach (var i in shop.Inventory)
			{
				_logger.LogInformation($@"{(i.ShopBuysThis ? "Buys" : string.Empty)} {(i.ShopSellsThis ? "Sells" : string.Empty)} {i.Name} {i.BasePrice} {i.BasePriceOffsetPercentage:n0}%");
			}

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

		private List<ShopItem> GetInventory(Node prices, ShopLayoutNode shopNode)
		{
			var items = new List<ShopItem>();

			if (shopNode?.ShopInventoryNodes.Length > 0)
			{
				items = shopNode.ShopInventoryNodes
				                .Select(itemNode => GetItemNode(itemNode, prices).GetAwaiter().GetResult())
				                .Where(i => i != null)
				                .ToList();
			}

			return items;
		}

		private async Task<ShopItem> GetItemNode(ShopInventoryNode itemNode, Node prices)
		{
			var product = FindInventoryNode(prices, itemNode.InventoryId);
			if (product == null)
			{
				_logger.LogWarning($"Can't find product {itemNode.Name} ({itemNode.InventoryId}) ");
				return null;
			}

			var entity = await _entityParser.Parse(Path.Combine(DataRoot, product.Filename), OnXmlLoadout);

			var item = new ShopItem
			           {
				           Name = itemNode.Name.ToLower(),
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
				           BasePrice = product.BasePrice,
				           Filename = product.Filename
			           };

			if (entity?.Components.SAttachableComponentParams != null)
			{
				item.DisplayName =
					_localisationService.GetText(entity.Components.SAttachableComponentParams.AttachDef.Localization
					                                   .Name);
				item.Tags = entity.Components.SAttachableComponentParams.AttachDef.Tags.Split(" ");
				item.Type = entity.Components.SAttachableComponentParams.AttachDef.Type;
				item.SubType = entity.Components.SAttachableComponentParams.AttachDef.SubType;
			}

			if (entity?.Components.CommodityComponentParams != null)
			{
				item.DisplayName = _localisationService.GetText(entity.Components.CommodityComponentParams.name);
			}

			return item;
		}

		private static Node FindInventoryNode(Node node, string inventoryId)
		{
			return node.Id == inventoryId
				       ? node
				       : node.RetailProducts?.Select(n => FindInventoryNode(n, inventoryId))
				             .FirstOrDefault(found => found != null);
		}

		private static async Task<T> Parse<T>(string xmlFilename)
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

			var serialiser = new XmlSerializer(typeof(T), new XmlRootAttribute {ElementName = rootNodeName});

			using var stream = new XmlNodeReader(doc);
			var entity = (T) serialiser.Deserialize(stream);
			return entity;
		}
	}
}
