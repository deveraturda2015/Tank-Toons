using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class SavegameData
{
	public const int currentSaveFileVersion = 1;

	private Dictionary<string, string> Data;

	[CompilerGenerated]
	private static Converter<string, int> _003C_003Ef__mg_0024cache0;

	[CompilerGenerated]
	private static Converter<string, int> _003C_003Ef__mg_0024cache1;

	[CompilerGenerated]
	private static Converter<string, int> _003C_003Ef__mg_0024cache2;

	[CompilerGenerated]
	private static Converter<string, int> _003C_003Ef__mg_0024cache3;

	public int EditorItemsLevel
	{
		get
		{
			int num = -1;
			if (Data.ContainsKey(EncryptString.Encrypt("EditorItemsLevel")))
			{
				return int.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("EditorItemsLevel")]));
			}
			return GlobalCommons.Instance.levelsContainer.GetItemsLevel(GlobalCommons.Instance.globalGameStats.LevelsCompleted);
		}
	}

	public int SequentalFailsCount
	{
		get
		{
			int result = 0;
			if (Data.ContainsKey(EncryptString.Encrypt("SequentalFailsCount")))
			{
				result = int.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("SequentalFailsCount")]));
			}
			return result;
		}
	}

	public int GameVersion
	{
		get
		{
			int result = 0;
			if (Data.ContainsKey(EncryptString.Encrypt("GameVersion")))
			{
				result = int.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("GameVersion")]));
			}
			return result;
		}
	}

	public int SequentalWinsCount
	{
		get
		{
			int result = 0;
			if (Data.ContainsKey(EncryptString.Encrypt("SequentalWinsCount")))
			{
				result = int.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("SequentalWinsCount")]));
			}
			return result;
		}
	}

	public int StaticMovementPadPositionX
	{
		get
		{
			int result = -1;
			if (Data.ContainsKey(EncryptString.Encrypt("StaticMovementPadPositionX")))
			{
				result = int.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("StaticMovementPadPositionX")]));
			}
			return result;
		}
	}

	public int StaticMovementPadPositionY
	{
		get
		{
			int result = -1;
			if (Data.ContainsKey(EncryptString.Encrypt("StaticMovementPadPositionY")))
			{
				result = int.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("StaticMovementPadPositionY")]));
			}
			return result;
		}
	}

	public int StaticShootingPadPositionX
	{
		get
		{
			int result = -1;
			if (Data.ContainsKey(EncryptString.Encrypt("StaticShootingPadPositionX")))
			{
				result = int.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("StaticShootingPadPositionX")]));
			}
			return result;
		}
	}

	public int StaticShootingPadPositionY
	{
		get
		{
			int result = -1;
			if (Data.ContainsKey(EncryptString.Encrypt("StaticShootingPadPositionY")))
			{
				result = int.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("StaticShootingPadPositionY")]));
			}
			return result;
		}
	}

	public float ControlsSensitivityCoefficient
	{
		get
		{
			float result = 1f;
			if (Data.ContainsKey(EncryptString.Encrypt("ControlsSensitivityCoefficient")))
			{
				result = float.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("ControlsSensitivityCoefficient")]), CultureInfo.InvariantCulture);
			}
			return result;
		}
	}

	public int CurrentLittleTargetCoinsBonus
	{
		get
		{
			int result = 50;
			if (Data.ContainsKey(EncryptString.Encrypt("CurrentLittleTargetCoinsBonus")))
			{
				result = int.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("CurrentLittleTargetCoinsBonus")]));
			}
			return result;
		}
	}

	public Vector2 EditorLevelCoords
	{
		get
		{
			Vector2 result = Vector2.zero;
			if (Data.ContainsKey(EncryptString.Encrypt("EditorLevelCoords")))
			{
				string text = EncryptString.Decrypt(Data[EncryptString.Encrypt("EditorLevelCoords")]);
				result = new Vector2(float.Parse(text.Substring(0, text.IndexOf(':')), CultureInfo.InvariantCulture), float.Parse(text.Substring(text.IndexOf(':') + 1), CultureInfo.InvariantCulture));
			}
			return result;
		}
	}

	public int SaveFileCounter
	{
		get
		{
			int result = 0;
			if (Data.ContainsKey(EncryptString.Encrypt("SaveFileCounter")))
			{
				result = int.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("SaveFileCounter")]));
			}
			return result;
		}
	}

	public bool FancyExplosionsEffectEnabled
	{
		get
		{
			if (Data.ContainsKey(EncryptString.Encrypt("FancyExplosionsEffectEnabled")))
			{
				return bool.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("FancyExplosionsEffectEnabled")]));
			}
			return true;
		}
	}

	public bool ArmorUpgradeTutorialShown
	{
		get
		{
			if (Data.ContainsKey(EncryptString.Encrypt("ArmorUpgradeTutorialShown")))
			{
				return bool.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("ArmorUpgradeTutorialShown")]));
			}
			return true;
		}
	}

	public bool EngineSoundEnabled
	{
		get
		{
			if (Data.ContainsKey(EncryptString.Encrypt("EngineSoundEnabled")))
			{
				return bool.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("EngineSoundEnabled")]));
			}
			return true;
		}
	}

	public bool AutoAimEnabled
	{
		get
		{
			if (Data.ContainsKey(EncryptString.Encrypt("AutoAimEnabled")))
			{
				return bool.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("AutoAimEnabled")]));
			}
			return true;
		}
	}

	public bool PrizeTutorialShown
	{
		get
		{
			if (Data.ContainsKey(EncryptString.Encrypt("PrizeTutorialShown")))
			{
				return bool.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("PrizeTutorialShown")]));
			}
			return false;
		}
	}

	public bool ScreenShakeEnabled
	{
		get
		{
			if (Data.ContainsKey(EncryptString.Encrypt("ScreenShakeEnabled")))
			{
				return bool.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("ScreenShakeEnabled")]));
			}
			return true;
		}
	}

	public bool ScreenFlashesEnabled
	{
		get
		{
			if (Data.ContainsKey(EncryptString.Encrypt("ScreenFlashesEnabled")))
			{
				return bool.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("ScreenFlashesEnabled")]));
			}
			return true;
		}
	}

	public bool IsPayingPlayer
	{
		get
		{
			if (Data.ContainsKey(EncryptString.Encrypt("IsPayingPlayer")))
			{
				return bool.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("IsPayingPlayer")]));
			}
			return DoubleCoinsPurchased;
		}
	}

	public bool FirstRewadPrizePicked
	{
		get
		{
			if (Data.ContainsKey(EncryptString.Encrypt("FirstRewadPrizePicked")))
			{
				return bool.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("FirstRewadPrizePicked")]));
			}
			return false;
		}
	}

	public List<int> CompletedLevelsProgressIndicators
	{
		get
		{
			if (Data.ContainsKey(EncryptString.Encrypt("CompletedLevelsProgressIndicators")))
			{
				return Array.ConvertAll(EncryptString.Decrypt(Data[EncryptString.Encrypt("CompletedLevelsProgressIndicators")]).Split(','), int.Parse).OfType<int>().ToList();
			}
			List<int> list = new List<int>();
			list.Add(0);
			return list;
		}
	}

	public List<string> CurrentEditorLevel
	{
		get
		{
			if (Data.ContainsKey(EncryptString.Encrypt("CurrentEditorLevel")))
			{
				return EncryptString.Decrypt(Data[EncryptString.Encrypt("CurrentEditorLevel")]).Split(',').OfType<string>()
					.ToList();
			}
			return null;
		}
	}

	public int[] AchievementLevels => Array.ConvertAll(EncryptString.Decrypt(Data[EncryptString.Encrypt("achievementsLevels")]).Split(','), int.Parse);

	public TileMap.TilesType CurrentEditorLevelSceneryType
	{
		get
		{
			TileMap.TilesType result = TileMap.TilesType.SummerTiles;
			if (Data.ContainsKey(EncryptString.Encrypt("CurrentEditorLevelSceneryType")))
			{
				result = (TileMap.TilesType)int.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("CurrentEditorLevelSceneryType")]));
			}
			return result;
		}
	}

	public int TimesLastLevelAttempted
	{
		get
		{
			int result = 1;
			if (Data.ContainsKey(EncryptString.Encrypt("timesLastLevelAttempted")))
			{
				result = int.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("timesLastLevelAttempted")]));
			}
			return result;
		}
	}

	public int Money => int.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("money")]));

	public int[] WeaponsLevels => Array.ConvertAll(EncryptString.Decrypt(Data[EncryptString.Encrypt("weaponsLevels")]).Split(','), int.Parse);

	public int TankSpeedLevel => int.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("tankSpeedLevel")]));

	public int TankArmorLevel => int.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("tankArmorLevel")]));

	public int LevelsCompleted => int.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("levelsCompleted")]));

	public int[] StatsValues => Array.ConvertAll(EncryptString.Decrypt(Data[EncryptString.Encrypt("statsValues")]).Split(','), int.Parse);

	public int CurrentTargetValue => int.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("currentLittleTargetValue")]));

	public int CurrentTargetIndex => int.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("currentLittleTargetIndex")]));

	public string HiScore => EncryptString.Decrypt(Data[EncryptString.Encrypt("HiScore")]);

	public int MaxLevelIncome => int.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("MaxLevelIncome")]));

	public int PrizeLevel => int.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("PrizeLevel")]));

	public string PlayerID
	{
		get
		{
			if (Data.ContainsKey(EncryptString.Encrypt("PlayerID")))
			{
				return EncryptString.Decrypt(Data[EncryptString.Encrypt("PlayerID")]);
			}
			return GlobalCommons.Instance.globalGameStats.PlayerID;
		}
	}

	public string LevelShareNickname
	{
		get
		{
			if (Data.ContainsKey(EncryptString.Encrypt("LevelShareNickname")))
			{
				return EncryptString.Decrypt(Data[EncryptString.Encrypt("LevelShareNickname")]);
			}
			return GlobalCommons.Instance.globalGameStats.LevelShareNickname;
		}
	}

	public int ProgressIndicator
	{
		get
		{
			int result = 0;
			if (Data.ContainsKey(EncryptString.Encrypt("ProgressIndicator")))
			{
				result = int.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("ProgressIndicator")]));
			}
			return result;
		}
	}

	public DateTime LastTimeGotPrize => DateTime.ParseExact(EncryptString.Decrypt(Data[EncryptString.Encrypt("LastTimeGotPrize")]), "yyyy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture);

	public DateTime SaveTime
	{
		get
		{
			if (Data.ContainsKey(EncryptString.Encrypt("SaveTime")))
			{
				return DateTime.ParseExact(EncryptString.Decrypt(Data[EncryptString.Encrypt("SaveTime")]), "yyyy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture);
			}
			return new DateTime(1999, 1, 1, 1, 1, 1);
		}
	}

	public int SaveFileVersion => int.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("saveFileVersion")]));

	public bool TutorialCompleted => bool.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("tutorialCompleted")]));

	public bool ShowGamePads
	{
		get
		{
			if (Data.ContainsKey(EncryptString.Encrypt("showGamePads")))
			{
				return bool.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("showGamePads")]));
			}
			return true;
		}
	}

	public bool ShowNewUserLevelMenuNotification
	{
		get
		{
			if (Data.ContainsKey(EncryptString.Encrypt("ShowNewUserLevelMenuNotification")))
			{
				return bool.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("ShowNewUserLevelMenuNotification")]));
			}
			return true;
		}
	}

	public bool DidSaveToCloud
	{
		get
		{
			if (Data.ContainsKey(EncryptString.Encrypt("DidSaveToCloud")))
			{
				return bool.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("DidSaveToCloud")]));
			}
			return true;
		}
	}

	public bool CloudSaveTutorialShown
	{
		get
		{
			if (Data.ContainsKey(EncryptString.Encrypt("CloudSaveTutorialShown")))
			{
				return bool.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("CloudSaveTutorialShown")]));
			}
			return false;
		}
	}

	public bool UseStaticControls
	{
		get
		{
			if (Data.ContainsKey(EncryptString.Encrypt("UseStaticControls")))
			{
				return bool.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("UseStaticControls")]));
			}
			return false;
		}
	}

	public bool WeaponsTutorialPending => bool.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("weaponsTutorialPending")]));

	public bool SurvivalUnlockedMessagePending => bool.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("SurvivalUnlockedMessagePending")]));

	public bool ArenaUnlockedMessagePending => bool.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("ArenaUnlockedMessagePending")]));

	public bool WeaponUpgradeMessagePending => bool.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("WeaponUpgradeMessagePending")]));

	public bool RewardedAdSkipWarningShown => bool.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("RewardedAdSkipWarningShown")]));

	public bool RatedGame
	{
		get
		{
			if (Data.ContainsKey(EncryptString.Encrypt("RatedGame")))
			{
				return bool.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("RatedGame")]));
			}
			return false;
		}
	}

	public bool DoubleCoinsPurchased => bool.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("DoubleCoinsPurchased")]));

	public bool MusicMuted => bool.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("musicMuted")]));

	public bool SoundMuted => bool.Parse(EncryptString.Decrypt(Data[EncryptString.Encrypt("soundMuted")]));

	public SavegameData(int[] achievementLevels, int money, int[] weaponsLevels, int tankSpeedLevel, int tankArmorLevel, int[] statsValues, int currentLittleTargetIndex, int currentLittleTargetValue, int levelsCompleted, bool tutorialCompleted, bool weaponsTutorialPending, bool soundMuted, bool musicMuted, bool ArenaUnlockedMessagePending, bool SurvivalUnlockedMessagePending, bool WeaponUpgradeMessagePending, int HiScore, bool RewardedAdSkipWarningShown, bool DoubleCoinsPurchased, int MaxLevelIncome, DateTime LastTimeGotPrize, int PrizeLevel, int timesLastLevelAttempted, bool RatedGame, int ProgressIndicator, bool AutoAimEnabled, bool ScreenShakeEnabled, bool ScreenFlashesEnabled, bool FirstRewadPrizePicked, int SaveFileCounter, bool PrizeTutorialShown, bool ArmorUpgradeTutorialShown, bool EngineSoundEnabled, DateTime SaveTime, List<int> CompletedLevelsProgressIndicators, bool FancyExplosionsEffectEnabled, int CurrentLittleTargetCoinsBonus, List<string> CurrentEditorLevel, TileMap.TilesType CurrentEditorLevelSceneryType, int EditorItemsLevel, string PlayerID, bool IsPayingPlayer, Vector2 EditorLevelCoords, int SequentalWinsCount, int SequentalFailsCount, int GameVersion, string LevelShareNickname, bool showGamePads, bool ShowNewUserLevelMenuNotification, bool DidSaveToCloud, bool CloudSaveTutorialShown, bool UseStaticControls, int StaticMovementPadPositionX, int StaticMovementPadPositionY, int StaticShootingPadPositionX, int StaticShootingPadPositionY, float ControlsSensitivityCoefficient)
	{
		Data = new Dictionary<string, string>();
		Data.Add(EncryptString.Encrypt("GameVersion"), EncryptString.Encrypt(GameVersion.ToString()));
		Data.Add(EncryptString.Encrypt("SequentalWinsCount"), EncryptString.Encrypt(SequentalWinsCount.ToString()));
		Data.Add(EncryptString.Encrypt("StaticMovementPadPositionX"), EncryptString.Encrypt(StaticMovementPadPositionX.ToString()));
		Data.Add(EncryptString.Encrypt("StaticMovementPadPositionY"), EncryptString.Encrypt(StaticMovementPadPositionY.ToString()));
		Data.Add(EncryptString.Encrypt("StaticShootingPadPositionX"), EncryptString.Encrypt(StaticShootingPadPositionX.ToString()));
		Data.Add(EncryptString.Encrypt("StaticShootingPadPositionY"), EncryptString.Encrypt(StaticShootingPadPositionY.ToString()));
		Data.Add(EncryptString.Encrypt("ControlsSensitivityCoefficient"), EncryptString.Encrypt(ControlsSensitivityCoefficient.ToString(CultureInfo.InvariantCulture)));
		Data.Add(EncryptString.Encrypt("SequentalFailsCount"), EncryptString.Encrypt(SequentalFailsCount.ToString()));
		Data.Add(EncryptString.Encrypt("PlayerID"), EncryptString.Encrypt(PlayerID));
		Data.Add(EncryptString.Encrypt("LevelShareNickname"), EncryptString.Encrypt(LevelShareNickname));
		Data.Add(EncryptString.Encrypt("LastTimeGotPrize"), EncryptString.Encrypt($"{LastTimeGotPrize:yyyy-MM-dd_HH-mm-ss}"));
		Data.Add(EncryptString.Encrypt("PrizeLevel"), EncryptString.Encrypt(PrizeLevel.ToString()));
		Data.Add(EncryptString.Encrypt("MaxLevelIncome"), EncryptString.Encrypt(MaxLevelIncome.ToString()));
		Data.Add(EncryptString.Encrypt("DoubleCoinsPurchased"), EncryptString.Encrypt(DoubleCoinsPurchased.ToString()));
		Data.Add(EncryptString.Encrypt("RewardedAdSkipWarningShown"), EncryptString.Encrypt(RewardedAdSkipWarningShown.ToString()));
		Data.Add(EncryptString.Encrypt("HiScore"), EncryptString.Encrypt(HiScore.ToString()));
		Data.Add(EncryptString.Encrypt("WeaponUpgradeMessagePending"), EncryptString.Encrypt(WeaponUpgradeMessagePending.ToString()));
		Data.Add(EncryptString.Encrypt("ArenaUnlockedMessagePending"), EncryptString.Encrypt(ArenaUnlockedMessagePending.ToString()));
		Data.Add(EncryptString.Encrypt("SurvivalUnlockedMessagePending"), EncryptString.Encrypt(SurvivalUnlockedMessagePending.ToString()));
		Data.Add(EncryptString.Encrypt("soundMuted"), EncryptString.Encrypt(soundMuted.ToString()));
		Data.Add(EncryptString.Encrypt("musicMuted"), EncryptString.Encrypt(musicMuted.ToString()));
		Data.Add(EncryptString.Encrypt("weaponsTutorialPending"), EncryptString.Encrypt(weaponsTutorialPending.ToString()));
		Data.Add(EncryptString.Encrypt("tutorialCompleted"), EncryptString.Encrypt(tutorialCompleted.ToString()));
		Data.Add(EncryptString.Encrypt("showGamePads"), EncryptString.Encrypt(showGamePads.ToString()));
		Data.Add(EncryptString.Encrypt("ShowNewUserLevelMenuNotification"), EncryptString.Encrypt(ShowNewUserLevelMenuNotification.ToString()));
		Data.Add(EncryptString.Encrypt("DidSaveToCloud"), EncryptString.Encrypt(DidSaveToCloud.ToString()));
		Data.Add(EncryptString.Encrypt("CloudSaveTutorialShown"), EncryptString.Encrypt(CloudSaveTutorialShown.ToString()));
		Data.Add(EncryptString.Encrypt("UseStaticControls"), EncryptString.Encrypt(UseStaticControls.ToString()));
		Data.Add(EncryptString.Encrypt("achievementsLevels"), EncryptString.Encrypt(string.Join(",", Array.ConvertAll(achievementLevels, (int i) => i.ToString()))));
		Data.Add(EncryptString.Encrypt("levelsCompleted"), EncryptString.Encrypt(levelsCompleted.ToString()));
		Data.Add(EncryptString.Encrypt("money"), EncryptString.Encrypt(money.ToString()));
		Data.Add(EncryptString.Encrypt("weaponsLevels"), EncryptString.Encrypt(string.Join(",", Array.ConvertAll(weaponsLevels, (int i) => i.ToString()))));
		Data.Add(EncryptString.Encrypt("tankSpeedLevel"), EncryptString.Encrypt(tankSpeedLevel.ToString()));
		Data.Add(EncryptString.Encrypt("tankArmorLevel"), EncryptString.Encrypt(tankArmorLevel.ToString()));
		Data.Add(EncryptString.Encrypt("statsValues"), EncryptString.Encrypt(string.Join(",", Array.ConvertAll(statsValues, (int i) => i.ToString()))));
		Data.Add(EncryptString.Encrypt("currentLittleTargetIndex"), EncryptString.Encrypt(currentLittleTargetIndex.ToString()));
		Data.Add(EncryptString.Encrypt("currentLittleTargetValue"), EncryptString.Encrypt(currentLittleTargetValue.ToString()));
		Data.Add(EncryptString.Encrypt("saveFileVersion"), EncryptString.Encrypt(1.ToString()));
		Data.Add(EncryptString.Encrypt("timesLastLevelAttempted"), EncryptString.Encrypt(timesLastLevelAttempted.ToString()));
		Dictionary<string, string> data = Data;
		string key = EncryptString.Encrypt("CurrentEditorLevelSceneryType");
		int num = (int)CurrentEditorLevelSceneryType;
		data.Add(key, EncryptString.Encrypt(num.ToString()));
		Data.Add(EncryptString.Encrypt("RatedGame"), EncryptString.Encrypt(RatedGame.ToString()));
		Data.Add(EncryptString.Encrypt("ProgressIndicator"), EncryptString.Encrypt(ProgressIndicator.ToString()));
		Data.Add(EncryptString.Encrypt("AutoAimEnabled"), EncryptString.Encrypt(AutoAimEnabled.ToString()));
		Data.Add(EncryptString.Encrypt("ScreenShakeEnabled"), EncryptString.Encrypt(ScreenShakeEnabled.ToString()));
		Data.Add(EncryptString.Encrypt("ScreenFlashesEnabled"), EncryptString.Encrypt(ScreenFlashesEnabled.ToString()));
		Data.Add(EncryptString.Encrypt("FirstRewadPrizePicked"), EncryptString.Encrypt(FirstRewadPrizePicked.ToString()));
		Data.Add(EncryptString.Encrypt("IsPayingPlayer"), EncryptString.Encrypt(IsPayingPlayer.ToString()));
		Data.Add(EncryptString.Encrypt("SaveFileCounter"), EncryptString.Encrypt(SaveFileCounter.ToString()));
		Data.Add(EncryptString.Encrypt("PrizeTutorialShown"), EncryptString.Encrypt(PrizeTutorialShown.ToString()));
		Data.Add(EncryptString.Encrypt("ArmorUpgradeTutorialShown"), EncryptString.Encrypt(ArmorUpgradeTutorialShown.ToString()));
		Data.Add(EncryptString.Encrypt("EngineSoundEnabled"), EncryptString.Encrypt(EngineSoundEnabled.ToString()));
		Data.Add(EncryptString.Encrypt("SaveTime"), EncryptString.Encrypt($"{SaveTime:yyyy-MM-dd_HH-mm-ss}"));
		Data.Add(EncryptString.Encrypt("CompletedLevelsProgressIndicators"), EncryptString.Encrypt(string.Join(",", Array.ConvertAll(CompletedLevelsProgressIndicators.ToArray(), (int i) => i.ToString()))));
		Data.Add(EncryptString.Encrypt("FancyExplosionsEffectEnabled"), EncryptString.Encrypt(FancyExplosionsEffectEnabled.ToString()));
		Data.Add(EncryptString.Encrypt("CurrentLittleTargetCoinsBonus"), EncryptString.Encrypt(CurrentLittleTargetCoinsBonus.ToString()));
		Data.Add(EncryptString.Encrypt("EditorItemsLevel"), EncryptString.Encrypt(EditorItemsLevel.ToString()));
		Data.Add(EncryptString.Encrypt("EditorLevelCoords"), EncryptString.Encrypt(EditorLevelCoords.x.ToString(CultureInfo.InvariantCulture) + ":" + EditorLevelCoords.y.ToString(CultureInfo.InvariantCulture)));
		if (CurrentEditorLevel != null)
		{
			Data.Add(EncryptString.Encrypt("CurrentEditorLevel"), EncryptString.Encrypt(string.Join(",", Array.ConvertAll(CurrentEditorLevel.ToArray(), (string i) => i.ToString()))));
		}
	}
}
