using System;
using System.Collections.Generic;

namespace Loader
{
	public class StandardisedRadar
	{
		public double DetectionLifetime { get; set; }
		public double AltitudeCeiling { get; set; }
		public bool CrossSectionOcclusion { get; set; }

		public List<StandardisedSignatureDetection> Signatures { get; set; }
	}
}
