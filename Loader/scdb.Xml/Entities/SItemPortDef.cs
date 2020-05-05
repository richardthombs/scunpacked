using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SItemPortDef
	{
		[XmlAttribute]
		public string controllableTags;

		[XmlAttribute]
		public string DefaultWeaponGroup;

		[XmlAttribute]
		public string DisplayName;

		[XmlAttribute]
		public string Flags;

		[XmlAttribute]
		public double InteractionPointSize;

		[XmlAttribute]
		public int MaxSize;

		[XmlAttribute]
		public int MinSize;

		[XmlAttribute]
		public string Name;

		[XmlAttribute]
		public string PortTags;

		[XmlAttribute]
		public string RequiredPortTags;

		public SItemPortDefTypes[] Types;
	}
}
