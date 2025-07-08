using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GlobalCommons : MonoBehaviour
{
	internal enum GameplayModes
	{
		RegularLevel,
		ArenaLevel,
		SurvivalLevel,
		TutorialLevel,
		EditorLevel,
		CustomLevel
	}

	public enum LevelResolution
	{
		Failed,
		Completed
	}

	private static GlobalCommons instance;

	internal GlobalGameStats globalGameStats;

	internal CachedWebRequests CachedWebRequests;

	internal MessagesController MessagesController;

	internal StateFaderController StateFaderController;

	internal SetCanvasBounds CanvasBoundsController;

	internal float HPBarShakeFactor = 0.008f;

	internal float gridSize = 1.09f;

	internal float horizontalEnemyDisableTreshhold;

	internal float verticalEnemyDisableTreshhold;

	internal float verticalCameraBorderWithCompensation;

	internal float horizontalCameraBorderWithCompensation;

	internal float frameTime;

	internal int vsyncCount = 1;
 
    private int framerateToSet = 60;

	internal int AnalogControlsFilterMilliseconds;

	internal bool UseStaticControlsAsDefault;

	internal CloudSaveManager SaveManager;

	internal const float MaxTankTurretShiftFactor = 0.1f;

	internal const float TankTurretShiftReturnFactor = 1.25f;

	internal LevelsContainer levelsContainer;

	private int SelectedLevel = -1;

	internal GameplayModes gameplayMode;

	internal float analogDeadzone;

	private float analogMax;

	internal AdsProcessor AdsProcessor;

	internal const float MINIMUM_SMALL_HP_BAR_WIDTH = 0.05f;

	internal const float SMALL_HP_BAR_SCALE_SPEED_FACTOR = 10f;

	internal const float UI_ITEM_SLIDE_FACTOR = 37f;

	internal const float UI_ITEM_ROTATION_FACTOR = 25f;

	internal PromotionController PromotionController;

	private List<TileMap.TilesType> menuTilesTypesSequence;

	private int menuTilesTypesSequenceIndex;

	internal static int FlashFrameCount = 3;

	private const string PLAYERPREFS_SAVE_STRING = "sdat";

	internal WeaponTypes LastSelectedWeapon;

	internal const string PASS_PHRASE = "spphr";

	private string saveDataPath = string.Empty;

	internal LevelResolution lastLevelResolution;

	internal string SceneToTransferTo = "MainMenu";

	private DateTime LostFocusTimestamp = DateTime.Now;

	private GameObject PauseItemsGO;

	internal string SaveDataPath
	{
		get
		{
			if (string.IsNullOrEmpty(saveDataPath))
			{
				saveDataPath = Path.Combine(Application.persistentDataPath, "atsd.dat");
			}
			return saveDataPath;
		}
	}

	internal float AnalogMax => analogMax * globalGameStats.ControlsSensitivityCoefficient;

	public int SelectedLevelBalanceFactor
	{
		get
		{
			if (SelectedLevel <= levelsContainer.LevelsCount)
			{
				return SelectedLevel;
			}
			return levelsContainer.LevelsCount;
		}
	}

	public int ActualSelectedLevel
	{
		get
		{
			return SelectedLevel;
		}
		set
		{
			SelectedLevel = value;
		}
	}

	public static GlobalCommons Instance
	{
		get
		{
			return instance;
		}
		set
		{
			if (instance == null)
			{
				instance = value;
			}
		}
	}

	internal float DynamicHorizontalScreenBorderPlusOneCell => Camera.main.orthographicSize * Camera.main.aspect + gridSize;

	internal float DynamicVerticalScreenBorderDistancePlusOneCell => Camera.main.orthographicSize + gridSize;

	internal float DynamicVerticalCameraBorder => Camera.main.orthographicSize;

	internal float DynamicHorizontalCameraBorder => Camera.main.orthographicSize * Camera.main.aspect;

	private void Awake()
	{
		bool isEditor = Application.isEditor;
		if (Instance == null)
		{
			Instance = this;
			ProcessEarlyGameLoading();
			Initialize();
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}
	}

	internal static void ProcessWithCoroutine(Action action)
	{
		if (instance != null)
		{
			instance.StartCoroutine(instance.CallbackCoroutine(action));
		}
		else
		{
			action();
		}
	}

	private IEnumerator CallbackCoroutine(Action action)
	{
		yield return new WaitForEndOfFrame();
		action?.Invoke();
	}

	private void ProcessEarlyGameLoading()
	{
		if (globalGameStats == null)
		{
			globalGameStats = new GlobalGameStats();
		}
		LoadGame();
		SetDefaultStaticControlsValuesIfNeeded();
	}

	private void SetDefaultStaticControlsValuesIfNeeded()
	{
		if (globalGameStats.StaticMovementPadPositionX == -1 && globalGameStats.StaticMovementPadPositionY == -1 && globalGameStats.StaticShootingPadPositionX == -1 && globalGameStats.StaticShootingPadPositionY == -1)
		{
			SetDefaultStaticControlsValues();
		}
	}

	internal void SetDefaultStaticControlsValues()
	{
		globalGameStats.StaticMovementPadPositionX = Mathf.RoundToInt((float)Screen.height / 3f);
		globalGameStats.StaticMovementPadPositionY = Mathf.RoundToInt((float)Screen.height / 3f);
		globalGameStats.StaticShootingPadPositionX = Mathf.RoundToInt((float)Screen.width - (float)Screen.height / 3f);
		globalGameStats.StaticShootingPadPositionY = Mathf.RoundToInt((float)Screen.height / 3f);
	}

	private void TestLevelGenerator()
	{
		int levelsCompleted = Instance.globalGameStats.LevelsCompleted;
		for (int i = 5; i < 76; i++)
		{
			Instance.globalGameStats.LevelsCompleted = i;
			for (int j = 0; j < 100; j++)
			{
				BoardCreator boardCreator = new BoardCreator();
				boardCreator.GenerateLevel();
			}
		}
		Instance.globalGameStats.LevelsCompleted = levelsCompleted;
		DebugHelper.Log("level generator test complete...");
	}

	private void Update()
	{
		MessagesController.Update();
		CachedWebRequests.UpdateCachedRequests();
		StateFaderController.Update();
	}

	private void InitMenuTilesTypesSequence()
	{
		menuTilesTypesSequence = new List<TileMap.TilesType>();
		object list2;
		if (UnityEngine.Random.value > 0.5f)
		{
			List<TileMap.TilesType> list = new List<TileMap.TilesType>();
			list.Add(TileMap.TilesType.SummerTiles);
			list.Add(TileMap.TilesType.DesertTiles);
			list.Add(TileMap.TilesType.WinterTiles);
			list2 = list;
		}
		else
		{
			List<TileMap.TilesType> list = new List<TileMap.TilesType>();
			list.Add(TileMap.TilesType.SummerTiles);
			list.Add(TileMap.TilesType.WinterTiles);
			list.Add(TileMap.TilesType.DesertTiles);
			list2 = list;
		}
		List<TileMap.TilesType> list3 = (List<TileMap.TilesType>)list2;
		System.Random rnd = new System.Random();
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < list3.Count; j++)
			{
				menuTilesTypesSequence.Add(list3[j]);
			}
			do
			{
				list3 = (from item in list3
					orderby rnd.Next()
					select item).ToList();
			}
			while (list3[0] == menuTilesTypesSequence[menuTilesTypesSequence.Count - 1]);
		}
	}

	public TileMap.TilesType GetTilesTypeForMenu()
	{
		TileMap.TilesType result = menuTilesTypesSequence[menuTilesTypesSequenceIndex];
		menuTilesTypesSequenceIndex++;
		if (menuTilesTypesSequenceIndex == menuTilesTypesSequence.Count)
		{
			menuTilesTypesSequenceIndex = 0;
		}
		return result;
	}

	private void Initialize()
	{
		CanvasBoundsController = instance.gameObject.AddComponent<SetCanvasBounds>();
		RemoteSettings.Updated += HandleRemoteSettingsUpdate;
		CachedWebRequests = new CachedWebRequests();
		StateFaderController = new StateFaderController();
		Shader.WarmupAllShaders();
		DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
		InitMenuTilesTypesSequence();
		float num = (float)Screen.height / Screen.dpi;
		analogMax = 0.38f / num * (float)Screen.height;
		analogMax = Mathf.Clamp(analogMax, 40f, 350f);
		analogDeadzone = analogMax / 4f;
		Camera main = Camera.main;
		float num2 = CameraController.GetOrthoSize() / 5f;
		verticalCameraBorderWithCompensation = main.orthographicSize * num2;
		horizontalCameraBorderWithCompensation = main.orthographicSize * main.aspect * num2;
		verticalEnemyDisableTreshhold = main.orthographicSize * num2 * 1.45f;
		horizontalEnemyDisableTreshhold = main.orthographicSize * main.aspect * num2 * 1.45f;
		frameTime = 1f / (float)framerateToSet;
		QualitySettings.antiAliasing = 0;
		QualitySettings.shadowCascades = 0;
		QualitySettings.SetQualityLevel(0);
		Application.targetFrameRate = framerateToSet;
		Prefabs.InitializePrefabs();
		Materials.InitializeMaterials();
		Textures.InitializeTextures();
		Sprites.InitializeSprites();
		Sounds.InitializeSounds();
		levelsContainer = new LevelsContainer();
		AdsProcessor = new AdsProcessor();
		AdsProcessor.Init();
		PromotionController = new PromotionController();
		base.gameObject.AddComponent<Purchaser>();
		base.gameObject.AddComponent<RequestsHandler>();
		base.gameObject.AddComponent<SocialWorker>();
		SaveManager = new PGPSaveManager();
		MessagesController = new MessagesController();
	}

	private void HandleRemoteSettingsUpdate()
	{
		AnalogControlsFilterMilliseconds = RemoteSettings.GetInt("ANALOG_CONTROLS_FILTER_MILLISECONDS", 0);
		UseStaticControlsAsDefault = RemoteSettings.GetBool("STATIC_CONTROLS_AS_DEFAULT", defaultValue: false);
		if (globalGameStats != null && globalGameStats.ControlsSensitivityCoefficient == 1f)
		{
			globalGameStats.ControlsSensitivityCoefficient = (float)RemoteSettings.GetInt("DEFAULT_CONTROLS_SENSITIVITY_COEFFICIENT", Mathf.RoundToInt(100f)) / 100f;
		}
	}

	private void OnApplicationFocus(bool focusStatus)
	{
		globalGameStats.AskForReviewFactor = 0;
		if (focusStatus)
		{
			LostFocusTimestamp = DateTime.Now;
		}
		else if (AdsProcessor != null)
		{
			AdsProcessor.ShiftAdTimers(DateTime.Now - LostFocusTimestamp);
		}
	}

	public GameObject GetPauseItemsGO()
	{
		return PauseItemsGO;
	}

	public void SetLoadingGO(GameObject pauseItemsGameObject)
	{
		if (PauseItemsGO == null)
		{
			PauseItemsGO = UnityEngine.Object.Instantiate(pauseItemsGameObject);
			UnityEngine.Object.DontDestroyOnLoad(PauseItemsGO);
		}
	}

	private void LoadGame()
	{
		DebugHelper.Log("trying to load game");
		SavegameData savegameData = null;
		if (File.Exists(SaveDataPath))
		{
			try
			{
				FileStream fileStream = File.Open(SaveDataPath, FileMode.Open);
				savegameData = (SavegameData)new BinaryFormatter().Deserialize(fileStream);
				fileStream.Close();
				DebugHelper.Log("load through fs succeeded");
			}
			catch (Exception)
			{
				DebugHelper.Log("load through fs failed - exception");
				if (Application.isEditor)
				{
					Instance.MessagesController.ShowSimpleMessage("OMG SAVE FILE DESERILIZATION FAILED!!!");
				}
			}
		}
		else
		{
			DebugHelper.Log("load through fs failed - save file does not exist");
		}
		if (savegameData == null && PlayerPrefs.HasKey("sdat"))
		{
			try
			{
				string @string = PlayerPrefs.GetString("sdat");
				byte[] buffer = Convert.FromBase64String(@string);
				using (MemoryStream serializationStream = new MemoryStream(buffer))
				{
					savegameData = (SavegameData)new BinaryFormatter().Deserialize(serializationStream);
					DebugHelper.Log("load through pp succeeded");
				}
			}
			catch (Exception)
			{
				DebugHelper.Log("load through pp failed - parse error");
			}
		}
		else
		{
			DebugHelper.Log("load through pp failed - data does not exist");
		}
		if (savegameData != null)
		{
			DebugHelper.Log("loading game data");
			ProceedWithGameLoad(savegameData);
		}
	}

	internal bool ProceedWithGameLoad(SavegameData savegameData, bool cloudLoading = false)
	{
		try
		{
			if (cloudLoading && globalGameStats.GameVersion < savegameData.GameVersion)
			{
				return false;
			}
			for (int i = 0; i < savegameData.WeaponsLevels.Length; i++)
			{
				if (globalGameStats.WeaponsLevels.Length > i)
				{
					globalGameStats.WeaponsLevels[i] = savegameData.WeaponsLevels[i];
				}
				else
				{
					globalGameStats.WeaponsLevels[i] = 0;
				}
			}
			globalGameStats.AchievementsTracker.AllAchievementLevels = savegameData.AchievementLevels;
			globalGameStats.Money = savegameData.Money;
			globalGameStats.TankArmorLevel = savegameData.TankArmorLevel;
			globalGameStats.TankSpeedLevel = savegameData.TankSpeedLevel;
			globalGameStats.GameStatistics.StatsValues = savegameData.StatsValues;
			globalGameStats.LittleTargetsTracker.CurrentTargetIndex = savegameData.CurrentTargetIndex;
			globalGameStats.LittleTargetsTracker.CurrentTargetValue = savegameData.CurrentTargetValue;
			globalGameStats.LevelsCompleted = savegameData.LevelsCompleted;
			globalGameStats.TutorialCompleted = savegameData.TutorialCompleted;


			globalGameStats.WeaponsTutorialPending = savegameData.WeaponsTutorialPending;
			globalGameStats.SurvivalUnlockedMessagePending = savegameData.SurvivalUnlockedMessagePending;
			globalGameStats.ArenaUnlockedMessagePending = savegameData.ArenaUnlockedMessagePending;
			globalGameStats.WeaponUpgradeMessagePending = savegameData.WeaponUpgradeMessagePending;
			globalGameStats.HiScore = EncryptString.Encrypt(savegameData.HiScore);
			globalGameStats.RewardedAdSkipWarningShown = savegameData.RewardedAdSkipWarningShown;
			globalGameStats.MaxLevelIncome = savegameData.MaxLevelIncome;
			globalGameStats.LastTimeGotPrize = savegameData.LastTimeGotPrize;
			globalGameStats.PrizeLevel = savegameData.PrizeLevel;
			globalGameStats.timesLastLevelAttempted = savegameData.TimesLastLevelAttempted;
			globalGameStats.RatedGame = savegameData.RatedGame;
			globalGameStats.ProgressIndicator = savegameData.ProgressIndicator;
			globalGameStats.AutoAimEnabled = savegameData.AutoAimEnabled;
			globalGameStats.ScreenFlashesEnabled = savegameData.ScreenFlashesEnabled;
			globalGameStats.ScreenShakeEnabled = savegameData.ScreenShakeEnabled;
			globalGameStats.FirstRewadPrizePicked = savegameData.FirstRewadPrizePicked;
			globalGameStats.SaveFileCounter = savegameData.SaveFileCounter;
			globalGameStats.PrizeTutorialShown = savegameData.PrizeTutorialShown;
			globalGameStats.CloudSaveTutorialShown = savegameData.CloudSaveTutorialShown;
			globalGameStats.ArmorUpgradeTutorialShown = savegameData.ArmorUpgradeTutorialShown;
			globalGameStats.EngineSoundEnabled = savegameData.EngineSoundEnabled;
			globalGameStats.CompletedLevelsProgressIndicators = savegameData.CompletedLevelsProgressIndicators;
			globalGameStats.FancyExplosionsEffectEnabled = savegameData.FancyExplosionsEffectEnabled;
			globalGameStats.LittleTargetsTracker.CurrentLittleTargetCoinsBonus = savegameData.CurrentLittleTargetCoinsBonus;
			globalGameStats.PlayerID = savegameData.PlayerID;
			globalGameStats.IsPayingPlayer = savegameData.IsPayingPlayer;
			globalGameStats.SequentalWinsCount = savegameData.SequentalWinsCount;
			globalGameStats.SequentalFailsCount = savegameData.SequentalFailsCount;
			globalGameStats.ShowGamePads = savegameData.ShowGamePads;
			globalGameStats.ShowNewUserLevelMenuNotification = savegameData.ShowNewUserLevelMenuNotification;
			globalGameStats.DidSaveToCloud = savegameData.DidSaveToCloud;
			globalGameStats.UseStaticControls = savegameData.UseStaticControls;
			if (!cloudLoading || LevelEditorController.LevelToWorkWith == null)
			{
				LevelEditorController.LevelToWorkWithPan = savegameData.EditorLevelCoords;
				LevelEditorController.LevelToWorkWith = savegameData.CurrentEditorLevel;
				LevelEditorController.LevelToWorkWithTilesType = savegameData.CurrentEditorLevelSceneryType;
			}
			if (!cloudLoading)
			{
				globalGameStats.LevelShareNickname = savegameData.LevelShareNickname;
				SoundManager.soundMuted = savegameData.SoundMuted;
				SoundManager.musicMuted = savegameData.MusicMuted;
				globalGameStats.StaticMovementPadPositionX = savegameData.StaticMovementPadPositionX;
				globalGameStats.StaticMovementPadPositionY = savegameData.StaticMovementPadPositionY;
				globalGameStats.StaticShootingPadPositionX = savegameData.StaticShootingPadPositionX;
				globalGameStats.StaticShootingPadPositionY = savegameData.StaticShootingPadPositionY;
				globalGameStats.ControlsSensitivityCoefficient = savegameData.ControlsSensitivityCoefficient;
			}
			if (!globalGameStats.DoubleCoinsPurchased)
			{
				globalGameStats.DoubleCoinsPurchased = savegameData.DoubleCoinsPurchased;
			}
			if (globalGameStats.EditorItemsLevel < savegameData.EditorItemsLevel)
			{
				globalGameStats.EditorItemsLevel = savegameData.EditorItemsLevel;
			}
			TimeSpan t = DateTime.Now - savegameData.SaveTime;
			int num = 0;
			if (t >= TimeSpan.FromHours(150.0))
			{
				num = 6;
			}
			else if (t >= TimeSpan.FromHours(96.0))
			{
				num = 5;
			}
			else if (t >= TimeSpan.FromHours(48.0))
			{
				num = 4;
			}
			else if (t >= TimeSpan.FromHours(24.0))
			{
				num = 3;
			}
			else if (t >= TimeSpan.FromHours(8.0))
			{
				num = 2;
			}
			else if (t >= TimeSpan.FromHours(4.0))
			{
				num = 1;
			}
			if (num > 0)
			{
				if (globalGameStats.SequentalFailsCount > 0)
				{
					globalGameStats.SequentalFailsCount += num;
				}
				if (globalGameStats.SequentalWinsCount > 0)
				{
					globalGameStats.SequentalWinsCount -= num;
					if (globalGameStats.SequentalWinsCount < 0)
					{
						globalGameStats.SequentalWinsCount = 0;
					}
				}
				globalGameStats.ProgressIndicator -= Mathf.CeilToInt(Mathf.Pow(2f, num));
			}
		}
		catch (Exception)
		{
			DebugHelper.Log("Failed to load game data, probably something wrong with dict keys");
			return false;
		}
		return true;
	}

	internal SavegameData GetSavegameData()
	{
		return new SavegameData(globalGameStats.AchievementsTracker.AllAchievementLevels, globalGameStats.Money, globalGameStats.WeaponsLevels, globalGameStats.TankSpeedLevel, globalGameStats.TankArmorLevel, globalGameStats.GameStatistics.StatsValues, globalGameStats.LittleTargetsTracker.CurrentTargetIndex, globalGameStats.LittleTargetsTracker.CurrentTargetValue, globalGameStats.LevelsCompleted, globalGameStats.TutorialCompleted, globalGameStats.WeaponsTutorialPending, SoundManager.soundMuted, SoundManager.musicMuted, globalGameStats.ArenaUnlockedMessagePending, globalGameStats.SurvivalUnlockedMessagePending, globalGameStats.WeaponUpgradeMessagePending, globalGameStats.IntHiScore, globalGameStats.RewardedAdSkipWarningShown, globalGameStats.DoubleCoinsPurchased, globalGameStats.MaxLevelIncome, globalGameStats.LastTimeGotPrize, globalGameStats.PrizeLevel, globalGameStats.timesLastLevelAttempted, globalGameStats.RatedGame, globalGameStats.ProgressIndicator, globalGameStats.AutoAimEnabled, globalGameStats.ScreenShakeEnabled, globalGameStats.ScreenFlashesEnabled, globalGameStats.FirstRewadPrizePicked, globalGameStats.SaveFileCounter, globalGameStats.PrizeTutorialShown, globalGameStats.ArmorUpgradeTutorialShown, globalGameStats.EngineSoundEnabled, DateTime.Now, globalGameStats.CompletedLevelsProgressIndicators, globalGameStats.FancyExplosionsEffectEnabled, globalGameStats.LittleTargetsTracker.CurrentLittleTargetCoinsBonus, LevelEditorController.LevelToWorkWith, LevelEditorController.LevelToWorkWithTilesType, globalGameStats.EditorItemsLevel, globalGameStats.PlayerID, globalGameStats.IsPayingPlayer, LevelEditorController.LevelToWorkWithPan, globalGameStats.SequentalWinsCount, globalGameStats.SequentalFailsCount, globalGameStats.GameVersion, globalGameStats.LevelShareNickname, globalGameStats.ShowGamePads, globalGameStats.ShowNewUserLevelMenuNotification, globalGameStats.DidSaveToCloud, globalGameStats.CloudSaveTutorialShown, globalGameStats.UseStaticControls, globalGameStats.StaticMovementPadPositionX, globalGameStats.StaticMovementPadPositionY, globalGameStats.StaticShootingPadPositionX, globalGameStats.StaticShootingPadPositionY, globalGameStats.ControlsSensitivityCoefficient);
	}

	internal string GetSavegameDataString()
	{
		MemoryStream memoryStream = new MemoryStream();
		new BinaryFormatter().Serialize(memoryStream, GetSavegameData());
		return Convert.ToBase64String(memoryStream.ToArray());
	}

	public void SaveGame()
	{
		DebugHelper.Log("trying to save game");
		globalGameStats.SaveFileCounter++;
		SavegameData savegameData = GetSavegameData();
		try
		{
			FileStream fileStream = File.Create(SaveDataPath);
			new BinaryFormatter().Serialize(fileStream, savegameData);
			fileStream.Close();
			DebugHelper.Log("successful save through fs");
		}
		catch (Exception)
		{
			DebugHelper.Log("fs save failed");
		}
		try
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				new BinaryFormatter().Serialize(memoryStream, savegameData);
				string value = Convert.ToBase64String(memoryStream.ToArray());
				PlayerPrefs.SetString("sdat", value);
			}
			DebugHelper.Log("save through pp succeeded");
		}
		catch (Exception)
		{
			DebugHelper.Log("save through pp failed");
		}
	}
}
