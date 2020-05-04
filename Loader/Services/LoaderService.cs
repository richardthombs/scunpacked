using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Loader.Entries;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Loader.Services
{
	internal abstract class LoaderService<T> where T : ILoaderItem
	{
		protected readonly ILogger<LoaderService<T>> Logger;
		protected readonly ServiceOptions Options;

		private Dictionary<string, T> _items;

		protected LoaderService(ILogger<LoaderService<T>> logger, IOptions<ServiceOptions> options)
		{
			Logger = logger;
			Options = options.Value;
		}

		public Dictionary<string, T> Items
		{
			get
			{
				_items ??= GetItems();
				return _items;
			}
		}

		private Dictionary<string, T> GetItems()
		{
			var items = LoadItems().GetAwaiter().GetResult();
			return items.ToDictionary(i => i.Id.ToString());
		}

		protected abstract string FileName { get; }

		protected abstract Task<List<T>> LoadItems();

		public virtual async Task LoadItems(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return;
			}

			var output = Path.Combine(Options.Output, FileName);
			Logger.LogDebug("{0} in {1}", nameof(WriteItems), output);
			var json = await File.ReadAllTextAsync(output, cancellationToken);
			var items = JsonConvert.DeserializeObject<List<T>>(json);
			_items = items.ToDictionary(i => i.Id.ToString());
		}

		public virtual Task WriteItems(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.CompletedTask;
			}

			var output = Path.Combine(Options.Output, FileName);
			Logger.LogDebug("{0} in {1}", nameof(WriteItems), output);
			return File.WriteAllTextAsync(output, JsonConvert.SerializeObject(Items.Values), cancellationToken);
		}
	}
}
