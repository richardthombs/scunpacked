namespace scdb.Xml.Entities
{
	public class DamageResistance
	{
		public DamageResistanceEntry PhysicalResistance { get; set; }
		public DamageResistanceEntry EnergyResistance { get; set; }
		public DamageResistanceEntry DistortionResistance { get; set; }
		public DamageResistanceEntry ThermalResistance { get; set; }
		public DamageResistanceEntry BiochemicalResistance { get; set; }
		public DamageResistanceEntry StunResistance { get; set; }
	}
}