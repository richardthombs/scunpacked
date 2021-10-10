using System;
using System.Collections.Generic;
using System.Linq;

namespace Loader
{
	public class PortClassifier
	{
		public (string, string) ClassifyPort(StandardisedItemPort port)
		{
			/*
				The order here is very important to try and catch obscure corner cases
			*/

			if (port.Types == null && port.InstalledItem == null) return ("DISABLED", "DISABLED");

			// Tractor beams
			if (FuzzyNameMatch(port, "tractor")) return ("Utility", "Utility hardpoints");

			// Utility hardpoints
			if (FuzzyNameMatch(port, "utility")) return ("Utility", "Utility hardpoints");

			if (port.Uneditable && port.InstalledItem != null)
			{
				var guess = GuessByInstalledItem(port);
				if (guess != null) return guess.Value;
			}

			// Mining
			if (port.Accepts("WeaponMining.Gun")) return ("Mining", "Mining hardpoints");
			if (port.Accepts("MiningArm")) return ("Mining", "Mining arm");

			if (port.Accepts("Turret.*"))
			{
				return ("Weapons", FuzzyNameMatch(port, "remote") ? "Remote turrets" : "Weapon hardpoints");
			}

			if (port.Accepts("TurretBase.MannedTurret"))
			{
				if (port.InstalledItem?.Ports?.Any(x => x.InstalledItem?.Type == "WeaponMining.Gun") ?? false) return ("Mining", "Mining turrets"); // Argo Mole
				if (FuzzyNameMatch(port, "tractor")) return ("Utility", "Utility turrets");
				else return ("Weapons", "Manned turrets");
			}

			// Weapons
			if (port.Accepts("MissileLauncher.MissileRack")) return ("Weapons", "Missile racks");
			if (port.Accepts("WeaponGun")) return ("Weapons", "Weapon hardpoints");
			if (port.Accepts("Missile.Missile")) return ("Weapons", "Missiles");
			if (port.Accepts("EMP")) return ("Weapons", "EMP hardpoints");
			if (port.Accepts("WeaponDefensive.CountermeasureLauncher")) return ("Weapons", "Countermeasures");
			if (port.Accepts("QuantumInterdictionGenerator")) return ("Weapons", "QIG hardpoints");

			// Systems
			if (port.Accepts("PowerPlant")) return ("Systems", "Power plants");
			if (port.Accepts("Cooler")) return ("Systems", "Coolers");
			if (port.Accepts("Shield")) return ("Systems", "Shield generators");
			if (port.Accepts("WeaponRegenPool")) return ("Systems", "Weapon regen pool");

			// Propulsion
			if (port.Accepts("FuelIntake")) return ("Propulsion", "Fuel intakes");
			if (port.Accepts("FuelTank")) return ("Propulsion", "Fuel tanks");
			if (port.Accepts("QuantumFuelTank.QuantumFuel")) return ("Propulsion", "Quantum fuel tanks");
			if (port.Accepts("QuantumDrive.QDrive")) return ("Propulsion", "Quantum drives");

			// Main Thrusters
			if (port.Accepts("MainThruster.*"))
			{
				if (FuzzyNameMatch(port, "retro")) return ("Thrusters", "Retro thrusters");
				else if (FuzzyNameMatch(port, "vtol")) return ("Thrusters", "VTOL thrusters");
				else return ("Thrusters", "Main thrusters");
			}

			// Maneuvering Thrusters
			if (port.Accepts("ManneuverThruster.*"))
			{
				if (FuzzyNameMatch(port, "retro")) return ("Thrusters", "Retro thrusters");
				else if (FuzzyNameMatch(port, "vtol")) return ("Thrusters", "VTOL thrusters");
				else return ("Thrusters", "Maneuvering thrusters");
			}

			// Avionics
			if (port.Accepts("Avionics.Motherboard")) return ("Avionics", "Computers");
			if (port.Accepts("Radar")) return ("Avionics", "Radars");
			if (port.Accepts("Radar.ShortRangeRadar")) return ("Avionics", "Radars");
			if (port.Accepts("Radar.MidRangeRadar")) return ("Avionics", "Radars");
			if (port.Accepts("Scanner")) return ("Avionics", "Scanners");
			if (port.Accepts("Scanner.Gun")) return ("Avionics", "Scanners");
			if (port.Accepts("Ping")) return ("Avionics", "Pings");
			if (port.Accepts("Transponder")) return ("Avionics", "Transponders");
			if (port.Accepts("SelfDestruct")) return ("Avionics", "Self destruct");

			// Cargo
			if (port.Accepts("Cargo")) return ("Cargo", "Cargo grids");
			if (port.Accepts("Container.Cargo")) return ("Cargo", "Cargo containers");

			// Armor
			if (port.Accepts("Armor")) return ("Armor", "Armor");

			// Misc
			if (port.Accepts("Usable")) return ("Misc", "Usables");
			if (port.Accepts("Room")) return ("Misc", "Rooms");
			if (port.Accepts("Door")) return ("Misc", "Doors");
			if (port.Accepts("Paints")) return ("Misc", "Paints");

			// Attachments to larger objects
			if (FuzzyNameMatch(port, "BatteryPort")) return ("Attachments", "Batteries");
			if (port.Accepts("WeaponAttachment.Barrel")) return ("Attachments", "Weapon attachments");
			if (port.Accepts("WeaponAttachment.FiringMechanism")) return ("Attachments", "Weapon attachments");
			if (port.Accepts("WeaponAttachment.PowerArray")) return ("Attachments", "Weapon attachments");
			if (port.Accepts("WeaponAttachment.Ventilation")) return ("Attachments", "Weapon attachments");
			if (port.Accepts("ControlPanel.DoorPart")) return ("Attachments", "Door attachments");
			if (port.Accepts("Misc.DoorPart")) return ("Attachments", "Door attachments");
			if (port.Accepts("Button.DoorPart")) return ("Attachments", "Door attachments");
			if (port.Accepts("Sensor.DoorPart")) return ("Attachments", "Door attachments");
			if (port.Accepts("Lightgroup.DoorPart")) return ("Attachments", "Door attachments");
			if (port.Accepts("Decal.DoorPart")) return ("Attachments", "Door attachments");

			// Seating
			if (port.Accepts("Seat")) return ("Seating", "Seats");
			if (port.Accepts("SeatAccess")) return ("Seating", "Seat access");

			return ("UNKNOWN", "UNKNOWN");
		}

