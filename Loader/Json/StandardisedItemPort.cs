using System.Collections.Generic;

namespace Loader
{
	public class StandardisedItemPort
	{
		public string PortName { get; set; }
		public int Size { get; set; }
		public string Loadout { get; set; }
		public StandardisedItem InstalledItem { get; set; }
		public List<string> Types { get; set; }
		public List<string> Flags { get; set; }
		public string Category { get; set; }
		public bool Uneditable { get; set; }

		public override string ToString()
		{
			return $"{PortName}: {InstalledItem?.ClassName ?? "Nothing"}";
		}

		public bool Accepts(string typePattern)
		{
			return ItemTypeMatchHelpers.TypeMatch(Types, typePattern);
		}

		public bool ShouldSerializeTypes() => Types?.Count > 0;
		public bool ShouldSerializeFlags() => Flags?.Count > 0;
		public bool ShouldSerializeUneditable() => Uneditable;
	}
}
