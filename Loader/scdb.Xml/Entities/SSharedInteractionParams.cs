using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SSharedInteractionParams
	{
		[XmlAttribute]
		public bool available;

		[XmlAttribute]
		public string DisplayName;

		[XmlAttribute]
		public string DisplayType;

		[XmlAttribute]
		public bool FocusModeOnly;

		[XmlAttribute]
		public string GenericCursor;

		[XmlAttribute]
		public bool Linkable;

		[XmlAttribute]
		public bool LockedByLinks;

		[XmlAttribute]
		public string Name;

		[XmlAttribute]
		public bool RequiresAuthorizedUser;

		[XmlAttribute]
		public string RoomTag;

		[XmlAttribute]
		public bool Sendable;

		[XmlAttribute]
		public string UsableTag;
	}
}
