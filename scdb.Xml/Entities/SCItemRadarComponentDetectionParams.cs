using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemRadarComponentDetectionParams
	{
		[XmlAttribute]
		public bool detectable;

		[XmlAttribute]
		public double liveDetectionRange;

		[XmlAttribute]
		public double partialDetectionRange;

		[XmlAttribute]
		public double timeoutRangeMultiplier;

		[XmlAttribute]
		public double signatureBoostMultiplier;
	}
}