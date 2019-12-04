using System.Collections.Generic;

namespace scdb.Models
{
	public class ItemPort
	{
		public string port;
		public string item;
		public int? minsize;
		public int? maxsize;
		public Dictionary<string, bool> flags;
		public Dictionary<string, bool> types;
	}
}
