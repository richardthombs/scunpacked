using System.Collections.Generic;

namespace Loader
{
	public class StandardisedWeapon
	{
		public StandardisedAmmunition Ammunition { get; set; }
		public List<StandardisedWeaponMode> Modes { get; set; }
	}
}
