using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SEntityComponentMiningLaserParams
	{
		[XmlAttribute]
		public string globalParams;

		public MiningLaserModifiers miningLaserModifiers;

		[XmlAttribute]
		public string throttleLerpSpeed;
	}
}
