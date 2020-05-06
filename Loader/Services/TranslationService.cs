using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Loader.Helper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Loader.Services
{
	internal class TranslationService
	{
		private readonly ILogger<TranslationService> _logger;

		private readonly IJsonFileReaderWriter _jsonFileReaderWriter;

		private readonly ServiceOptions _options;

		private Dictionary<string, string> _translations;

		public TranslationService(ILogger<TranslationService> logger, IOptions<ServiceOptions> options,
		                          IJsonFileReaderWriter jsonFileReaderWriter)
		{
			_logger = logger;
			_jsonFileReaderWriter = jsonFileReaderWriter;
			_options = options.Value;
		}

		public Dictionary<string, string> Translations
		{
			get
			{
				_translations ??= LoadTranslations().GetAwaiter().GetResult();
				return _translations;
			}
		}

		private async Task<Dictionary<string, string>> LoadTranslations()
		{
			_logger.LogDebug(nameof(LoadTranslations));
			var allLines =
				await File.ReadAllLinesAsync(Path.Combine(_options.SCData, @"Data\Localization\english\global.ini"));
			return allLines.Select(line =>
			                       {
				                       var firstIndexOfEqual = line.IndexOf('=');
				                       var key = line[..firstIndexOfEqual];
				                       var value = line[(firstIndexOfEqual + 1)..];
				                       return (Key: key.Trim(), Value: value.Trim());
			                       })
			               .ToDictionary(parts => parts.Key, parts => parts.Value, StringComparer.OrdinalIgnoreCase);
		}

		public Task WriteTranslations()
		{
			_logger.LogDebug(nameof(WriteTranslations));
			return _jsonFileReaderWriter.WriteFile(Path.Combine(_options.Output, "labels.json"), () => Translations);
		}
	}
}
