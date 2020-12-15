using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemScannerComponentParams
	{
		[XmlAttribute]
		public string scanGroupFilter;

		[XmlAnyAttribute]
		public double scanRange;
	}

	public class SGlobalContactBlobParams
	{

	}
}
