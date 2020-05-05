using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Loader.SCDb.Xml.Vehicles;
using Microsoft.Extensions.Logging;

namespace Loader.Parser
{
	public class VehicleParser
	{
		private readonly ILogger<VehicleParser> _logger;

		public VehicleParser(ILogger<VehicleParser> logger)
		{
			_logger = logger;
		}

		public Vehicle Parse(string fullXmlPath, string modificationName)
		{
			if (!File.Exists(fullXmlPath))
			{
				_logger.LogWarning("Vehicle implementation file does not exist");
				return null;
			}

			var vehicleBase = ParseVehicle(fullXmlPath, modificationName);
			if (!string.IsNullOrWhiteSpace(modificationName))
			{
				var modification = vehicleBase.Modifications.FirstOrDefault(x => x.name == modificationName);
				if (modification != null)
				{
					if (!string.IsNullOrWhiteSpace(modification.patchFile))
					{
						var patchFilename =
							Path.ChangeExtension(Path.Combine(Path.GetDirectoryName(fullXmlPath),
							                                  modification.patchFile.Replace("/", "\\")),
							                     ".xml");
						_logger.LogInformation(patchFilename);
						var patches = ParsePatchFile(patchFilename);
						if (patches.Parts != null)
						{
							vehicleBase.Parts = patches.Parts;
						}

						if (patches.MovementParams != null)
						{
							vehicleBase.MovementParams = patches.MovementParams;
						}
					}
				}
				else
				{
					_logger.LogWarning($"Could not process vehicle modification '{modificationName}'");
				}
			}

			return vehicleBase;
		}

		private Vehicle ParseVehicle(string vehiclePath, string modificationName)
		{
			var doc = LoadVehicleXml(vehiclePath);

			if (!string.IsNullOrWhiteSpace(modificationName))
			{
				ProcessModificationElems(doc, modificationName);
			}

			var serialiser = new XmlSerializer(typeof(Vehicle));

			using var stream = new XmlNodeReader(doc);
			var vehicle = (Vehicle) serialiser.Deserialize(stream);
			return vehicle;
		}

		private XmlDocument LoadVehicleXml(string vehiclePath)
		{
			var xml = File.ReadAllText(vehiclePath);
			var doc = new XmlDocument();
			doc.LoadXml(xml);
			return doc;
		}

		private void ProcessModificationElems(XmlDocument doc, string modificationName)
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
					var attr = node.Attributes[attrName] ?? node.Attributes.Append(doc.CreateAttribute(attrName));
					attr.Value = attrValue;
				}
			}
		}

		private Modifications ParsePatchFile(string modificationsPath)
		{
			var xml = File.ReadAllText(modificationsPath);
			var doc = new XmlDocument();
			doc.LoadXml(xml);

			var serialiser = new XmlSerializer(typeof(Modifications));

			using var stream = new XmlNodeReader(doc);
			var modifications = (Modifications) serialiser.Deserialize(stream);
			return modifications;
		}
	}
}
