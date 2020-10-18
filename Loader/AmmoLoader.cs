using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

using scdb.Xml.Entities;

namespace Loader
{
	public class AmmoLoader
	{
		public string DataRoot { get; set; }
		public string OutputFolder { get; set; }

		public List<AmmoIndexEntry> Load()
		{
			Directory.CreateDirectory(Path.Combine(OutputFolder, "ammo"));

			var index = new List<AmmoIndexEntry>();
			index.AddRange(Load(@"Data\Libs\Foundry\Records\ammoparams\vehicle"));
			index.AddRange(Load(@"Data\Libs\Foundry\Records\ammoparams\fps"));

			File.WriteAllText(Path.Combine(OutputFolder, "ammo.json"), JsonConvert.SerializeObject(index));

			return index;
		}

		List<AmmoIndexEntry> Load(string entityFolder)
		{
			var index = new List<AmmoIndexEntry>();

			foreach (var entityFilename in Directory.EnumerateFiles(Path.Combine(DataRoot, entityFolder), "*.xml", SearchOption.AllDirectories))
			{
				Console.WriteLine(entityFilename);

				var parser = new ClassParser<AmmoParams>();
				var entity = parser.Parse(entityFilename);
				if (entity == null) continue;

				var jsonFilename = Path.Combine(OutputFolder, "ammo", $"{entity.__ref.ToLower()}.json");
				var json = JsonConvert.SerializeObject(new
				{
					Raw = new
					{
						Entity = entity,
					}
				});

				File.WriteAllText(jsonFilename, json);

				BulletProjectileParams projectiles = entity.projectileParams.BulletProjectileParams;

				var indexEntry = new AmmoIndexEntry
				{
					className = entity.ClassName,
					reference = entity.__ref,
					damage = Damage.FromDamageInfo(projectiles != null && projectiles.damage.Length > 0 ? projectiles.damage[0] : new DamageInfo()),
					speed = entity.speed,
					range = entity.lifetime * entity.speed,
					detonates = entity.projectileParams.BulletProjectileParams?.detonationParams?.ProjectileDetonationParams?.explosionParams != null,
					detonationDamage = Damage.FromDamageInfo(entity.projectileParams.BulletProjectileParams?.detonationParams?.ProjectileDetonationParams?.explosionParams?.damage[0])
				};

				index.Add(indexEntry);
			}

			return index;
		}
	}
}
