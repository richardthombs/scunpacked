using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class EntityClassDefinition
	{
		[XmlAttribute]
		public string __type { get; set; }

		[
			XmlArrayItem(typeof(VehicleComponentParams)),
			XmlArrayItem(typeof(SEntityComponentDefaultLoadoutParams)),
			XmlArrayItem(typeof(SAttachableComponentParams)),
			XmlArrayItem(typeof(SHealthComponentParams))
		]
		public Component[] Components { get; set; }
	}
}