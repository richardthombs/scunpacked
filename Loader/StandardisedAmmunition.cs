namespace Loader
{
	public class StandardisedAmmunition
	{
		public double Speed { get; set; }
		public double Range { get; set; }
		public double Size { get; set; }
		public double? Capacity { get; set; }
		public StandardisedDamage ImpactDamage { get; set; }
		public StandardisedDamage DetonationDamage { get; set; }
	}
}
