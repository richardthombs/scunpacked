using System;
using System.Xml;
using System.Xml.Serialization;

namespace shipparser
{
	public class Vehicle
	{
		[XmlAttribute]
		public string name { get; set; }

		[XmlAttribute]
		public string displayname { get; set; }

		[XmlAttribute]
		public string subType { get; set; }

		[XmlAttribute]
		public int size { get; set; }

		[XmlAttribute]
		public string requiredItemTags { get; set; }

		[XmlAttribute]
		public string itemPortTags { get; set; }

		[XmlAttribute]
		public string crossSectionMultiplier { get; set; }

		public Camera[] Cameras { get; set; }

		public Pipe[] Pipes { get; set; }

		public Part[] Parts { get; set; }
		public MovementParams MovementParams { get; set; }
	}

	public class Camera
	{
		[XmlAttribute]
		public string type { get; set; }

		[XmlAttribute]
		public string attachPoint { get; set; }

		[XmlAttribute]
		public string cameraOffset { get; set; }

		[XmlAttribute]
		public string cameraRotationOffset { get; set; }

		[XmlAttribute]
		public string cameraSecondaryOffset { get; set; }

		[XmlAttribute]
		public string filename { get; set; }
	}

	public class Pipe
	{
		[XmlAttribute]
		public string name { get; set; }

		[XmlAttribute]
		public string @class { get; set; }
	}


	public class Part
	{
		[XmlAttribute]
		public string name { get; set; }

		[XmlAttribute]
		public double mass { get; set; }

		[XmlAttribute]
		public double damageMax { get; set; }

		[XmlAttribute]
		public string @class { get; set; }

		public Part[] Parts { get; set; }

		public ItemPort ItemPort { get; set; }

		override public string ToString()
		{
			if (!String.IsNullOrEmpty(name)) return name;
			if (!String.IsNullOrEmpty(@class)) return @class;
			return base.ToString();
		}
	}

	public class ItemPort
	{
		[XmlAttribute]
		public int minsize { get; set; }

		[XmlAttribute]
		public int maxsize { get; set; }

		[XmlAttribute]
		public string display_name { get; set; }

		[XmlAttribute]
		public string flags { get; set; }

		public Type[] Types { get; set; }

		public Connection[] Connections { get; set; }
		public ControllerDef ControllerDef { get; set; }

		override public string ToString()
		{
			if (!String.IsNullOrEmpty(display_name)) return display_name;
			if (Types != null && Types.Length > 0) return Types[0].type;
			return base.ToString();
		}
	}

	public class Type
	{
		[XmlAttribute]
		public string type { get; set; }

		[XmlAttribute]
		public string subtypes { get; set; }
	}

	public class ControllerDef
	{
		public UsableDef UsableDef { get; set; }
	}

	public class UsableDef
	{
		public PriorityGroup[] PriorityGroups { get; set; }
	}

	public class PriorityGroup
	{
		[XmlAttribute]
		public string itemType { get; set; }

		[XmlAttribute]
		public string defaultPriority { get; set; }
	}

	public class Connection
	{
		[XmlAttribute]
		public string pipeClass { get; set; }

		[XmlAttribute]
		public string pipe { get; set; }
	}


	public class AngleRange
	{
		[XmlAttribute]
		public int min { get; set; }

		[XmlAttribute]
		public int max { get; set; }
	}

	public class MovementParams
	{
		public SpaceshipMovement Spaceship { get; set; }
	}

	public class SpaceshipMovement
	{
		[XmlAttribute]
		public double engineWarmupDelay { get; set; }

		[XmlAttribute]
		public double engineIgnitionTime { get; set; }

		[XmlAttribute]
		public double enginePowerMax { get; set; }

		[XmlAttribute]
		public double rotationDamping { get; set; }

		[XmlAttribute]
		public double maxCruiseSpeed { get; set; }

		[XmlAttribute]
		public double maxEngineThrust { get; set; }

		[XmlAttribute]
		public double maxRetroThrust { get; set; }

		[XmlAttribute]
		public double maxDirectionalThrust { get; set; }

		[XmlAttribute]
		public string maxAngularVelocity { get; set; }

		[XmlAttribute]
		public string maxAngularAcceleration { get; set; }

		[XmlAttribute]
		public double maxJerk { get; set; }

		[XmlAttribute]
		public double maxAngJerk { get; set; }

		[XmlAttribute]
		public string maxTorqueAlpha { get; set; }

		[XmlAttribute]
		public string maxTorqueAngle { get; set; }
	}
}
