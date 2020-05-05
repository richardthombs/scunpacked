using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
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
		public SWeaponActionFireBeamParams SWeaponActionFireBeamParams;

		public SWeaponActionFireRapidParams SWeaponActionFireRapidParams;

		public SWeaponActionFireSingleParams SWeaponActionFireSingleParams;

		public SWeaponActionSequenceParams SWeaponActionSequenceParams;
	}

	public class SWeaponActionFireSingleParams
	{
		[XmlAttribute]
		public double fireRate;

		[XmlAttribute]
		public double heatPerShot;

		public launchParams launchParams;

		[XmlAttribute]
		public string localisedName;

		[XmlAttribute]
		public string name;
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
		public double attack;

		[XmlAttribute]
		public double decay;

		[XmlAttribute]
		public double firstAttack;

		[XmlAttribute]
		public double max;

		[XmlAttribute]
		public double min;
	}

	public class SWeaponActionFireRapidParams : SWeaponActionFireSingleParams
	{
		[XmlAttribute]
		public double spinDownTime;

		[XmlAttribute]
		public string spinParam;

		[XmlAttribute]
		public double spinUpTime;
	}

	public class SWeaponActionSequenceParams
	{
		[XmlAttribute]
		public string localisedName;

		[XmlAttribute]
		public string mode;

		[XmlAttribute]
		public string name;

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

	public class SWeaponActionFireBeamParams
	{
		[XmlAttribute]
		public string aiShootingMode;

		public DamagePerSecond damagePerSecond;

		[XmlAttribute]
		public double energyRate;

		[XmlAttribute]
		public string fireHelper;

		[XmlAttribute]
		public double fullDamageRange;

		[XmlAttribute]
		public double heatPerSecond;

		[XmlAttribute]
		public double hitRadius;

		[XmlAttribute]
		public string hitType;

		[XmlAttribute]
		public string localisedName;

		[XmlAttribute]
		public string name;

		[XmlAttribute]
		public double toggle;

		[XmlAttribute]
		public double zeroDamageRange;
	}

	public class DamagePerSecond
	{
		public DamageInfo DamageInfo;
	}
}
