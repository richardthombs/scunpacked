using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

using Newtonsoft.Json;

using scdb.Xml.Entities;
using scdb.Xml.Vehicles;

namespace Loader
{
	public class ShipLoader
	{
		public string OutputFolder { get; set; }
		public string DataRoot { get; set; }

		string[] avoids =
		{
			// CIG tags
			"pu",
			"ai",
			"civ",
			"qig",
			"crim",
			"pir",
			"template",
			"wreck",
			"piano",
			"swarm",
			"nointerior",
			"s42",
			"hijacked",
			"comms",
			"outlaw",
			"outlaws",
			"uee",
			"tow",
			"shields",
			"mlhm1",
			"microtech",
			"shubin",
			"drug",
			"advocacy",
			"derelict",
			"drone",
			"eaobjectivedestructable",

			// Skin variants
			"pink",
			"yellow",
			"emerald",
			"dunestalker",
			"snowblind",
			"shipshowdown",
			"showdown",
			"citizencon2018",
			"citizencon",
			"pirate",
			"talus",
			"carbon",
			"bis2950",
			"fleetweek"
		};

		SortedSet<string> AcceptTypes = new SortedSet<string>();
		SortedSet<string> InstalledTypes = new SortedSet<string>();
		ItemBuilder itemBuilder;
		ManufacturerService manufacturerSvc;
		LocalisationService localisationSvc;
		EntityService entitySvc;
		ItemInstaller itemInstaller;
		LoadoutLoader loadoutLoader;
		InsuranceService insuranceSvc;

		public ShipLoader(ItemBuilder itemBuilder, ManufacturerService manufacturerSvc, LocalisationService localisationSvc, EntityService entitySvc, ItemInstaller itemInstaller, LoadoutLoader loadoutLoader, InsuranceService insuranceSvc)
		{
			this.itemBuilder = itemBuilder;
			this.manufacturerSvc = manufacturerSvc;
			this.localisationSvc = localisationSvc;
			this.entitySvc = entitySvc;
			this.itemInstaller = itemInstaller;
			this.loadoutLoader = loadoutLoader;
			this.insuranceSvc = insuranceSvc;
		}

		public List<(ShipIndexEntry, StandardisedShip)> Load(string shipFilter)
		{
			Directory.CreateDirectory(Path.Combine(OutputFolder, "ships"));
			Directory.CreateDirectory(Path.Combine(OutputFolder, "v2", "ships"));

			var index = new List<(ShipIndexEntry, StandardisedShip)>();
			index.AddRange(LoadFolder(@"Data\Libs\Foundry\Records\entities\spaceships", shipFilter));
			index.AddRange(LoadFolder(@"Data\Libs\Foundry\Records\entities\groundvehicles", shipFilter));

			var oldIndexItems = index.Select(x => x.Item1).ToList();
			var newIndexItems = index.Select(x => x.Item2).ToList();

			File.WriteAllText(Path.Combine(OutputFolder, "ships.json"), JsonConvert.SerializeObject(oldIndexItems));
			File.WriteAllText(Path.Combine(OutputFolder, "v2", "ships.json"), JsonConvert.SerializeObject(newIndexItems));

			Console.WriteLine();
			Console.WriteLine("*** All accepted types ***");
			foreach (var type in AcceptTypes)
			{
				Console.WriteLine(type);
			}

			Console.WriteLine();
			Console.WriteLine("*** All installed types ***");
			foreach (var type in InstalledTypes)
			{
				Console.WriteLine(type);
			}

			return index;
		}

