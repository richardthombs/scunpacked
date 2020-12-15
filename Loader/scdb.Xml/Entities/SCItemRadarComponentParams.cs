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
		public double enableCrossSectionOcclusion;

		public SCItemRadarComponentSignatureParams[] signatureParams;
	}
}
