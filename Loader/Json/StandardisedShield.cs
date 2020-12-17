using System;

namespace Loader
{
	public class StandardisedShield
	{
		public double Health { get; set; }
		public double Regeneration { get; set; }
		public double DownedDelay { get; set; }
		public double DamagedDelay { get; set; }

		public StandardisedShieldAbsorption Absorption { get; set; }
	}

	public class StandardisedShieldAbsorption
	{
		public StandardisedShieldAbsorptionRange Physical { get; set; }
		public StandardisedShieldAbsorptionRange Energy { get; set; }
		public StandardisedShieldAbsorptionRange Distortion { get; set; }
		public StandardisedShieldAbsorptionRange Thermal { get; set; }
		public StandardisedShieldAbsorptionRange Biochemical { get; set; }
		public StandardisedShieldAbsorptionRange Stun { get; set; }
	}

	public class StandardisedShieldAbsorptionRange
	{
		public double Minimum { get; set; }
		public double Maximum { get; set; }
	}
}