		List<(ShipIndexEntry, StandardisedShip)> LoadFolder(string entityFolder, string shipFilter)
		{
			var shiplist = new List<(ShipIndexEntry, StandardisedShip)>();

			foreach (var entityFilename in Directory.EnumerateFiles(Path.Combine(DataRoot, entityFolder), "*.xml"))
			{
				if (avoidFile(entityFilename)) continue;
				if (shipFilter != null && !entityFilename.Contains(shipFilter, StringComparison.OrdinalIgnoreCase)) continue;

				var shipTuple = LoadShip(entityFilename);
				if (shipTuple == null)
				{
					Console.WriteLine("This doesn't seem to be a vehicle");
					continue;
				}

				(var vehicle, var entity, var parts, var ship, var ports) = shipTuple.Value;

				File.WriteAllText(Path.Combine(OutputFolder, "v2", "ships", $"{entity.ClassName.ToLower()}.json"), JsonConvert.SerializeObject(ship));
				File.WriteAllText(Path.Combine(OutputFolder, "v2", "ships", $"{entity.ClassName.ToLower()}-parts.json"), JsonConvert.SerializeObject(parts));
				File.WriteAllText(Path.Combine(OutputFolder, "v2", "ships", $"{entity.ClassName.ToLower()}-ports.json"), JsonConvert.SerializeObject(ports));

				var v2json = JsonConvert.SerializeObject(new
				{
					Entity = entity,
					Vehicle = vehicle
				});
				File.WriteAllText(Path.Combine(OutputFolder, "v2", "ships", $"{entity.ClassName.ToLower()}-raw.json"), v2json);

				var v1json = JsonConvert.SerializeObject(new
				{
					Raw = new
					{
						Entity = entity,
						Vehicle = vehicle
					}
				});
				File.WriteAllText(Path.Combine(OutputFolder, "ships", $"{entity.ClassName.ToLower()}.json"), v1json);

				// Index entry
				var indexEntry = CreateIndexEntry(entity, vehicle, ship);

				// New index
				shiplist.Add((indexEntry, ship));
			}

			return shiplist;
		}

		(Vehicle, EntityClassDefinition, List<StandardisedPart>, StandardisedShip, StandardisedPortSummary)? LoadShip(string entityFilename)
		{
			Console.WriteLine(entityFilename);

			var entity = LoadEntity(entityFilename);
			var vehicle = LoadVehicle(entity);

			var parts = InitialiseShip(entity, vehicle);

			if (parts != null)
			{
				GuessPortCategories(parts);
				CreatePartsDump(entity, parts);
			}

			StandardisedPortSummary portSummary = parts != null ? BuildPortSummary(parts) : null;
			StandardisedShip shipSummary = parts != null ? BuildShipSummary(entity, parts, portSummary) : null;

			return (vehicle, entity, parts, shipSummary, portSummary);
		}

		bool avoidFile(string filename)
		{
			var fileSplit = Path.GetFileNameWithoutExtension(filename).Split('_');
			return fileSplit.Any(part => avoids.Contains(part));
		}

		ShipIndexEntry CreateIndexEntry(EntityClassDefinition entity, Vehicle vehicle, StandardisedShip shipSummary)
		{
			bool isGroundVehicle = entity.Components?.VehicleComponentParams.vehicleCareer == "@vehicle_focus_ground";
			bool isGravlevVehicle = entity.Components?.VehicleComponentParams.isGravlevVehicle ?? false;
			bool isSpaceship = !(isGroundVehicle || isGravlevVehicle);
			var manufacturer = manufacturerSvc.GetManufacturer(entity.Components.VehicleComponentParams.manufacturer, entity.ClassName);

			var indexEntry = new ShipIndexEntry
			{
				className = entity.ClassName,
				name = entity.Components.VehicleComponentParams.vehicleName,
				career = entity.Components.VehicleComponentParams.vehicleCareer,
				role = entity.Components.VehicleComponentParams.vehicleRole,
				dogFightEnabled = Convert.ToBoolean(entity.Components.VehicleComponentParams.dogfightEnabled),
				size = shipSummary?.Size,
				isGroundVehicle = isGroundVehicle,
				isGravlevVehicle = isGravlevVehicle,
				isSpaceship = isSpaceship,
				noParts = vehicle == null || vehicle.Parts == null || vehicle.Parts.Length == 0,
				manufacturerCode = manufacturer?.Code,
				manufacturerName = manufacturer?.Name
			};

			return indexEntry;
		}

		EntityClassDefinition LoadEntity(string entityFilename)
		{
			var entity = entitySvc.GetByFilename(entityFilename);
			return entity;
		}

		Vehicle LoadVehicle(EntityClassDefinition entity)
		{
			var vehicleFilename = entity.Components?.VehicleComponentParams?.vehicleDefinition;
			if (vehicleFilename == null) return null;

			vehicleFilename = Path.Combine(DataRoot, "Data", vehicleFilename.Replace('/', '\\'));
			var vehicleModification = entity.Components?.VehicleComponentParams?.modification;

			if (String.IsNullOrEmpty(vehicleModification)) Console.WriteLine(vehicleFilename);
			else Console.WriteLine($"{vehicleFilename} ({vehicleModification})");

			var vehicleParser = new VehicleParser();
			var vehicle = vehicleParser.Parse(vehicleFilename, vehicleModification);

			return vehicle;
		}

