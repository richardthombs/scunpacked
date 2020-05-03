using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Loader.Entries;
using Loader.Parser;
using Loader.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Loader
{
	internal class MainService : IHostedService
	{
		private readonly IHostApplicationLifetime _applictionLifetime;
		private readonly ILogger<MainService> _logger;
		private readonly ServiceOptions _options;
		private readonly IServiceProvider _serviceProvider;

		public MainService(ILogger<MainService> logger, IOptions<ServiceOptions> config,
		                   IHostApplicationLifetime applicationLifetime, IServiceProvider serviceProvider)
		{
			_logger = logger;
			_options = config.Value;
			_applictionLifetime = applicationLifetime;
			_serviceProvider = serviceProvider;
		}

		private CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();
		private TaskCompletionSource<bool> TaskCompletionSource { get; } = new TaskCompletionSource<bool>();

		public Task StartAsync(CancellationToken cancellationToken)
		{
			// Start our application code.
			Task.Run(() => DoWork(CancellationTokenSource.Token));
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			CancellationTokenSource.Cancel();
			return TaskCompletionSource.Task;
		}

		public async Task DoWork(CancellationToken cancellationToken)
		{
			try
			{
				if (OptionsAreOk())
				{
					if (_options.ItemFile != null)
					{
						var entityParser = _serviceProvider.GetService<EntityParser>();
						var entity = entityParser.Parse(_options.ItemFile, Task.FromResult);
						var json = JsonConvert.SerializeObject(entity);
						Console.WriteLine(json);
					}
					else
					{
						if (!cancellationToken.IsCancellationRequested)
						{
							CreateOrCleanupOutputDirectory();

							var manufacturerService = _serviceProvider.GetService<LoaderService<Manufacturer>>();
							await manufacturerService.WriteItems(cancellationToken);

							var shipsService = _serviceProvider.GetService<LoaderService<Ship>>();
							await shipsService.WriteItems(cancellationToken);

							var itemService = _serviceProvider.GetService<LoaderService<Item>>();
							await itemService.WriteItems(cancellationToken);

							var retailsService = _serviceProvider.GetService<LoaderService<RetailProduct>>();
							await retailsService.WriteItems(cancellationToken);

							var shopService = _serviceProvider.GetService<LoaderService<Shop>>();
							await shopService.WriteItems(cancellationToken);
						}
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, "Exception occured");
			}
			
			_logger.LogInformation("Stopping");
			TaskCompletionSource.SetResult(true);
			_applictionLifetime.StopApplication();
		}

		private bool OptionsAreOk()
		{
			var badArgs = false;
			if (!string.IsNullOrEmpty(_options.ItemFile) &&
			    (!string.IsNullOrEmpty(_options.SCData) || !string.IsNullOrEmpty(_options.Output)))
			{
				badArgs = true;
			}
			else if (string.IsNullOrEmpty(_options.ItemFile) &&
			         (string.IsNullOrEmpty(_options.SCData) || string.IsNullOrEmpty(_options.Output)))
			{
				badArgs = true;
			}

			if (!badArgs)
			{
				return true;
			}

			Console.WriteLine("Usage:");
			Console.WriteLine("    Loader.exe -input=<path to extracted Star Citizen data> -output=<path to JSON output folder>");
			Console.WriteLine(" or Loader.exe -itemfile=<path to an SCItem XML file");
			Console.WriteLine();

			return false;
		}

		private void CreateOrCleanupOutputDirectory()
		{
			if (Directory.Exists(_options.Output))
			{
				Directory.Delete(_options.Output, true);
			}

			Directory.CreateDirectory(_options.Output);
		}
	}
}
