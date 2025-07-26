using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class LevelSelectionMenuController : MonoBehaviour
{
	private Button menuButton;

	private Button arenaButton;

	private Button survivalButton;

	private Button unlockButton;

	private Button LockButton;

	private Button CustomLevelButton;

	private Button CustomLevelButton2;

	private Button LevelEditorButton;

	public static bool ShowNewEditorItemsAvailableMessage;

	internal static int lastSelectedScreen = -1;

	public const int levelsPerScreen = 15;

	private GameObject pageIndicators;

	private int levelPagesCount;

	private List<GameObject> levelPages;

	private HorizontalScrollSnap scrollSnap;

	public Sprite ActiveLevelsPageSprite;

	public Sprite InactiveLevelsPageSprite;

	public Sprite DisabledArenaButtonSprite;

	public Sprite DisabledSurvivalButtonSprite;

	private List<List<RectTransform>> allLevelItems;

	private Image newLevelItemImage;

	private float newLevelItemShakeTimestamp;

	private float newLevelItemRotationPunch = 10f;

	private float defaultLevelItemScale;

	private bool levelSelected;

	private void Start()
	{
		AdmobManager.instance.HideBanner();

		newLevelItemShakeTimestamp = Time.fixedTime;
		levelPagesCount = GlobalCommons.Instance.levelsContainer.LevelsCount / 15;
		if (lastSelectedScreen == -1)
		{
			lastSelectedScreen = Mathf.FloorToInt(GlobalCommons.Instance.globalGameStats.LevelsCompleted / 15) + 1;
		}
		if (lastSelectedScreen == levelPagesCount + 1)
		{
			lastSelectedScreen = levelPagesCount;
		}
		scrollSnap = GameObject.Find("Scroll View").GetComponent<HorizontalScrollSnap>();
		menuButton = GameObject.Find("MenuButton").GetComponent<Button>();
		menuButton.onClick.AddListener(delegate
		{
			MenuButtonClick();
		});
		arenaButton = GameObject.Find("ArenaButton").GetComponent<Button>();
		arenaButton.onClick.AddListener(delegate
		{
			ArenaButtonClick();
		});
		if (GlobalCommons.Instance.globalGameStats.LevelsCompleted < 10)
		{
			arenaButton.GetComponent<Image>().sprite = DisabledArenaButtonSprite;
		}
		survivalButton = GameObject.Find("SurvivalButton").GetComponent<Button>();
		survivalButton.onClick.AddListener(delegate
		{
			SurvivalButtonClick();
		});
		if (GlobalCommons.Instance.globalGameStats.LevelsCompleted < 20)
		{
			survivalButton.GetComponent<Image>().sprite = DisabledSurvivalButtonSprite;
		}
		unlockButton = GameObject.Find("UnlockButton").GetComponent<Button>();
		unlockButton.onClick.AddListener(delegate
		{
			UnlockButtonClick();
		});
		LockButton = GameObject.Find("LockButton").GetComponent<Button>();
		LockButton.onClick.AddListener(delegate
		{
			LockButtonClick();
		});
		//CustomLevelButton = GameObject.Find("CustomLevelButton").GetComponent<Button>();
		//CustomLevelButton.onClick.AddListener(delegate
		//{
		//	CustomLevelButtonClick();
		//});
		//CustomLevelButton2 = GameObject.Find("CustomLevelButton2").GetComponent<Button>();
		//CustomLevelButton2.onClick.AddListener(delegate
		//{
		//	CustomLevelButton2Click();
		//});
		LevelEditorButton = GameObject.Find("LevelEditorButton").GetComponent<Button>();
		LevelEditorButton.onClick.AddListener(delegate
		{
			EditorButtonClick();
		});
		allLevelItems = new List<List<RectTransform>>();
		for (int i = 0; i < levelPagesCount; i++)
		{
			allLevelItems.Add(new List<RectTransform>());
			GameObject levelsScreen = GameObject.Find("LevelsScreen" + (i + 1).ToString());
			PopulateLevelScreen(levelsScreen, i);
		}
		SoundManager.instance.ToggleMusic(SoundManager.MusicType.MenusMusic);
		scrollSnap.SelectStartingScreen(lastSelectedScreen);
		InitializePageIndicators();
		if (GlobalCommons.Instance.globalGameStats.ArenaUnlockedMessagePending)
		{
			SoundManager.instance.PlayRewadWinSound();
			GameObject gameObject = UnityEngine.Object.Instantiate(Prefabs.ArenaModeUnlockedMessage, Vector3.zero, Quaternion.identity);
			gameObject.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
			GlobalCommons.Instance.globalGameStats.ArenaUnlockedMessagePending = false;
		}
		if (GlobalCommons.Instance.globalGameStats.SurvivalUnlockedMessagePending)
		{
			SoundManager.instance.PlayRewadWinSound();
			GameObject gameObject2 = UnityEngine.Object.Instantiate(Prefabs.SurvivalModeUnlockedMessage, Vector3.zero, Quaternion.identity);
			gameObject2.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
			GlobalCommons.Instance.globalGameStats.SurvivalUnlockedMessagePending = false;
		}
		GlobalCommons.Instance.SaveGame();
		unlockButton.GetComponent<Image>().SetAlpha(0f);
		LockButton.GetComponent<Image>().SetAlpha(0f);
		UnityEngine.Object.Destroy(unlockButton.gameObject);
		UnityEngine.Object.Destroy(LockButton.gameObject);
		EventSystem component = GameObject.Find("EventSystem").GetComponent<EventSystem>();
		float num = 2.54f;
		float num2 = 0.25f;
		component.pixelDragThreshold = (int)(num2 * Screen.dpi / num);
		if (ShowNewEditorItemsAvailableMessage)
		{
			ShowNewEditorItemsAvailableMessage = false;
			GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("MessageNewEditorItemsAvailable"), 1.5f);
			SoundManager.instance.PlayRewadWinSound();
		}
	}

	private void CustomLevelButton2Click()
	{
		SoundManager.instance.PlayButtonClickSound();
		RectTransform component = CustomLevelButton2.GetComponent<RectTransform>();
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
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("UserLevels");
	}

	private void CustomLevelButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		RectTransform component = CustomLevelButton.GetComponent<RectTransform>();
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
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("UserLevels");
	}

	public void ProcessUserLevelLoad()
	{
		KickAllLevelItems();
		RememberLastSelectedScreen();
	}

	private void EditorButtonClick()
	{
		KickAllLevelItems();
		RectTransform component = LevelEditorButton.GetComponent<RectTransform>();
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
		RememberLastSelectedScreen();
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.gameplayMode = GlobalCommons.GameplayModes.EditorLevel;
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("LevelEditor");
	}

	private void InitializePageIndicators()
	{
		pageIndicators = GameObject.Find("PageIndicators");
		levelPages = new List<GameObject>();
		float num = 45f;
		for (int i = 0; i < levelPagesCount + 1; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Prefabs.levelPageIcon);
			gameObject.transform.SetParent(pageIndicators.transform, worldPositionStays: false);
			gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(((float)i - 2.5f) * num, 0f);
			gameObject.GetComponent<Image>().sprite = InactiveLevelsPageSprite;
			levelPages.Add(gameObject);
			gameObject.name = "PageBtn" + i.ToString();
			Button pageIndicatorGOBtn = gameObject.GetComponent<Button>();
			pageIndicatorGOBtn.onClick.AddListener(delegate
			{
				PageButtonClick(pageIndicatorGOBtn);
			});
		}
	}

	private void PageButtonClick(Button pageIndicatorGO)
	{
		int screenIndex = int.Parse(pageIndicatorGO.name.Substring(7));
		scrollSnap.SelectScreen(screenIndex);
	}

	private void RememberLastSelectedScreen()
	{
		lastSelectedScreen = scrollSnap.CurrentScreen() + 1;
	}

	private void PopulateLevelScreen(GameObject levelsScreen, int screenId)
	{
		int num = 1;
		float num2 = Camera.main.aspect - 1f;
		if (num2 < 0f)
		{
			num2 = 0f;
		}
		float num3 = 150f + 60f * num2 * 0.8f;
		float num4 = 150f;
		Vector2 vector = default(Vector2);
		for (int num5 = 2; num5 >= 0; num5--)
		{
			for (int i = 0; i < 5; i++)
			{
				Vector3 position = levelsScreen.transform.position;
				float x = position.x + (float)(i - 2) * num3;
				Vector3 position2 = levelsScreen.transform.position;
				vector = new Vector2(x, position2.y + (float)(num5 - 1) * num4 + 50f);
				int num6 = num + screenId * 15;
				GameObject original = (num6 != GlobalCommons.Instance.globalGameStats.LevelsCompleted + 1) ? Prefabs.levelSelectButtonPrefab : Prefabs.levelSelectButtonNewPrefab;
				GameObject levelItemGO = UnityEngine.Object.Instantiate(original);
				if (num6 == GlobalCommons.Instance.globalGameStats.LevelsCompleted + 1)
				{
					newLevelItemImage = levelItemGO.GetComponent<Image>();
					Vector3 localScale = newLevelItemImage.transform.localScale;
					defaultLevelItemScale = localScale.x;
					Transform transform = newLevelItemImage.transform;
					float x2 = defaultLevelItemScale * 1.1f;
					float y = defaultLevelItemScale * 1.1f;
					Vector3 localScale2 = newLevelItemImage.transform.localScale;
					transform.localScale = new Vector3(x2, y, localScale2.z);
					newLevelItemImage.material = Materials.FlashWhiteMaterial;
					newLevelItemImage.material.SetFloat("_FlashAmount", 0f);
				}
				levelItemGO.transform.SetParent(levelsScreen.transform, worldPositionStays: false);
				RectTransform component = levelItemGO.GetComponent<RectTransform>();
				component.anchoredPosition = vector;
				levelItemGO.GetComponent<LevelNumComponent>().LevelNumber = num6;
				if ((GlobalCommons.Instance.globalGameStats.LevelsCompleted >= num6 - 1) ? true : false)
				{
					levelItemGO.GetComponent<Button>().onClick.AddListener(delegate
					{
						LevelButtonClick(levelItemGO);
					});
					GameObject gameObject = UnityEngine.Object.Instantiate(Prefabs.levelSelectButtonNumberPrefab);
					gameObject.transform.SetParent(levelItemGO.transform, worldPositionStays: false);
					gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
					gameObject.GetComponent<Text>().text = num6.ToString();
					RectTransform component2 = gameObject.GetComponent<RectTransform>();
					RectTransform rectTransform = component2;
					Vector2 anchoredPosition = component2.anchoredPosition;
					float x3 = anchoredPosition.x - 3f;
					Vector2 anchoredPosition2 = component2.anchoredPosition;
					rectTransform.anchoredPosition = new Vector2(x3, anchoredPosition2.y + 4f);
				}
				else
				{
					levelItemGO.GetComponent<Button>().onClick.AddListener(delegate
					{
						LockedLevelButtonClick(levelItemGO);
					});
					GameObject gameObject2 = UnityEngine.Object.Instantiate(Prefabs.levelLockedIconPrefab);
					gameObject2.transform.SetParent(levelItemGO.transform, worldPositionStays: false);
					gameObject2.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
					RectTransform component3 = gameObject2.GetComponent<RectTransform>();
					RectTransform rectTransform2 = component3;
					Vector2 anchoredPosition3 = component3.anchoredPosition;
					float x4 = anchoredPosition3.x - 3f;
					Vector2 anchoredPosition4 = component3.anchoredPosition;
					rectTransform2.anchoredPosition = new Vector2(x4, anchoredPosition4.y + 3f);
					RectTransform rectTransform3 = component3;
					Vector3 localScale3 = component3.localScale;
					rectTransform3.localScale = new Vector3(0.8f, 0.85f, localScale3.z);
				}
				if (screenId == lastSelectedScreen - 1)
				{
					RectTransform rectTransform4 = component;
					Vector2 anchoredPosition5 = component.anchoredPosition;
					float x5 = anchoredPosition5.x + vector.x / 2f;
					Vector2 anchoredPosition6 = component.anchoredPosition;
					rectTransform4.anchoredPosition = new Vector2(x5, anchoredPosition6.y + vector.y / 2f);
					component.DOAnchorPos(vector, 0.34f);
				}
				allLevelItems[screenId].Add(component);
				num++;
			}
		}
	}

	private void Update()
	{
		if (newLevelItemImage != null && Time.fixedTime > newLevelItemShakeTimestamp + 1f && !levelSelected)
		{
			newLevelItemRotationPunch *= -1f;
			newLevelItemShakeTimestamp = Time.fixedTime;
			float duration = 0.25f;
			newLevelItemImage.transform.DOPunchRotation(new Vector3(0f, 0f, newLevelItemRotationPunch), duration);
			newLevelItemImage.material.SetFloat("_FlashAmount", 0.5f);
			newLevelItemImage.material.DOFloat(0f, "_FlashAmount", duration);
			Transform transform = newLevelItemImage.transform;
			float x = defaultLevelItemScale * 1.2f;
			float y = defaultLevelItemScale * 1.2f;
			Vector3 localScale = newLevelItemImage.transform.localScale;
			transform.localScale = new Vector3(x, y, localScale.z);
			newLevelItemImage.transform.DOScale(defaultLevelItemScale * 1.1f, duration);
		}
		int num = scrollSnap.CurrentScreen();
		for (int i = 0; i < levelPages.Count; i++)
		{
			Image component = levelPages[i].GetComponent<Image>();
			if (num == i)
			{
				if (component.sprite != ActiveLevelsPageSprite)
				{
					component.sprite = ActiveLevelsPageSprite;
					Color red = Color.red;
					switch (i % 3)
					{
					case 0:
						red = Color.yellow;
						break;
					case 1:
						red = Color.cyan;
						break;
					case 2:
						red = Color.green;
						break;
					}
				}
			}
			else
			{
				component.sprite = InactiveLevelsPageSprite;
			}
		}
	}

	private void MenuButtonClick()
	{
		RectTransform component = menuButton.GetComponent<RectTransform>();
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
		RememberLastSelectedScreen();
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("Upgrades");
	}

	private void UnlockButtonClick()
	{
		GlobalCommons.Instance.globalGameStats.LevelsCompleted = GlobalCommons.Instance.levelsContainer.LevelsCount - 1;
		RememberLastSelectedScreen();
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("LevelSelection");
	}

	private void LockButtonClick()
	{
		GlobalCommons.Instance.globalGameStats.LevelsCompleted = 0;
		RememberLastSelectedScreen();
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("LevelSelection");
	}

	private void SurvivalButtonClick()
	{
		if (GlobalCommons.Instance.globalGameStats.LevelsCompleted < 20)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Prefabs.SurvivalModeLockedMessage, Vector3.zero, Quaternion.identity);
			gameObject.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
			SoundManager.instance.PlayButtonClickSound();
			return;
		}
		KickAllLevelItems();
		RectTransform component = survivalButton.GetComponent<RectTransform>();
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
		RememberLastSelectedScreen();
		GlobalCommons.Instance.gameplayMode = GlobalCommons.GameplayModes.SurvivalLevel;
		SoundManager.instance.PlayLevelSelectSound();
		SoundManager.instance.FadeOutMusic();
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("LoadingScene");
	}

	public static void InitSurvivalMode()
	{
		GlobalCommons.Instance.ActualSelectedLevel = 1;
	}

	private void KickAllLevelItems()
	{
		List<RectTransform> list = allLevelItems[lastSelectedScreen - 1];
		for (int i = 0; i < list.Count; i++)
		{
			RectTransform rectTransform = list[i];
			rectTransform.DOAnchorPos(rectTransform.anchoredPosition * 1.25f, 0.17f);
		}
	}

	private void ArenaButtonClick()
	{
		if (GlobalCommons.Instance.globalGameStats.LevelsCompleted < 10)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Prefabs.ArenaModeLockedMessage, Vector3.zero, Quaternion.identity);
			gameObject.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
			SoundManager.instance.PlayButtonClickSound();
			return;
		}
		KickAllLevelItems();
		RectTransform component = arenaButton.GetComponent<RectTransform>();
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
		RememberLastSelectedScreen();
		GlobalCommons.Instance.ActualSelectedLevel = GlobalCommons.Instance.globalGameStats.LevelsCompleted + 1;
		GlobalCommons.Instance.gameplayMode = GlobalCommons.GameplayModes.ArenaLevel;
		SoundManager.instance.PlayLevelSelectSound();
		SoundManager.instance.FadeOutMusic();
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("LoadingScene");
	}

	private void LockedLevelButtonClick(GameObject btn)
	{
		SoundManager.instance.PlayNotAvailableSound();
		btn.transform.DOShakePosition(0.1f, 15f);
	}

	private void LevelButtonClick(GameObject btn)
	{
		RememberLastSelectedScreen();
		List<RectTransform> list = allLevelItems[lastSelectedScreen - 1];
		for (int i = 0; i < list.Count; i++)
		{
			RectTransform rectTransform = list[i];
			if (!(rectTransform.gameObject == btn.gameObject))
			{
				rectTransform.DOAnchorPos(rectTransform.anchoredPosition * 1.25f, 0.17f);
			}
		}
		int levelNumber = btn.GetComponent<LevelNumComponent>().LevelNumber;
		RectTransform component = btn.GetComponent<RectTransform>();
		component.DOKill();
		RectTransform target = component;
		Vector3 localScale = component.localScale;
		target.DOScale(new Vector3(2f, 2f, localScale.z), 0.17f);
		float z = -25f + 25f * UnityEngine.Random.Range(0f, 2f);
		component.DORotate(new Vector3(0f, 0f, z), 0.17f);
		btn.transform.SetSiblingIndex(btn.transform.parent.childCount - 1);
		GlobalCommons.Instance.ActualSelectedLevel = levelNumber;
		GlobalCommons.Instance.gameplayMode = GlobalCommons.GameplayModes.RegularLevel;
		levelSelected = true;
		SoundManager.instance.PlayLevelSelectSound();
		SoundManager.instance.FadeOutMusic();
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
}