		void GuessPortCategories(List<StandardisedPart> parts)
		{
			var classifier = new PortClassifier();
			foreach ((var port, var depth) in FindItemPorts(parts, x => true))
			{
				var portCategory = classifier.ClassifyPort(port);
				port.Category = portCategory.Item2;
			}
		}

		void CreatePartsDump(EntityClassDefinition entity, List<StandardisedPart> parts)
		{
			var sb = new StringBuilder();
			sb.AppendLine(entity.ClassName);
			foreach ((var port, var depth) in FindItemPorts(parts, x => true))
			{
				var indent = new String(' ', depth);
				var typesAccepted = String.Join(", ", port.Types ?? new List<string>());
				var typeInstalled = port.InstalledItem?.Type ?? "";
				var portSize = $"S{port.Size}";
				var installedItemName = port.InstalledItem?.ClassName ?? port.Loadout ?? "";

				// Keep the dump free of clutter
				if (port.Category == "Door attachments") continue;
				if (port.Category == "Weapon attachments") continue;
				if (port.Category == "Batteries") continue;
				if (port.Category == "Seats") continue;
				if (port.Category == "Seat access") continue;

				//if (port.Category == "UNKNOWN" || port.Category == "DISABLED") continue;

				sb.Append($"{indent + port.PortName,-50}");
				sb.Append($"{portSize,-5}");
				sb.Append($"{typesAccepted,-50}");
				sb.Append($"{installedItemName,-65}");
				sb.Append($"{port.Category,-40}");
				sb.AppendLine();

				if (port.Types != null) foreach (var t in port.Types) AcceptTypes.Add(t);
				if (port.InstalledItem?.Type != null) InstalledTypes.Add(port.InstalledItem.Type);
			}
			sb.AppendLine();

			var newFilename = Path.Combine(OutputFolder, "v2", "ships", $"{entity.ClassName.ToLower()}-dump.txt");
			File.WriteAllText(newFilename, sb.ToString());
		}

		List<StandardisedPart> InitialiseShip(EntityClassDefinition entity, Vehicle vehicle)
		{
			var loadout = loadoutLoader.Load(entity);

			var partList = vehicle != null ? BuildPartList(vehicle.Parts) : DeducePartList(loadout);

			itemInstaller.InstallLoadout(partList, loadout);

			InstallFakeItems(partList);

			return partList;
		}

		List<StandardisedPart> BuildPartList(Part[] parts)
		{
			var partList = new List<StandardisedPart>();

			if (parts != null)
			{
				foreach (var p in parts)
				{
					if (p.skipPart) continue;

					partList.Add(BuildPart(p));
				}
			}

			return partList;
		}

		StandardisedPart BuildPart(Part part)
		{
			return new StandardisedPart
			{
				Name = part.name,
				Parts = BuildPartList(part.Parts),
				Port = BuildPort(part),
				MaximumDamage = GetDamageMax(part) > 0 ? GetDamageMax(part) : (double?)null,
				Mass = ParseMass(part.mass),
				ShipDestructionDamage = CalculateDamageToDestroyShip(part),
				PartDetachDamage = CalculateDamageToDetach(part)
			};
		}

		double? ParseMass(string mass)
		{
			if (String.IsNullOrWhiteSpace(mass)) return null;

			var hacked = "";
			foreach (var c in mass)
			{
				if (Char.IsDigit(c) || c == '.') hacked += c;
			}

			return double.Parse(hacked, CultureInfo.InvariantCulture);
		}

		double? CalculateDamageToDestroyShip(Part part)
		{
			var destroyGroup = part.DamageBehaviors?.FirstOrDefault(x => x.Group?.name == "Destroy");
			if (destroyGroup == null) return null;

			return destroyGroup.damageRatioMin > 0 ? destroyGroup.damageRatioMin * GetDamageMax(part) : GetDamageMax(part);
		}

		double GetDamageMax(Part part)
		{
			// Some ships (Cutlas Black for instance) have got parts with "damagemax" rather than "damageMax"
			if (part.damageMax > 0) return part.damageMax;
			else if (part.damagemax > 0) return part.damagemax;
			else return 0;
		}

		double? CalculateDamageToDetach(Part part)
		{
			if (GetDamageMax(part) == 0 || part.detachRatio == 0) return null;
			return GetDamageMax(part) * part.detachRatio;
		}

