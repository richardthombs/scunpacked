namespace Loader
{
	public class StandardisedPropulsionSummary
	{
		public double FuelCapacity { get; set; }
		public double FuelIntakeRate { get; set; }
		public StandardisedThrusterSummary FuelUsage { get; set; }
		public StandardisedThrusterSummary ThrustCapacity { get; set; }
		public double IntakeToMainFuelRatio { get; set; }
		public double IntakeToTankCapacityRatio { get; set; }
		public double TimeForIntakesToFilllTank { get; set; }
		public double ManeuveringTimeTillEmpty { get; set; }
	}
}
