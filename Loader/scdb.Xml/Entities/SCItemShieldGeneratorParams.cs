using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SCItemShieldGeneratorParams
	{
		[XmlAttribute]
		public double DamagedRegenDelay;

		[XmlAttribute]
		public double DecayRatio;

		[XmlAttribute]
		public double DownedRegenDelay;

		[XmlAttribute]
		public double MaxReallocation;

		[XmlAttribute]
		public double MaxShieldHealth;

		[XmlAttribute]
		public double MaxShieldRegen;

		[XmlAttribute]
		public double ReallocationRate;

		public SShieldAbsorption[] ShieldAbsorption;

		public SShieldHardening ShieldHardening;

		public SShieldResistance[] ShieldResistance;
	}
}
