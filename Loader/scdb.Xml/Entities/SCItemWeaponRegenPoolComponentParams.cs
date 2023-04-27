using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemWeaponRegenPoolComponentParams
	{
		[XmlAttribute]
		public double regenFillRate;

		public string _ammoLoadString;
		[XmlAttribute("ammoLoad")]
		public string ammoLoadString
		{
			get => _ammoLoadString;
			set
			{
				if(int.TryParse(value, System.Globalization.NumberStyles.AllowExponent, System.Globalization.NumberFormatInfo.InvariantInfo, out int parsedAmmoLoadValue))
				{
					ammoLoad = parsedAmmoLoadValue;
				}
			}
		}

		[XmlIgnore]
		public int ammoLoad;

		[XmlAttribute]
		public bool respectsCapacitorAssignments;

		[XmlAttribute]
		public string capacitorAssignmentInputOutputRegen;

		[XmlAttribute]
		public string capacitorAssignmentInputOutputAmmoLoad;
	}
}
