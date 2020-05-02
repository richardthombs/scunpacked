using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SCItemTurretParams
	{
		[XmlAttribute]
		public double inputLevelOverOneRate;

		[XmlAttribute]
		public double inputLevelUnderOneRate;

		[XmlAttribute]
		public double joystickSensitivityBoost;

		[XmlAttribute]
		public double powerRequirement;

		[XmlAttribute]
		public string rotationStyle;
	}
}
