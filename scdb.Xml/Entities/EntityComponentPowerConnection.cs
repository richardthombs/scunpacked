using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class EntityComponentPowerConnection
	{
		[XmlAttribute]
		public double PowerBase;

		[XmlAttribute]
		public double PowerDraw;

		[XmlAttribute]
		public double TimeToReachDrawRequest;

		[XmlAttribute]
		public double SafeguardPriority;

		[XmlAttribute]
		public bool DisplayedInPoweredItemList;

		[XmlAttribute]
		public bool IsThrottleable;

		[XmlAttribute]
		public bool IsOverclockable;

		[XmlAttribute]
		public double OverclockThresholdMin;

		[XmlAttribute]
		public double OverclockThresholdMax;

		[XmlAttribute]
		public double OverpowerPerformance;

		[XmlAttribute]
		public double OverclockPerformance;

		[XmlAttribute]
		public double PowerToEM;

		[XmlAttribute]
		public double DecayRateOfEM;

		[XmlAttribute]
		public double WarningDelayTime;

		[XmlAttribute]
		public double WarningDisplayTime;
	}
}