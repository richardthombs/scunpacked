using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class Camera
	{
		[XmlAttribute]
		public string type;

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
	}
}
