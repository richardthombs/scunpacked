using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Linq2XmlGame
{
	internal class Program
	{
		public static async Task Main(string[] args)
		{
			var host = Host.CreateDefaultBuilder(args) 
			               .ConfigureServices((hostContext, services) => { services.AddHostedService<MyService>(); })
			               .Build();

			await host.RunAsync();
		}
	}

	internal class MyService : IHostedService
	{
		private readonly IHostApplicationLifetime _applictionLifetime;

		private readonly ILogger<MyService> _logger;

		private const string XmlFilename = @"D:\projekte\unp4k\src\unp4k\bin\Debug\net47\win-x64\Data\Libs\Subsumption\Shops\RetailProductPrices.xml";

		public MyService(ILogger<MyService> logger, IHostApplicationLifetime applicationLifetime)
		{
			_logger = logger;
			_applictionLifetime = applicationLifetime;
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
			// Defer completion promise, until our application has reported it is done.
			return TaskCompletionSource.Task;
		}

		public Task DoWork(CancellationToken cancellationToken)
		{
			var document = XElement.Load(XmlFilename);

			var paths = document.Descendants("Node")
			                        .Select(n => n.Attribute("Filename")?.Value ?? "Unknown")
			                        .Select(fn => Path.GetRelativePath(@"Data\Libs\Foundry\Records\Entities",
			                                                           Path.GetDirectoryName(fn)))
			                        .Distinct();
			foreach (var path in paths)
			{
				Console.WriteLine(path);
			}

			_logger.LogInformation("Stopping");
			TaskCompletionSource.SetResult(true);
			_applictionLifetime.StopApplication();
			return Task.CompletedTask;
		}
	}
}
