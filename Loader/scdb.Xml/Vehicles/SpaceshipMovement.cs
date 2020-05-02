using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Vehicles
{
	public class SpaceshipMovement
	{
		[XmlAttribute]
		public double engineIgnitionTime;

		[XmlAttribute]
		public double enginePowerMax;

		[XmlAttribute]
		public double engineWarmupDelay;

		[XmlAttribute]
		public double maxAngJerk;

		[XmlAttribute]
		public string maxAngularAcceleration;

		[XmlAttribute]
		public string maxAngularVelocity;

		[XmlAttribute]
		public double maxCruiseSpeed;

		[XmlAttribute]
		public double maxDirectionalThrust;

		[XmlAttribute]
		public double maxEngineThrust;

		[XmlAttribute]
		public double maxJerk;

		[XmlAttribute]
		public double maxRetroThrust;

		[XmlAttribute]
		public string maxTorqueAlpha;

		[XmlAttribute]
		public string maxTorqueAngle;

		[XmlAttribute]
		public double rotationDamping;
	}
}
