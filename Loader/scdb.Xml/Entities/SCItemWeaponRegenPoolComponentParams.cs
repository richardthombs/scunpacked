using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemWeaponRegenPoolComponentParams
	{
		[XmlAttribute]
		public double regenFillRate;

		[XmlAttribute]
		public int ammoLoad;

		[XmlAttribute]
		public bool respectsCapacitorAssignments;

		[XmlAttribute]
		public string capacitorAssignmentInputOutputRegen;

		[XmlAttribute]
		public string capacitorAssignmentInputOutputAmmoLoad;
	}
}
