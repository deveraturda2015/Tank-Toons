using System;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsTracker
{
	public enum Achievement
	{
		TanksDestroyer,
		TowersDestroyer,
		SpawnersDestroyer,
		WallsDestroyer,
		LevelsCompleter,
		CoinsCollecter,
		BarrelsExploder,
		BonusCratesPopper,
		EnemyRocketsDodger,
		AwesomeCombo,
		MineEnemyExploder,
		Marksman,
		TimePlaying,
		Survivor,
		Penetrator,
		Missilolo
	}

	private int[] achievementsLevels;

	private int maxAchievementLevel;

	private List<int[]> achievementsValues;

	public int[] AllAchievementLevels
	{
		get
		{
			return achievementsLevels;
		}
		set
		{
			achievementsLevels = value;
		}
	}

	public int MaxAchievementLevel => maxAchievementLevel;

	public AchievementsTracker()
	{
		achievementsLevels = new int[16];
		achievementsValues = new List<int[]>();
		achievementsValues.Add(new int[5]
		{
			100,
			500,
			1500,
			3500,
			5000
		});
		achievementsValues.Add(new int[5]
		{
			10,
			50,
			150,
			350,
			500
		});
		achievementsValues.Add(new int[5]
		{
			13,
			80,
			250,
			450,
			700
		});
		achievementsValues.Add(new int[5]
		{
			60,
			350,
			1000,
			1500,
			3200
		});
		achievementsValues.Add(new int[5]
		{
			10,
			50,
			150,
			350,
			500
		});
		achievementsValues.Add(new int[5]
		{
			600,
			3500,
			10000,
			15000,
			32000
		});
		achievementsValues.Add(new int[5]
		{
			15,
			50,
			100,
			180,
			320
		});
		achievementsValues.Add(new int[5]
		{
			16,
			150,
			320,
			600,
			900
		});
		achievementsValues.Add(new int[5]
		{
			13,
			80,
			250,
			450,
			700
		});
		achievementsValues.Add(new int[5]
		{
			10,
			20,
			30,
			40,
			50
		});
		achievementsValues.Add(new int[5]
		{
			30,
			60,
			100,
			150,
			200
		});
		achievementsValues.Add(new int[5]
		{
			90,
			94,
			97,
			99,
			100
		});
		achievementsValues.Add(new int[5]
		{
			1200,
			3600,
			7200,
			14400,
			28800
		});
		achievementsValues.Add(new int[5]
		{
			12,
			9,
			6,
			3,
			1
		});
		achievementsValues.Add(new int[5]
		{
			2,
			3,
			4,
			5,
			6
		});
		achievementsValues.Add(new int[5]
		{
			15,
			45,
			100,
			160,
			250
		});
		maxAchievementLevel = achievementsValues[0].Length;
	}

	public int GetAchievementLevel(Achievement type)
	{
		return achievementsLevels[(int)type];
	}

	public void ProcessAfterLevelCheck()
	{
		if (GameStatistics.IsCurrentGameModeEligibleForStatsAndAchievements())
		{
			CheckAchievement(Achievement.Marksman);
			CheckAchievement(Achievement.Survivor);
		}
	}

	public void ProcessStatChange(GameStatistics.Stat stat)
	{
		if (GameStatistics.IsCurrentGameModeEligibleForStatsAndAchievements())
		{
			switch (stat)
			{
			case GameStatistics.Stat.TimesMissed:
			case GameStatistics.Stat.TimesHit:
			case GameStatistics.Stat.TotalMoneyCollected:
			case GameStatistics.Stat.TotalMoneySpent:
				break;
			case GameStatistics.Stat.BarrelsExploded:
				CheckAchievement(Achievement.BarrelsExploder);
				break;
			case GameStatistics.Stat.BiggestCombo:
				CheckAchievement(Achievement.AwesomeCombo);
				break;
			case GameStatistics.Stat.BonusCratesPopped:
				CheckAchievement(Achievement.BonusCratesPopper);
				break;
			case GameStatistics.Stat.CoinsCollected:
				CheckAchievement(Achievement.CoinsCollecter);
				break;
			case GameStatistics.Stat.EnemyRocketsDodged:
				CheckAchievement(Achievement.EnemyRocketsDodger);
				break;
			case GameStatistics.Stat.LevelsCompleted:
				CheckAchievement(Achievement.LevelsCompleter);
				break;
			case GameStatistics.Stat.RailgunSimultaneousHits:
				CheckAchievement(Achievement.Penetrator);
				break;
			case GameStatistics.Stat.SecondsSpentPlaying:
				CheckAchievement(Achievement.TimePlaying);
				break;
			case GameStatistics.Stat.SpawnersDestroyed:
				CheckAchievement(Achievement.SpawnersDestroyer);
				break;
			case GameStatistics.Stat.TanksDestroyed:
				CheckAchievement(Achievement.TanksDestroyer);
				break;
			case GameStatistics.Stat.TimesHitEnemyWithMine:
				CheckAchievement(Achievement.MineEnemyExploder);
				break;
			case GameStatistics.Stat.TimesHitEnemyWithGuidedMissile:
				CheckAchievement(Achievement.Missilolo);
				break;
			case GameStatistics.Stat.TowersDestroyed:
				CheckAchievement(Achievement.TowersDestroyer);
				break;
			case GameStatistics.Stat.WallsDestroyed:
				CheckAchievement(Achievement.WallsDestroyer);
				break;
			}
		}
	}

	private void CheckAchievement(Achievement type)
	{
		if (!GameStatistics.IsCurrentGameModeEligibleForStatsAndAchievements())
		{
			return;
		}
		bool flag = false;
		switch (type)
		{
		case Achievement.TanksDestroyer:
			flag = CheckSimpleGlobalValueAchievement(type, GameStatistics.Stat.TanksDestroyed);
			break;
		case Achievement.TowersDestroyer:
			flag = CheckSimpleGlobalValueAchievement(type, GameStatistics.Stat.TowersDestroyed);
			break;
		case Achievement.SpawnersDestroyer:
			flag = CheckSimpleGlobalValueAchievement(type, GameStatistics.Stat.SpawnersDestroyed);
			break;
		case Achievement.WallsDestroyer:
			flag = CheckSimpleGlobalValueAchievement(type, GameStatistics.Stat.WallsDestroyed);
			break;
		case Achievement.LevelsCompleter:
			flag = CheckSimpleGlobalValueAchievement(type, GameStatistics.Stat.LevelsCompleted);
			break;
		case Achievement.CoinsCollecter:
			flag = CheckSimpleGlobalValueAchievement(type, GameStatistics.Stat.CoinsCollected);
			break;
		case Achievement.BarrelsExploder:
			flag = CheckSimpleGlobalValueAchievement(type, GameStatistics.Stat.BarrelsExploded);
			break;
		case Achievement.BonusCratesPopper:
			flag = CheckSimpleGlobalValueAchievement(type, GameStatistics.Stat.BonusCratesPopped);
			break;
		case Achievement.EnemyRocketsDodger:
			flag = CheckSimpleGlobalValueAchievement(type, GameStatistics.Stat.EnemyRocketsDodged);
			break;
		case Achievement.AwesomeCombo:
			flag = CheckSimpleGlobalValueAchievement(type, GameStatistics.Stat.BiggestCombo);
			break;
		case Achievement.Missilolo:
			flag = CheckSimpleGlobalValueAchievement(type, GameStatistics.Stat.TimesHitEnemyWithMine);
			break;
		case Achievement.MineEnemyExploder:
			flag = CheckSimpleGlobalValueAchievement(type, GameStatistics.Stat.TimesHitEnemyWithMine);
			break;
		case Achievement.TimePlaying:
			flag = CheckSimpleGlobalValueAchievement(type, GameStatistics.Stat.SecondsSpentPlaying);
			break;
		case Achievement.Penetrator:
			flag = CheckSimpleGlobalValueAchievement(type, GameStatistics.Stat.RailgunSimultaneousHits);
			break;
		case Achievement.Marksman:
			flag = CheckMarksmanAchievement();
			break;
		case Achievement.Survivor:
			flag = CheckSurvivorAchievementAchievement();
			break;
		default:
			throw new Exception("unknown achievement lol");
		}
		if (flag)
		{
			NewAchievementPlate component = UnityEngine.Object.Instantiate(Prefabs.NewAchievementPlate).GetComponent<NewAchievementPlate>();
			component.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
			component.InitializePlate(type);
			if (achievementsLevels[(int)type] == 5)
			{
				SocialWorker.Instance.ReportAchievement(type);
			}
		}
	}

	private bool CheckSurvivorAchievementAchievement()
	{
		int num = achievementsLevels[13];
		if (num < maxAchievementLevel)
		{
			int num2 = achievementsValues[13][num];
			int num3 = Mathf.FloorToInt(GameplayCommons.Instance.playersTankController.HPPercentage * 100f);
			if (num3 > 0 && num3 <= num2)
			{
				achievementsLevels[13]++;
				CheckSurvivorAchievementAchievement();
				return true;
			}
		}
		return false;
	}

	private bool CheckMarksmanAchievement()
	{
		if (GameplayCommons.Instance.playersTankController.PlayerDead)
		{
			return false;
		}
		int num = achievementsLevels[11];
		if (num < maxAchievementLevel)
		{
			int num2 = achievementsValues[11][num];
			int stat = GameplayCommons.Instance.levelStateController.currentGameStats.GameStatistics.GetStat(GameStatistics.Stat.TimesHit);
			float num3 = GameplayCommons.Instance.levelStateController.currentGameStats.GameStatistics.GetStat(GameStatistics.Stat.TimesMissed);
			if (stat < 1)
			{
				return false;
			}
			int num4 = Mathf.CeilToInt((float)stat / ((float)stat + num3) * 100f);
			if (num4 >= num2)
			{
				achievementsLevels[11]++;
				CheckMarksmanAchievement();
				return true;
			}
		}
		return false;
	}

	private bool CheckSimpleGlobalValueAchievement(Achievement achievementType, GameStatistics.Stat statType)
	{
		int num = achievementsLevels[(int)achievementType];
		if (num < maxAchievementLevel)
		{
			int num2 = achievementsValues[(int)achievementType][num];
			int stat = GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(statType);
			if (stat >= num2)
			{
				achievementsLevels[(int)achievementType]++;
				CheckSimpleGlobalValueAchievement(achievementType, statType);
				return true;
			}
		}
		return false;
	}
}