		(string, string)? GuessByInstalledItem(StandardisedItemPort port)
		{
			switch (port.InstalledItem.Type)
			{
				case "WeaponGun.Gun":
					return ("Weapons", "Weapon hardpoints");

				case "TurretBase.MannedTurret":
					if (port.InstalledItem.Ports.Any(x => x.InstalledItem?.Type == "WeaponMining.Gun")) return ("Mining", "Mining turrets"); // Argo Mole
					return ("Weapons", "Manned turrets");

				case "Container.Cargo": return ("Cargo", "Cargo containers");
			}

			return null;
		}

		public static bool FuzzyNameMatch(StandardisedItemPort port, string lookFor)
		{
			if (port.PortName.Contains(lookFor, StringComparison.OrdinalIgnoreCase)) return true;
			if (port.InstalledItem?.ClassName.Contains(lookFor, StringComparison.OrdinalIgnoreCase) ?? false) return true;
			if (port.Loadout?.Contains(lookFor, StringComparison.OrdinalIgnoreCase) ?? false) return true;
			return false;
		}

	}

	public class ItemTypeMatchHelpers
	{
		public static bool TypeMatch(string type, string typePattern)
		{
			var patternSplit = typePattern.Split('.', 2);

			var patternType = patternSplit[0];
			if (patternType == "*") patternType = null;

			var patternSubType = patternSplit.Length > 1 ? patternSplit[1] : null;
			if (patternSubType == "*") patternSubType = null;

			var typeSplit = type.Split('.', 2);
			var typeType = typeSplit[0];
			var typeSubType = typeSplit.Length > 1 ? typeSplit[1] : null;

			if (!String.IsNullOrEmpty(patternType) && !String.Equals(patternType, typeType, StringComparison.OrdinalIgnoreCase)) return false;
			if (!String.IsNullOrEmpty(patternSubType) && !String.Equals(patternSubType, typeSubType, StringComparison.OrdinalIgnoreCase)) return false;

			return true;
		}

		public static bool TypeMatch(List<string> types, string typePattern)
		{
			if (types == null) return false;

			foreach (var type in types)
			{
				if (TypeMatch(type, typePattern)) return true;
			}
			return false;
		}

	}
}
