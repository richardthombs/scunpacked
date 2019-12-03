using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemWeaponComponentParams
	{
		[XmlAttribute]
		public string ammoContainerRecord;

		public connectionParams connectionParams;
	}
}