internal class FMath
{
	public static float MoveFloatStepClamp(float current, float target, float step)
	{
		if (current < target)
		{
			current += step;
			if (current > target)
			{
				current = target;
			}
		}
		else
		{
			current -= step;
			if (current < target)
			{
				current = target;
			}
		}
		return current;
	}
}
