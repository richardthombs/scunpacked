using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SCItemQuantumJammerParams
	{
		[XmlAttribute]
		public double greenZoneCheckRange;

		[XmlAttribute]
		public double jammerRange;

		[XmlAttribute]
		public double maxPowerDraw;

		[XmlAttribute]
		public string setJammerSwitchOff;

		[XmlAttribute]
		public string setJammerSwitchOn;
	}
}
