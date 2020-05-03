//-----------------------------------------------------------------------
// <copyright file="D:\projekte\scunpacked\Tests\GenericHostTest\Program.cs" company="primsoft.NET">
// Author: Joerg Primke
// Copyright (c) primsoft.NET. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GenericHostTest
{
	public class MyServiceOptions
	{
		public string SCData { get; set; }
		public string Output { get; set; }
		public string ItemFile { get; set; }

		public bool WriteRawJsonFiles { get; set; } = true;
	}

	internal class Program
	{
		public static async Task Main(string[] args)
		{
			var switchMappings = new Dictionary<string, string>()
			                     {
				                     {"-scdata", "SCData"},
				                     {"-input", "SCData"},
				                     {"-output", "Output"},
				                     {"-itemfile", "ItemFile"},
				                     {"--scdata", "SCData"},
				                     {"--input", "SCData"},
				                     {"--output", "Output"},
				                     {"--itemfile", "ItemFile"},
				                     {"--writeJson", "WriteRawJsonFiles"},
									 {"-writeJson", "WriteRawJsonFiles"}
								 };

			var host = Host.CreateDefaultBuilder()	// without args because we can't change it later
			               .ConfigureAppConfiguration(builder => { builder.AddCommandLine(args, switchMappings); })
			               .ConfigureServices((hostContext, services) =>
			                                  {
				                                  var config = hostContext.Configuration;
				                                  services.AddHostedService<MyService>();
				                                  services.Configure<MyServiceOptions>(config);
			                                  })
			               .Build();

			await host.RunAsync();
		}
	}



	internal class MyService : IHostedService
	{
		private readonly ILogger<MyService> _logger;
		private MyServiceOptions _config;
		private readonly IHostApplicationLifetime _applictionLifetime;

		public MyService(ILogger<MyService> logger, IOptions<MyServiceOptions> config, IHostApplicationLifetime applicationLifetime)
		{
			_logger = logger;
			_config = config.Value;
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

		public async Task DoWork(CancellationToken cancellationToken)
		{
			var i = 0;
			while (!cancellationToken.IsCancellationRequested && i <= 10)
			{
				_logger.LogInformation($"Hello World - i = {i}");
				await Task.Delay(1000);
				i++;
			}

			_logger.LogInformation("Stopping");
			TaskCompletionSource.SetResult(true);
			_applictionLifetime.StopApplication();
		}
	}
}
