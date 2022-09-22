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

			return vehicleBase;
		}

		Vehicle ParseVehicle(string vehiclePath, string modificationName)
		{
			var doc = LoadVehicleXml(vehiclePath);

			if (!String.IsNullOrWhiteSpace(modificationName))
			{
				ProcessPatchFile(doc, modificationName, vehiclePath);
				ProcessModificationElems(doc, modificationName);
			}

			var serialiser = new XmlSerializer(typeof(Vehicle));
			using (var stream = new XmlNodeReader(doc))
			{
				try
				{
					var vehicle = (Vehicle)serialiser.Deserialize(stream);
					return vehicle;
				}
				catch (InvalidOperationException ex)
				{
					Console.WriteLine($@"Deserialisation failed while parsing {stream.Name} value ""{stream.Value}"": {ex.InnerException.Message}");
					throw;
				}
			}
		}

		XmlDocument LoadVehicleXml(string vehiclePath)
		{
			var xml = File.ReadAllText(vehiclePath);
			xml = FixXmlFileSerializationProblems(xml);
			var doc = new XmlDocument();
			doc.LoadXml(xml);
			return doc;
		}

		void ProcessPatchFile(XmlDocument doc, string modificationName, string vehiclePath)
		{
			var modificationNode = doc.SelectNodes($"//Modifications/Modification[@name='{modificationName}']")[0];
			var patchFile = modificationNode.Attributes["patchFile"]?.Value;
			if (String.IsNullOrEmpty(patchFile)) return;

			var patchFilename = Path.ChangeExtension(Path.Combine(Path.GetDirectoryName(vehiclePath), patchFile.Replace("/", "\\")), ".xml");
			if (!File.Exists(patchFilename)) return;

			Console.WriteLine(patchFilename);

			var patchDocText = File.ReadAllText(patchFilename);
			var patchDoc = new XmlDocument();
			patchDoc.LoadXml(patchDocText);

			foreach (XmlNode patchNode in patchDoc.DocumentElement.ChildNodes)
			{
				var id = patchNode.Attributes["id"]?.Value;
				if (id == null)
				{
					Console.WriteLine($"Can't load modifications for {patchNode.Name} - there is no id attribute");
					continue;
				}

				var nodes = doc.SelectNodes($"//*[@id='{id}']");
				foreach (XmlNode node in nodes)
				{
					node.InnerXml = patchNode.InnerXml;
				}
			}
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

		string FixXmlFileSerializationProblems(string xmlFileText)
		{
			if(xmlFileText.Contains("..") == true)
			{
				xmlFileText = xmlFileText.Replace("..", ".");
			}

			return xmlFileText;
		}
	}
}
