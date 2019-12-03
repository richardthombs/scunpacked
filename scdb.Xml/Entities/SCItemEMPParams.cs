using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemEMPParams
	{
		[XmlAttribute]
		public double chargeTime;

		[XmlAttribute]
		public double distortionDamage;

		[XmlAttribute]
		public double empRadius;

		[XmlAttribute]
		public double minEmpRadius;

		[XmlAttribute]
		public double physRadius;

		[XmlAttribute]
		public double minPhysRadius;

		[XmlAttribute]
		public double pressure;

		[XmlAttribute]
		public double unleashTime;

		[XmlAttribute]
		public double cooldownTime;

		[XmlAttribute]
		public string ChargingTag;

		[XmlAttribute]
		public string ChargedTag;

		[XmlAttribute]
		public string StartChargingTrigger;

		[XmlAttribute]
		public string StopChargingTrigger;

		[XmlAttribute]
		public string StartChargedTrigger;

		[XmlAttribute]
		public string StopChargedTrigger;

		[XmlAttribute]
		public string StartUnleashTrigger;

		[XmlAttribute]
		public string StopUnleashTrigger;
	}
}