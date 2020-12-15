using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class DefaultEntitlementEntityParams
	{
		[XmlAttribute]
		public string entitlementPolicy;
	}
}
