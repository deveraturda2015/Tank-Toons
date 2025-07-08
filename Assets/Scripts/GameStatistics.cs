public class GameStatistics
{
	public enum Stat
	{
		TanksDestroyed,
		TowersDestroyed,
		SpawnersDestroyed,
		WallsDestroyed,
		LevelsCompleted,
		CoinsCollected,
		BarrelsExploded,
		BonusCratesPopped,
		EnemyRocketsDodged,
		BiggestCombo,
		TimesHitEnemyWithMine,
		TimesMissed,
		TimesHit,
		SecondsSpentPlaying,
		RailgunSimultaneousHits,
		TotalMoneyCollected,
		TotalMoneySpent,
		TimesHitEnemyWithGuidedMissile
	}

	private int[] statsValues;

	private bool isGlobal;

	public int[] StatsValues
	{
		get
		{
			return statsValues;
		}
		set
		{
			statsValues = value;
		}
	}

	public GameStatistics(bool isGlobal)
	{
		this.isGlobal = isGlobal;
		statsValues = new int[19];
	}

	public static bool IsCurrentGameModeEligibleForStatsAndAchievements()
	{
		if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.EditorLevel || GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.CustomLevel)
		{
			return false;
		}
		return true;
	}

	public int GetStat(Stat type)
	{
		return statsValues[(int)type];
	}

	public void IncreaseStat(Stat type)
	{
		if (IsCurrentGameModeEligibleForStatsAndAchievements())
		{
			statsValues[(int)type]++;
			if (isGlobal && (bool)GameplayCommons.Instance)
			{
				GameplayCommons.Instance.levelStateController.currentGameStats.GameStatistics.IncreaseStat(type);
				GlobalCommons.Instance.globalGameStats.AchievementsTracker.ProcessStatChange(type);
			}
		}
	}

	public void IncreaseStat(Stat type, int val)
	{
		if (IsCurrentGameModeEligibleForStatsAndAchievements())
		{
			statsValues[(int)type] += val;
			if (isGlobal && (bool)GameplayCommons.Instance)
			{
				GameplayCommons.Instance.levelStateController.currentGameStats.GameStatistics.IncreaseStat(type, val);
				GlobalCommons.Instance.globalGameStats.AchievementsTracker.ProcessStatChange(type);
			}
		}
	}

	public void UpdateMaxStat(Stat type, int val)
	{
		if (IsCurrentGameModeEligibleForStatsAndAchievements())
		{
			if (statsValues[(int)type] < val)
			{
				statsValues[(int)type] = val;
			}
			if (isGlobal && (bool)GameplayCommons.Instance)
			{
				GameplayCommons.Instance.levelStateController.currentGameStats.GameStatistics.UpdateMaxStat(type, val);
				GlobalCommons.Instance.globalGameStats.AchievementsTracker.ProcessStatChange(type);
			}
		}
	}
}
