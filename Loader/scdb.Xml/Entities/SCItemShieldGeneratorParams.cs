using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemShieldGeneratorParams
	{
		[XmlAttribute]
		public double MaxShieldHealth;

		[XmlAttribute]
		public double MaxShieldRegen;

		[XmlAttribute]
		public double DecayRatio;

		[XmlAttribute]
		public double DownedRegenDelay;

		[XmlAttribute]
		public double DamagedRegenDelay;
		[XmlAttribute]
		public double MaxReallocation;

		[XmlAttribute]
		public double ReallocationRate;

		public SShieldHardening ShieldHardening;
		public SShieldResistance[] ShieldResistance;
		public SShieldAbsorption[] ShieldAbsorption;
	}
}