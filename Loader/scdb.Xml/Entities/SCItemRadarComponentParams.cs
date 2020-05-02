using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SCItemRadarComponentParams
	{
		[XmlAttribute]
		public double altitudeCeiling;

		[XmlAttribute]
		public double detectionLifetime;

		[XmlAttribute]
		public double enableCrossSectionOcclusion;

		public SCItemRadarComponentSignatureParams[] signatureParams;
	}
}
