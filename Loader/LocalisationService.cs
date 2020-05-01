using System;
using System.Collections.Generic;

namespace Loader
{
	public class LocalisationService
	{
		Dictionary<string, string> labels;

		public LocalisationService(Dictionary<string, string> labels)
		{
			this.labels = labels;
		}

		public string GetText(string label)
		{
			var key = label.StartsWith("@") ? label.Substring(1) : label;

			if (labels.ContainsKey(key)) return labels[key];

			return null;
		}
	}
}