		StandardisedItemPort BuildPort(Part part)
		{
			if (part.ItemPort == null) return null;

			var stdPort = new StandardisedItemPort
			{
				PortName = part.name,
				Size = part.ItemPort.maxsize,
				Types = BuildPortTypes(part),
				Flags = BuildPortFlags(part),
			};

			stdPort.Uneditable = stdPort.Flags.Contains("$uneditable") || stdPort.Flags.Contains("uneditable");

			return stdPort;
		}

		List<string> BuildPortTypes(Part part)
		{
			var types = new List<string>();
			if (part.ItemPort.Types == null) return null;

			foreach (var partType in part.ItemPort.Types)
			{
				var major = partType.type;
				if (String.IsNullOrWhiteSpace(major)) continue;

				if (String.IsNullOrWhiteSpace(partType.subtypes)) types.Add(itemBuilder.BuildTypeName(major, null));
				else
				{
					foreach (var subType in partType.subtypes.Split(","))
					{
						var minor = subType;
						types.Add(itemBuilder.BuildTypeName(major, minor));
					}
				}
			}

			return types;
		}

		List<string> BuildPortFlags(Part part)
		{
			var flags = new List<string>();

			if (part.ItemPort.flags != null)
			{
				foreach (var flag in part.ItemPort.flags.Split(" "))
				{
					if (!String.IsNullOrEmpty(flag)) flags.Add(flag);
				}
			}

			return flags;
		}

		IEnumerable<(StandardisedPart, int)> FindParts(List<StandardisedPart> parts, Predicate<StandardisedPart> predicate, int depth = 0)
		{
			foreach (var part in parts)
			{
				if (predicate(part)) yield return (part, depth);
				if (part.Parts != null)
				{
					var partMatches = FindParts(part.Parts, predicate, depth + 1);
					foreach (var p in partMatches) yield return p;
				}
			}
		}

		IEnumerable<(StandardisedItemPort, int)> FindItemPorts(List<StandardisedPart> parts, Predicate<StandardisedItemPort> predicate, bool stopOnFind = false, Predicate<StandardisedItemPort> stopPredicate = null, int depth = 0)
		{
			foreach (var part in parts)
			{
				if (part.Port != null)
				{
					if (stopPredicate != null && stopPredicate(part.Port)) continue;

					if (predicate(part.Port))
					{
						yield return (part.Port, depth);
						if (stopOnFind) continue;
					}

					if (part.Port.InstalledItem != null)
					{
						var itemMatches = FindItemPorts(part.Port.InstalledItem, predicate, stopOnFind, stopPredicate, depth + 1);
						foreach (var m in itemMatches) yield return m;
					}
				}
				var partMatches = FindItemPorts(part.Parts, predicate, stopOnFind, stopPredicate, depth + 1);
				foreach (var m in partMatches) yield return m;
			}
		}

		IEnumerable<(StandardisedItemPort, int)> FindItemPorts(StandardisedItem item, Predicate<StandardisedItemPort> predicate, bool stopOnFind, Predicate<StandardisedItemPort> stopPredicate, int depth)
		{
			foreach (var port in item.Ports)
			{
				if (stopPredicate != null && stopPredicate(port)) continue;

				if (predicate(port))
				{
					yield return (port, depth);
					if (stopOnFind) continue;
				}

				if (port.InstalledItem != null)
				{
					var matches = FindItemPorts(port.InstalledItem, predicate, stopOnFind, stopPredicate, depth + 1);
					foreach (var m in matches) yield return m;
				}
			}
		}

