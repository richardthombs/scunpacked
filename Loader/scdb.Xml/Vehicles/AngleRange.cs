using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Vehicles
{
	public class AngleRange
	{
		[XmlAttribute]
		public int max;

		[XmlAttribute]
		public int min;
	}
}
