using System.Collections.Generic;

namespace Loader.Services
{
	internal class LocalisationService
	{
		private readonly TranslationService _translationService;

		public LocalisationService(TranslationService translationService)
		{
			_translationService = translationService;
		}

		public string GetText(string label)
		{
			var key = label.StartsWith("@") ? label.Substring(1) : label;

			return _translationService.Translations.GetValueOrDefault(key);
		}
	}
}
