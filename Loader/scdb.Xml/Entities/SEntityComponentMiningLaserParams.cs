using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
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
