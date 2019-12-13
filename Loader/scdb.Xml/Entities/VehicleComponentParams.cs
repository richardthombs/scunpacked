using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class VehicleComponentParams
	{
		[XmlAttribute]
		public string landingSystem;

		[XmlAttribute]
		public string manufacturer;

		[XmlAttribute]
		public string vehicleDefinition;

		[XmlAttribute]
		public string modification;

		[XmlAttribute]
		public int dogfightEnabled;

		[XmlAttribute]
		public int unmovable;

		[XmlAttribute]
		public bool isGravlevVehicle;

		[XmlAttribute]
		public double incomingDamageModifierToAI;

		[XmlAttribute]
		public double emergencyStatusDamageThreshold;

		[XmlAttribute]
		public int crewSize;

		[XmlAttribute]
		public string vehicleName;

		[XmlAttribute]
		public string vehicleDescription;

		[XmlAttribute]
		public string vehicleCareer;

		[XmlAttribute]
		public string vehicleCareerRef;

		[XmlAttribute]
		public string vehicleRole;

		[XmlAttribute]
		public string vehicleRoleRef;
	}
}
