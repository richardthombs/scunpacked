using System.Collections.Generic;

namespace Loader
{
	public class StandardisedWeapon
	{
		public StandardisedAmmunition Ammunition { get; set; }
		public List<StandardisedWeaponMode> Modes { get; set; }
		public StandardisedWeaponConsumption Consumption { get; set; }
	}

	public class StandardisedWeaponConsumption
	{
		public double RequestedRegenPerSec { get; set; }
		public double Cooldown { get; set; }
		public double CostPerBullet { get; set; }
		public double RequestedAmmoLoad { get; set; }
	}
}
