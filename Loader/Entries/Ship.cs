using System;

namespace Loader.Entries
{
	public class Ship : ILoaderItem
	{
		public string Career { get; set; }

		public string ClassName { get; set; }

		public bool DogFightEnabled { get; set; }

		public bool IsGravlevVehicle { get; set; }

		public bool IsGroundVehicle { get; set; }

		public bool IsSpaceship { get; set; }

		public string JsonFilename { get; set; }

		public string Name { get; set; }

		public bool NoParts { get; set; }

		public string Role { get; set; }

		public int? Size { get; set; }

		public string SubType { get; set; }

		public string Type { get; set; }

		public string Description { get; set; }

		public Manufacturer Manufacturer { get; set; }

		public Guid ManufacturerId { get; set; }

		public Guid Id { get; set; }
	}
}
