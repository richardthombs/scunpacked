using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class SpaceshipMovement
	{
		[XmlAttribute]
		public double engineWarmupDelay;

		[XmlAttribute]
		public double engineIgnitionTime;

		[XmlAttribute]
		public double enginePowerMax;

		[XmlAttribute]
		public double rotationDamping;

		[XmlAttribute]
		public double maxCruiseSpeed;

		[XmlAttribute]
		public double maxEngineThrust;

		[XmlAttribute]
		public double maxRetroThrust;

		[XmlAttribute]
		public double maxDirectionalThrust;

		[XmlAttribute]
		public string maxAngularVelocity;

		[XmlAttribute]
		public string maxAngularAcceleration;

		[XmlAttribute]
		public double maxJerk;

		[XmlAttribute]
		public double maxAngJerk;

		[XmlAttribute]
		public string maxTorqueAlpha;

		[XmlAttribute]
		public string maxTorqueAngle;
	}
}
