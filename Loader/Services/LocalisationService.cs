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
			if (string.IsNullOrWhiteSpace(label))
			{
				return null;
			}

			var key = label[0] == '@' ? label[1..] : label;

			return _translationService.Translations.GetValueOrDefault(key.Trim());
		}
	}
}
