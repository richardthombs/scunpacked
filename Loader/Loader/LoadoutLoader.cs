using System.IO;
using System.Threading.Tasks;
using Loader.Parser;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Loader.Loader
{
	public class LoadoutLoader
	{
		private readonly ILogger<LoadoutLoader> _logger;
		private readonly DefaultLoadoutParser _defaultLoadoutParser;
		private readonly ServiceOptions _options;

		public LoadoutLoader(ILogger<LoadoutLoader> logger, IOptions<ServiceOptions> options, DefaultLoadoutParser defaultLoadoutParser)
		{
			_logger = logger;
			_defaultLoadoutParser = defaultLoadoutParser;
			_options = options.Value;
		}

		public string OutputFolder => _options.Output;
		public string DataRoot => _options.SCData;

		public async Task<string> Load(string loadoutXmlPath)
		{
			_logger.LogDebug("{0} with {1}", nameof(Load), loadoutXmlPath);

			Directory.CreateDirectory(OutputFolder);

			if (string.IsNullOrWhiteSpace(loadoutXmlPath))
			{
				return "";
			}

			var windowsPath = loadoutXmlPath.Replace("/", "\\");
			_logger.LogInformation(windowsPath);

			var loadout = _defaultLoadoutParser.Parse(Path.Combine(DataRoot, "Data", windowsPath));

			var jsonFilename = Path.Combine(OutputFolder, $"{Path.GetFileNameWithoutExtension(loadoutXmlPath)}.json");
			if (_options.WriteRawJsonFiles)
			{
				var json = JsonConvert.SerializeObject(loadout);
				await File.WriteAllTextAsync(jsonFilename, json);
			}

			return Path.GetRelativePath(OutputFolder, jsonFilename);
		}
	}
}
