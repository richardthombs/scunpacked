using System.Collections.Generic;

namespace Loader
{
	public class StandardisedWeaponFitting
	{
		public int Size { get; set; }
		public bool Fixed { get; set; }
		public bool Gimballed { get; set; }
		public bool Turret { get; set; }
		public List<int> WeaponSizes { get; set; }

		public bool ShouldSerializeGimballed() => Gimballed;
		public bool ShouldSerializeFixed() => Fixed;
		public bool ShouldSerializeTurret() => Turret;
	}
}
