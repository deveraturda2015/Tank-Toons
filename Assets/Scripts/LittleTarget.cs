public class LittleTarget
{
	public enum TargetTypes
	{
		CollectCoins,
		DestroySpawners,
		DestroyTanks,
		DestroyCrates,
		DestroyBricks,
		ExplodeBarrels,
		DestroyTowers
	}

	private TargetTypes targetType;

	private int valueNeeded;

	public TargetTypes TargetType => targetType;

	public int ValueNeeded => valueNeeded;

	public LittleTarget(TargetTypes targetType, int valueNeeded)
	{
		this.targetType = targetType;
		this.valueNeeded = valueNeeded;
	}
}
