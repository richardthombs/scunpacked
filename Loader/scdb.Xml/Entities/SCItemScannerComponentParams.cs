using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemScannerComponentParams
	{
		[XmlAttribute]
		public string scanGroupFilter;

		[XmlAttribute]
		public double scanRange;
	}
}
