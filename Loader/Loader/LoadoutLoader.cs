using System.IO;
using System.Threading.Tasks;
using Loader.Helper;
using Loader.Parser;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Loader.Loader
{
	internal class LoadoutLoader
	{
		private readonly DefaultLoadoutParser _defaultLoadoutParser;

		private readonly IJsonFileReaderWriter _jsonFileReaderWriter;

		private readonly ILogger<LoadoutLoader> _logger;

		private readonly ServiceOptions _options;

		public LoadoutLoader(ILogger<LoadoutLoader> logger, IOptions<ServiceOptions> options,
		                     IJsonFileReaderWriter jsonFileReaderWriter, DefaultLoadoutParser defaultLoadoutParser)
		{
			_logger = logger;
			_jsonFileReaderWriter = jsonFileReaderWriter;
			_defaultLoadoutParser = defaultLoadoutParser;
			_options = options.Value;
		}

		public string OutputFolder => Path.Combine(_options.Output, "loadouts");

		public string DataRoot => _options.SCData;

		public Task<string> Load(string loadoutXmlPath)
		{
			_logger.LogDebug("{0} with {1}", nameof(Load), loadoutXmlPath);

			Directory.CreateDirectory(OutputFolder);

			if (string.IsNullOrWhiteSpace(loadoutXmlPath))
			{
				return Task.FromResult("");
			}

			var windowsPath = loadoutXmlPath.Replace("/", "\\");
			_logger.LogInformation(windowsPath);

			var loadout = _defaultLoadoutParser.Parse(Path.Combine(DataRoot, "Data", windowsPath));

			var jsonFilename = Path.Combine(OutputFolder, $"{Path.GetFileNameWithoutExtension(loadoutXmlPath)}.json");
			_ = _jsonFileReaderWriter.WriteFile(jsonFilename, () => loadout);

			return Task.FromResult(Path.GetRelativePath(OutputFolder, jsonFilename));
		}
	}
}
