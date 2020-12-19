using System.Collections.Generic;

namespace Loader
{
	public class InsuranceService
	{
		Dictionary<string, StandardisedInsurance> prices;

		public InsuranceService(Dictionary<string, StandardisedInsurance> prices)
		{
			this.prices = prices;
		}

		public StandardisedInsurance GetInsurance(string className)
		{
			if (prices.ContainsKey(className)) return prices[className];
			return null;
		}
	}
}
