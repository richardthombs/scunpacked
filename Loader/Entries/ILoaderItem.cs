using System;
using System.Collections.Generic;
using System.Text;

namespace Loader.Entries
{
	public interface ILoaderItem
	{
		Guid Id { get; set; }
	}
}
