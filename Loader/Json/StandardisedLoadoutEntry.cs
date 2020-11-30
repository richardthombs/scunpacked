using System.Collections.Generic;

namespace Loader
{
	public class StandardisedLoadoutEntry
	{
		public string PortName { get; set; }
		public string ClassName { get; set; }
		public List<StandardisedLoadoutEntry> Entries { get; set; }

		public override string ToString()
		{
			return $"{PortName} -> {ClassName}";
		}
	}
}