		StandardisedPortSummary BuildPortSummary(List<StandardisedPart> parts)
		{
			var portSummary = new StandardisedPortSummary();

			// Player controlled hardpoints (those not in a turret)
			portSummary.PilotHardpoints = FindItemPorts(parts, x => x.Category == "Weapon hardpoints", true, x => x.Category == "Manned turrets" || x.Category == "Remote turrets" || x.Category == "Mining turrets" || x.Category == "Utility turrets").Select(x => x.Item1).ToList();
			portSummary.MiningHardpoints = FindItemPorts(parts, x => x.Category == "Mining hardpoints", true, x => x.Category == "Manned turrets" || x.Category == "Remote turrets" || x.Category == "Mining turrets" || x.Category == "Utility turrets").Select(x => x.Item1).ToList();
			portSummary.UtilityHardpoints = FindItemPorts(parts, x => x.Category == "Utility hardpoints", true, x => x.Category == "Manned turrets" || x.Category == "Remote turrets" || x.Category == "Mining turrets" || x.Category == "Utility turrets").Select(x => x.Item1).ToList();

			// Turrets
			portSummary.MiningTurrets = FindItemPorts(parts, x => x.Category == "Mining turrets", true).Select(x => x.Item1).ToList();
			portSummary.MannedTurrets = FindItemPorts(parts, x => x.Category == "Manned turrets", true).Select(x => x.Item1).ToList();
			portSummary.RemoteTurrets = FindItemPorts(parts, x => x.Category == "Remote turrets", true).Select(x => x.Item1).ToList();
			portSummary.UtilityTurrets = FindItemPorts(parts, x => x.Category == "Utility turrets", true).Select(x => x.Item1).ToList();

			// Other hardpoints
			portSummary.InterdictionHardpoints = FindItemPorts(parts, x => x.Category == "EMP hardpoints" || x.Category == "QIG hardpoints", true).Select(x => x.Item1).ToList();
			portSummary.MissileRacks = FindItemPorts(parts, x => x.Category == "Missile racks", true).Select(x => x.Item1).ToList();
			portSummary.PowerPlants = FindItemPorts(parts, x => x.Category == "Power plants", true).Select(x => x.Item1).ToList();
			portSummary.Coolers = FindItemPorts(parts, x => x.Category == "Coolers", true).Select(x => x.Item1).ToList();
			portSummary.Shields = FindItemPorts(parts, x => x.Category == "Shield generators", true).Select(x => x.Item1).ToList();
			portSummary.CargoGrids = FindItemPorts(parts, x => x.Category == "Cargo grids", true).Select(x => x.Item1).ToList();
			portSummary.Countermeasures = FindItemPorts(parts, x => x.Category == "Countermeasures", true).Select(x => x.Item1).ToList();
			portSummary.MainThrusters = FindItemPorts(parts, x => x.Category == "Main thrusters", true).Select(x => x.Item1).ToList();
			portSummary.RetroThrusters = FindItemPorts(parts, x => x.Category == "Retro thrusters", true).Select(x => x.Item1).ToList();
			portSummary.VtolThrusters = FindItemPorts(parts, x => x.Category == "VTOL thrusters", true).Select(x => x.Item1).ToList();
			portSummary.ManeuveringThrusters = FindItemPorts(parts, x => x.Category == "Maneuvering thrusters", true).Select(x => x.Item1).ToList();
			portSummary.HydogenFuelIntakes = FindItemPorts(parts, x => x.Category == "Fuel intakes", true).Select(x => x.Item1).ToList();
			portSummary.HydrogenFuelTanks = FindItemPorts(parts, x => x.Category == "Fuel tanks", true).Select(x => x.Item1).ToList();
			portSummary.QuantumDrives = FindItemPorts(parts, x => x.Category == "Quantum drives", true).Select(x => x.Item1).ToList();
			portSummary.QuantumFuelTanks = FindItemPorts(parts, x => x.Category == "Quantum fuel tanks", true).Select(x => x.Item1).ToList();
			portSummary.Avionics = FindItemPorts(parts, x => x.Category == "Scanners" || x.Category == "Pings" || x.Category == "Radars" || x.Category == "Transponders", true).Select(x => x.Item1).ToList();

			return portSummary;
		}

