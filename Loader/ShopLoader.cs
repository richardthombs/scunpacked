using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

using Newtonsoft.Json;

using scdb.Xml.Shops;
using scdb.Xml.Entities;

namespace Loader
{
	public class ShopLoader
	{
		public string OutputFolder { get; set; }
		public string DataRoot { get; set; }

		string[] avoids =
		{
			"NovemberAnniversarySale2019",
			"NovemberAnniversarySale2018",
			"AC_Inventory",
			"TestInventories"
		};

		LocalisationService localisationService;
		EntityService entitySvc;
		List<RentalTemplate> rentalTemplates;

		public ShopLoader(LocalisationService localisationService, EntityService entitySvc)
		{
			this.localisationService = localisationService;
			this.entitySvc = entitySvc;
		}

		public List<Shop> Load()
		{
			var shops = new List<Shop>();

			var productList = Parse<Node>(Path.Combine(DataRoot, @"Data\Libs\Subsumption\Shops\RetailProductPrices.xml"));
			var shopRootNode = Parse<ShopLayoutNode>(Path.Combine(DataRoot, @"Data\Libs\Subsumption\Shops\ShopLayouts.xml"));
			var rentalTemplatesFolder = @"Data\Libs\Subsumption\Shops\Templates";

			this.rentalTemplates = new List<RentalTemplate>();

			foreach (var templateFilename in Directory.EnumerateFiles(Path.Combine(DataRoot, rentalTemplatesFolder), "*.xml", SearchOption.AllDirectories))
			{
				var rentalTemplateParser = new RentalTemplateParser();
				RentalTemplate entity = rentalTemplateParser.Parse(templateFilename);
				if (entity == null) continue;

				this.rentalTemplates.Add(entity);
			}

			foreach (var (shopNode, p) in GetShops(shopRootNode))
			{
				var shop = new Shop
				{
					name = GetShopName(shopNode.Name),
					profitMargin = shopNode.ProfitMargin,
					acceptsStolenGoods = Convert.ToBoolean(shopNode.AcceptsStolenGoods),
					reference = shopNode.ID,
					parent = shopRootNode.ID,
					containerPath = p
				};

				shop.inventory = GetInventory(productList, shopNode).ToArray();
				shops.Add(shop);

				Console.WriteLine($"ShopLoader: {shop.containerPath}");
			}

			File.WriteAllText(Path.Combine(OutputFolder, "shops.json"), JsonConvert.SerializeObject(shops));

			return shops;
		}

		IEnumerable<(ShopLayoutNode, string)> GetShops(ShopLayoutNode node, string path = "")
		{
			if (avoids.Any(x => node.Name == x)) yield break;

			if (node.ShopInventoryNodes?.Length > 0) yield return (node, path);

			if (node.ShopLayoutNodes?.Length > 0)
			{
				foreach (var layout in node.ShopLayoutNodes)
				{
					foreach (var (shop, p) in GetShops(layout, Path.Combine(path, layout.Name)))
					{
						yield return (shop, p);
					}
				}
			}

			yield break;
		}

		List<ShopItem> GetInventory(Node prices, ShopLayoutNode shopNode)
		{
			var items = new List<ShopItem>();

			if (shopNode?.ShopInventoryNodes.Length > 0)
			{
				foreach (var itemNode in shopNode.ShopInventoryNodes)
				{
					var product = FindInventoryNode(prices, itemNode.InventoryID);
					if (product == null) Console.WriteLine($"ShopLoader: Can't find product {itemNode.Name} ({itemNode.InventoryID}) ");
					else
					{
						var entity = entitySvc.GetByFilename(Path.Combine(DataRoot, product.Filename));

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
							shopRentThis = itemNode.TransactionTypes.Any(x => x.Data == "Rent"),
							basePrice = product.BasePrice,
							filename = product.Filename,
							node_reference = itemNode.ID,
							item_reference = itemNode.InventoryID,
							rentalTemplates = new List<ShopRentalTemplate>()
						};

						foreach (var rentalTemplate in itemNode.RentalTemplates)
						{
							var template = rentalTemplates.Find(y => y.ShopRentalTemplate.ID == rentalTemplate.Data);
							if (template != null)
							{
								item.rentalTemplates.Add(template.ShopRentalTemplate);
							}
						}

						if (entity?.Components.SAttachableComponentParams != null)
						{
							item.displayName = localisationService.GetText(entity.Components.SAttachableComponentParams.AttachDef.Localization.Name);
							item.tags = entity.Components.SAttachableComponentParams.AttachDef.Tags.Split(" ");
							item.type = entity.Components.SAttachableComponentParams.AttachDef.Type;
							item.subType = entity.Components.SAttachableComponentParams.AttachDef.SubType;
						}

						if (entity?.Components.CommodityComponentParams != null)
						{
							item.displayName = localisationService.GetText(entity.Components.CommodityComponentParams.name);
						}

						items.Add(item);
					}
				}
			}

			return items;
		}

		Node FindInventoryNode(Node node, string inventoryId)
		{
			if (node.ID == inventoryId) return node;

			if (node.RetailProducts != null)
			{
				foreach (var n in node.RetailProducts)
				{
					var found = FindInventoryNode(n, inventoryId);
					if (found != null) return found;
				}
			}

			return null;
		}

		T Parse<T>(string xmlFilename)
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

			var serialiser = new XmlSerializer(typeof(T), new XmlRootAttribute { ElementName = rootNodeName });
			using (var stream = new XmlNodeReader(doc))
			{
				var entity = (T)serialiser.Deserialize(stream);
				return entity;
			}
		}

		string GetShopName(string internalName)
		{
			if (ShopNames.Lookup.ContainsKey(internalName)) return ShopNames.Lookup[internalName];

			Console.WriteLine($"ShopLoader: Don't know the friendly name for shop '{internalName}'");
			return internalName;
		}
	}
}
