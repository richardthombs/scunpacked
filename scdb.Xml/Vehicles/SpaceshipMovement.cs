using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class SpaceshipMovement
	{
		[XmlAttribute]
		public double engineWarmupDelay { get; set; }

		[XmlAttribute]
		public double engineIgnitionTime { get; set; }

		[XmlAttribute]
		public double enginePowerMax { get; set; }

		[XmlAttribute]
		public double rotationDamping { get; set; }

		[XmlAttribute]
		public double maxCruiseSpeed { get; set; }

		[XmlAttribute]
		public double maxEngineThrust { get; set; }

		[XmlAttribute]
		public double maxRetroThrust { get; set; }

		[XmlAttribute]
		public double maxDirectionalThrust { get; set; }

		[XmlAttribute]
		public string maxAngularVelocity { get; set; }

		[XmlAttribute]
		public string maxAngularAcceleration { get; set; }

		[XmlAttribute]
		public double maxJerk { get; set; }

		[XmlAttribute]
		public double maxAngJerk { get; set; }

		[XmlAttribute]
		public string maxTorqueAlpha { get; set; }

		[XmlAttribute]
		public string maxTorqueAngle { get; set; }
	}
}