		StandardisedShip BuildShipSummary(EntityClassDefinition entity, List<StandardisedPart> parts, StandardisedPortSummary portSummary)
		{
			var shipSummary = new StandardisedShip
			{
				ClassName = entity.ClassName,
				Name = localisationSvc.GetText(entity.Components.VehicleComponentParams.vehicleName, entity.ClassName),
				Description = localisationSvc.GetText(entity.Components.VehicleComponentParams.vehicleDescription),
				Career = localisationSvc.GetText(entity.Components.VehicleComponentParams.vehicleCareer),
				Role = localisationSvc.GetText(entity.Components.VehicleComponentParams.vehicleRole),
				Manufacturer = manufacturerSvc.GetManufacturer(entity.Components.VehicleComponentParams.manufacturer, entity.ClassName),
				Size = entity.Components.SAttachableComponentParams.AttachDef.Size,
				Crew = entity.Components.VehicleComponentParams.crewSize,
				WeaponCrew = portSummary.MannedTurrets.Count + portSummary.RemoteTurrets.Count,
				OperationsCrew = Math.Max(portSummary.MiningTurrets.Count, portSummary.UtilityTurrets.Count),
				Mass = FindParts(parts, x => true).Sum((x) => x.Item1.Mass ?? 0),
				Cargo = (int)(portSummary.CargoGrids
					.Where(x => x.InstalledItem?.CargoGrid != null)
					.Where(x => !x.InstalledItem.CargoGrid.MiningOnly)
					.Sum(x => x.InstalledItem.CargoGrid.Capacity)),
				Insurance = insuranceSvc.GetInsurance(entity.ClassName)
			};

			shipSummary.IsVehicle = entity.Components?.VehicleComponentParams.vehicleCareer == "@vehicle_focus_ground";
			shipSummary.IsGravlev = entity.Components?.VehicleComponentParams.isGravlevVehicle ?? false;
			shipSummary.IsSpaceship = !(shipSummary.IsVehicle || shipSummary.IsGravlev);

			// Quantum travel
			var quantumDrive = portSummary.QuantumDrives.FirstOrDefault(x => x.InstalledItem != null)?.InstalledItem.QuantumDrive;
			var quantumFuelCapacity = portSummary.QuantumFuelTanks.Sum(x => x.InstalledItem?.QuantumFuelTank?.Capacity ?? 0);
			var distanceBetweenPOandArcCorp = 41927351070;
			shipSummary.QuantumTravel = new StandardisedQuantumTravelSummary
			{
				FuelCapacity = quantumFuelCapacity,
				Range = (quantumFuelCapacity / quantumDrive?.FuelRate) ?? 0,
				Speed = quantumDrive?.StandardJump.Speed ?? 0,
				SpoolTime = quantumDrive?.StandardJump.SpoolUpTime ?? 0,
				PortOlisarToArcCorpTime = (distanceBetweenPOandArcCorp / quantumDrive?.StandardJump.Speed) ?? 0,
				PortOlisarToArcCorpFuel = (distanceBetweenPOandArcCorp * quantumDrive?.FuelRate) ?? 0,
				PortOlisarToArcCorpAndBack = ((quantumFuelCapacity / quantumDrive?.FuelRate) / (2 * distanceBetweenPOandArcCorp)) ?? 0
			};

			// Propulsion
			shipSummary.Propulsion = new StandardisedPropulsionSummary
			{
				FuelCapacity = portSummary.HydrogenFuelTanks.Sum(x => x.InstalledItem?.HydrogenFuelTank?.Capacity ?? 0),
				FuelIntakeRate = portSummary.HydogenFuelIntakes.Sum(x => x.InstalledItem?.HydrogenFuelIntake?.Rate ?? 0),
				FuelUsage = new StandardisedThrusterSummary
				{
					Main = portSummary.MainThrusters.Sum(x => x.InstalledItem?.Thruster?.MaxThrustFuelRate ?? 0),
					Retro = portSummary.RetroThrusters.Sum(x => x.InstalledItem?.Thruster?.MaxThrustFuelRate ?? 0),
					Vtol = portSummary.VtolThrusters.Sum(x => x.InstalledItem?.Thruster?.MaxThrustFuelRate ?? 0),
					Maneuvering = portSummary.ManeuveringThrusters.Sum(x => x.InstalledItem?.Thruster?.MaxThrustFuelRate ?? 0)
				},
				ThrustCapacity = new StandardisedThrusterSummary
				{
					Main = portSummary.MainThrusters.Sum(x => x.InstalledItem?.Thruster?.ThrustCapacity ?? 0),
					Retro = portSummary.RetroThrusters.Sum(x => x.InstalledItem?.Thruster?.ThrustCapacity ?? 0),
					Vtol = portSummary.VtolThrusters.Sum(x => x.InstalledItem?.Thruster?.ThrustCapacity ?? 0),
					Maneuvering = portSummary.ManeuveringThrusters.Sum(x => x.InstalledItem?.Thruster?.ThrustCapacity ?? 0)
				}
			};
			shipSummary.Propulsion.IntakeToMainFuelRatio = shipSummary.Propulsion.FuelIntakeRate / shipSummary.Propulsion.FuelUsage.Main;
			shipSummary.Propulsion.IntakeToTankCapacityRatio = shipSummary.Propulsion.FuelIntakeRate / shipSummary.Propulsion.FuelCapacity;
			shipSummary.Propulsion.TimeForIntakesToFillTank = shipSummary.Propulsion.FuelCapacity / shipSummary.Propulsion.FuelIntakeRate;
			shipSummary.Propulsion.ManeuveringTimeTillEmpty = shipSummary.Propulsion.FuelCapacity / (shipSummary.Propulsion.FuelUsage.Main + shipSummary.Propulsion.FuelUsage.Maneuvering / 2 - shipSummary.Propulsion.FuelIntakeRate);

			// Flight characteristics
			var (ifcs, _) = FindItemPorts(parts, x => x.InstalledItem?.Ifcs != null).FirstOrDefault();
			var G = 9.80665f;
			shipSummary.FlightCharacteristics = new StandardisedFlightCharacteristics
			{
				ScmSpeed = ifcs?.InstalledItem.Ifcs.MaxSpeed ?? 0,
				MaxSpeed = ifcs?.InstalledItem.Ifcs.MaxAfterburnSpeed ?? 0,
				Acceleration = new StandardisedThrusterSummary
				{
					Main = shipSummary.Propulsion.ThrustCapacity.Main / shipSummary.Mass,
					Retro = shipSummary.Propulsion.ThrustCapacity.Retro / shipSummary.Mass,
					Vtol = shipSummary.Propulsion.ThrustCapacity.Vtol / shipSummary.Mass,
					Maneuvering = shipSummary.Propulsion.ThrustCapacity.Maneuvering / shipSummary.Mass
				},
				AccelerationG = new StandardisedThrusterSummary
				{
					Main = shipSummary.Propulsion.ThrustCapacity.Main / shipSummary.Mass / G,
					Retro = shipSummary.Propulsion.ThrustCapacity.Retro / shipSummary.Mass / G,
					Vtol = shipSummary.Propulsion.ThrustCapacity.Vtol / shipSummary.Mass / G,
					Maneuvering = shipSummary.Propulsion.ThrustCapacity.Maneuvering / shipSummary.Mass / G
				}
			};
			shipSummary.FlightCharacteristics.ZeroToScm = shipSummary.FlightCharacteristics.ScmSpeed / shipSummary.FlightCharacteristics.Acceleration.Main;
			shipSummary.FlightCharacteristics.ZeroToMax = shipSummary.FlightCharacteristics.MaxSpeed / shipSummary.FlightCharacteristics.Acceleration.Main;
			shipSummary.FlightCharacteristics.ScmToZero = shipSummary.FlightCharacteristics.ScmSpeed / shipSummary.FlightCharacteristics.Acceleration.Retro;
			shipSummary.FlightCharacteristics.MaxToZero = shipSummary.FlightCharacteristics.MaxSpeed / shipSummary.FlightCharacteristics.Acceleration.Retro;

			// Destruction damage
			shipSummary.DamageBeforeDestruction =
				FindParts(parts, (x) => x.ShipDestructionDamage > 0)
				.ToDictionary(x => x.Item1.Name, x => x.Item1.ShipDestructionDamage ?? 0);

			// Detach damage
			shipSummary.DamageBeforeDetach =
				FindParts(parts, (x) => x.PartDetachDamage > 0 && !x.ShipDestructionDamage.HasValue)
				.ToDictionary(x => x.Item1.Name, x => x.Item1.PartDetachDamage ?? 0);

			// Weapon fittings
			shipSummary.PilotHardpoints = CalculateWeaponFittings(portSummary.PilotHardpoints);
			shipSummary.MannedTurrets = CalculateWeaponFittings(portSummary.MannedTurrets);
			shipSummary.RemoteTurrets = CalculateWeaponFittings(portSummary.RemoteTurrets);

			return shipSummary;
		}

