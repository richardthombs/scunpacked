using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemRadarComponentParams
	{
		[XmlAttribute]
		public double detectionLifetime;

		[XmlAttribute]
		public double altitudeCeiling;

		[XmlAttribute]
		public bool enableCrossSectionOcclusion;

		public SCItemRadarSignatureDetectionParams[] signatureDetection;
	}
}
