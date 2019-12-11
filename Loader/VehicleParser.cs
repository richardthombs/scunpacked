using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using scdb.Xml.Vehicles;

namespace Loader
{
	public class VehicleParser
	{
		public Vehicle Parse(string fullXmlPath, string modification)
		{
			if (!File.Exists(fullXmlPath))
			{
				Console.WriteLine("Vehicle implementation file does not exist");
				return null;
			}

			return ParseVehicle(fullXmlPath, modification);
		}

		Vehicle ParseVehicle(string vehiclePath, string modification)
		{

			var xml = File.ReadAllText(vehiclePath);
			var doc = new XmlDocument();
			doc.LoadXml(xml);

			var serialiser = new XmlSerializer(typeof(Vehicle));
			using (var stream = new XmlNodeReader(doc))
			{
				var vehicle = (Vehicle)serialiser.Deserialize(stream);
				return vehicle;
			}
		}
	}
}
