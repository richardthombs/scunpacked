using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class IFCSParams
	{
		[XmlAttribute]
		public double angularTurbulenceAmplitude;

		[XmlAttribute]
		public double centerOfLiftOffset;

		[XmlAttribute]
		public double centerOfPressureOffset;

		[XmlAttribute]
		public double coefficientOfLift;

		[XmlAttribute]
		public double groundTurbulenceAmplitude;

		[XmlAttribute]
		public double linearTurbulenceAmplitude;

		[XmlAttribute]
		public double maxAfterburnSpeed;

		[XmlAttribute]
		public double maxSpeed;

		[XmlAttribute]
		public double precisionLandingMultiplier;

		[XmlAttribute]
		public double precisionMaxDistance;

		[XmlAttribute]
		public double precisionMinDistance;

		[XmlAttribute]
		public bool refreshCachesOnLandingMode;

		[XmlAttribute]
		public double torqueDistanceThreshold;
	}
}
