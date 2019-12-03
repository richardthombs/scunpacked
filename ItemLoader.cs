using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

using Newtonsoft.Json;

using scdb.Xml.Turbulent;

namespace shipparser
{
	public class ItemLoader
	{
		public string OutputFolder { get; set; }
		public string DataRoot { get; set; }

		public List<IndexEntry> Load()
		{
			var itemsFolder = Path.Combine(DataRoot, @"Data\Libs\Foundry\Records\entities\scitem\ships");
			var index = new List<IndexEntry>();

			foreach (var folder in Directory.EnumerateDirectories(itemsFolder))
			{
				Console.WriteLine(Path.GetFileNameWithoutExtension(folder));
				var itemType = Path.GetFileNameWithoutExtension(folder);
				var outputFolder = /*OutputFolder; //*/Path.Combine(OutputFolder, itemType);
				Directory.CreateDirectory(outputFolder);

				foreach (var filename in Directory.EnumerateFiles(folder, "*.xml", SearchOption.AllDirectories))
				{
					Console.WriteLine(filename);
					var entityParser = new EntityParser();
					var entity = entityParser.Parse(filename);

					var json = JsonConvert.SerializeObject(entity, Newtonsoft.Json.Formatting.Indented);
					var jsonFilename = Path.Combine(outputFolder, $"{entity.ClassName.ToLower()}.json");
					File.WriteAllText(jsonFilename, json);

					index.Add(new IndexEntry
					{
						@class = entity.ClassName,
						item = entity.ClassName.ToLower(),
						json = Path.GetRelativePath(Path.GetDirectoryName(OutputFolder), jsonFilename),
						kind = itemType,
						Type = entity.Components?.SAttachableComponentParams?.AttachDef.Type,
						SubType = entity.Components?.SAttachableComponentParams?.AttachDef.SubType
					});
				}

			}

			return index;
		}

		TurbulentEntry GetTurbulentEntry(string turbulentXmlFile)
		{
			var rootNode = Path.GetFileNameWithoutExtension(turbulentXmlFile);
			rootNode = rootNode.Replace("igp", "IGP");
			rootNode = $"TurbulentEntry.{rootNode}";

			var xml = File.ReadAllText(turbulentXmlFile);
			var doc = new XmlDocument();
			doc.LoadXml(xml);

			var serialiser = new XmlSerializer(typeof(TurbulentEntry), new XmlRootAttribute { ElementName = rootNode });
			using (var stream = new XmlNodeReader(doc))
			{
				var entry = (TurbulentEntry)serialiser.Deserialize(stream);
				return entry;
			}
		}
	}
}
