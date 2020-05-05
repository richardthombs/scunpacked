using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class STargetingParams
	{
		[XmlAttribute]
		public double lockTime;

		[XmlAttribute]
		public double trackingAngle;

		[XmlAttribute]
		public double trackingDistanceMax;

		[XmlAttribute]
		public double trackingNoiseAmplifier;

		[XmlAttribute]
		public double trackingSignalAmplifier;

		[XmlAttribute]
		public double trackingSignalAmplifierSeeking;

		[XmlAttribute]
		public double trackingSignalMin;

		[XmlAttribute]
		public string trackingSignalType;
	}
}
