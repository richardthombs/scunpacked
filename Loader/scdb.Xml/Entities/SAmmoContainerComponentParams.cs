using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SAmmoContainerComponentParams
	{
		[XmlAttribute]
		public string ammoParamsRecord;

		[XmlAttribute]
		public double initialAmmoCount;

		[XmlAttribute]
		public double maxAmmoCount;
	}
}
