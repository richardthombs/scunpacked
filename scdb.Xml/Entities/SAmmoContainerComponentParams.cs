using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SAmmoContainerComponentParams
	{
		[XmlAttribute]
		public double initialAmmoCount;

		[XmlAttribute]
		public double maxAmmoCount;

		[XmlAttribute]
		public string ammoParamsRecord;
	}
}