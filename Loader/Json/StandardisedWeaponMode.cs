namespace Loader
{
	public class StandardisedWeaponMode
	{
		public string Name { get; set; }
		public string LocalisedName { get; set; }
		public double RoundsPerMinute { get; set; }
		public string FireType { get; set; }
		public double AmmoPerShot { get; set; }
		public double PelletsPerShot { get; set; }
		public StandardisedDamage DamagePerShot { get; set; }
		public StandardisedDamage DamagePerSecond { get; set; }
	}
}
