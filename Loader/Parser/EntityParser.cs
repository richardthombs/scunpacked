//-----------------------------------------------------------------------
// <copyright file="D:\projekte\scunpacked\Loader\EntityParser.cs" company="primsoft.NET">
// Author: Joerg Primke
// Copyright (c) primsoft.NET. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Loader.SCDb.Xml.Entities;
using Microsoft.Extensions.Logging;

namespace Loader.Parser
{
	public class EntityParser
	{
		private static readonly ConcurrentDictionary<string, string> FilenameToClassMap =
			new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		private static readonly ConcurrentDictionary<string, EntityClassDefinition> Cache =
			new ConcurrentDictionary<string, EntityClassDefinition>(StringComparer.OrdinalIgnoreCase);

		private readonly ILogger<EntityParser> _logger;

		public EntityParser(ILogger<EntityParser> logger)
		{
			_logger = logger;
		}

		public async Task<EntityClassDefinition> Parse(string fullXmlPath, Func<string, Task<string>> onXmlLoadout)
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

			var entity = await ParseEntityDefinition(fullXmlPath, onXmlLoadout);
			FilenameToClassMap.TryAdd(fullXmlPath, entity.ClassName);
			Cache.TryAdd(entity.ClassName, entity);

			return entity;
		}

		private async Task<EntityClassDefinition> ParseEntityDefinition(
			string shipEntityPath, Func<string, Task<string>> onXmlLoadout)
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

			var serialiser = new XmlSerializer(typeof(EntityClassDefinition),
			                                   new XmlRootAttribute {ElementName = rootNodeName});

			using var stream = new XmlNodeReader(doc);
			try
			{
				var entity = (EntityClassDefinition) serialiser.Deserialize(stream);
				entity.ClassName = className;

				if (entity.Components?.SEntityComponentDefaultLoadoutParams?.loadout?.SItemPortLoadoutXMLParams != null)
				{
					entity.Components.SEntityComponentDefaultLoadoutParams.loadout.SItemPortLoadoutXMLParams.loadoutPath
						= await onXmlLoadout(entity.Components.SEntityComponentDefaultLoadoutParams.loadout
						                           .SItemPortLoadoutXMLParams.loadoutPath);
				}

				return entity;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error in {0}", nameof(ParseEntityDefinition));
				throw;
			}
		}
	}
}
