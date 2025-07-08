using System.Collections.Generic;
using UnityEngine;

public class SurvivalModeController : MonoBehaviour
{
	private List<Vector3> spawnPoints;

	public const int MAX_ENEMIES = 45;

	private bool revealedMap;

	private List<SurvivalSpawnedEnemy> enemiesPack;

	private float lastTimeCheckedEnemies;

	private const float enemiesCheckInterval = 2f;

	private int difficultyTier;

	private int tanksDestroyed;

	private void Start()
	{
		lastTimeCheckedEnemies = Time.fixedTime;
		if (GlobalCommons.Instance.gameplayMode != GlobalCommons.GameplayModes.SurvivalLevel)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void ProcessEnemyTankDestruction()
	{
		tanksDestroyed++;
		GlobalCommons.Instance.ActualSelectedLevel = Mathf.CeilToInt((float)tanksDestroyed / 5f);
	}

	private void Update()
	{
		if (!revealedMap)
		{
			revealedMap = true;
			GameplayCommons.Instance.visibilityController.RevealAllImmediate();
		}
		if (spawnPoints == null || !GameplayCommons.Instance.LevelFullyInitialized)
		{
			return;
		}
		if (GameplayCommons.Instance.LevelFullyInitialized && GameplayCommons.Instance.enemiesTracker.GetDestObstacleTypeCount(DestructableObstacleTypes.bonusCrate) < 3)
		{
			Vector3 spawnPoint = GetSpawnPoint();
			Object.Instantiate(Prefabs.bonusCratePrefab, spawnPoint, Quaternion.identity);
			GameplayCommons.Instance.effectsSpawner.CreateSpawnerSpawnEffect(spawnPoint);
		}
		if (GameplayCommons.Instance.LevelFullyInitialized && GameplayCommons.Instance.enemiesTracker.GetDestObstacleTypeCount(DestructableObstacleTypes.explosiveBarrel) < 14)
		{
			Vector3 spawnPoint2 = GetSpawnPoint();
			Object.Instantiate(Prefabs.explodingBarrelPrefab, spawnPoint2, Quaternion.identity);
			GameplayCommons.Instance.effectsSpawner.CreateSpawnerSpawnEffect(spawnPoint2);
		}
		if (!(Time.fixedTime - lastTimeCheckedEnemies >= 2f) || GameplayCommons.Instance.levelStateController.IsFreezeActive)
		{
			return;
		}
		lastTimeCheckedEnemies = Time.fixedTime;
		UpdateEnemiesPack();
		for (int i = 0; i < enemiesPack.Count; i++)
		{
			SurvivalSpawnedEnemy survivalSpawnedEnemy = enemiesPack[i];
			if (survivalSpawnedEnemy.spawnedTank == null)
			{
				Vector3 spawnPoint3 = GetSpawnPoint();
				bool isBoss = UnityEngine.Random.value < survivalSpawnedEnemy.chanceForBoss;
				EnemyTankController component = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, spawnPoint3, Quaternion.identity).GetComponent<EnemyTankController>();
				component.InitializeEnemy(EnemyTankController.EnemyTypes.tank, survivalSpawnedEnemy.weaponType, isBoss);
				GameplayCommons.Instance.effectsSpawner.CreateSpawnerSpawnEffect(spawnPoint3);
				survivalSpawnedEnemy.spawnedTank = component.gameObject;
			}
		}
	}

