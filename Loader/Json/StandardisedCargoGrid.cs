namespace Loader
{
	public class StandardisedCargoGrid
	{
		public double Capacity { get; set; }
		public double Width { get; set; }
		public double Height { get; set; }
		public double Depth { get; set; }
		public bool MiningOnly { get; set; }

		public bool ShouldSerializeMiningOnly() => MiningOnly;
	}
}
