using UnityEngine;

public class AutoAimHelper
{
	private static AutoaimTarget autoaimTarget = new AutoaimTarget();

	private static int targetCheckTicker = 0;

	private static int tanksSortTicker = 0;

	private static int spawnersSortTicker = 0;

	public static Vector3? GetAutoaimVectorOrPosition(bool getPosition = false)
	{
		return autoaimTarget.GetNormalizedTargetVectorOrPosition(getPosition);
	}

	public static bool CheckTargetChangedFlag()
	{
		return autoaimTarget.CheckTargetChangeFlag();
	}

	public static Vector3? UpdateAutoaimLogicAndGetVector()
	{
		Vector3? normalizedTargetVectorOrPosition = autoaimTarget.GetNormalizedTargetVectorOrPosition();
		if (tanksSortTicker > 0)
		{
			tanksSortTicker--;
		}
		else
		{
			tanksSortTicker = Random.Range(5, 10);
			GameplayCommons.Instance.enemiesTracker.AllEnemies.Sort(delegate(EnemyTankController x, EnemyTankController y)
			{
				Vector2 vector5 = x.TankBase.transform.position - GameplayCommons.Instance.playersTankController.TankBase.transform.position;
				Vector2 vector6 = y.TankBase.transform.position - GameplayCommons.Instance.playersTankController.TankBase.transform.position;
				return vector5.sqrMagnitude.CompareTo(vector6.sqrMagnitude);
			});
		}
		if (spawnersSortTicker > 0)
		{
			spawnersSortTicker--;
		}
		else
		{
			spawnersSortTicker = Random.Range(5, 10);
			GameplayCommons.Instance.enemiesTracker.AllSpawners.Sort(delegate(EnemySpawnerController x, EnemySpawnerController y)
			{
				Vector2 vector3 = x.transform.position - GameplayCommons.Instance.playersTankController.TankBase.transform.position;
				Vector2 vector4 = y.transform.position - GameplayCommons.Instance.playersTankController.TankBase.transform.position;
				return vector3.sqrMagnitude.CompareTo(vector4.sqrMagnitude);
			});
		}
		if (targetCheckTicker > 0)
		{
			targetCheckTicker--;
			return normalizedTargetVectorOrPosition;
		}
		targetCheckTicker = Random.Range(2, 4);
		if (GameplayCommons.Instance.enemiesTracker.AllEnemies.Count > 0)
		{
			for (int i = 0; i < GameplayCommons.Instance.enemiesTracker.AllEnemies.Count; i++)
			{
				EnemyTankController enemyTankController = GameplayCommons.Instance.enemiesTracker.AllEnemies[i];
				Vector2 direction = enemyTankController.TankBase.transform.position - GameplayCommons.Instance.playersTankController.TankBase.transform.position;
				Vector2 vector = Camera.main.transform.position - enemyTankController.TankBase.transform.position;
				if (!(Mathf.Abs(vector.x) > GlobalCommons.Instance.horizontalCameraBorderWithCompensation) && !(Mathf.Abs(vector.y) > GlobalCommons.Instance.verticalCameraBorderWithCompensation) && !GameplayCommons.Instance.visibilityController.CheckCoverNearPoint(enemyTankController.TankBase.transform.position) && Physics2D.Raycast(GameplayCommons.Instance.playersTankController.TankBase.transform.position, direction, direction.magnitude, LayerMasks.allObstacleTypesLayerMask).collider == null)
				{
					autoaimTarget.SetTarget(enemyTankController);
					return autoaimTarget.GetNormalizedTargetVectorOrPosition();
				}
			}
		}
		if (GameplayCommons.Instance.enemiesTracker.AllSpawners.Count > 0)
		{
			for (int j = 0; j < GameplayCommons.Instance.enemiesTracker.AllSpawners.Count; j++)
			{
				EnemySpawnerController enemySpawnerController = GameplayCommons.Instance.enemiesTracker.AllSpawners[j];
				Vector2 direction2 = enemySpawnerController.transform.position - GameplayCommons.Instance.playersTankController.TankBase.transform.position;
				Vector2 vector2 = Camera.main.transform.position - enemySpawnerController.transform.position;
				if (!(Mathf.Abs(vector2.x) > GlobalCommons.Instance.horizontalCameraBorderWithCompensation) && !(Mathf.Abs(vector2.y) > GlobalCommons.Instance.verticalCameraBorderWithCompensation) && !GameplayCommons.Instance.visibilityController.CheckCoverNearPoint(enemySpawnerController.transform.position) && Physics2D.Raycast(GameplayCommons.Instance.playersTankController.TankBase.transform.position, direction2, direction2.magnitude, LayerMasks.allObstacleTypesLayerMask).collider == null)
				{
					autoaimTarget.SetTarget(enemySpawnerController);
					return autoaimTarget.GetNormalizedTargetVectorOrPosition();
				}
			}
		}
		autoaimTarget.SetNullTarget();
		return null;
	}
}
