using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemCargoGridParams
	{
		[XmlAttribute]
		public double crateGenPercentageOnDestroy;

		[XmlAttribute]
		public double crateMaxOnDestroy;

		[XmlAttribute]
		public bool personalInventory;

		[XmlAttribute]
		public bool invisible;

		[XmlAttribute]
		public bool miningOnly;

		[XmlAttribute]
		public double minVolatilePowerToExplode;

		public Vec3 dimensions;
	}
}
