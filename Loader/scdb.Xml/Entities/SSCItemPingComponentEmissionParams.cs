using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SSCItemPingComponentEmissionParams
	{
		[XmlAttribute]
		public double boostDiminishTime;

		[XmlArray]
		public Single[] signatureAdditions;
	}

}
