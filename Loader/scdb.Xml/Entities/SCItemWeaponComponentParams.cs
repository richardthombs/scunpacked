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
		public SWeaponActionFireBeamParams SWeaponActionFireBeamParams;
		public SWeaponActionFireChargedParams SWeaponActionFireChargedParams;
	}

	public class SWeaponActionFireSingleParams
	{
		[XmlAttribute]
		public string name;

		[XmlAttribute]
		public string localisedName;

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

	public class SWeaponActionSequenceParams
	{
		[XmlAttribute]
		public string name;

		[XmlAttribute]

		public string localisedName;
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

	public class SWeaponActionFireBeamParams
	{
		[XmlAttribute]
		public string name;

		[XmlAttribute]
		public string localisedName;

		[XmlAttribute]
		public string aiShootingMode;

		[XmlAttribute]
		public string fireHelper;

		[XmlAttribute]
		public double toggle;

		[XmlAttribute]
		public double energyRate;

		[XmlAttribute]
		public double fullDamageRange;

		[XmlAttribute]
		public double zeroDamageRange;

		[XmlAttribute]
		public double heatPerSecond;

		[XmlAttribute]
		public string hitType;

		[XmlAttribute]
		public double hitRadius;

		public DamagePerSecond damagePerSecond;
	}

	public class SWeaponActionFireChargedParams
	{
		[XmlAttribute]
		public string name;

		[XmlAttribute]
		public string localisedName;

		[XmlAttribute]
		public string aiShootingMode;

		[XmlAttribute]
		public double chargeTime;

		[XmlAttribute]
		public double overchargeTime;

		[XmlAttribute]
		public double overchargedTime;

		[XmlAttribute]
		public double cooldownTime;

		public fireActions weaponAction;
	}

	public class DamagePerSecond
	{
		public DamageInfo DamageInfo;
	}
}
