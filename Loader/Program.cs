//-----------------------------------------------------------------------
// <copyright file="D:\projekte\scunpacked\Loader\Program.cs" company="primsoft.NET">
// Author: Joerg Primke
// Copyright (c) primsoft.NET. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Loader.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Loader
{
	internal class Program
	{
		private static async Task Main(string[] args)
		{
			JsonConvert.DefaultSettings = () => new JsonSerializerSettings
			                                    {
				                                    Formatting = Formatting.Indented,
				                                    NullValueHandling = NullValueHandling.Ignore
			                                    };

			var switchMappings = new Dictionary<string, string>
			                     {
				                     {"-scdata", "SCData"},
				                     {"-input", "SCData"},
				                     {"-output", "Output"},
				                     {"-itemfile", "ItemFile"},
				                     {"--scdata", "SCData"},
				                     {"--input", "SCData"},
				                     {"--output", "Output"},
				                     {"--itemfile", "ItemFile"}
			                     };

			var host = Host.CreateDefaultBuilder() // WARNING without args! because we can't change it later
			               .ConfigureAppConfiguration(builder => { builder.AddCommandLine(args, switchMappings); })
			               .ConfigureServices((hostContext, services) =>
			                                  {
				                                  var config = hostContext.Configuration;
				                                  services.AddHostedService<MainService>();
				                                  services.AddLoaderServices();
				                                  services.Configure<ServiceOptions>(config);
			                                  })
			               .Build();

			await host.RunAsync();
		}
	}
}
