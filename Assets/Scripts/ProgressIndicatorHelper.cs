using UnityEngine;

internal class ProgressIndicatorHelper
{
	private const float DAMAGEMULTIPLIER_MIN = 0.65f;

	private const float DAMAGEMULTIPLIER_MAX = 30f;

	public const int PROGRESS_INDICATOR_MOD_TANKS_DESTROYED_REQUIRED = 1;

	public const int PROGRESS_INDICATOR_MOD_SECONDS_REQUIRED = 8;

	private const int LAST_EASY_LEVEL = 12;

	private const int MAX_POW_FACTOR = 3;

	public static void ModifyProgressIndicator()
	{
		if (!IsActive())
		{
			return;
		}
		if (!GameplayCommons.Instance.playersTankController.PlayerDead)
		{
			if (GameplayCommons.Instance.levelStateController.currentGameStats.GameStatistics.GetStat(GameStatistics.Stat.TanksDestroyed) >= 1 && GameplayCommons.Instance.levelStateController.currentGameStats.GameStatistics.GetStat(GameStatistics.Stat.SecondsSpentPlaying) >= 8)
			{
				GlobalCommons.Instance.globalGameStats.SequentalFailsCount = 0;
				GlobalCommons.Instance.globalGameStats.SequentalWinsCount++;
				GlobalCommons.Instance.globalGameStats.AskForReviewFactor++;
				int num = GlobalCommons.Instance.globalGameStats.SequentalWinsCount;
				if (num > 3)
				{
					num = 3;
				}
				GlobalCommons.Instance.globalGameStats.ProgressIndicator += (int)Mathf.Pow(2f, num);
			}
		}
		else if (GameplayCommons.Instance.levelStateController.currentGameStats.GameStatistics.GetStat(GameStatistics.Stat.TanksDestroyed) >= 1 && GameplayCommons.Instance.levelStateController.currentGameStats.GameStatistics.GetStat(GameStatistics.Stat.SecondsSpentPlaying) >= 8)
		{
			GlobalCommons.Instance.globalGameStats.SequentalWinsCount = 0;
			GlobalCommons.Instance.globalGameStats.AskForReviewFactor = 0;
			if (GlobalCommons.Instance.globalGameStats.IsPayingPlayer && GlobalCommons.Instance.globalGameStats.SequentalFailsCount == 0)
			{
				GlobalCommons.Instance.globalGameStats.SequentalFailsCount = 2;
			}
			GlobalCommons.Instance.globalGameStats.SequentalFailsCount++;
			int num2 = GlobalCommons.Instance.globalGameStats.SequentalFailsCount + 1;
			if (num2 > 3)
			{
				num2 = 3;
			}
			GlobalCommons.Instance.globalGameStats.ProgressIndicator -= (int)Mathf.Pow(2f, num2);
			if (GlobalCommons.Instance.globalGameStats.LevelsCompleted <= 12)
			{
				GlobalCommons.Instance.globalGameStats.ProgressIndicator--;
			}
		}
		if (GlobalCommons.Instance.globalGameStats.LevelsCompleted <= 12)
		{
			if (GlobalCommons.Instance.globalGameStats.SequentalWinsCount > 1)
			{
				GlobalCommons.Instance.globalGameStats.SequentalWinsCount = 1;
			}
			if (GlobalCommons.Instance.globalGameStats.ProgressIndicator > 0)
			{
				GlobalCommons.Instance.globalGameStats.ProgressIndicator = 0;
			}
		}
	}

	private static bool IsActive()
	{
		return CurrentModeAffectsPI();
	}

	private static bool CurrentModeAffectsPI()
	{
		return GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.RegularLevel || GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.ArenaLevel;
	}

	private static bool CurrentModeIsAffectedByPI()
	{
		return GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.RegularLevel || GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.ArenaLevel || GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.SurvivalLevel || GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.EditorLevel || GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.CustomLevel;
	}

	public static float GetDamageMultiplier()
	{
		float result = 1f;
		if (!CurrentModeIsAffectedByPI())
		{
			return result;
		}
		float min = 0.65f;
		float max = 30f;
		int num = GlobalCommons.Instance.globalGameStats.ProgressIndicator;
		if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.RegularLevel && GlobalCommons.Instance.globalGameStats.CompletedLevelsProgressIndicators.Count >= GlobalCommons.Instance.ActualSelectedLevel + 1)
		{
			DebugHelper.Log("There is a record in CompletedLevelsProgressIndicators, using value");
			num = GlobalCommons.Instance.globalGameStats.CompletedLevelsProgressIndicators[GlobalCommons.Instance.ActualSelectedLevel];
		}
		else
		{
			DebugHelper.Log("Level was not already completed or game mode in not regular level, using current PI");
		}
		result = 1f + (float)num * 0.05f;
		result = Mathf.Clamp(result, min, max);
		if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.EditorLevel)
		{
			LevelEditorController.EditorLevelDamageMultiplier = result;
		}
		if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.CustomLevel)
		{
			result = LevelEditorController.LoadedCustomLevelDamageMultiplier;
		}
		DebugHelper.Log("DAMAGE MULTIPLIER FOR THIS ROUND IS: " + result);
		return result;
	}
}
