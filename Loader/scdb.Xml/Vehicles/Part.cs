using System;
using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class Part
	{
		[XmlAttribute]
		public string name;

		[XmlAttribute]
		public string mass; // Because 3.8.0 PTU has got dodgy data in the mass attribute. Gladius mod has got weird unicode in it too

		[XmlAttribute]
		public double damageMax;

		[XmlAttribute]
		public double damagemax; // Cutlass Black and others have got typos

		[XmlAttribute]
		public string @class;

		[XmlAttribute]
		public bool skipPart;

		[XmlAttribute]
		public double detachRatio;

		public Part[] Parts;

		public ItemPort ItemPort;

		public DamageBehavior[] DamageBehaviors;

		[XmlAttribute]
		public double Probability;

		public bool ShouldSerializeskipPart() => skipPart;
		public bool ShouldSerializeProbability() => Probability > 0;
		public bool ShouldSerializedamageMax() => damageMax > 0;
		public bool ShouldSerializedetachRatio() => detachRatio > 0;
	}
}
