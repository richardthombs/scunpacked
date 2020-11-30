namespace Loader
{
	public class StandardisedDamage
	{
		public double Physical { get; set; }
		public double Energy { get; set; }
		public double Distortion { get; set; }
		public double Thermal { get; set; }
		public double Biochemical { get; set; }
		public double Stun { get; set; }

		public static StandardisedDamage operator +(StandardisedDamage a, StandardisedDamage b)
			=> new StandardisedDamage
			{
				Physical = (a?.Physical ?? 0) + (b?.Physical ?? 0),
				Energy = (a?.Energy ?? 0) + (b?.Energy ?? 0),
				Distortion = (a?.Distortion ?? 0) + (b?.Distortion ?? 0),
				Thermal = (a?.Thermal ?? 0) + (b?.Thermal ?? 0),
				Biochemical = (a?.Biochemical ?? 0) + (b?.Biochemical ?? 0),
				Stun = (a?.Stun ?? 0) + (b?.Stun ?? 0)
			};

		public static StandardisedDamage operator *(StandardisedDamage a, double b)
			=> new StandardisedDamage
			{
				Physical = (a?.Physical ?? 0) * b,
				Energy = (a?.Energy ?? 0) * b,
				Distortion = (a?.Distortion ?? 0) * b,
				Thermal = (a?.Thermal ?? 0) * b,
				Biochemical = (a?.Biochemical ?? 0) * b,
				Stun = (a?.Stun ?? 0) * b
			};

		public static StandardisedDamage operator /(StandardisedDamage a, double b)
			=> new StandardisedDamage
			{
				Physical = (a?.Physical ?? 0) / b,
				Energy = (a?.Energy ?? 0) / b,
				Distortion = (a?.Distortion ?? 0) / b,
				Thermal = (a?.Thermal ?? 0) / b,
				Biochemical = (a?.Biochemical ?? 0) / b,
				Stun = (a?.Stun ?? 0) / b
			};

		public bool ShouldSerializePhysical() => Physical > 0;
		public bool ShouldSerializeEnergy() => Energy > 0;
		public bool ShouldSerializeDistortion() => Distortion > 0;
		public bool ShouldSerializeThermal() => Thermal > 0;
		public bool ShouldSerializeBiochemical() => Biochemical > 0;
		public bool ShouldSerializeStun() => Stun > 0;
	}
}
