using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class EntityClassDefinition
	{
		public string ClassName;

		public Components Components;

		public StaticEntityClassData StaticEntityClassData;

		[XmlElement(ElementName = "tags")]
		public Reference[] Tags;

		[XmlAttribute(AttributeName = "__ref")]
		public string Id;
	}

	public class StaticEntityClassData
	{
		public DefaultEntitlementEntityParams DefaultEntitlementEntityParams;
	}

	public class DefaultEntitlementEntityParams
	{
		[XmlAttribute(AttributeName = "entitlementPolicy")]
		public string EntitlementPolicy;
	}

	public class Reference
	{
		[XmlAttribute(AttributeName = "value")]
		public string Value;
	}
}
