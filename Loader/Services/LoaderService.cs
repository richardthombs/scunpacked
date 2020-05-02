using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Loader.Services
{
	internal abstract class LoaderService<T>
	{
		protected readonly ILogger<LoaderService<T>> Logger;
		protected readonly ServiceOptions Options;

		private List<T> _items;

		protected LoaderService(ILogger<LoaderService<T>> logger, IOptions<ServiceOptions> options)
		{
			Logger = logger;
			Options = options.Value;
		}

		public List<T> Items
		{
			get
			{
				_items ??= LoadItems().GetAwaiter().GetResult();
				return _items;
			}
		}

		protected abstract string FileName { get; }

		protected abstract Task<List<T>> LoadItems();

		public virtual Task WriteItems(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.CompletedTask;
			}

			var output = Path.Combine(Options.Output, FileName);
			Logger.LogDebug("{0} in {1}", nameof(WriteItems), output);
			return File.WriteAllTextAsync(output, JsonConvert.SerializeObject(Items), cancellationToken);
		}
	}
}
