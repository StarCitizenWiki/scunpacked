namespace Loader
{
	public class StandardisedConsumableParams
	{
		public string containerTypeTag { get; set; }
		public bool oneShotConsume { get; set; }
		public bool containerClosed { get; set; }
		public bool canBeReclosed { get; set; }
		public bool discardWhenConsumed { get; set; }

		public double consumeVolume { get; set; }
	}
}
