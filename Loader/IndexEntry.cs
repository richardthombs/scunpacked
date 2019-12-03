namespace Loader
{
	public class IndexEntry
	{
		public string json;
		public string @class;
		public string item;
		public string kind;
		public string Type;
		public string SubType;
	}

	public class ShipIndexEntry : IndexEntry
	{
		public ShipHeadlines Headlines;
	}
}
