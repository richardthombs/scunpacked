using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Vehicles
{
	public class AngleRange
	{
		[XmlAttribute]
		public int min;

		[XmlAttribute]
		public int max;
	}
}
