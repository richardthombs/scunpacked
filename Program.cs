namespace shipparser
{
	class Program
	{
		static void Main(string[] args)
		{
			var scDataRoot = @"c:\dev\scdata\3.7.2";

			var shipLoader = new ShipLoader
			{
				OutputFolder = @".\json\ships",
				DataRoot = scDataRoot
			};
			shipLoader.Load();

			var itemLoader = new ItemLoader
			{
				OutputFolder = @".\json\items",
				DataRoot = scDataRoot
			};
			itemLoader.Load();
		}
	}
}
