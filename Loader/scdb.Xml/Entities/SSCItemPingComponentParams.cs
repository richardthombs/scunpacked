using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SSCItemPingComponentParams
	{
		[XmlAttribute]
		public double maximumChargeTime;

		[XmlAttribute]
		public double maximumCooldownTime;

		[XmlAttribute]
		public string contactBlobParams;

		public SSCItemPingComponentEmissionParams emissionParams;
		public SSCItemPingComponentQueryParams queryParams;

		[XmlArray]
		public SSCItemPingComponentFocusAngleSignatureParams[] pingFocusAngleParams;
	}

}
