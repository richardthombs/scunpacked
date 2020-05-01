using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class EntityClassDefinition
	{
		public StaticEntityClassData StaticEntityClassData;
		public Reference[] tags;
		public Components Components;

		public string ClassName;
	}

	public class StaticEntityClassData
	{
		public DefaultEntitlementEntityParams DefaultEntitlementEntityParams;
	}

	public class DefaultEntitlementEntityParams
	{
		[XmlAttribute]
		public string entitlementPolicy;
	}

	public class Reference
	{
		[XmlAttribute]
		public string value;
	}
}
