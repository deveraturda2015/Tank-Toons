using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayCommons : MonoBehaviour
{
	private static GameplayCommons instance;

	public List<string> currentLevel;

	public const float ROTAION_SPEED_FACTOR_NORMAL = 30f;

	public const float ROTAION_SPEED_FACTOR_AUTOAIM = 12f;

	internal EnemiesTracker enemiesTracker;

	internal TouchesController touchesController;

	internal WeaponsController weaponsController;

	internal WeaponSwitchController weaponSwitchController;

	internal LevelResultsController levelResultsController;

	internal CameraController cameraController;

	internal LevelStateController levelStateController;

	internal PlayersTankController playersTankController;

	internal LevelBuilder levelBuilder;

	internal GameplayUIController gameplayUIController;

	internal EffectsSpawner effectsSpawner;

	internal GameObjectPools gameObjectPools;

	internal TutorialController tutorialController;

	internal VisibilityController visibilityController;

	internal AutoaimPointerController autoaimPointerController;

	internal Canvas canvas;

	internal RectTransform canvasRT;

	internal TileMap tileMap;

	internal bool LevelFullyInitialized;

	internal const float zIndexDecrement = 0.001f;

	internal float wallShardZIndex;

	internal float coinZIndex;

	internal float explosionDecalZIndex;

	internal float playerActivationTime;

	private bool gamePaused;

	private bool newRegularLevelCompleted;

	private float desiredTimeScale = 1f;

	internal bool spawnedEnemyThisFrame;

	private float lastTimeCapturedScreenshot;

	private int screenshotSessionId;

	private static int screenshotCounter;

	private GameObject PauseItemsGO;

	private const float WEATHER_CHANCE = 0.8f;

	internal bool NewRegularLevelCompleted
	{
		get
		{
			return GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.RegularLevel && newRegularLevelCompleted;
		}
		set
		{
			newRegularLevelCompleted = value;
		}
	}

	internal bool LastLevelCompleted => newRegularLevelCompleted && GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.RegularLevel && instance.playersTankController.HPPercentage > 0f && GlobalCommons.Instance.ActualSelectedLevel == GlobalCommons.Instance.levelsContainer.LevelsCount;

	public bool GamePaused => gamePaused;

	public static GameplayCommons Instance
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

	private void Awake()
	{
		screenshotSessionId = UnityEngine.Random.Range(100000, 999999);
		Instance = this;
		switch (GlobalCommons.Instance.gameplayMode)
		{
		case GlobalCommons.GameplayModes.SurvivalLevel:
			LevelSelectionMenuController.InitSurvivalMode();
			break;
		case GlobalCommons.GameplayModes.CustomLevel:
			if (GlobalCommons.Instance.globalGameStats.PlayerID != LevelEditorController.LoadedCustomLevelCreatorID)
			{
				//AsyncRestCaller.TrackLevelPlay();
			}
			break;
		}
		TileFlyIn.destroyDelayFrameOffset = 1;
		BushController.showFrameOffset = 1;
		PhysicsLayers.InitializePhysicsLayers();
		LayerMasks.InitializeLayerMasks();
		enemiesTracker = new EnemiesTracker();
		canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
		canvasRT = canvas.GetComponent<RectTransform>();
		gameObjectPools = new GameObjectPools();
		tileMap = UnityEngine.Object.FindObjectOfType<TileMap>();
		touchesController = UnityEngine.Object.FindObjectOfType<TouchesController>();
		weaponsController = new WeaponsController();
		weaponSwitchController = UnityEngine.Object.FindObjectOfType<WeaponSwitchController>();
		levelResultsController = UnityEngine.Object.FindObjectOfType<LevelResultsController>();
		cameraController = UnityEngine.Object.FindObjectOfType<CameraController>();
		levelStateController = UnityEngine.Object.FindObjectOfType<LevelStateController>();
		levelBuilder = UnityEngine.Object.FindObjectOfType<LevelBuilder>();
		gameplayUIController = UnityEngine.Object.FindObjectOfType<GameplayUIController>();
		visibilityController = UnityEngine.Object.FindObjectOfType<VisibilityController>();

		autoaimPointerController = UnityEngine.Object.Instantiate(Prefabs.autoaimPointerPrefab).GetComponent<AutoaimPointerController>();

		if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.TutorialLevel)
		{
			tutorialController = new TutorialController();
		}
		else
		{
			UnityEngine.Object.Destroy(GameObject.Find("TutorialCrateSprite"));
			UnityEngine.Object.Destroy(GameObject.Find("TutorialBarrelSprite"));
			UnityEngine.Object.Destroy(GameObject.Find("TutorialBonuses"));
			UnityEngine.Object.Destroy(GameObject.Find("TutorialCoins"));
			UnityEngine.Object.Destroy(GameObject.Find("TutorialBlocksSprite"));
			UnityEngine.Object.Destroy(GameObject.Find("TutorialSpawnerSprite"));
		}
		PauseItemsGO = UnityEngine.Object.Instantiate(GlobalCommons.Instance.GetPauseItemsGO());

		PauseItemsGO.transform.SetParent(canvas.transform, worldPositionStays: false);

		Text component = PauseItemsGO.transform.Find("TipText").GetComponent<Text>();
		component.text = LoadingSceneController.GetCurrentTankmanPhrase();
		Image component2 = PauseItemsGO.transform.Find("TankmanImage").GetComponent<Image>();
		component2.sprite = LoadingSceneController.tankmanSprites[LoadingSceneController.currentTankmanImageID];
		if (GlobalCommons.Instance.gameplayMode != GlobalCommons.GameplayModes.TutorialLevel)
		{
			component2.SetAlpha(1f);
		}
		else
		{
			component2.SetAlpha(0f);
		}
	}

	public void RemoveLoadingSign()
	{
		CanvasGroup component = PauseItemsGO.GetComponent<CanvasGroup>();
		float duration = 0.5f;
		Image component2 = component.transform.Find("LoadingImage").GetComponent<Image>();
		Image component3 = component.transform.Find("TankmanImage").GetComponent<Image>();
		component3.sprite = LoadingSceneController.tankmanSprites[LoadingSceneController.currentTankmanImageID];
		Text component4 = component.transform.Find("TipText").GetComponent<Text>();
		float num = 70f;
		RectTransform rectTransform = component2.rectTransform;
		Vector2 anchoredPosition = component2.rectTransform.anchoredPosition;
		rectTransform.DOAnchorPosY(anchoredPosition.y - num, duration).SetEase(Ease.InCubic);
		component2.DOFade(0f, duration);
		if (GlobalCommons.Instance.gameplayMode != GlobalCommons.GameplayModes.TutorialLevel)
		{
			component3.SetAlpha(1f);
			RectTransform rectTransform2 = component3.rectTransform;
			Vector2 anchoredPosition2 = component3.rectTransform.anchoredPosition;
			rectTransform2.DOAnchorPosY(anchoredPosition2.y - num, duration).SetEase(Ease.InCubic).SetDelay(0.2f);
			component3.DOFade(0f, duration).SetDelay(0.2f);
		}
		else
		{
			component3.SetAlpha(0f);
		}
		RectTransform rectTransform3 = component4.rectTransform;
		Vector2 anchoredPosition3 = component4.rectTransform.anchoredPosition;
		rectTransform3.DOAnchorPosY(anchoredPosition3.y - num, duration).SetEase(Ease.InCubic).SetDelay(0.4f);
		component4.DOFade(0f, duration).OnCompleteWithCoroutine(CompleteLoadingSignRemoval).SetDelay(0.4f);
	}

	private void CompleteLoadingSignRemoval()
	{
		UnityEngine.Object.Destroy(PauseItemsGO);
	}

	public void SetDesiredTimeScale(float val)
	{
		desiredTimeScale = val;
	}

	public void InitializeEffectsSpawner(TileMap.TilesType tilesType)
	{
		if (effectsSpawner != null)
		{
			return;
		}
		switch (tilesType)
		{
		case TileMap.TilesType.DesertTiles:
			effectsSpawner = new EffectsSpawner(EffectsSpawner.EffectsSpawnerPreset.GameplayNoWeather, tilesType);
			break;
		case TileMap.TilesType.SummerTiles:
			if (GlobalCommons.Instance.gameplayMode != GlobalCommons.GameplayModes.TutorialLevel && ((UnityEngine.Random.value > 0.5f) ? true : false))
			{
				effectsSpawner = new EffectsSpawner(EffectsSpawner.EffectsSpawnerPreset.GameplayWithRain, tilesType);
			}
			else
			{
				effectsSpawner = new EffectsSpawner(EffectsSpawner.EffectsSpawnerPreset.GameplayNoWeather, tilesType);
				//effectsSpawner = new EffectsSpawner(EffectsSpawner.EffectsSpawnerPreset.GameplayWithRain, tilesType);
		     }
			break;
		case TileMap.TilesType.WinterTiles:
			if ((UnityEngine.Random.value > 0.8f) ? true : false)
			{
				effectsSpawner = new EffectsSpawner(EffectsSpawner.EffectsSpawnerPreset.GameplayWithSnow, tilesType);
			}
			else
			{
				effectsSpawner = new EffectsSpawner(EffectsSpawner.EffectsSpawnerPreset.GameplayNoWeather, tilesType);
			}
			break;
		}
	}

	private void Update()
	{
		bool isEditor = Application.isEditor;
		spawnedEnemyThisFrame = false;
		effectsSpawner.Update();
		if (tutorialController != null)
		{
			tutorialController.Update();
		}
		if (Time.timeScale == desiredTimeScale)
		{
			return;
		}
		if (desiredTimeScale < Time.timeScale)
		{
			Time.timeScale = desiredTimeScale;
			return;
		}
		float num = Time.timeScale + 2f * Time.unscaledDeltaTime;
		if (num > desiredTimeScale)
		{
			num = desiredTimeScale;
		}
		Time.timeScale = num;
	}

	private void OnApplicationFocus(bool focusStatus)
	{
		if (!focusStatus && !gamePaused && Application.platform != 0 && Application.platform != RuntimePlatform.WindowsEditor && GlobalCommons.Instance.gameplayMode != GlobalCommons.GameplayModes.TutorialLevel)
		{
			TogglePause();
		}
	}

	public void TogglePause()
	{
		if (LevelFullyInitialized && !levelStateController.GameplayStopped && !levelStateController.LevelCompletionPending && (gamePaused || !(instance.playersTankController.HPPercentage <= 0f)))
		{
            AdmobManager.instance.ShowBanner();

            if (gamePaused)
			{

                gamePaused = false;
				desiredTimeScale = 1f;
				instance.touchesController.ResetAllTouches();
			}
			else
			{
				gamePaused = true;
				desiredTimeScale = 0f;
			}
			Object.FindObjectOfType<PauseMenuController>().SetState(gamePaused);
			DebugHelper.Log("Toggled pause, new state is: " + gamePaused.ToString());
		}
	}
}
