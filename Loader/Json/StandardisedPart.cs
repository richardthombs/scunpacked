using System.Collections.Generic;

namespace Loader
{
	public class StandardisedPart
	{
		public string Name { get; set; }
		public double? MaximumDamage { get; set; }
		public double? Mass { get; set; }
		public StandardisedItemPort Port { get; set; }
		public List<StandardisedPart> Parts { get; set; }
		public double? ShipDestructionDamage { get; set; }
		public double? PartDetachDamage { get; set; }

		public override string ToString()
		{
			return Name;
		}

		public bool ShouldSerializeParts() => Parts?.Count > 0;
		public bool ShouldSerializeDamageToDetach() => !ShipDestructionDamage.HasValue;
	}
}
