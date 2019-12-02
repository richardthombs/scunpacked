using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class VehicleComponentParams
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
}