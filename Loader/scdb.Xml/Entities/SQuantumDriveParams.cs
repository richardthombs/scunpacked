using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SQuantumDriveParams
	{
		[XmlAttribute]
		public double driveSpeed;

		[XmlAttribute]
		public double cooldownTime;

		[XmlAttribute]
		public double stageOneAccelRate;

		[XmlAttribute]
		public double stageTwoAccelRate;

		[XmlAttribute]
		public double engageSpeed;

		[XmlAttribute]
		public double interdictionEffectTime;

		[XmlAttribute]
		public double calibrationRate;

		[XmlAttribute]
		public double minCalibrationRequirement;

		[XmlAttribute]
		public double maxCalibrationRequirement;

		[XmlAttribute]
		public double calibrationProcessAngleLimit;

		[XmlAttribute]
		public double calibrationWarningAngleLimit;

		[XmlAttribute]
		public double calibrationDelayInSeconds;

		[XmlAttribute]
		public double spoolUpTime;

		[XmlAttribute]
		public string turnOnSpoolInteraction;

		[XmlAttribute]
		public string turnOffSpoolInteraction;
	}
}