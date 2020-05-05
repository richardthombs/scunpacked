using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SCItemQuantumDriveParams
	{
		[XmlAttribute]
		public double disconnectRange;

		public QuantumDriveHeatParams heatParams;

		[XmlAttribute]
		public double jumpRange;

		public SQuantumDriveParams @params;

		[XmlAttribute]
		public double quantumFuelRequirement;

		public SQuantumDriveParams splineJumpParams;

		[XmlAttribute]
		public string tracePoint;
	}
}
