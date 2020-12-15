using System.Collections.Generic;

namespace Loader
{
	public class StandardisedPortSummary
	{
		public StandardisedPortSummary()
		{
			PilotHardpoints = new List<StandardisedItemPort>();
			MannedTurrets = new List<StandardisedItemPort>();
			RemoteTurrets = new List<StandardisedItemPort>();
			MiningTurrets = new List<StandardisedItemPort>();
			UtilityTurrets = new List<StandardisedItemPort>();
			MiningHardpoints = new List<StandardisedItemPort>();
			UtilityHardpoints = new List<StandardisedItemPort>();
			MissileRacks = new List<StandardisedItemPort>();
			Countermeasures = new List<StandardisedItemPort>();
			Shields = new List<StandardisedItemPort>();
			PowerPlants = new List<StandardisedItemPort>();
			Coolers = new List<StandardisedItemPort>();
			QuantumDrives = new List<StandardisedItemPort>();
			QuantumFuelTanks = new List<StandardisedItemPort>();
			MainThrusters = new List<StandardisedItemPort>();
			RetroThrusters = new List<StandardisedItemPort>();
			VtolThrusters = new List<StandardisedItemPort>();
			ManeuveringThrusters = new List<StandardisedItemPort>();
			HydrogenFuelTanks = new List<StandardisedItemPort>();
			HydogenFuelIntakes = new List<StandardisedItemPort>();
			InterdictionHardpoints = new List<StandardisedItemPort>();
			CargoGrids = new List<StandardisedItemPort>();
			Avionics = new List<StandardisedItemPort>();
		}

		public List<StandardisedItemPort> PilotHardpoints { get; set; }
		public List<StandardisedItemPort> MannedTurrets { get; set; }
		public List<StandardisedItemPort> RemoteTurrets { get; set; }
		public List<StandardisedItemPort> MiningTurrets { get; set; }
		public List<StandardisedItemPort> UtilityTurrets { get; set; }
		public List<StandardisedItemPort> MiningHardpoints { get; set; }
		public List<StandardisedItemPort> UtilityHardpoints { get; set; }
		public List<StandardisedItemPort> MissileRacks { get; set; }
		public List<StandardisedItemPort> Countermeasures { get; set; }
		public List<StandardisedItemPort> Shields { get; set; }
		public List<StandardisedItemPort> PowerPlants { get; set; }
		public List<StandardisedItemPort> Coolers { get; set; }
		public List<StandardisedItemPort> QuantumDrives { get; set; }
		public List<StandardisedItemPort> QuantumFuelTanks { get; set; }
		public List<StandardisedItemPort> MainThrusters { get; set; }
		public List<StandardisedItemPort> RetroThrusters { get; set; }
		public List<StandardisedItemPort> VtolThrusters { get; set; }
		public List<StandardisedItemPort> ManeuveringThrusters { get; set; }
		public List<StandardisedItemPort> HydrogenFuelTanks { get; set; }
		public List<StandardisedItemPort> HydogenFuelIntakes { get; set; }
		public List<StandardisedItemPort> InterdictionHardpoints { get; set; }
		public List<StandardisedItemPort> CargoGrids { get; set; }
		public List<StandardisedItemPort> Avionics { get; set; }
	}
}
