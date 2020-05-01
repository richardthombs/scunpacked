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
using System.Xml;
using System.Xml.Serialization;
using Loader.SCDb.Xml.Shops;
using Microsoft.Extensions.Logging;

namespace Loader
{
	public class ShopLoader
	{
		private readonly string[] _avoids =
		{
			"NovemberAnniversarySale2019", "NovemberAnniversarySale2018", "AC_Inventory", "TestInventories"
		};

		private readonly LocalisationService _localisationService;
		private readonly RetailProductService _retailProductService;
		private readonly EntityParser _entityParser;
		private readonly ILogger<ShopLoader> _logger;

		public ShopLoader(ILogger<ShopLoader> logger, LocalisationService localisationService, RetailProductService retailProductService, EntityParser entityParser)
		{
			_localisationService = localisationService;
			_retailProductService = retailProductService;
			_entityParser = entityParser;
			_logger = logger;
		}

		public string DataRoot { get; set; }

		public Func<string, string> OnXmlLoadout { get; set; }

		public List<Shop> Load()
		{
			var shopRootNode =
				Parse<ShopLayoutNode>(Path.Combine(DataRoot, @"Data\Libs\Subsumption\Shops\ShopLayouts.xml"));
			var productList =
				Parse<Node>(Path.Combine(DataRoot, @"Data\Libs\Subsumption\Shops\RetailProductPrices.xml"));

			return GetShops(shopRootNode).Select(node => GetShop(node, productList)).ToList();
		}

		private Shop GetShop((ShopLayoutNode, string) node, Node productList)
		{
			var (shopNode, p) = node;

			var shop = new Shop
			           {
				           name = shopNode.Name,
				           profitMargin = shopNode.ProfitMargin,
				           acceptsStolenGoods = Convert.ToBoolean(shopNode.AcceptsStolenGoods),
				           containerPath = p,
				           inventory = GetInventory(productList, shopNode).ToArray()
			           };

			_logger.LogInformation($"{shop.containerPath}: {shop.name}, {shop.inventory.Length} items");
			foreach (var i in shop.inventory)
			{
				_logger.LogInformation($@"{(i.shopBuysThis ? "Buys" : string.Empty)} {(i.shopSellsThis ? "Sells" : string.Empty)} {i.name} {i.basePrice} {i.basePriceOffsetPercentage:n0}%");
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
				items = shopNode.ShopInventoryNodes.Select(itemNode => GetItemNode(itemNode, prices))
				                .Where(i => i != null)
				                .ToList();
			}

			return items;
		}

		private ShopItem GetItemNode(ShopInventoryNode itemNode, Node prices)
		{
			var product = FindInventoryNode(prices, itemNode.InventoryID);
			if (product == null)
			{
				_logger.LogWarning($"Can't find product {itemNode.Name} ({itemNode.InventoryID}) ");
				return null;
			}

			var entity = _entityParser.Parse(Path.Combine(DataRoot, product.Filename), OnXmlLoadout);

			var item = new ShopItem
			           {
				           name = itemNode.Name.ToLower(),
				           basePriceOffsetPercentage = itemNode.BasePriceOffsetPercentage,
				           maxDiscountPercentage = itemNode.MaxDiscountPercentage,
				           maxPremiumPercentage = itemNode.MaxPremiumPercentage,
				           inventory = itemNode.Inventory,
				           optimalInventoryLevel = itemNode.OptimalInventoryLevel,
				           maxInventory = itemNode.MaxInventory,
				           autoRestock = Convert.ToBoolean(itemNode.AutoRestock),
				           autoConsume = Convert.ToBoolean(itemNode.AutoConsume),
				           refreshRatePercentagePerMinute = itemNode.RefreshRatePercentagePerMinute,
				           shopBuysThis = itemNode.TransactionTypes.Any(x => x.Data == "Sell"),
				           shopSellsThis = itemNode.TransactionTypes.Any(x => x.Data == "Buy"),
				           basePrice = product.BasePrice,
				           filename = product.Filename
			           };

			if (entity?.Components.SAttachableComponentParams != null)
			{
				item.displayName =
					_localisationService.GetText(entity.Components.SAttachableComponentParams.AttachDef.Localization
					                                   .Name);
				item.tags = entity.Components.SAttachableComponentParams.AttachDef.Tags.Split(" ");
				item.type = entity.Components.SAttachableComponentParams.AttachDef.Type;
				item.subType = entity.Components.SAttachableComponentParams.AttachDef.SubType;
			}

			if (entity?.Components.CommodityComponentParams != null)
			{
				item.displayName = _localisationService.GetText(entity.Components.CommodityComponentParams.name);
			}

			return item;
		}

		private static Node FindInventoryNode(Node node, string inventoryId)
		{
			return node.ID == inventoryId ? node : node.RetailProducts?.Select(n => FindInventoryNode(n, inventoryId)).FirstOrDefault(found => found != null);
		}

		private static T Parse<T>(string xmlFilename)
		{
			string rootNodeName;
			using (var reader = XmlReader.Create(new StreamReader(xmlFilename)))
			{
				reader.MoveToContent();
				rootNodeName = reader.Name;
			}

			var xml = File.ReadAllText(xmlFilename);
			var doc = new XmlDocument();
			doc.LoadXml(xml);

			var serialiser = new XmlSerializer(typeof(T), new XmlRootAttribute {ElementName = rootNodeName});

			using var stream = new XmlNodeReader(doc);
			var entity = (T) serialiser.Deserialize(stream);
			return entity;
		}
	}
}
