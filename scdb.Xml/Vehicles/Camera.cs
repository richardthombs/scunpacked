using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class Camera
	{
		[XmlAttribute]
		public string type { get; set; }

		[XmlAttribute]
		public string attachPoint { get; set; }

		[XmlAttribute]
		public string cameraOffset { get; set; }

		[XmlAttribute]
		public string cameraRotationOffset { get; set; }

		[XmlAttribute]
		public string cameraSecondaryOffset { get; set; }

		[XmlAttribute]
		public string filename { get; set; }
	}
}
