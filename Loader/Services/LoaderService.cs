using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Loader.Entries;
using Loader.Helper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Loader.Services
{
	internal abstract class LoaderService<T> where T : ILoaderItem
	{
		protected readonly IJsonFileReaderWriter JsonFileReaderWriter;

		protected readonly ILogger<LoaderService<T>> Logger;

		protected readonly ServiceOptions Options;

		private Dictionary<string, T> _items;

		protected LoaderService(ILogger<LoaderService<T>> logger, IOptions<ServiceOptions> options,
		                        IJsonFileReaderWriter jsonFileReaderWriter)
		{
			Logger = logger;
			JsonFileReaderWriter = jsonFileReaderWriter;
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

		protected abstract string FileName { get; }

		private Dictionary<string, T> GetItems()
		{
			var items = LoadItems().GetAwaiter().GetResult();
			return items.ToDictionary(i => i.Id.ToString());
		}

		protected abstract Task<List<T>> LoadItems();

		public virtual async Task LoadItemsFromJsonFile(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return;
			}

			var input = Path.Combine(Options.Output, FileName);
			var items = await JsonFileReaderWriter.ReadFile<List<T>>(input, cancellationToken);
			_items = items.ToDictionary(i => i.Id.ToString());
		}

		public virtual Task WriteItems(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.CompletedTask;
			}

			var output = Path.Combine(Options.Output, FileName);
			return JsonFileReaderWriter.WriteFile(output, () => _items.Values, cancellationToken);
		}
	}
}
