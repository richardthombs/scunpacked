using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using scdb.Xml.Shops;

namespace Loader
{
	public class ShopLoader
	{
		public string DataRoot { get; set; }

		public void Load()
		{
			var prices = Parse<Node>(Path.Combine(DataRoot, @"Data\Libs\Subsumption\Shops\RetailProductPrices.xml"));
			var shops = Parse<ShopLayoutNode>(Path.Combine(DataRoot, @"Data\Libs\Subsumption\Shops\ShopLayouts.xml"));
			var x = shops.ShopLayoutNodes[0].ShopLayoutNodes[0].ShopInventoryNodes[0];

			PrintShops(prices, shops);

			Console.ReadLine();
		}

		void PrintShops(Node prices, ShopLayoutNode node, int depth = 0)
		{
			Console.WriteLine($"{new string(' ', depth)}{node.Name}");
			if (node.ShopInventoryNodes != null) foreach (var inventory in node.ShopInventoryNodes) PrintInventory(prices, inventory, depth + 1);
			foreach (var layout in node.ShopLayoutNodes) PrintShops(prices, layout, depth + 1);

		}

		void PrintInventory(Node prices, ShopInventoryNode node, int depth)
		{
			var invNode = FindInventoryNode(prices, node.InventoryID);
			if (invNode == null) Console.WriteLine("Can't find " + node.InventoryID);
			Console.WriteLine($"{new string('.', depth)}{node.Name} {invNode.BasePrice} {invNode.MaxDiscountPercentage}% - {invNode.MaxPremiumPercentage}%");
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
	}
}
