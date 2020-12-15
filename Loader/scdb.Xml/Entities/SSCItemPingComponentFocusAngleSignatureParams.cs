using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SSCItemPingComponentFocusAngleSignatureParams
	{
		[XmlArray]
		public Single[] signatureMultipliers;
	}

}
