using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SSCItemPingComponentQueryParams
	{
		[XmlAttribute]
		public double queryAcceleration;

		[XmlAttribute]
		public string defaultAngle;
	}

}
