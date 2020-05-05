using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Vehicles
{
	public class Camera
	{
		[XmlAttribute]
		public string attachPoint;

		[XmlAttribute]
		public string cameraOffset;

		[XmlAttribute]
		public string cameraRotationOffset;

		[XmlAttribute]
		public string cameraSecondaryOffset;

		[XmlAttribute]
		public string filename;

		[XmlAttribute]
		public string type;
	}
}
