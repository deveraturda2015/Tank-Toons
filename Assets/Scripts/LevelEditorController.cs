using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelEditorController : MonoBehaviour
{
	public enum LevelResizeDirection
	{
		Left,
		Right,
		Up,
		Down
	}

	internal enum LevelValidationResult
	{
		OK,
		Fail,
		TooManyEnemies
	}

	public Sprite BushWinterSprite;

	public Sprite BushDesertSprite;

	public Sprite BushSummerSprite;

	private GameObject levelContent;

	private RectTransform levelContentRT;

	private RectTransform viewportRT;

	private bool initialCentering = true;

	private float gridItemsScale = 1f;

	private float gridOffset = -1f;

	private int columns;

	private int rows;

	private Point2 minimunLevelDimensions;

	private const int MAX_LEVEL_WIDTH = 40;

	private const int MAX_LEVEL_HEIGHT = 40;

	private GameObject[,] level;

	private Text DrawScrollText;

	private char selectedChar = '4';

	private List<Point2> fillProcessedItems;

	private const int NULL_FINGER_ID = -100500;

	private int currentlyTrackedFingerId = -100500;

	private Vector2 currentlyTrackedFingerBeganCoord;

	public Sprite[] DrawSwitchSprites;

	private bool drawMode;

	private Button DrawSwitch;

	private ScrollRect scrollRect;

	private Button SceneryButton;

	private Button ShareButton;

	private Text SelectedItemDescriptionText;

	private float ItemsOffScreenCheckTicker;

	private const int OFF_SCREEN_CHECK_TICKER_MAX = 3;

	internal static List<string> LevelToWorkWith;

	internal static Vector2 LevelToWorkWithPan;

	internal static TileMap.TilesType LevelToWorkWithTilesType = TileMap.TilesType.SummerTiles;

	internal static bool CanShareLevel = false;

	internal static bool OpenUploadMenu = false;

	internal static float EditorLevelDamageMultiplier = 0f;

	private List<EditorItem> LevelItemsPool;

	private float cameraScaleCoefficient;

	internal static string LoadedCustomLevelID;

	internal static string LoadedCustomLevelCreatorID;

	internal static List<string> LoadedCustomLevel;

	internal static float LoadedCustomLevelDamageMultiplier;

	internal static TileMap.TilesType LoadedCustomLevelTilesType = TileMap.TilesType.SummerTiles;

	public const char fillChar = '@';

	private int pixelDragTreshold;

	public Sprite InactiveShareSprite;

	public Sprite ActiveShareSprite;

	public EffectsSpawner effectsSpawner;

	public static readonly Dictionary<string, char> ButtonNamesToLevelChars = new Dictionary<string, char>
	{
		{
			"PlayerSpawnPoint",
			'4'
		},
		{
			"Clear",
			'0'
		},
		{
			"CornerBottomRight",
			'c'
		},
		{
			"CornerBottomLeft",
			'd'
		},
		{
			"IndestructibleCornerBottomRight",
			'D'
		},
		{
			"IndestructibleCornerBottomLeft",
			'E'
		},
		{
			"WallHarder",
			'8'
		},
		{
			"Wall",
			'1'
		},
		{
			"Bush",
			'I'
		},
		{
			"BonusCrate",
			'7'
		},
		{
			"ExplosiveBarrel",
			'6'
		},
		{
			"CornerTopRight",
			'a'
		},
		{
			"CornerTopLeft",
			'b'
		},
		{
			"IndestructibleCornerTopRight",
			'B'
		},
		{
			"IndestructibleCornerTopLeft",
			'C'
		},
		{
			"IndestructableWall",
			'9'
		},
		{
			"IndestructableWallCracked",
			'g'
		},
		{
			"SpecWall",
			'e'
		},
		{
			"EnemySC",
			'('
		},
		{
			"EnemySimpleTank1",
			'X'
		},
		{
			"EnemySimpleTank2",
			'Y'
		},
		{
			"EnemySimpleTank3",
			'Z'
		},
		{
			"EnemySimpleTank4",
			'!'
		},
		{
			"EnemySimpleTank5",
			'*'
		},
		{
			"EnemySimpleTank6",
			'#'
		},
		{
			"EnemySimpleTank7",
			'$'
		},
		{
			"EnemySimpleTank8",
			'%'
		},
		{
			"EnemySimpleTank9",
			'^'
		},
		{
			"EnemySimpleTank10",
			'&'
		},
		{
			"Turret1",
			'u'
		},
		{
			"Turret2",
			'v'
		},
		{
			"Turret3",
			'w'
		},
		{
			"Turret4",
			'x'
		},
		{
			"Turret5",
			'y'
		},
		{
			"Turret6",
			'z'
		},
		{
			"Turret7",
			'A'
		},
		{
			"Turret8",
			'R'
		},
		{
			"Turret9",
			'S'
		},
		{
			"Turret10",
			'T'
		},
		{
			"Boss1",
			'n'
		},
		{
			"Boss2",
			'o'
		},
		{
			"Boss3",
			'p'
		},
		{
			"Boss4",
			'q'
		},
		{
			"Boss5",
			'r'
		},
		{
			"Boss6",
			's'
		},
		{
			"Boss7",
			't'
		},
		{
			"Boss8",
			'U'
		},
		{
			"Boss9",
			'V'
		},
		{
			"Boss10",
			'W'
		},
		{
			"Spawner1",
			'5'
		},
		{
			"Spawner2",
			'h'
		},
		{
			"Spawner3",
			'i'
		},
		{
			"Spawner4",
			'j'
		},
		{
			"Spawner5",
			'k'
		},
		{
			"Spawner6",
			'l'
		},
		{
			"Spawner7",
			'm'
		},
		{
			"Spawner8",
			'O'
		},
		{
			"Spawner9",
			'P'
		},
		{
			"Spawner10",
			'Q'
		},
		{
			"FillBtn",
			'@'
		}
	};

	private EditorItemSelectionMenu itemSelectionMenu;

	private GameObject EditorResizeMenu;

	private Button ItemSelectorButton;

	private float globalItemsScaleCoeff = 0.8f;

	internal static bool LoadPlayersCustomLevel = false;

	private List<EditorItem> ActiveLevelItems;

	private void Start()
	{
		if (false || LoadPlayersCustomLevel)
		{
			LoadPlayersCustomLevel = false;
			LevelToWorkWith = LoadedCustomLevel;
		}
		if (Application.isEditor)
		{
			LevelToWorkWith = GlobalCommons.Instance.levelsContainer.GetLevelByIndex(73);
		}
		cameraScaleCoefficient = 5f / CameraController.GetOrthoSize();
		LevelItemsPool = new List<EditorItem>();
		for (int i = 0; i < 1600; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Prefabs.levelEditorItemPrefab);
			gameObject.GetComponent<EditorItem>().InitializeItem();
			LevelItemsPool.Add(gameObject.GetComponent<EditorItem>());
		}
		levelContent = GameObject.Find("LevelContent");
		levelContentRT = levelContent.GetComponent<RectTransform>();
		viewportRT = levelContentRT.transform.parent.GetComponent<RectTransform>();
		gridItemsScale = cameraScaleCoefficient * globalItemsScaleCoeff;
		scrollRect = GameObject.Find("EditorScrollView").GetComponent<ScrollRect>();
		effectsSpawner = new EffectsSpawner(EffectsSpawner.EffectsSpawnerPreset.GameplayNoWeather);
		SoundManager.instance.ToggleMusic(SoundManager.MusicType.MenusMusic);
		SelectedItemDescriptionText = GameObject.Find("SelectedItemDescriptionText").GetComponent<Text>();
		SelectedItemDescriptionText.text = GetDescriptionForSelectedChar();
		itemSelectionMenu = UnityEngine.Object.Instantiate(Prefabs.EditorItemSelectionMenu, Vector3.zero, Quaternion.identity).GetComponent<EditorItemSelectionMenu>();
		itemSelectionMenu.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
		itemSelectionMenu.InitializeMenu();
		itemSelectionMenu.gameObject.SetActive(value: false);
		EditorResizeMenu = UnityEngine.Object.Instantiate(Prefabs.EditorResizeMenu, Vector3.zero, Quaternion.identity);
		EditorResizeMenu.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
		EditorResizeMenu.SetActive(value: false);
		DrawSwitch = GameObject.Find("DrawSwitch").GetComponent<Button>();
		Button component = GameObject.Find("LogButton").GetComponent<Button>();
		component.onClick.AddListener(delegate
		{
			LogButtonClick();
		});
		component.GetComponent<Image>().SetAlpha(0f);
		UnityEngine.Object.Destroy(component.gameObject);
		Button component2 = GameObject.Find("NewButton").GetComponent<Button>();
		component2.onClick.AddListener(delegate
		{
			NewButtonClick();
		});
		Button component3 = GameObject.Find("ExpandBtn").GetComponent<Button>();
		component3.onClick.AddListener(delegate
		{
			ResizeLevelButtonClick();
		});
		ItemSelectorButton = GameObject.Find("ItemSelectorButton").GetComponent<Button>();
		ItemSelectorButton.onClick.AddListener(delegate
		{
			ItemSelectorClick();
		});
		DrawSwitch = GameObject.Find("DrawSwitch").GetComponent<Button>();
		DrawSwitch.onClick.AddListener(delegate
		{
			DrawSwitchClick();
		});
		Button component4 = GameObject.Find("BackButton").GetComponent<Button>();
		component4.onClick.AddListener(delegate
		{
			BackClick();
		});
		Button component5 = GameObject.Find("PlayButton").GetComponent<Button>();
		component5.onClick.AddListener(delegate
		{
			PlayClick();
		});
		SceneryButton = GameObject.Find("SceneryButton").GetComponent<Button>();
		SceneryButton.onClick.AddListener(delegate
		{
			SceneryButtnClick();
		});
		RefreshSceneryButtonSprite();
		ShareButton = GameObject.Find("ShareButton").GetComponent<Button>();
		ShareButton.onClick.AddListener(delegate
		{
			ShareButtonClick();
		});
		if (!CanShareLevel)
		{
			ShareButton.image.sprite = InactiveShareSprite;
		}
		DrawScrollText = GameObject.Find("DrawScrollText").GetComponent<Text>();
		DrawScrollText.SetAlpha(0f);
		ResetLevelItems();
		EventSystem component6 = GameObject.Find("EventSystem").GetComponent<EventSystem>();
		float num = 2.54f;
		float num2 = 0.25f;
		component6.pixelDragThreshold = (int)(num2 * Screen.dpi / num);
		pixelDragTreshold = component6.pixelDragThreshold;
		if (OpenUploadMenu)
		{
			OpenUploadMenu = false;
			if (CanShareLevel)
			{
				ShareButtonClick(playSound: false);
			}
		}
	}

	public static string GetLevelForUploading()
	{
		string[] obj = new string[5]
		{
			string.Join(",", Array.ConvertAll(LevelToWorkWith.ToArray(), (string i) => i.ToString())),
			":",
			EditorLevelDamageMultiplier.ToString(CultureInfo.InvariantCulture),
			":",
			null
		};
		int levelToWorkWithTilesType = (int)LevelToWorkWithTilesType;
		obj[4] = levelToWorkWithTilesType.ToString();
		return string.Concat(obj);
	}

	public static bool LoadCustomLevel(string levelStr)
	{
		try
		{
			int num = levelStr.IndexOf(':');
			string text = levelStr.Substring(0, num);
			LoadedCustomLevel = text.Split(',').OfType<string>().ToList();
			string text2 = levelStr.Substring(num + 1);
			num = text2.IndexOf(':');
			LoadedCustomLevelDamageMultiplier = float.Parse(text2.Substring(0, num), CultureInfo.InvariantCulture);
			LoadedCustomLevelTilesType = (TileMap.TilesType)int.Parse(text2.Substring(num + 1));
			if (ValidateLevel(LoadedCustomLevel, silent: true) == LevelValidationResult.Fail)
			{
				throw new Exception("aaatatatata...");
			}
		}
		catch (Exception)
		{
			return false;
		}
		return true;
	}

	private void ShareButtonClick(bool playSound = true)
	{
		if (playSound)
		{
			SoundManager.instance.PlayButtonClickSound();
		}
		LevelToWorkWith = GetLevelList();
		if (CanShareLevel && ValidateLevel(LevelToWorkWith) != LevelValidationResult.Fail)
		{
			UploadingProgressMenu component = UnityEngine.Object.Instantiate(Prefabs.UploadingProgressMenu).GetComponent<UploadingProgressMenu>();
			component.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
		}
		else
		{
			GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("EditorCompleteToShare"));
		}
	}

	public void CompleteUpload()
	{
		ShareButton.image.sprite = InactiveShareSprite;
	}

	private void PlayClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		RectTransform component = GameObject.Find("PlayButton").GetComponent<RectTransform>();
		Vector3 localScale = component.transform.localScale;
		float x = localScale.x;
		Transform transform = component.transform;
		float x2 = x * 0.7f;
		float y = x * 0.7f;
		Vector3 localScale2 = component.transform.localScale;
		transform.localScale = new Vector3(x2, y, localScale2.z);
		RectTransform target = component;
		float x3 = x * 1f;
		float y2 = x * 1f;
		Vector3 localScale3 = component.localScale;
		target.DOScale(new Vector3(x3, y2, localScale3.z), 0.17f);
		LevelToWorkWith = GetLevelList();
		switch (ValidateLevel(LevelToWorkWith))
		{
		case LevelValidationResult.OK:
			TestCurrentLevel();
			break;
		case LevelValidationResult.TooManyEnemies:
			GlobalCommons.Instance.MessagesController.ShowConfirmationDialog(LocalizationManager.Instance.GetLocalizedText("EditorTooManyEnemies") + Environment.NewLine + LocalizationManager.Instance.GetLocalizedText("EditorProceedTesting"), TestCurrentLevel, null);
			break;
		}
	}

	private void SaveLevelPan()
	{
		LevelToWorkWithPan = new Vector2(Mathf.Clamp01(scrollRect.horizontalNormalizedPosition), Mathf.Clamp01(scrollRect.verticalNormalizedPosition));
	}

	private void TestCurrentLevel()
	{
		SoundManager.instance.FadeOutMusic();
		GlobalCommons.Instance.SaveGame();
		GlobalCommons.Instance.gameplayMode = GlobalCommons.GameplayModes.EditorLevel;
		if (AdsProcessor.GetInterstitialAdsAllowedBeforeLevelStart())
		{
			GlobalCommons.Instance.SceneToTransferTo = "LoadingScene";
			GlobalCommons.Instance.StateFaderController.ChangeSceneTo("PlayAdScene");
		}
		else
		{
			GlobalCommons.Instance.StateFaderController.ChangeSceneTo("LoadingScene");
		}
	}

	internal static LevelValidationResult ValidateLevel(List<string> levelToTest, bool silent = false)
	{
		bool flag = false;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		string text = "5hijklmOPQ";
		string text2 = "nopqrstUVW";
		string text3 = "uvwxyzARST";
		string text4 = "XYZ!*#$%^&(";
		for (int i = 0; i < levelToTest.Count; i++)
		{
			char[] array = levelToTest[i].ToCharArray();
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j] == '4')
				{
					flag = true;
				}
				if (text.IndexOf(array[j]) != -1)
				{
					num++;
				}
				if (text2.IndexOf(array[j]) != -1)
				{
					num3++;
				}
				if (text3.IndexOf(array[j]) != -1)
				{
					num4++;
				}
				if (text4.IndexOf(array[j]) != -1)
				{
					num2++;
				}
			}
		}
		if (!flag)
		{
			if (!silent)
			{
				GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("EditorStartingPointNotSpecified"));
			}
			return LevelValidationResult.Fail;
		}
		if (num == 0 && num3 == 0 && num4 == 0 && num2 == 0)
		{
			if (!silent)
			{
				GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("EditorCongratsWon") + Environment.NewLine + LocalizationManager.Instance.GetLocalizedText("EditorAddEnemies"));
			}
			return LevelValidationResult.Fail;
		}
		int num5 = num * 6 + num3 + num4 + num2;
		if (num5 > 300)
		{
			return LevelValidationResult.TooManyEnemies;
		}
		return LevelValidationResult.OK;
	}

	private void BackClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		RectTransform component = GameObject.Find("BackButton").GetComponent<RectTransform>();
		Vector3 localScale = component.transform.localScale;
		float x = localScale.x;
		Transform transform = component.transform;
		float x2 = x * 0.7f;
		float y = x * 0.7f;
		Vector3 localScale2 = component.transform.localScale;
		transform.localScale = new Vector3(x2, y, localScale2.z);
		RectTransform target = component;
		float x3 = x * 1.1f;
		float y2 = x * 1.1f;
		Vector3 localScale3 = component.localScale;
		target.DOScale(new Vector3(x3, y2, localScale3.z), 0.17f);
		LevelToWorkWith = GetLevelList();
		GlobalCommons.Instance.SaveGame();
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("LevelSelection");
	}

	public Point2 GetLevelDimensions()
	{
		return new Point2(columns, rows);
	}

	private void ItemSelectorClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		itemSelectionMenu.gameObject.SetActive(value: true);
		itemSelectionMenu.FadeIn();
	}

	private void ResizeLevelButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		EditorResizeMenu.SetActive(value: true);
	}

	public bool ExpandLevel(LevelResizeDirection direction)
	{
		LevelToWorkWith = GetLevelList();
		if (LevelToWorkWith.Count >= 40 && (direction == LevelResizeDirection.Up || direction == LevelResizeDirection.Down))
		{
			return false;
		}
		if (LevelToWorkWith[0].Length >= 40 && (direction == LevelResizeDirection.Left || direction == LevelResizeDirection.Right))
		{
			return false;
		}
		ExpandLevelToWorkWith(direction);
		ResetLevelItems();
		return true;
	}

	public bool ShrinkLevel(LevelResizeDirection direction)
	{
		LevelToWorkWith = GetLevelList();
		if (LevelToWorkWith.Count <= minimunLevelDimensions.y && (direction == LevelResizeDirection.Down || direction == LevelResizeDirection.Up))
		{
			return false;
		}
		if (LevelToWorkWith[0].Length <= minimunLevelDimensions.x && (direction == LevelResizeDirection.Left || direction == LevelResizeDirection.Right))
		{
			return false;
		}
		ShrinkLevelToWorkWith(direction);
		ResetLevelItems();
		if (scrollRect.horizontalNormalizedPosition > 1f)
		{
			scrollRect.horizontalNormalizedPosition = 1f;
		}
		if (scrollRect.verticalNormalizedPosition < 0f)
		{
			scrollRect.verticalNormalizedPosition = 0f;
		}
		return true;
	}

	private void DrawSwitchClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		drawMode = !drawMode;
		if (drawMode)
		{
			DrawSwitch.image.sprite = DrawSwitchSprites[1];
			scrollRect.enabled = false;
		}
		else
		{
			DrawSwitch.image.sprite = DrawSwitchSprites[0];
			scrollRect.enabled = true;
		}
		if (drawMode)
		{
			DrawScrollText.text = LocalizationManager.Instance.GetLocalizedText("EditorDrawMode");
		}
		else
		{
			DrawScrollText.text = LocalizationManager.Instance.GetLocalizedText("EditorScrollMode");
		}
		DrawScrollText.DOKill();
		DrawScrollText.SetAlpha(1f);
		DrawScrollText.DOFade(0f, 0.5f).SetDelay(1f);
	}

	private void ShrinkLevelToWorkWith(LevelResizeDirection direction = LevelResizeDirection.Right)
	{
		StringBuilder stringBuilder = new StringBuilder();
		switch (direction)
		{
		case LevelResizeDirection.Down:
			LevelToWorkWith.RemoveAt(0);
			LevelToWorkWith.RemoveAt(0);
			LevelToWorkWith.Insert(0, new string('9', LevelToWorkWith[0].Length));
			break;
		case LevelResizeDirection.Up:
			LevelToWorkWith.RemoveAt(LevelToWorkWith.Count - 1);
			LevelToWorkWith.RemoveAt(LevelToWorkWith.Count - 1);
			LevelToWorkWith.Add(new string('9', LevelToWorkWith[0].Length));
			break;
		case LevelResizeDirection.Right:
			for (int j = 0; j < LevelToWorkWith.Count; j++)
			{
				StringBuilder stringBuilder3 = new StringBuilder();
				stringBuilder3.Append(LevelToWorkWith[j]);
				stringBuilder3.Remove(stringBuilder3.Length - 1, 1);
				stringBuilder3[stringBuilder3.Length - 1] = '9';
				LevelToWorkWith[j] = stringBuilder3.ToString();
			}
			break;
		case LevelResizeDirection.Left:
			for (int i = 0; i < LevelToWorkWith.Count; i++)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.Append(LevelToWorkWith[i]);
				stringBuilder2.Remove(0, 1);
				stringBuilder2[0] = '9';
				LevelToWorkWith[i] = stringBuilder2.ToString();
			}
			break;
		}
	}

	private void ExpandLevelToWorkWith(LevelResizeDirection direction = LevelResizeDirection.Right)
	{
		for (int i = 0; i < LevelToWorkWith.Count; i++)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(LevelToWorkWith[i]);
			if (direction == LevelResizeDirection.Left)
			{
				stringBuilder.Insert(0, new string('9', 1));
				if (i == 0 || i == LevelToWorkWith.Count - 1)
				{
					stringBuilder[1] = '9';
				}
				else
				{
					stringBuilder[1] = '0';
				}
			}
			if (direction == LevelResizeDirection.Right)
			{
				stringBuilder.Append(new string('9', 1));
				if (i == 0 || i == LevelToWorkWith.Count - 1)
				{
					stringBuilder[stringBuilder.Length - 2] = '9';
				}
				else
				{
					stringBuilder[stringBuilder.Length - 2] = '0';
				}
			}
			LevelToWorkWith[i] = stringBuilder.ToString();
		}
		for (int j = 0; j < 1; j++)
		{
			if (direction == LevelResizeDirection.Down)
			{
				LevelToWorkWith.Insert(0, new string('9', LevelToWorkWith[0].Length));
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.Append(LevelToWorkWith[1]);
				for (int k = 0; k < stringBuilder2.Length; k++)
				{
					if (k == 0 || k == stringBuilder2.Length - 1)
					{
						stringBuilder2[k] = '9';
					}
					else
					{
						stringBuilder2[k] = '0';
					}
				}
				LevelToWorkWith[1] = stringBuilder2.ToString();
			}
			if (direction != LevelResizeDirection.Up)
			{
				continue;
			}
			LevelToWorkWith.Add(new string('9', LevelToWorkWith[0].Length));
			StringBuilder stringBuilder3 = new StringBuilder();
			stringBuilder3.Append(LevelToWorkWith[1]);
			for (int l = 0; l < stringBuilder3.Length; l++)
			{
				if (l == 0 || l == stringBuilder3.Length - 1)
				{
					stringBuilder3[l] = '9';
				}
				else
				{
					stringBuilder3[l] = '0';
				}
			}
			LevelToWorkWith[LevelToWorkWith.Count - 2] = stringBuilder3.ToString();
		}
	}

	private void ResetLevelItems()
	{
		ActiveLevelItems = new List<EditorItem>();
		GameObject gameObject = UnityEngine.Object.Instantiate(Prefabs.levelEditorItemPrefab);
		gameObject.transform.SetParent(levelContent.transform, worldPositionStays: false);
		gameObject.GetComponent<RectTransform>().localScale = new Vector3(gridItemsScale, gridItemsScale, 1f);
		float width = gameObject.GetComponent<RectTransform>().rect.width;
		float height = gameObject.GetComponent<RectTransform>().rect.height;
		if (width != height)
		{
			throw new Exception("level item is not square");
		}
		float scaleFactor = GameObject.Find("Canvas").GetComponent<Canvas>().scaleFactor;
		gridOffset = width * scaleFactor / (float)Screen.height * Camera.main.orthographicSize * 2f * gridItemsScale;
		RectTransform component = GameObject.Find("Canvas").GetComponent<RectTransform>();
		minimunLevelDimensions = new Point2(Mathf.CeilToInt(component.rect.width / gameObject.GetComponent<RectTransform>().rect.width / cameraScaleCoefficient / globalItemsScaleCoeff), Mathf.CeilToInt(component.rect.height / gameObject.GetComponent<RectTransform>().rect.height / cameraScaleCoefficient / globalItemsScaleCoeff));
		if (LevelToWorkWith != null)
		{
			rows = LevelToWorkWith.Count;
			columns = LevelToWorkWith[0].Length;
		}
		else
		{
			rows = minimunLevelDimensions.y;
			columns = minimunLevelDimensions.x;
		}
		float num = 2f;
		float num2 = (num + num - 1f) * width + (float)columns * width * cameraScaleCoefficient * globalItemsScaleCoeff;
		if (num2 < component.rect.width)
		{
			num2 = component.rect.width;
		}
		float num3 = (num + num - 1f) * width + (float)rows * width * cameraScaleCoefficient * globalItemsScaleCoeff;
		if (num3 < component.rect.height)
		{
			num3 = component.rect.height;
		}
		RectTransform component2 = levelContent.GetComponent<RectTransform>();
		component2.sizeDelta = new Vector2(num2, num3);
		level = new GameObject[columns, rows];
		Vector3 position = levelContent.GetComponent<RectTransform>().position;
		float x = position.x + num;
		Vector3 position2 = levelContent.GetComponent<RectTransform>().position;
		float y = position2.y - num;
		Vector3 position3 = levelContent.GetComponent<RectTransform>().position;
		Vector3 vector = new Vector3(x, y, position3.z);
		int num4 = 0;
		for (int i = 0; i < columns; i++)
		{
			int num5 = rows;
			while (num5-- > 0)
			{
				Vector3 v = new Vector3(vector.x + (float)i * gridOffset, vector.y - (float)num5 * gridOffset, vector.z);
				EditorItem editorItem = LevelItemsPool[num4];
				ActiveLevelItems.Add(editorItem);
				if (!editorItem.gameObject.activeInHierarchy)
				{
					editorItem.gameObject.SetActive(value: true);
				}
				if (editorItem.gameObject.transform.parent != levelContent.transform)
				{
					editorItem.gameObject.transform.SetParent(levelContent.transform, worldPositionStays: false);
					editorItem.rectTransform.localScale = new Vector3(gridItemsScale, gridItemsScale, 1f);
				}
				editorItem.SetPosition(v);
				editorItem.SetGridPosition(i, num5);
				if (LevelToWorkWith != null)
				{
					editorItem.UpdateItem(LevelToWorkWith[rows - (num5 + 1)][i]);
				}
				else if (num5 == 0 || num5 == rows - 1)
				{
					editorItem.UpdateItem('9');
				}
				else if (i == 0 || i == columns - 1)
				{
					editorItem.UpdateItem('9');
				}
				else
				{
					editorItem.UpdateItem('0');
				}
				level[i, num5] = editorItem.gameObject;
				num4++;
			}
		}
		UnityEngine.Object.Destroy(gameObject.gameObject);
		if (initialCentering)
		{
			initialCentering = false;
			if (LevelToWorkWith != null)
			{
				scrollRect.horizontalNormalizedPosition = LevelToWorkWithPan.x;
				scrollRect.verticalNormalizedPosition = LevelToWorkWithPan.y;
			}
			else
			{
				scrollRect.horizontalNormalizedPosition = 0.5f;
				scrollRect.verticalNormalizedPosition = 0.5f;
			}
		}
		LevelItemsPool.ForEach(delegate(EditorItem itm)
		{
			itm.Visible = false;
		});
		ActiveLevelItems.ForEach(delegate(EditorItem itm)
		{
			itm.Visible = true;
		});
	}

	private void Update()
	{
		ProcessTouches();
		effectsSpawner.Update();
		if (Application.isEditor && Input.mousePresent && (Input.GetMouseButton(1) || UnityEngine.Input.GetKeyDown(KeyCode.Space)))
		{
			ProcessFingerDrag(UnityEngine.Input.mousePosition, checkDrawMode: false);
		}
		float num = viewportRT.rect.width + 60f;
		float num2 = viewportRT.rect.height + 60f;
		float num3 = levelContentRT.rect.width / 2f;
		float num4 = levelContentRT.rect.height / 2f;
		ItemsOffScreenCheckTicker += 1f;
		if (ItemsOffScreenCheckTicker == 3f)
		{
			ItemsOffScreenCheckTicker = 0f;
		}
		for (int i = 0; i < ActiveLevelItems.Count; i++)
		{
			if ((float)(i % 3) != ItemsOffScreenCheckTicker)
			{
				continue;
			}
			EditorItem editorItem = ActiveLevelItems[i];
			bool flag = true;
			Vector2 anchoredPosition = levelContentRT.anchoredPosition;
			if (anchoredPosition.x + editorItem.AnchoredPosition.x + num3 < -60f)
			{
				flag = false;
			}
			else
			{
				Vector2 anchoredPosition2 = levelContentRT.anchoredPosition;
				if (anchoredPosition2.x + editorItem.AnchoredPosition.x + num3 > num)
				{
					flag = false;
				}
				else
				{
					Vector2 anchoredPosition3 = levelContentRT.anchoredPosition;
					if (0f - anchoredPosition3.y - editorItem.AnchoredPosition.y + num4 < -60f)
					{
						flag = false;
					}
					else
					{
						Vector2 anchoredPosition4 = levelContentRT.anchoredPosition;
						if (0f - anchoredPosition4.y - editorItem.AnchoredPosition.y + num4 > num2)
						{
							flag = false;
						}
					}
				}
			}
			if (editorItem.Visible != flag)
			{
				editorItem.Visible = flag;
			}
		}
	}

	private void ProcessTouches()
	{
		int touchCount = UnityEngine.Input.touchCount;
		for (int i = 0; i < touchCount; i++)
		{
			Touch touch = UnityEngine.Input.GetTouch(i);
			switch (touch.phase)
			{
			case TouchPhase.Began:
				if (currentlyTrackedFingerId == -100500 && CheckLevelItemTapEligible(touch.position))
				{
					currentlyTrackedFingerId = touch.fingerId;
					currentlyTrackedFingerBeganCoord = touch.position;
					ProcessFingerDrag(touch.position);
				}
				break;
			case TouchPhase.Moved:
			{
				if (currentlyTrackedFingerId != touch.fingerId)
				{
					break;
				}
				ProcessFingerDrag(touch.position);
				if (drawMode)
				{
					break;
				}
				Vector2 position3 = touch.position;
				if (!(Mathf.Abs(position3.x - currentlyTrackedFingerBeganCoord.x) >= (float)pixelDragTreshold))
				{
					Vector2 position4 = touch.position;
					if (!(Mathf.Abs(position4.y - currentlyTrackedFingerBeganCoord.y) >= (float)pixelDragTreshold))
					{
						break;
					}
				}
				currentlyTrackedFingerBeganCoord = new Vector2(100500f, 100500f);
				break;
			}
			case TouchPhase.Ended:
			{
				if (currentlyTrackedFingerId != touch.fingerId)
				{
					break;
				}
				currentlyTrackedFingerId = -100500;
				if (drawMode)
				{
					break;
				}
				Vector2 position = touch.position;
				if (Mathf.Abs(position.x - currentlyTrackedFingerBeganCoord.x) < (float)pixelDragTreshold)
				{
					Vector2 position2 = touch.position;
					if (Mathf.Abs(position2.y - currentlyTrackedFingerBeganCoord.y) < (float)pixelDragTreshold)
					{
						ProcessFingerDrag(touch.position, checkDrawMode: false);
					}
				}
				break;
			}
			case TouchPhase.Canceled:
				if (currentlyTrackedFingerId == touch.fingerId)
				{
					currentlyTrackedFingerId = -100500;
				}
				break;
			}
		}
	}

	private void ProcessFingerDrag(Vector2 touchPosition, bool checkDrawMode = true)
	{
		if (checkDrawMode && !drawMode)
		{
			return;
		}
		Vector3 vector = Camera.main.ScreenToWorldPoint(touchPosition);
		float num = gridOffset / 2f;
		int num2 = rows;
		while (num2-- > 0)
		{
			for (int i = 0; i < columns; i++)
			{
				GameObject gameObject = level[i, num2];
				float x = vector.x;
				Vector3 position = gameObject.transform.position;
				if (Mathf.Abs(x - position.x) < num)
				{
					float y = vector.y;
					Vector3 position2 = gameObject.transform.position;
					if (Mathf.Abs(y - position2.y) < num && num2 != 0 && num2 != rows - 1 && i != 0 && i != columns - 1 && CheckLevelItemTapEligible(touchPosition))
					{
						LevelItemClick(gameObject);
					}
				}
			}
		}
	}

	private bool CheckLevelItemTapEligible(Vector2 touchPosition)
	{
		GraphicRaycaster component = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
		PointerEventData pointerEventData = new PointerEventData(null);
		pointerEventData.position = touchPosition;
		List<RaycastResult> list = new List<RaycastResult>();
		component.Raycast(pointerEventData, list);
		if (list.Count == 1)
		{
			return true;
		}
		return false;
	}

	public void RefreshSceneryButtonSprite()
	{
		switch (LevelToWorkWithTilesType)
		{
		case TileMap.TilesType.SummerTiles:
			SceneryButton.image.sprite = BushSummerSprite;
			break;
		case TileMap.TilesType.DesertTiles:
			SceneryButton.image.sprite = BushDesertSprite;
			break;
		case TileMap.TilesType.WinterTiles:
			SceneryButton.image.sprite = BushWinterSprite;
			break;
		}
	}

	private void LevelItemClick(GameObject btn)
	{
		if (selectedChar == '@')
		{
			int num = rows;
			while (num-- > 0)
			{
				for (int i = 0; i < columns; i++)
				{
					GameObject x = level[i, num];
					if (x == btn)
					{
						FillWithClear(new Point2(i, num));
					}
				}
			}
			return;
		}
		EditorItem component = btn.GetComponent<EditorItem>();
		if (selectedChar == '4')
		{
			int num3 = rows;
			while (num3-- > 0)
			{
				for (int j = 0; j < columns; j++)
				{
					GameObject gameObject = level[j, num3];
					EditorItem component2 = gameObject.GetComponent<EditorItem>();
					if (component2.ItemChar == '4')
					{
						if (component == component2)
						{
							return;
						}
						component2.UpdateItem('0');
					}
				}
			}
		}
		if (component.ItemChar != selectedChar)
		{
			if (CanShareLevel)
			{
				CanShareLevel = false;
				ShareButton.image.sprite = InactiveShareSprite;
			}
			component.UpdateItem(selectedChar);
			effectsSpawner.CreateSpawnerSpawnEffect(btn.transform.position, ignoreChecks: true);
			SoundManager.instance.PlayEnemyEmojiSound();
		}
	}

	private void EditorItemClick(Button btn)
	{
		if (!ButtonNamesToLevelChars.ContainsKey(btn.name))
		{
			throw new Exception("cannot convert editor button name to level item char");
		}
		selectedChar = ButtonNamesToLevelChars[btn.name];
	}

	public void SelectItem(char itemChar, Sprite sprite)
	{
		selectedChar = itemChar;
		ItemSelectorButton.image.sprite = sprite;
		SelectedItemDescriptionText.text = GetDescriptionForSelectedChar();
	}

	private void FillWithClear(Point2 pt)
	{
		fillProcessedItems = new List<Point2>();
		ProcessElementFillWithClear(pt);
	}

	private void ProcessElementFillWithClear(Point2 pt)
	{
		if (fillProcessedItems.IndexOf(pt) == -1)
		{
			fillProcessedItems.Add(pt);
			EditorItem component = level[pt.x, pt.y].GetComponent<EditorItem>();
			component.UpdateItem('0');
			EditorItem component2 = level[pt.x + 1, pt.y].GetComponent<EditorItem>();
			if (component2.ItemChar == 'f')
			{
				ProcessElementFillWithClear(new Point2(pt.x + 1, pt.y));
			}
			EditorItem component3 = level[pt.x - 1, pt.y].GetComponent<EditorItem>();
			if (component3.ItemChar == 'f')
			{
				ProcessElementFillWithClear(new Point2(pt.x - 1, pt.y));
			}
			EditorItem component4 = level[pt.x, pt.y + 1].GetComponent<EditorItem>();
			if (component4.ItemChar == 'f')
			{
				ProcessElementFillWithClear(new Point2(pt.x, pt.y + 1));
			}
			EditorItem component5 = level[pt.x, pt.y - 1].GetComponent<EditorItem>();
			if (component5.ItemChar == 'f')
			{
				ProcessElementFillWithClear(new Point2(pt.x, pt.y - 1));
			}
		}
	}

	private List<string> GetLevelList()
	{
		List<string> list = new List<string>();
		int num = rows;
		while (num-- > 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < columns; i++)
			{
				GameObject gameObject = level[i, num];
				stringBuilder.Append(gameObject.GetComponent<EditorItem>().ItemChar);
			}
			list.Add(stringBuilder.ToString());
		}
		SaveLevelPan();
		return list;
	}

	private void SceneryButtnClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		LevelEditorSeasonMenu component = UnityEngine.Object.Instantiate(Prefabs.LevelEditorSeasonMenu).GetComponent<LevelEditorSeasonMenu>();
		component.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
	}

	private void NewButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.MessagesController.ShowConfirmationDialog(LocalizationManager.Instance.GetLocalizedText("EditorClearLevel"), ConfirmNewLevel, null);
	}

	private void ConfirmNewLevel()
	{
		initialCentering = true;
		LevelToWorkWith = null;
		ResetLevelItems();
	}

	private void LogButtonClick()
	{
		List<string> levelList = GetLevelList();
		LevelUtils.CropLevel(levelList);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < levelList.Count; i++)
		{
			stringBuilder.Append('"');
			for (int j = 0; j < levelList[0].Length; j++)
			{
				stringBuilder.Append(levelList[i][j]);
			}
			stringBuilder.Append('"');
			stringBuilder.Append(',');
			stringBuilder.Append(Environment.NewLine);
		}
		DebugHelper.Log(stringBuilder.ToString());
	}

	private string GetDescriptionForSelectedChar()
	{
		switch (selectedChar)
		{
		case '5':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription1");
		case 'h':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription2");
		case 'i':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription3");
		case 'j':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription4");
		case 'k':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription5");
		case 'l':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription6");
		case 'm':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription7");
		case 'u':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription8");
		case 'v':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription9");
		case 'w':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription10");
		case 'x':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription11");
		case 'y':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription12");
		case 'z':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription13");
		case 'A':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription14");
		case 'n':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription15");
		case 'o':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription16");
		case 'p':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription17");
		case 'q':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription18");
		case 'r':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription19");
		case 's':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription20");
		case 't':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription21");
		case '1':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription22");
		case '8':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription23");
		case '9':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription24");
		case 'g':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription25");
		case 'e':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription26");
		case '0':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription27");
		case '7':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription28");
		case '6':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription29");
		case '4':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription30");
		case 'd':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription22");
		case 'c':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription22");
		case 'b':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription22");
		case 'a':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription22");
		case 'f':
			return "Wololo";
		case 'B':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription24");
		case 'C':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription24");
		case 'D':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription24");
		case 'E':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription24");
		case 'I':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription31");
		case 'X':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription32");
		case 'Y':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription33");
		case 'Z':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription34");
		case '!':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription35");
		case '*':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription36");
		case '#':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription37");
		case '$':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription38");
		case '%':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription39");
		case '^':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription40");
		case '&':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription41");
		case 'R':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription45");
		case 'S':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription46");
		case 'T':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription47");
		case 'U':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription42");
		case 'V':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription43");
		case 'W':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription44");
		case 'O':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription48");
		case 'P':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription49");
		case 'Q':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription50");
		case '(':
			return LocalizationManager.Instance.GetLocalizedText("EditorItemDescription51");
		default:
			throw new Exception("no description specified for level item char: " + selectedChar.ToString());
		}
	}
}
