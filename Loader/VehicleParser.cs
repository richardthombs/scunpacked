using System;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using scdb.Xml.Vehicles;

namespace Loader
{
	public class VehicleParser
	{
		public Vehicle Parse(string fullXmlPath, string modificationName)
		{
			if (!File.Exists(fullXmlPath))
			{
				Console.WriteLine("Vehicle implementation file does not exist");
				return null;
			}

			var vehicleBase = ParseVehicle(fullXmlPath);
			if (!String.IsNullOrWhiteSpace(modificationName))
			{
				var modification = vehicleBase.Modifications.FirstOrDefault(x => x.name == modificationName);
				if (modification != null && !String.IsNullOrWhiteSpace(modification.patchFile))
				{
					var modFilename = Path.ChangeExtension(Path.Combine(Path.GetDirectoryName(fullXmlPath), modification.patchFile.Replace("/", "\\")), ".xml");
					Console.WriteLine(modFilename);
					var modifications = ParseModifications(modFilename);
					if (modifications.Parts != null) vehicleBase.Parts = modifications.Parts;
					if (modifications.MovementParams != null) vehicleBase.MovementParams = modifications.MovementParams;
				}
				else Console.WriteLine($"Could not process vehicle modification '{modificationName}'");
			}
			return vehicleBase;
		}

		Vehicle ParseVehicle(string vehiclePath)
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

		Modifications ParseModifications(string modificationsPath)
		{
			var xml = File.ReadAllText(modificationsPath);
			var doc = new XmlDocument();
			doc.LoadXml(xml);

			var serialiser = new XmlSerializer(typeof(Modifications));
			using (var stream = new XmlNodeReader(doc))
			{
				var modifications = (Modifications)serialiser.Deserialize(stream);
				return modifications;
			}
		}
	}
}
