namespace Loader
{
	public class StandardisedDriveCharacteristics
	{
		public double TopSpeed { get; set; }
		public double ReverseSpeed { get; set; }
		public double Acceleration { get; set; }
		public double Decceleration { get; set; }

		public double ZeroToMax { get; set; }
		public double MaxToZero { get; set; }

		public double ZeroToReverse { get; set; }
		public double ReverseToZero { get; set; }
	}
}
