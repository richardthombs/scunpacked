using scdb.Xml.Vehicles;
using scdb.Xml.DefaultLoadouts;
using scdb.Xml.Entities;

namespace Loader
{
	public class ShipIndexEntry
	{
		public string JsonFilename;
		public string ClassName;
		public string ItemName;
		public string Type;
		public string SubType;
		public string EntityFilename;
		public string VehicleFilename;
		public string DefaultLoadoutFilename;
		public EntityClassDefinition Entity;
		public Vehicle Vehicle;
		public Loadout DefaultLoadout;
	}
}
