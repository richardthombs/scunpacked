
using scdb.Xml.Entities;
using scdb.Xml.Loadouts;
using scdb.Xml.Vehicles;

namespace Loader
{
	public class Ship
	{
		public EntityClassDefinition Entity { get; set; }
		public Vehicle Vehicle { get; set; }
		public Loadout DefaultLoadout { get; set; }
		public scdb.Models.loadout[] loadout { get; set; }
	}
}
