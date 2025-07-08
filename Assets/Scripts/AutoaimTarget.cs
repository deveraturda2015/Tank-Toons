using System;
using UnityEngine;

internal class AutoaimTarget
{
	private enum TargetType
	{
		Tank,
		Spawner,
		None
	}

	private EnemyTankController tankTarget;

	private EnemySpawnerController spawnerTarget;

	private TargetType targetType = TargetType.None;

	private bool targetChangeFlag;

	public void SetNullTarget()
	{
		if (targetType != TargetType.None)
		{
			targetType = TargetType.None;
			tankTarget = null;
			spawnerTarget = null;
			targetChangeFlag = true;
		}
	}

	public void SetTarget(EnemyTankController etc)
	{
		if (targetType != 0 || !(tankTarget == etc))
		{
			targetType = TargetType.Tank;
			tankTarget = etc;
			spawnerTarget = null;
			targetChangeFlag = true;
		}
	}

	public void SetTarget(EnemySpawnerController esc)
	{
		if (targetType != TargetType.Spawner || !(spawnerTarget == esc))
		{
			targetType = TargetType.Spawner;
			tankTarget = null;
			spawnerTarget = esc;
			targetChangeFlag = true;
		}
	}

	public Vector3? GetNormalizedTargetVectorOrPosition(bool getPosition = false)
	{
		switch (targetType)
		{
		case TargetType.None:
			return null;
		case TargetType.Spawner:
			if (spawnerTarget != null)
			{
				if (getPosition)
				{
					return spawnerTarget.transform.position;
				}
				return (spawnerTarget.transform.position - GameplayCommons.Instance.playersTankController.TankBase.transform.position).normalized * GlobalCommons.Instance.AnalogMax;
			}
			SetNullTarget();
			return null;
		case TargetType.Tank:
			if (tankTarget != null)
			{
				if (getPosition)
				{
					return tankTarget.TankBase.transform.position;
				}
				return (tankTarget.TankBase.transform.position - GameplayCommons.Instance.playersTankController.TankBase.transform.position).normalized * GlobalCommons.Instance.AnalogMax;
			}
			SetNullTarget();
			return null;
		default:
			throw new Exception("Cannot get target coords for " + targetType.ToString());
		}
	}

	public bool CheckTargetChangeFlag()
	{
		if (targetChangeFlag)
		{
			targetChangeFlag = false;
			return true;
		}
		return false;
	}
}