		List<StandardisedWeaponFitting> CalculateWeaponFittings(List<StandardisedItemPort> ports)
		{
			var fittings = new List<StandardisedWeaponFitting>();
			foreach (var port in ports)
			{
				var fitting = CalculateWeaponFitting(port);

				fittings.Add(fitting);
			}
			return fittings;
		}

		StandardisedWeaponFitting CalculateWeaponFitting(StandardisedItemPort port)
		{
			// If the turret or gimbal can't be removed
			if ((port.Uneditable || !AcceptsWeapon(port)) && (IsTurret(port) || IsGimbal(port))) return new StandardisedWeaponFitting
			{
				Size = port.Size,
				Gimballed = IsGimbal(port),
				Turret = IsTurret(port),
				WeaponSizes = ListTurretPortSizes(port)
			};

			// If the turret or gimbal can be removed and replaced with a weapon
			if ((IsTurret(port) || IsGimbal(port)) && AcceptsWeapon(port)) return new StandardisedWeaponFitting
			{
				Size = port.Size,
				Fixed = true,
				WeaponSizes = new List<int> { port.Size }
			};

			// Otherwise it is probably a regular weapon
			return new StandardisedWeaponFitting
			{
				Size = port.Size,
				Fixed = true,
				WeaponSizes = new List<int> { port.Size }
			};
		}

