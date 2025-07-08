using System.Collections.Generic;
using UnityEngine;

public class LevelStateController : MonoBehaviour
{
	private float levelCompletedTimestamp = 99999f;

	private bool objectiveCompletePanelShown;

	public const float LEVELRESULTSTIMEOUT_WON_BASE = 3.5f;

	public const float LEVELRESULTSTIMEOUT_FAILED = 2f;

	public const float OBJECTIVECOMPLETEPANELTIMEOUT = 0.2f;

	private float levelResultsTimeoutToUse;

	internal const float levelCompleteCratesBreakTimeoutMin = 0.2f;

	internal const float levelCompleteCratesBreakTimeoutMax = 0.8f;

	private float lastTimePickedFreeze = -100f;

	private bool freezeActive;

	private float lastTimePickedInvisibility = -100f;

	private bool invisibilityActive;

	private float lastTimePickedDoubleDamage = -100f;

	private bool doubleDamageActive;

	internal CurrentGameStats currentGameStats;

	private bool levelCompletionPending;

	private List<Vector3> allWaypoints;

	private ComboController comboController;

	internal bool GameplayStopped;

	public ComboController ComboController => comboController;

	public bool IsFreezeActive => freezeActive;

	public bool IsInvisibilityActive => invisibilityActive;

	public bool IsDoubleDamageActive => doubleDamageActive;

	public bool IsAnyActiveBonusActive => freezeActive || invisibilityActive || doubleDamageActive;

	public List<Vector3> Waypoints => allWaypoints;

	public bool LevelCompletionPending => levelCompletionPending;

	public float LevelCompletedTimestamp => levelCompletedTimestamp;

