using System;
using System.Xml;
using System.Xml.Serialization;

namespace shipparser
{
	public class EntityClassDefinition
	{
		[XmlAttribute]
		public string __type { get; set; }

		[
			XmlArrayItem(typeof(VehicleComponentParams)),
			XmlArrayItem(typeof(SEntityComponentDefaultLoadoutParams)),
			XmlArrayItem(typeof(SAttachableComponentParams)),
			XmlArrayItem(typeof(SHealthComponentParams))
		]
		public Component[] Components { get; set; }
	}

	public abstract class Component
	{
		[XmlAttribute]
		public string __type { get; set; }
	}

	public class VehicleComponentParams : Component
	{
		[XmlAttribute]
		public string landingSystem { get; set; }

		[XmlAttribute]
		public string manufacturer { get; set; }

		[XmlAttribute]
		public string vehicleDefinition { get; set; }

		[XmlAttribute]
		public string modification { get; set; }

		[XmlAttribute]
		public int dogfightEnabled { get; set; }

		[XmlAttribute]
		public int unmovable { get; set; }

		[XmlAttribute]
		public int isGravlevVehicle { get; set; }

		[XmlAttribute]
		public double incomingDamageModifierToAI { get; set; }

		[XmlAttribute]
		public double emergencyStatusDamageThreshold { get; set; }

		[XmlAttribute]
		public int crewSize { get; set; }

		[XmlAttribute]
		public string vehicleName { get; set; }

		[XmlAttribute]
		public string vehicleDescription { get; set; }

		[XmlAttribute]
		public string vehicleCareer { get; set; }

		[XmlAttribute]
		public string vehicleCareerRef { get; set; }

		[XmlAttribute]
		public string vehicleRole { get; set; }

		[XmlAttribute]
		public string vehicleRoleRef { get; set; }
	}

	public class SEntityComponentDefaultLoadoutParams : Component
	{
		public loadout loadout { get; set; }
	}

	public class loadout
	{
		public SItemPortLoadoutXMLParams SItemPortLoadoutXMLParams { get; set; }
		public SItemPortLoadoutManualParams SItemPortLoadoutManualParams { get; set; }
	}

	public class SItemPortLoadoutManualParams
	{
		public SItemPortLoadoutEntryParams[] entries { get; set; }
	}

	public class SItemPortLoadoutEntryParams
	{
		[XmlAttribute]
		public string itemPortName { get; set; }

		[XmlAttribute]
		public string entityClassName { get; set; }
	}

	public class SItemPortLoadoutXMLParams
	{
		[XmlAttribute]
		public string loadoutPath { get; set; }

		[XmlAttribute]
		public string __type { get; set; }
	}

	public class SAttachableComponentParams : Component
	{
		public SItemDefinition AttachDef { get; set; }
	}

	public class SItemDefinition
	{
		[XmlAttribute]
		public string Type { get; set; }

		[XmlAttribute]
		public string SubType { get; set; }

		[XmlAttribute]
		public int Size { get; set; }

		[XmlAttribute]
		public int Grade { get; set; }

		[XmlAttribute]
		public string Manufacturer { get; set; }

		[XmlAttribute]
		public string Tags;

		[XmlAttribute]
		public string __type { get; set; }

		public SCItemLocalization Localization { get; set; }
	}

	public class SCItemLocalization
	{
		[XmlAttribute]
		public string Name { get; set; }

		[XmlAttribute]
		public string Description { get; set; }
	}

	public class SHealthComponentParams : Component
	{
		[XmlAttribute]
		public int Health { get; set; }

		public DamageResistances DamageResistances { get; set; }
	}

	public class DamageResistances
	{
		public DamageResistance DamageResistance { get; set; }
	}

	public class DamageResistance
	{
		public DamageResistanceEntry PhysicalResistance { get; set; }
		public DamageResistanceEntry EnergyResistance { get; set; }
		public DamageResistanceEntry DistortionResistance { get; set; }
		public DamageResistanceEntry ThermalResistance { get; set; }
		public DamageResistanceEntry BiochemicalResistance { get; set; }
	}

	public class DamageResistanceEntry
	{
		[XmlAttribute]
		public double Multiplier { get; set; }

		[XmlAttribute]
		public int Threshold { get; set; }
	}

	public class TurbulentEntry
	{
		[XmlAttribute]
		public string turbulentName { get; set; }

		[XmlAttribute]
		public string itemClass { get; set; }
	}
}