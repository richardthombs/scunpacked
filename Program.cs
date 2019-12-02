namespace shipparser
{
	class Program
	{
		static void Main(string[] args)
		{
			var outputFolder = @".\json";
			var scDataRoot = @"c:\dev\scdata\3.7.2";

			var shipLoader = new ShipLoader
			{
				OutputFolder = outputFolder,
				DataRoot = scDataRoot
			};
			shipLoader.Load();
		}
	}
}