	private void Start()
	{
		levelResultsTimeoutToUse = 3.5f;
		if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.TutorialLevel)
		{
			levelResultsTimeoutToUse *= 0.25f;
		}
		comboController = new ComboController();
		currentGameStats = new CurrentGameStats();
	}

	public void SetWaypoints(List<Vector3> waypoints)
	{
		allWaypoints = waypoints;
	}

	public void FreezeEnemies()
	{
		freezeActive = true;
		lastTimePickedFreeze = Time.fixedTime;
		int num = GameplayCommons.Instance.enemiesTracker.AllHomingRockets.Count;
		while (num-- > 0)
		{
			HomingRocket homingRocket = GameplayCommons.Instance.enemiesTracker.AllHomingRockets[num];
			homingRocket.ProcessDestruction();
		}
	}

	internal void ProcessInvisibilityPickup()
	{
		invisibilityActive = true;
		lastTimePickedInvisibility = Time.fixedTime;
		GameplayCommons.Instance.playersTankController.SetAlpha(0.5f);
		HidePlayerFromAllEnemies();
	}

	internal void ProcessDoubleDamagePickup()
	{
		doubleDamageActive = true;
		lastTimePickedDoubleDamage = Time.fixedTime;
	}

	internal void HidePlayerFromAllEnemies()
	{
		for (int i = 0; i < GameplayCommons.Instance.enemiesTracker.AllEnemies.Count; i++)
		{
			GameplayCommons.Instance.enemiesTracker.AllEnemies[i].HidePlayerFromEnemy();
		}
	}

	public void ShowLevelResultsPanel()
	{
		GameplayStopped = true;
		GlobalCommons.GameplayModes gameplayMode = GlobalCommons.Instance.gameplayMode;
		if (gameplayMode == GlobalCommons.GameplayModes.EditorLevel)
		{
			EditorLevelCompletedMenu component = UnityEngine.Object.Instantiate(Prefabs.EditorLevelCompletedMenu).GetComponent<EditorLevelCompletedMenu>();
			component.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
		}
		else
		{
			GameplayCommons.Instance.levelResultsController.FadeIn();
		}
	}

	internal float GetActiveBonusPercentage(BonusController.BonusType bonusType)
	{
		switch (bonusType)
		{
		case BonusController.BonusType.doubleDamageBonus:
			return 1f - (Time.fixedTime - lastTimePickedDoubleDamage) / PlayerBalance.doubleDamageTimeout;
		case BonusController.BonusType.freezeBonus:
			return 1f - (Time.fixedTime - lastTimePickedFreeze) / PlayerBalance.freezeTimeout;
		case BonusController.BonusType.invisibilityBonus:
			return 1f - (Time.fixedTime - lastTimePickedInvisibility) / PlayerBalance.invisibilityTimeout;
		default:
			return 0.5f;
		}
	}

	internal float GetCurrentActiveBonusPercentage()
	{
		if (freezeActive)
		{
			return 1f - (Time.fixedTime - lastTimePickedFreeze) / PlayerBalance.freezeTimeout;
		}
		if (invisibilityActive)
		{
			return 1f - (Time.fixedTime - lastTimePickedInvisibility) / PlayerBalance.invisibilityTimeout;
		}
		if (doubleDamageActive)
		{
			return 1f - (Time.fixedTime - lastTimePickedDoubleDamage) / PlayerBalance.doubleDamageTimeout;
		}
		return 0.5f;
	}

	private void Update()
	{
		comboController.Update();
		if (freezeActive && Time.fixedTime - lastTimePickedFreeze >= PlayerBalance.freezeTimeout)
		{
			freezeActive = false;
			SoundManager.instance.PlayUnfreezeSound();
			for (int i = 0; i < GameplayCommons.Instance.enemiesTracker.AllEnemies.Count; i++)
			{
				GameplayCommons.Instance.enemiesTracker.AllEnemies[i].Unfreeze();
			}
		}
		if (invisibilityActive)
		{
			float num = PlayerBalance.invisibilityTimeout - (Time.fixedTime - lastTimePickedInvisibility);
			if (num <= 2f && num > 0f)
			{
				if ((int)(num * 10f) % 2 != 0)
				{
					GameplayCommons.Instance.playersTankController.SetAlpha(0.5f);
				}
				else
				{
					GameplayCommons.Instance.playersTankController.SetAlpha(1f);
				}
			}
			if (num <= 0f)
			{
				invisibilityActive = false;
				GameplayCommons.Instance.playersTankController.SetAlpha(1f);
				GameplayCommons.Instance.effectsSpawner.CreateSpawnerSpawnEffect(GameplayCommons.Instance.playersTankController.TankBase.transform.position);
			}
		}
		if (doubleDamageActive)
		{
			float num2 = PlayerBalance.doubleDamageTimeout - (Time.fixedTime - lastTimePickedDoubleDamage);
			if (num2 <= 0f)
			{
				doubleDamageActive = false;
				GameplayCommons.Instance.effectsSpawner.CreateSpawnerSpawnEffect(GameplayCommons.Instance.playersTankController.TankBase.transform.position);
			}
		}
		if (GlobalCommons.Instance.gameplayMode != GlobalCommons.GameplayModes.SurvivalLevel && !GameplayStopped && GameplayCommons.Instance.enemiesTracker.AllEnemies.Count == 0 && GameplayCommons.Instance.enemiesTracker.AllSpawners.Count == 0 && GameplayCommons.Instance.playersTankController.PlayerActive && !GameplayCommons.Instance.playersTankController.PlayerDead && GameplayCommons.Instance.LevelFullyInitialized)
		{
			levelCompletionPending = true;
			if (levelCompletedTimestamp >= 99999f)
			{
				levelCompletedTimestamp = Time.fixedTime;
			}
			else
			{
				if (GameplayCommons.Instance.playersTankController.PlayerDead || GameplayCommons.Instance.enemiesTracker.AreMoneyBonusesPresent())
				{
					return;
				}
				if (GlobalCommons.Instance.gameplayMode != GlobalCommons.GameplayModes.TutorialLevel && !objectiveCompletePanelShown && Time.fixedTime - levelCompletedTimestamp > 0.2f)
				{
					objectiveCompletePanelShown = true;
					if (GlobalCommons.Instance.globalGameStats.LittleTargetsTracker.ProcessAfterLevelCheck())
					{
						GameObject.Find("ObjectiveCompletePanelController").GetComponent<ObjectiveCompletePanelController>().Activate();
					}
					else
					{
						levelResultsTimeoutToUse *= 0.5f;
					}
				}
				if (Time.fixedTime - levelCompletedTimestamp > levelResultsTimeoutToUse)
				{
					if ((bool)GameplayCommons.Instance.weaponsController.ActiveGuidedRocket)
					{
						GameplayCommons.Instance.weaponsController.ActiveGuidedRocket.DestroyRocket();
					}
					GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.LevelsCompleted);
					GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.SecondsSpentPlaying, Mathf.CeilToInt(Time.timeSinceLevelLoad));
					ShowLevelResultsPanel();
				}
			}
		}
		else
		{
			levelCompletedTimestamp = 99999f;
			levelCompletionPending = false;
		}
	}

	public static void ProcessLevelEarnings()
	{
		if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.SurvivalLevel)
		{
			Object.FindObjectOfType<SurvivalModeController>().ProcessEarnings();
		}
	}
}
