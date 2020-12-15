using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SSCItemScannerComponentScanGroupFilter
	{
		[XmlArray]
		public Reference[] positiveTags;

		[XmlArray]
		public Reference[] negativeTags;
	}
}
