namespace Loader
{
	public class StandardisedQuantumDrive
	{
		public double FuelRate { get; set; }
		public double JumpRange { get; set; }
		public StandardisedJumpPerformance StandardJump { get; set; }
		public StandardisedJumpPerformance SplineJump { get; set; }

	}

	public class StandardisedQuantumTravelPerformance
	{
		public double MaximumRange { get; set; }
	}
}
