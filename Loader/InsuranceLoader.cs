using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Scdb.Xml;

namespace Loader
{
	public class InsuranceLoader
	{
		public string DataRoot { get; set; }

		public Dictionary<string, StandardisedInsurance> Load()
		{
			var output = new Dictionary<string, StandardisedInsurance>();

			var insurance = Parse<ShipInsuranceRecord>(Path.Combine(DataRoot, @"data\libs\foundry\records\shipinsurancerecord\shipinsurance.xml"));
			foreach (var record in insurance.allShips)
			{
				if (output.ContainsKey(record.shipEntityClassName)) continue; // Fix for Redeemer appearing twice

				output.Add(record.shipEntityClassName, new StandardisedInsurance
				{
					StandardClaimTime = record.baseWaitTimeMinutes,
					ExpeditedClaimTime = record.mandatoryWaitTimeMinutes,
					ExpeditedCost = record.baseExpeditingFee
				});
			}

			return output;
		}

		T Parse<T>(string xmlFilename)
		{
			string rootNodeName;
			using (var reader = XmlReader.Create(new StreamReader(xmlFilename)))
			{
				reader.MoveToContent();
				rootNodeName = reader.Name;
			}

			var xml = File.ReadAllText(xmlFilename);
			var doc = new XmlDocument();
			doc.LoadXml(xml);

			var serialiser = new XmlSerializer(typeof(T), new XmlRootAttribute { ElementName = rootNodeName });
			using (var stream = new XmlNodeReader(doc))
			{
				var entity = (T)serialiser.Deserialize(stream);
				return entity;
			}
		}
	}
}
