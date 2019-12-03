using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SDegradationParams
	{
		[XmlAttribute]
		public double MaxLifetimeHours;

		[XmlAttribute]
		public double InitialAgeRatio;

		[XmlAttribute]
		public bool DegradeOnlyWhenAttached;
	}
}