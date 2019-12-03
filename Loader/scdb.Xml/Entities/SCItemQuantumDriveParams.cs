using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemQuantumDriveParams
	{
		[XmlAttribute]
		public string tracePoint;

		[XmlAttribute]
		public double quantumFuelRequirement;

		[XmlAttribute]
		public double jumpRange;

		[XmlAttribute]
		public double disconnectRange;

		public SQuantumDriveParams @params;
		public SQuantumDriveParams splineJumpParams;
		public QuantumDriveHeatParams heatParams;
	}
}