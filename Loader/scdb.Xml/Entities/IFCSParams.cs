using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class IFCSParams
	{
		[XmlAttribute]
		public double maxSpeed;

		[XmlAttribute]
		public double maxAfterburnSpeed;

		[XmlAttribute]
		public double torqueDistanceThreshold;

		[XmlAttribute]
		public bool refreshCachesOnLandingMode;

		[XmlAttribute]
		public double coefficientOfLift;

		[XmlAttribute]
		public double centerOfLiftOffset;

		[XmlAttribute]
		public double centerOfPressureOffset;

		[XmlAttribute]
		public double linearTurbulenceAmplitude;

		[XmlAttribute]
		public double angularTurbulenceAmplitude;

		[XmlAttribute]
		public double groundTurbulenceAmplitude;

		[XmlAttribute]
		public double precisionMinDistance;

		[XmlAttribute]
		public double precisionMaxDistance;

		[XmlAttribute]
		public double precisionLandingMultiplier;
	}
}