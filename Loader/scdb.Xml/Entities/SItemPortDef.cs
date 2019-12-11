using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SItemPortDef
	{
		[XmlAttribute]
		public string Name;

		[XmlAttribute]
		public string DisplayName;

		[XmlAttribute]
		public string PortTags;

		[XmlAttribute]
		public string RequiredPortTags;

		[XmlAttribute]
		public string Flags;

		[XmlAttribute]
		public int MinSize;

		[XmlAttribute]
		public int MaxSize;

		[XmlAttribute]
		public double InteractionPointSize;

		[XmlAttribute]
		public string DefaultWeaponGroup;

		[XmlAttribute]
		public string controllableTags;

		public SItemPortDefTypes[] Types;
	}
}
