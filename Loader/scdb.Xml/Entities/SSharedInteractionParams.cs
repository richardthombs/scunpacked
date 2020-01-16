using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SSharedInteractionParams
	{
		[XmlAttribute]
		public string Name;

		[XmlAttribute]
		public string RoomTag;

		[XmlAttribute]
		public string UsableTag;

		[XmlAttribute]
		public string DisplayName;

		[XmlAttribute]
		public string DisplayType;

		[XmlAttribute]
		public string GenericCursor;

		[XmlAttribute]
		public bool FocusModeOnly;

		[XmlAttribute]
		public bool Sendable;

		[XmlAttribute]
		public bool Linkable;

		[XmlAttribute]
		public bool LockedByLinks;

		[XmlAttribute]
		public bool RequiresAuthorizedUser;

		[XmlAttribute]
		public bool available;
	}
}
