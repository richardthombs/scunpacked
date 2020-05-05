using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SCItemEMPParams
	{
		[XmlAttribute]
		public string ChargedTag;

		[XmlAttribute]
		public double chargeTime;

		[XmlAttribute]
		public string ChargingTag;

		[XmlAttribute]
		public double cooldownTime;

		[XmlAttribute]
		public double distortionDamage;

		[XmlAttribute]
		public double empRadius;

		[XmlAttribute]
		public double minEmpRadius;

		[XmlAttribute]
		public double minPhysRadius;

		[XmlAttribute]
		public double physRadius;

		[XmlAttribute]
		public double pressure;

		[XmlAttribute]
		public string StartChargedTrigger;

		[XmlAttribute]
		public string StartChargingTrigger;

		[XmlAttribute]
		public string StartUnleashTrigger;

		[XmlAttribute]
		public string StopChargedTrigger;

		[XmlAttribute]
		public string StopChargingTrigger;

		[XmlAttribute]
		public string StopUnleashTrigger;

		[XmlAttribute]
		public double unleashTime;
	}
}
