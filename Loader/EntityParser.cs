//-----------------------------------------------------------------------
// <copyright file="D:\projekte\scunpacked\Loader\EntityParser.cs" company="primsoft.NET">
// Author: Joerg Primke
// Copyright (c) primsoft.NET. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Loader.SCDb.Xml.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;

namespace Loader
{
	public class EntityParser
	{
		private readonly ILogger<EntityParser> _logger;

		private static readonly Dictionary<string, string> FilenameToClassMap =
			new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		private static readonly Dictionary<string, EntityClassDefinition> Cache =
			new Dictionary<string, EntityClassDefinition>(StringComparer.OrdinalIgnoreCase);

		public EntityParser(ILogger<EntityParser> logger)
		{
			_logger = logger;
		}

		public EntityClassDefinition Parse(string fullXmlPath, Func<string, string> onXmlLoadout)
		{
			if (FilenameToClassMap.ContainsKey(fullXmlPath))
			{
				var className = FilenameToClassMap[fullXmlPath];
				var cached = Cache[className];
				_logger.LogInformation($"Cached {className}");
				return cached;
			}

			_logger.LogInformation(fullXmlPath);
			if (!File.Exists(fullXmlPath))
			{
				_logger.LogWarning("Entity definition file does not exist");
				return null;
			}

			var entity = ParseEntityDefinition(fullXmlPath, onXmlLoadout);
			FilenameToClassMap.Add(fullXmlPath, entity.ClassName);
			Cache.Add(entity.ClassName, entity);

			return entity;
		}

		private static EntityClassDefinition ParseEntityDefinition(string shipEntityPath, Func<string, string> onXmlLoadout)
		{
			string rootNodeName;
			using (var reader = XmlReader.Create(new StreamReader(shipEntityPath)))
			{
				reader.MoveToContent();
				rootNodeName = reader.Name;
			}

			var split = rootNodeName.Split('.');
			var className = split[^1];

			var xml = File.ReadAllText(shipEntityPath);
			var doc = new XmlDocument();
			doc.LoadXml(xml);

			var serialiser =
				new XmlSerializer(typeof(EntityClassDefinition), new XmlRootAttribute { ElementName = rootNodeName });

			using var stream = new XmlNodeReader(doc);
			var entity = (EntityClassDefinition) serialiser.Deserialize(stream);
			entity.ClassName = className;

			if (entity.Components?.SEntityComponentDefaultLoadoutParams?.loadout?.SItemPortLoadoutXMLParams != null)
			{
				entity.Components.SEntityComponentDefaultLoadoutParams.loadout.SItemPortLoadoutXMLParams.loadoutPath =
					onXmlLoadout(entity.Components.SEntityComponentDefaultLoadoutParams.loadout
					                   .SItemPortLoadoutXMLParams.loadoutPath);
			}

			return entity;
		}
	}
}
