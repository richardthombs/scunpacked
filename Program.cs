using System.IO;
using System.Globalization;

using scdb.Xml.Entities;
using scdb.Xml.Loadouts;
using scdb.Xml.Vehicles;

namespace shipparser
{
	class ShipIndex
	{
		public string filename;
		public string itemClass;
		public string turbulentName;
	}

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

	public class Ship
	{
		public EntityClassDefinition Entity { get; set; }
		public Vehicle Vehicle { get; set; }
		public Loadout DefaultLoadout { get; set; }
		public scdb.Models.loadout[] loadout { get; set; }
	}
}
