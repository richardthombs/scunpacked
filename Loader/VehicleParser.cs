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

			var vehicleBase = ParseVehicle(fullXmlPath, modificationName);
			if (!String.IsNullOrWhiteSpace(modificationName))
			{
				var modification = vehicleBase.Modifications.FirstOrDefault(x => x.name == modificationName);
				if (modification != null)
				{
					if (!String.IsNullOrWhiteSpace(modification.patchFile))
					{
						var patchFilename = Path.ChangeExtension(Path.Combine(Path.GetDirectoryName(fullXmlPath), modification.patchFile.Replace("/", "\\")), ".xml");
						Console.WriteLine(patchFilename);
						var patches = ParsePatchFile(patchFilename);
						if (patches.Parts != null) vehicleBase.Parts = patches.Parts;
						if (patches.MovementParams != null) vehicleBase.MovementParams = patches.MovementParams;
					}
				}
				else Console.WriteLine($"Could not process vehicle modification '{modificationName}'");
			}
			return vehicleBase;
		}

		Vehicle ParseVehicle(string vehiclePath, string modificationName)
		{
			var doc = LoadVehicleXml(vehiclePath);

			if (!String.IsNullOrWhiteSpace(modificationName)) ProcessModificationElems(doc, modificationName);

			var serialiser = new XmlSerializer(typeof(Vehicle));
			using (var stream = new XmlNodeReader(doc))
			{
				var vehicle = (Vehicle)serialiser.Deserialize(stream);
				return vehicle;
			}
		}

		XmlDocument LoadVehicleXml(string vehiclePath)
		{
			var xml = File.ReadAllText(vehiclePath);
			var doc = new XmlDocument();
			doc.LoadXml(xml);
			return doc;
		}

		void ProcessModificationElems(XmlDocument doc, string modificationName)
		{
			var elems = doc.SelectNodes($"//Modifications/Modification[@name='{modificationName}']/Elems/Elem");
			foreach (XmlNode elem in elems)
			{
				var idRef = elem.Attributes["idRef"].Value;
				var attrName = elem.Attributes["name"].Value;
				var attrValue = elem.Attributes["value"].Value;

				var nodes = doc.SelectNodes($"//*[@id='{idRef}']");
				foreach (XmlNode node in nodes)
				{
					var attr = node.Attributes[attrName];
					if (attr == null) attr = node.Attributes.Append(doc.CreateAttribute(attrName));
					node.Attributes[attrName].Value = attrValue;
				}
			}
		}

		Modifications ParsePatchFile(string modificationsPath)
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
