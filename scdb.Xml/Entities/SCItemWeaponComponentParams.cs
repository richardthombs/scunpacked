using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemWeaponComponentParams
	{
		[XmlAttribute]
		public string ammoContainerRecord;

		public SWeaponConnectionParams connectionParams;

		public fireActions fireActions;
	}

	public class fireActions
	{
		public SWeaponActionFireSingleParams SWeaponActionFireSingleParams;
		public SWeaponActionFireRapidParams SWeaponActionFireRapidParams;
		public SWeaponActionSequenceParams SWeaponActionSequenceParams;
	}

	public class SWeaponActionFireSingleParams : SWeaponActionParams
	{
		[XmlAttribute]
		public double fireRate;

		[XmlAttribute]
		public double heatPerShot;

		public launchParams launchParams;
	}

	public class launchParams
	{
		public SProjectileLauncher SProjectileLauncher;
	}

	public class SProjectileLauncher
	{
		[XmlAttribute]
		public double ammoCost;

		[XmlAttribute]
		public double pelletCount;

		public SSpreadParams spreadParams;
	}

	public class SSpreadParams
	{
		[XmlAttribute]
		public double min;

		[XmlAttribute]
		public double max;

		[XmlAttribute]
		public double firstAttack;

		[XmlAttribute]
		public double attack;

		[XmlAttribute]
		public double decay;
	}

	public class SWeaponActionFireRapidParams : SWeaponActionFireSingleParams
	{
		[XmlAttribute]
		public double spinUpTime;

		[XmlAttribute]
		public double spinDownTime;

		[XmlAttribute]
		public string spinParam;
	}

	public class SWeaponActionSequenceParams : SWeaponActionParams
	{
		[XmlAttribute]
		public string mode;

		public SWeaponSequenceEntryParams[] sequenceEntries;
	}

	public class SWeaponSequenceEntryParams
	{
		[XmlAttribute]
		public double delay;

		[XmlAttribute]
		public string unit;

		public fireActions weaponAction;
	}

	public class SWeaponActionParams
	{
		[XmlAttribute]
		public string name;

		[XmlAttribute]
		public string localisedName;
	}
}