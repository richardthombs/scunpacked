using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class BulletProjectileParams
	{
		public DamageInfo[] damage;
		public DetonationParams detonationParams;
	}

	public class DetonationParams
	{
		public ProjectileDetonationParams ProjectileDetonationParams;
	}

	public class ProjectileDetonationParams
	{
		[XmlAttribute]
		double armTime;

		[XmlAttribute]
		double safeDistance;

		[XmlAttribute]
		double destructDelay;

		[XmlAttribute]
		bool explodeOnImpact;

		[XmlAttribute]
		bool explodeOnFinalImpact;

		[XmlAttribute]
		bool explodeOnExpire;

		[XmlAttribute]
		bool explodeOnTargetRange;

		public ExplosionParams explosionParams;
	}
}
