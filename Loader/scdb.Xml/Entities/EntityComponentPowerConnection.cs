using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class EntityComponentPowerConnection
	{
		[XmlAttribute]
		public double DecayRateOfEM;

		[XmlAttribute]
		public bool DisplayedInPoweredItemList;

		[XmlAttribute]
		public bool IsOverclockable;

		[XmlAttribute]
		public bool IsThrottleable;

		[XmlAttribute]
		public double OverclockPerformance;

		[XmlAttribute]
		public double OverclockThresholdMax;

		[XmlAttribute]
		public double OverclockThresholdMin;

		[XmlAttribute]
		public double OverpowerPerformance;

		[XmlAttribute]
		public double PowerBase;

		[XmlAttribute]
		public double PowerDraw;

		[XmlAttribute]
		public double PowerToEM;

		[XmlAttribute]
		public double SafeguardPriority;

		[XmlAttribute]
		public double TimeToReachDrawRequest;

		[XmlAttribute]
		public double WarningDelayTime;

		[XmlAttribute]
		public double WarningDisplayTime;
	}
}
