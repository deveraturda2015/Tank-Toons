using System;
using System.Collections.Generic;

public class GlobalGameStats
{
	private string money = EncryptString.Encrypt("0");

	public List<int> CompletedLevelsProgressIndicators = new List<int>
	{
		0
	};

	private int[] weaponsLevels = new int[15]
	{
		1,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0
	};

	private int tankSpeedLevel;

	private int tankArmorLevel;

	public string PlayerID = PlayerIDGenerator.GenerateID();

	public string LevelShareNickname = string.Empty;

	public bool DidSaveToCloud;

	private int levelsCompleted;

	public int timesLastLevelAttempted = 1;

	private bool tutorialCompleted;

	private bool weaponsTutorialPending;

	private GameStatistics gameStatistics;

	private AchievementsTracker achievementsTracker;

	private LittleTargetsTracker littleTargetsTracker;

	public bool ArenaUnlockedMessagePending;

	public bool SurvivalUnlockedMessagePending;

	public bool WeaponUpgradeMessagePending = true;

	public bool PrizeTutorialShown;

	public bool CloudSaveTutorialShown;

	public bool UseStaticControls;

	public bool RewardedAdSkipWarningShown;

	public string HiScore = EncryptString.Encrypt("0");

	public bool ShowGamePads = true;

	public bool DoubleCoinsPurchased;

	private string maxLevelIncome = EncryptString.Encrypt("0");

	public DateTime LastTimeGotPrize = DateTime.Now;

	public int PrizeLevel;

	public float ControlsSensitivityCoefficient = 1f;

	public bool RatedGame;

	public int AskForReviewFactor;

	public int SequentalWinsCount;

	public int SequentalFailsCount;

	public int ProgressIndicator;

	public int SaveFileCounter;

	public bool AutoAimEnabled = true;

	public bool ScreenShakeEnabled = true;

	public bool ScreenFlashesEnabled = true;

	public bool FirstRewadPrizePicked;

	public bool ArmorUpgradeTutorialShown;

	public bool ShowNewUserLevelMenuNotification;

	public bool EngineSoundEnabled = true;

	public bool FancyExplosionsEffectEnabled = true;

	public bool isPayingPlayer;

	public int EditorItemsLevel = 1;

	public int GameVersion = 1;

	public int StaticMovementPadPositionX = -1;

	public int StaticMovementPadPositionY = -1;

	public int StaticShootingPadPositionX = -1;

	public int StaticShootingPadPositionY = -1;

	public bool IsPayingPlayer
	{
		get
		{
			return isPayingPlayer || DoubleCoinsPurchased;
		}
		set
		{
			isPayingPlayer = value;
		}
	}

	public AchievementsTracker AchievementsTracker => achievementsTracker;

	public GameStatistics GameStatistics => gameStatistics;

	public LittleTargetsTracker LittleTargetsTracker => littleTargetsTracker;

	public int[] WeaponsLevels
	{
		get
		{
			return weaponsLevels;
		}
		set
		{
			weaponsLevels = value;
		}
	}

	public int AvailableWeaponsCount
	{
		get
		{
			int num = 0;
			for (int i = 0; i < weaponsLevels.Length; i++)
			{
				if (weaponsLevels[i] > 0)
				{
					num++;
				}
			}
			return num;
		}
	}

	public int TankSpeedLevel
	{
		get
		{
			return tankSpeedLevel;
		}
		set
		{
			tankSpeedLevel = value;
		}
	}

	public bool TutorialCompleted
	{
		get
		{
			return tutorialCompleted;
		}
		set
		{
			tutorialCompleted = value;
		}
	}

	public bool WeaponsTutorialPending
	{
		get
		{
			return weaponsTutorialPending;
		}
		set
		{
			weaponsTutorialPending = value;
		}
	}

	public int TankArmorLevel
	{
		get
		{
			return tankArmorLevel;
		}
		set
		{
			tankArmorLevel = value;
		}
	}

	public int LevelsCompleted
	{
		get
		{
			return levelsCompleted;
		}
		set
		{
			levelsCompleted = value;
		}
	}

	public int Money
	{
		get
		{
			return int.Parse(EncryptString.Decrypt(money));
		}
		set
		{
			money = EncryptString.Encrypt(value.ToString());
		}
	}

	public int MaxLevelIncome
	{
		get
		{
			return int.Parse(EncryptString.Decrypt(maxLevelIncome));
		}
		set
		{
			maxLevelIncome = EncryptString.Encrypt(value.ToString());
		}
	}

	public int IntHiScore => int.Parse(EncryptString.Decrypt(HiScore));

	public GlobalGameStats()
	{
		gameStatistics = new GameStatistics(isGlobal: true);
		achievementsTracker = new AchievementsTracker();
		littleTargetsTracker = new LittleTargetsTracker();
	}

	public void MaximizeProgressValues()
	{
		tankSpeedLevel = PlayerBalance.speedUpgradeCost.Length;
		tankArmorLevel = PlayerBalance.armorUpgradeCost.Length;
		for (int i = 0; i < weaponsLevels.Length; i++)
		{
			weaponsLevels[i] = PlayerBalance.WeaponsUpgradesCost[0].Length;
		}
	}

	public void MinimizeProgressValues()
	{
		tankSpeedLevel = 0;
		tankArmorLevel = 0;
		Money = 0;
		for (int i = 0; i < weaponsLevels.Length; i++)
		{
			if (i == 0)
			{
				weaponsLevels[i] = 1;
			}
			else
			{
				weaponsLevels[i] = 0;
			}
		}
	}

	public void UpgradeTankArmor()
	{
		tankArmorLevel++;
	}

	public void UpgradeTankSpeed()
	{
		tankSpeedLevel++;
	}

	public void IncreaseMoney(int amount)
	{
		money = EncryptString.Encrypt((int.Parse(EncryptString.Decrypt(money)) + amount).ToString());
		gameStatistics.IncreaseStat(GameStatistics.Stat.TotalMoneyCollected, amount);
	}

	public void IncreaseScore(int amount)
	{
		HiScore = EncryptString.Encrypt((int.Parse(EncryptString.Decrypt(HiScore)) + amount).ToString());
	}

	public void DecreaseMoney(int amount)
	{
		money = EncryptString.Encrypt((int.Parse(EncryptString.Decrypt(money.ToString())) - amount).ToString());
		gameStatistics.IncreaseStat(GameStatistics.Stat.TotalMoneySpent, amount);
	}
}
