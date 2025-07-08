using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesTracker
{
	private List<EnemyTankController> enemyControllers;

	private List<EnemySpawnerController> enemySpawners;

	private List<DestructableObstacleController> destructableObstacles;

	private List<BonusController> bonuses;

	private List<BushController> bushes;

	private List<HomingRocket> homingRockets;

	private SurvivalModeController survivalController;

	public int MaxSpawners;

	public List<EnemyTankController> AllEnemies => enemyControllers;

	public List<EnemySpawnerController> AllSpawners => enemySpawners;

	public List<BushController> AllBushes => bushes;

	public List<DestructableObstacleController> AllDestObstacles => destructableObstacles;

	public List<BonusController> AllBonuses => bonuses;

	public List<HomingRocket> AllHomingRockets => homingRockets;

	public EnemiesTracker()
	{
		enemyControllers = new List<EnemyTankController>();
		enemySpawners = new List<EnemySpawnerController>();
		destructableObstacles = new List<DestructableObstacleController>();
		bonuses = new List<BonusController>();
		bushes = new List<BushController>();
		homingRockets = new List<HomingRocket>();
		if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.SurvivalLevel)
		{
			survivalController = UnityEngine.Object.FindObjectOfType<SurvivalModeController>();
		}
	}

	public void Track(EnemyTankController etc)
	{
		if (enemyControllers.IndexOf(etc) == -1)
		{
			enemyControllers.Add(etc);
		}
	}

	public void Track(HomingRocket hrc)
	{
		if (homingRockets.IndexOf(hrc) == -1)
		{
			homingRockets.Add(hrc);
		}
	}

	public void Track(EnemySpawnerController esc)
	{
		if (enemySpawners.IndexOf(esc) == -1)
		{
			enemySpawners.Add(esc);
			if (MaxSpawners < enemySpawners.Count)
			{
				MaxSpawners = enemySpawners.Count;
			}
		}
	}

	public void Track(DestructableObstacleController esc)
	{
		if (destructableObstacles.IndexOf(esc) == -1)
		{
			destructableObstacles.Add(esc);
		}
	}

	public void Track(BonusController bc)
	{
		if (bonuses.IndexOf(bc) == -1)
		{
			bonuses.Add(bc);
		}
	}

	public void Track(BushController bc)
	{
		if (bushes.IndexOf(bc) == -1)
		{
			bushes.Add(bc);
		}
	}

	public void UnTrack(BushController bc)
	{
		bushes.Remove(bc);
	}

	public void UnTrack(BonusController bc)
	{
		bonuses.Remove(bc);
	}

	public void UnTrack(HomingRocket hrc)
	{
		homingRockets.Remove(hrc);
	}

	public void UnTrack(EnemyTankController etc)
	{
		enemyControllers.Remove(etc);
		if ((bool)survivalController)
		{
			survivalController.ProcessEnemyTankDestruction();
		}
	}

	public void UnTrack(EnemySpawnerController esc)
	{
		enemySpawners.Remove(esc);
	}

	public void UnTrack(DestructableObstacleController esc)
	{
		destructableObstacles.Remove(esc);
	}

	public bool AreMoneyBonusesPresent()
	{
		for (int i = 0; i < AllBonuses.Count; i++)
		{
			if (AllBonuses[i].bonusType == BonusController.BonusType.bronzeCoinBonus || AllBonuses[i].bonusType == BonusController.BonusType.platinumCoinBonus || AllBonuses[i].bonusType == BonusController.BonusType.goldenCoinBonus)
			{
				return true;
			}
		}
		return false;
	}

	internal bool IsActiveBonusPresent()
	{
		for (int i = 0; i < bonuses.Count; i++)
		{
			if (Array.IndexOf(BonusController.activeBonusTypes, bonuses[i].bonusType) != -1)
			{
				return true;
			}
		}
		return false;
	}

	internal int GetDestObstacleTypeCount(DestructableObstacleTypes type)
	{
		int num = 0;
		for (int i = 0; i < destructableObstacles.Count; i++)
		{
			if (destructableObstacles[i].destructableObstacleType == type)
			{
				num++;
			}
		}
		return num;
	}

	internal bool CheckRevealMapBonusPresent()
	{
		for (int i = 0; i < bonuses.Count; i++)
		{
			if (bonuses[i].bonusType == BonusController.BonusType.revealMapBonus)
			{
				return true;
			}
		}
		return false;
	}
}
