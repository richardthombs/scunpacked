using scdb.Xml.Entities;
using scdb.Xml.DefaultLoadouts;

namespace Loader
{
	public class ItemIndexEntry
	{
		public string json;
		public string ClassName;
		public string ItemName;
		public string Type;
		public string SubType;
		public string EntityFilename;
		public string DefaultLoadoutFilename;
		public EntityClassDefinition Entity;
		public Loadout DefaultLoadout;
	}
}
