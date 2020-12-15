using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemRadarSignatureDetectionParams
	{
		[XmlAttribute]
		public bool detectable;

		[XmlAttribute]
		public double sensitivity;

		[XmlAttribute]
		public bool ambientPiercing;
	}
}
