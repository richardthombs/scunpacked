namespace Loader
{
	public class StandardisedFlightCharacteristics
	{
		public double ScmSpeed { get; set; }
		public double MaxSpeed { get; set; }
		public double ZeroToScm { get; set; }
		public double ZeroToMax { get; set; }
		public double ScmToZero { get; set; }
		public double MaxToZero { get; set; }
		public StandardisedThrusterSummary Acceleration { get; set; }
		public StandardisedThrusterSummary AccelerationG { get; set; }
	}
}
