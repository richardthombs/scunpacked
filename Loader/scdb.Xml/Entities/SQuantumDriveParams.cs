using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SQuantumDriveParams
	{
		[XmlAttribute]
		public double calibrationDelayInSeconds;

		[XmlAttribute]
		public double calibrationProcessAngleLimit;

		[XmlAttribute]
		public double calibrationRate;

		[XmlAttribute]
		public double calibrationWarningAngleLimit;

		[XmlAttribute]
		public double cooldownTime;

		[XmlAttribute]
		public double driveSpeed;

		[XmlAttribute]
		public double engageSpeed;

		[XmlAttribute]
		public double interdictionEffectTime;

		[XmlAttribute]
		public double maxCalibrationRequirement;

		[XmlAttribute]
		public double minCalibrationRequirement;

		[XmlAttribute]
		public double spoolUpTime;

		[XmlAttribute]
		public double stageOneAccelRate;

		[XmlAttribute]
		public double stageTwoAccelRate;

		[XmlAttribute]
		public string turnOffSpoolInteraction;

		[XmlAttribute]
		public string turnOnSpoolInteraction;
	}
}
