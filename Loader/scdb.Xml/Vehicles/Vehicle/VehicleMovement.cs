using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class VehicleMovement
	{
		[XmlAttribute] public double steerSpeed;
		[XmlAttribute] public double steerSpeedMin;
		[XmlAttribute] public double kvSteerMax;
		[XmlAttribute] public double v0SteerMax;
		[XmlAttribute] public double vMaxSteerMax;
		[XmlAttribute] public double steerRelaxation;
		[XmlAttribute] public double pedalLimitMax;
		[XmlAttribute] public double rpmGearShiftSpeed;
		[XmlAttribute] public double engineIgnitionTime;
		[XmlAttribute] public bool surfaceFXInFpView;
		[XmlAttribute] public bool isBreakingOnIdle;
		public Handling Handling;
	}
}
