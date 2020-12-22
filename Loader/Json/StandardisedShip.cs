using System.Collections.Generic;

namespace Loader
{
	public class StandardisedShip
	{
		public string ClassName { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Career { get; set; }
		public string Role { get; set; }
		public int Size { get; set; }
		public int Cargo { get; set; }
		public int Crew { get; set; }
		public int WeaponCrew { get; set; }
		public int OperationsCrew { get; set; }
		public double Mass { get; set; }
		public bool IsSpaceship { get; set; }
		public bool IsGravlev { get; set; }
		public bool IsVehicle { get; set; }
		public StandardisedManufacturer Manufacturer { get; set; }
		public Dictionary<string, double> DamageBeforeDestruction { get; set; }
		public Dictionary<string, double> DamageBeforeDetach { get; set; }
		public StandardisedFlightCharacteristics FlightCharacteristics { get; set; }
		public StandardisedPropulsionSummary Propulsion { get; set; }
		public StandardisedQuantumTravelSummary QuantumTravel { get; set; }
		public List<StandardisedWeaponFitting> PilotHardpoints { get; set; }
		public List<StandardisedWeaponFitting> MannedTurrets { get; set; }
		public List<StandardisedWeaponFitting> RemoteTurrets { get; set; }
		public StandardisedArmour Armour { get; set; }
		public StandardisedInsurance Insurance { get; set; }

		public bool ShouldSerializeIsVehicle() => IsVehicle;
		public bool ShouldSerializeIsGravlev() => IsGravlev;
	}
}