	private void UpdateEnemiesPack()
	{
		switch (difficultyTier)
		{
		case 0:
			if (Time.timeSinceLevelLoad > 30f)
			{
				ReinitPackEnemies(new List<SurvivalSpawnedEnemy>
				{
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.shotgun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.shotgun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.shotgun, 0f)
				});
			}
			else if (enemiesPack == null)
			{
				enemiesPack = new List<SurvivalSpawnedEnemy>
				{
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0f)
				};
			}
			break;
		case 1:
			if (Time.timeSinceLevelLoad > 50f)
			{
				ReinitPackEnemies(new List<SurvivalSpawnedEnemy>
				{
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.shotgun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.minigun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.shotgun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.minigun, 0f)
				});
			}
			break;
		case 2:
			if (Time.timeSinceLevelLoad > 70f)
			{
				ReinitPackEnemies(new List<SurvivalSpawnedEnemy>
				{
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0.2f),
					new SurvivalSpawnedEnemy(WeaponTypes.shotgun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.minigun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0.2f),
					new SurvivalSpawnedEnemy(WeaponTypes.shotgun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.minigun, 0f)
				});
			}
			break;
		case 3:
			if (Time.timeSinceLevelLoad > 90f)
			{
				ReinitPackEnemies(new List<SurvivalSpawnedEnemy>
				{
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.shotgun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0.2f),
					new SurvivalSpawnedEnemy(WeaponTypes.shotgun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.minigun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.cannon, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.shotgun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0.2f),
					new SurvivalSpawnedEnemy(WeaponTypes.shotgun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.minigun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.cannon, 0f)
				});
			}
			break;
		case 4:
			if (Time.timeSinceLevelLoad > 120f)
			{
				ReinitPackEnemies(new List<SurvivalSpawnedEnemy>
				{
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0.2f),
					new SurvivalSpawnedEnemy(WeaponTypes.shotgun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.shotgun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.minigun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.cannon, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.homingRocket, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0.2f),
					new SurvivalSpawnedEnemy(WeaponTypes.shotgun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.shotgun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.minigun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.cannon, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.homingRocket, 0f)
				});
			}
			break;
		case 5:
			if (Time.timeSinceLevelLoad > 140f)
			{
				ReinitPackEnemies(new List<SurvivalSpawnedEnemy>
				{
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0.2f),
					new SurvivalSpawnedEnemy(WeaponTypes.shotgun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.minigun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.minigun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.cannon, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.homingRocket, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.suicide, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0.2f),
					new SurvivalSpawnedEnemy(WeaponTypes.shotgun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.minigun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.minigun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.cannon, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.homingRocket, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.suicide, 0f)
				});
			}
			break;
		case 6:
			if (Time.timeSinceLevelLoad > 160f)
			{
				ReinitPackEnemies(new List<SurvivalSpawnedEnemy>
				{
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0.2f),
					new SurvivalSpawnedEnemy(WeaponTypes.shotgun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.minigun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.minigun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.cannon, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.homingRocket, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.suicide, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.laser, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0.2f),
					new SurvivalSpawnedEnemy(WeaponTypes.shotgun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.minigun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.minigun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.cannon, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.homingRocket, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.suicide, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.laser, 0f)
				});
			}
			break;
		case 7:
			if (Time.timeSinceLevelLoad > 180f)
			{
				ReinitPackEnemies(new List<SurvivalSpawnedEnemy>
				{
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0.2f),
					new SurvivalSpawnedEnemy(WeaponTypes.shotgun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.minigun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.cannon, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.cannon, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.homingRocket, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.suicide, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.laser, 0.05f),
					new SurvivalSpawnedEnemy(WeaponTypes.railgun, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0.2f),
					new SurvivalSpawnedEnemy(WeaponTypes.shotgun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.minigun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.cannon, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.cannon, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.homingRocket, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.suicide, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.laser, 0.05f),
					new SurvivalSpawnedEnemy(WeaponTypes.railgun, 0f)
				});
			}
			break;
		case 8:
			if (Time.timeSinceLevelLoad > 200f)
			{
				ReinitPackEnemies(new List<SurvivalSpawnedEnemy>
				{
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0.2f),
					new SurvivalSpawnedEnemy(WeaponTypes.shotgun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.minigun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.cannon, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.cannon, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.homingRocket, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.homingRocket, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.suicide, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.laser, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.railgun, 0.05f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.machinegun, 0.2f),
					new SurvivalSpawnedEnemy(WeaponTypes.shotgun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.minigun, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.cannon, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.cannon, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.homingRocket, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.homingRocket, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.suicide, 0f),
					new SurvivalSpawnedEnemy(WeaponTypes.laser, 0.1f),
					new SurvivalSpawnedEnemy(WeaponTypes.railgun, 0.05f)
				});
			}
			break;
		}
	}

	private void ReinitPackEnemies(List<SurvivalSpawnedEnemy> newEnemiesPack)
	{
		for (int i = 0; i < enemiesPack.Count; i++)
		{
			SurvivalSpawnedEnemy survivalSpawnedEnemy = enemiesPack[i];
			if (!(survivalSpawnedEnemy.spawnedTank != null))
			{
				continue;
			}
			for (int j = 0; j < newEnemiesPack.Count; j++)
			{
				SurvivalSpawnedEnemy survivalSpawnedEnemy2 = newEnemiesPack[j];
				if (survivalSpawnedEnemy2.spawnedTank == null && survivalSpawnedEnemy2.weaponType == survivalSpawnedEnemy.weaponType)
				{
					survivalSpawnedEnemy2.spawnedTank = survivalSpawnedEnemy.spawnedTank;
					break;
				}
			}
		}
		enemiesPack = newEnemiesPack;
		difficultyTier++;
	}

	public void Initialize(List<Vector3> spawners)
	{
		if (spawnPoints == null)
		{
			spawnPoints = spawners;
		}
	}

	public int ProcessEarnings()
	{
		int num = Mathf.FloorToInt((float)(MoneyLootCounter.GetEnemyMoneyLoot(forSurvival: true) * GameplayCommons.Instance.levelStateController.currentGameStats.GameStatistics.GetStat(GameStatistics.Stat.TanksDestroyed)) * 1.3f);
		GameplayCommons.Instance.levelStateController.currentGameStats.PickupMoney(num);
		return num;
	}

	private Vector3 GetSpawnPoint()
	{
		bool flag = true;
		int num = 0;
		Vector3 result;
		do
		{
			flag = true;
			result = spawnPoints[Random.Range(0, spawnPoints.Count)];
			List<EnemyTankController> allEnemies = GameplayCommons.Instance.enemiesTracker.AllEnemies;
			for (int i = 0; i < allEnemies.Count; i++)
			{
				EnemyTankController enemyTankController = allEnemies[i];
				Vector3 position = enemyTankController.TankBase.transform.position;
				if (Mathf.Abs(position.x - result.x) < 1.5f)
				{
					Vector3 position2 = enemyTankController.TankBase.transform.position;
					if (Mathf.Abs(position2.y - result.y) < 1.5f)
					{
						flag = false;
						break;
					}
				}
			}
			List<DestructableObstacleController> allDestObstacles = GameplayCommons.Instance.enemiesTracker.AllDestObstacles;
			for (int j = 0; j < allDestObstacles.Count; j++)
			{
				DestructableObstacleController destructableObstacleController = allDestObstacles[j];
				Vector3 position3 = destructableObstacleController.transform.position;
				if (Mathf.Abs(position3.x - result.x) < 1.5f)
				{
					Vector3 position4 = destructableObstacleController.transform.position;
					if (Mathf.Abs(position4.y - result.y) < 1.5f)
					{
						flag = false;
						break;
					}
				}
			}
			Vector3 position5 = GameplayCommons.Instance.playersTankController.TankBase.transform.position;
			if (Mathf.Abs(position5.x - result.x) < 7f)
			{
				Vector3 position6 = GameplayCommons.Instance.playersTankController.TankBase.transform.position;
				if (Mathf.Abs(position6.y - result.y) < 7f)
				{
					flag = false;
				}
			}
			num++;
		}
		while (!flag && num < 30);
		return result;
	}
}
