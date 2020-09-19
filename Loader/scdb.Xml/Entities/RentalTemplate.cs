using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class RentalTemplate
	{
		public ShopRentalTemplate ShopRentalTemplate;
	}

	public class ShopRentalTemplate
	{
		[XmlAttribute]
		public string ID;

		[XmlAttribute]
		public double PercentageOfSalePrice;

		[XmlAttribute]
		public int RentalDuration;
	}
}
