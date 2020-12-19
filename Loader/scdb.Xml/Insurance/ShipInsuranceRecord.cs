using System;
using System.Xml.Serialization;

namespace Scdb.Xml
{
	public class ShipInsuranceRecord
	{
		[XmlArray]
		public ShipInsuranceParams[] allShips;
	}

	public class ShipInsuranceParams
	{
		[XmlAttribute]
		public string shipEntityClassName;

		[XmlAttribute]
		public double baseWaitTimeMinutes;

		[XmlAttribute]
		public double mandatoryWaitTimeMinutes;

		[XmlAttribute]
		public double baseExpeditingFee;
	}
}
