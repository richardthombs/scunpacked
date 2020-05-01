using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SCItemTurretParams
	{
		[XmlAttribute]
		public string rotationStyle;

		[XmlAttribute]
		public double inputLevelUnderOneRate;

		[XmlAttribute]
		public double inputLevelOverOneRate;

		[XmlAttribute]
		public double joystickSensitivityBoost;

		[XmlAttribute]
		public double powerRequirement;
	}
}