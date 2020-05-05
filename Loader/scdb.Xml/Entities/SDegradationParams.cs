using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SDegradationParams
	{
		[XmlAttribute]
		public bool DegradeOnlyWhenAttached;

		[XmlAttribute]
		public double InitialAgeRatio;

		[XmlAttribute]
		public double MaxLifetimeHours;
	}
}
