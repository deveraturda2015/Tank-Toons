using System;
using UnityEngine;

public class LittleTargetsTracker
{
	public const int FIRST_TARGET_BONUS = 50;

	private int currentLittleTargetIndex;

	private int currentLittleTargetValue;

	private int currentLittleTargetCoinsBonus = 50;

	private int prevLittleTargetCoinsBonus;

	private int[] targetValues = new int[7]
	{
		30,
		1,
		6,
		6,
		1,
		2,
		1
	};

	private int[] targetValueIncrements = new int[7]
	{
		30,
		1,
		6,
		6,
		1,
		2,
		1
	};

	public int PrevLittleTargetCoinsBonus => prevLittleTargetCoinsBonus;

	public int CurrentLittleTargetCoinsBonus
	{
		get
		{
			return currentLittleTargetCoinsBonus;
		}
		set
		{
			currentLittleTargetCoinsBonus = value;
		}
	}

	public int CurrentTargetValue
	{
		get
		{
			return currentLittleTargetValue;
		}
		set
		{
			currentLittleTargetValue = value;
		}
	}

	public int CurrentTargetIndex
	{
		get
		{
			return currentLittleTargetIndex;
		}
		set
		{
			currentLittleTargetIndex = value;
		}
	}

	public LittleTarget CurrentTarget => GetLittleTargetForIndex(currentLittleTargetIndex);

	public LittleTarget PreviousTarget => GetLittleTargetForIndex(currentLittleTargetIndex - 1);

	private LittleTarget GetLittleTargetForIndex(int index)
	{
		LittleTarget.TargetTypes targetTypes = (LittleTarget.TargetTypes)(index % Enum.GetValues(typeof(LittleTarget.TargetTypes)).Length);
		int num = Mathf.FloorToInt(index / Enum.GetValues(typeof(LittleTarget.TargetTypes)).Length);
		int valueNeeded = targetValues[(int)targetTypes] + targetValueIncrements[(int)targetTypes] * num;
		return new LittleTarget(targetTypes, valueNeeded);
	}

	internal bool ProcessAfterLevelCheck()
	{
		if (false || GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.TutorialLevel || GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.CustomLevel || GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.EditorLevel)
		{
			return false;
		}
		switch (CurrentTarget.TargetType)
		{
		case LittleTarget.TargetTypes.CollectCoins:
			currentLittleTargetValue += GameplayCommons.Instance.levelStateController.currentGameStats.GameStatistics.GetStat(GameStatistics.Stat.CoinsCollected);
			break;
		case LittleTarget.TargetTypes.DestroyBricks:
			currentLittleTargetValue += GameplayCommons.Instance.levelStateController.currentGameStats.GameStatistics.GetStat(GameStatistics.Stat.WallsDestroyed);
			break;
		case LittleTarget.TargetTypes.DestroyCrates:
			currentLittleTargetValue += GameplayCommons.Instance.levelStateController.currentGameStats.GameStatistics.GetStat(GameStatistics.Stat.BonusCratesPopped);
			break;
		case LittleTarget.TargetTypes.DestroySpawners:
			currentLittleTargetValue += GameplayCommons.Instance.levelStateController.currentGameStats.GameStatistics.GetStat(GameStatistics.Stat.SpawnersDestroyed);
			break;
		case LittleTarget.TargetTypes.DestroyTanks:
			currentLittleTargetValue += GameplayCommons.Instance.levelStateController.currentGameStats.GameStatistics.GetStat(GameStatistics.Stat.TanksDestroyed);
			break;
		case LittleTarget.TargetTypes.DestroyTowers:
			currentLittleTargetValue += GameplayCommons.Instance.levelStateController.currentGameStats.GameStatistics.GetStat(GameStatistics.Stat.TowersDestroyed);
			break;
		case LittleTarget.TargetTypes.ExplodeBarrels:
			currentLittleTargetValue += GameplayCommons.Instance.levelStateController.currentGameStats.GameStatistics.GetStat(GameStatistics.Stat.BarrelsExploded);
			break;
		}
		if (currentLittleTargetValue >= CurrentTarget.ValueNeeded)
		{
			prevLittleTargetCoinsBonus = currentLittleTargetCoinsBonus;
			GlobalCommons.Instance.globalGameStats.IncreaseMoney(currentLittleTargetCoinsBonus);
			currentLittleTargetValue = 0;
			int num = Mathf.CeilToInt((float)GlobalCommons.Instance.globalGameStats.MaxLevelIncome * UnityEngine.Random.Range(1.33f, 1.66f));
			currentLittleTargetCoinsBonus = ((num <= currentLittleTargetCoinsBonus) ? currentLittleTargetCoinsBonus : num);
			currentLittleTargetIndex++;
			return true;
		}
		return false;
	}
}