		bool IsGimbal(StandardisedItemPort port)
		{
			switch (port.InstalledItem?.Type)
			{
				case "Turret.GunTurret":
					return true;
			}

			return false;
		}

		bool IsTurret(StandardisedItemPort port)
		{
			switch (port.InstalledItem?.Type)
			{
				case "Turret.BallTurret":
				case "Turret.CanardTurret":
				case "Turret.MissileTurret":
				case "Turret.NoseMounted":
				case "TurretBase.MannedTurret":
				case "TurretBase.Unmanned":
					return true;
			}

			return false;
		}

		bool AcceptsWeapon(StandardisedItemPort port)
		{
			if (port.Types.Contains("WeaponGun")) return true;
			if (port.Types.Contains("WeaponGun.Gun")) return true;
			if (port.Types.Contains("WeaponMining.Gun")) return true;
			return false;
		}

		List<int> ListTurretPortSizes(StandardisedItemPort port)
		{
			var sizes = new List<int>();
			foreach (var subPort in port.InstalledItem.Ports)
			{
				if (AcceptsWeapon(subPort)) sizes.Add(subPort.Size);
			}
			return sizes;
		}

		List<StandardisedPart> DeducePartList(List<StandardisedLoadoutEntry> stdLoadout)
		{
			var parts = new List<StandardisedPart>();

			foreach (var entry in stdLoadout)
			{
				var part = new StandardisedPart
				{
					Name = entry.PortName,
					Port = new StandardisedItemPort
					{
						PortName = entry.PortName,
						Size = GuessSizeFromItemClass(entry.ClassName),
						Loadout = entry.ClassName
					}
				};

				if (entry.Entries != null) part.Parts = DeducePartList(entry.Entries);

				parts.Add(part);
			}

			return parts;
		}

		int GuessSizeFromItemClass(string className)
		{
			var regex = new Regex("_S([0-9]+)");
			var match = regex.Match(className);
			if (match.Success) return Int32.Parse(match.Groups[1].Value);
			return 0;
		}

		void InstallFakeItems(List<StandardisedPart> parts)
		{
			foreach (var part in parts)
			{
				InstallFakeItems(part);
			}
		}

		void InstallFakeItems(StandardisedPart part)
		{
			if (part.Port == null) return;

			InstallFakeItem(part.Port);
			InstallFakeItems(part.Parts);
		}

		void InstallFakeItem(StandardisedItemPort port)
		{
			// This helps us patch together 1/2 completed ships like the Javelin
			if (PortClassifier.FuzzyNameMatch(port, "seat")) return;
			if (PortClassifier.FuzzyNameMatch(port, "radar")) return;
			if (PortClassifier.FuzzyNameMatch(port, "dashboard")) return;
			if (PortClassifier.FuzzyNameMatch(port, "controller")) return;

			if (PortClassifier.FuzzyNameMatch(port, "turret")) port.Types = new List<string> { "TurretBase.MannedTurret" };
			if (PortClassifier.FuzzyNameMatch(port, "powerplant")) port.Types = new List<string> { "PowerPlant.Power" };
			if (PortClassifier.FuzzyNameMatch(port, "cooler")) port.Types = new List<string> { "Cooler.UNDEFINED" };
			if (PortClassifier.FuzzyNameMatch(port, "shield_generator")) port.Types = new List<string> { "Shield.UNDEFINED" };
			if (PortClassifier.FuzzyNameMatch(port, "hardpoint_weapon")) port.Types = new List<string> { "WeaponGun.Gun" };
			if (PortClassifier.FuzzyNameMatch(port, "hardpoint_gimbal")) port.Types = new List<string> { "WeaponGun.Gun" };

			// This helps us patch together 1/2 completed ships like the Javelin
			if (port.InstalledItem != null)
			{
				if (port.Types == null) port.Types = new List<string> { port.InstalledItem.Type };
				if (port.Size == 0) port.Size = port.InstalledItem.Size;
			}
		}
	}
}
