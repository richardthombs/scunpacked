using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SEntityComponentMiningLaserParams
	{
		[XmlAttribute]
		public string globalParams;

		[XmlAttribute]
		public string throttleLerpSpeed;

		public MiningLaserModifiers miningLaserModifiers;
	}
}
