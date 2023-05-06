namespace Loader
{
	public class StandardisedAfterburner
	{
		public double SpoolUpTime { get; set; }
		public double CapacitorThresholdRatio { get; set; }
		public double CapacitorMax { get; set; }
		public double CapacitorAfterburnerIdleCost { get; set; }
		public double CapacitorAfterburnerLinearCost { get; set; }
		public double CapacitorAfterburnerAngularCost { get; set; }
		public double CapacitorRegenDelayAfterUse { get; set; }
		public double CapacitorRegenPerSec { get; set; }
	}
}
