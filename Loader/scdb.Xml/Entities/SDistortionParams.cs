using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SDistortionParams
	{
		[XmlAttribute]
		public double DecayRate;

		[XmlAttribute]
		public double Maximum;

		[XmlAttribute]
		public double OverloadRatio;

		[XmlAttribute]
		public double RecoveryRatio;

		[XmlAttribute]
		public double RecoveryTime;
	}
}
