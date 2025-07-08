using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UserLevelsMenuController : MonoBehaviour
{
	internal enum LevelListType
	{
		Latest,
		Hardest,
		MostPlayed,
		MostLiked,
		Random,
		My
	}

	internal enum LevelListPeriodType
	{
		Weekly,
		Monthly
	}

	internal enum LoadLevelsListCallbackType
	{
		Initial,
		Additional
	}

	private Button MenuButton;

	private CanvasGroup PeriodSelectorCG;

	private Image SelectionIndicator;

	private Button SelectorButton;

	private Button LatestButton;

	private Button HardestButton;

	private Button MostPlayedButton;

	private Button MostLikedButton;

	private Button RandomButton;

	private Button MyLevelsButton;

	private Button PlayByIdButton;

	private InputField LevelIdInputField;

	private float? LevelsListItemHeight;

	private Text ErrorText;

	private Text NoOwnLevelsText;

	private List<Button> TabButtons;

	private List<LevelsListItemController> LevelsListItems;

	private CanvasGroup LevelsListScrollViewCanvasGroup;

	private ScrollRect LevelsListScrollRect;

	private LoadingSpinnerController LoadingSpinner;

	private LevelListType CurrentLevelListType;

	private List<LevelListType> LevelListTypesWithPeriod = new List<LevelListType>
	{
		LevelListType.Hardest,
		LevelListType.MostLiked,
		LevelListType.MostPlayed
	};

	private LevelListPeriodType CurrentLevelListPeriodType;

	private bool LoadingInProgress;

	private bool InitialFetch = true;

	public Sprite SelectedTabSprite;

	public Sprite UnSelectedTabSprite;

	private static LevelListPeriodType LastLevelListPeriodType;

	private static LevelListType LastListType;

	private int PageIndex;

	private const int PAGE_SIZE = 10;

	private const int PAGES_MAX = 5;

	private RectTransform ViewportRT;

	private RectTransform ScrollContentHolderRT;

	private RectTransform LevelsListLoadingSignRT;

	private float TabButtonSelectedPosition;

	private float TabButtonUnselectedPosition;

	private bool TheEnd;

	private int MaxPages => (CurrentLevelListType != LevelListType.My) ? 5 : 100;

	private void Start()
	{
		CurrentLevelListType = LastListType;
		CurrentLevelListPeriodType = LastLevelListPeriodType;
		MenuButton = GlobalCommons.Instance.CanvasBoundsController.UIRectTransform.Find("MenuButton").GetComponent<Button>();
		MenuButton.onClick.AddListener(MenuButtonClick);
		LevelIdInputField = GlobalCommons.Instance.CanvasBoundsController.UIRectTransform.Find("LevelIdInputField").GetComponent<InputField>();
		PlayByIdButton = GlobalCommons.Instance.CanvasBoundsController.UIRectTransform.Find("PlayByIdButton").GetComponent<Button>();
		PlayByIdButton.onClick.AddListener(PlayByIdButtonClick);
		ErrorText = GlobalCommons.Instance.CanvasBoundsController.UIRectTransform.Find("ErrorText").GetComponent<Text>();
		ErrorText.gameObject.SetActive(value: false);
		NoOwnLevelsText = GlobalCommons.Instance.CanvasBoundsController.UIRectTransform.Find("NoOwnLevelsText").GetComponent<Text>();
		NoOwnLevelsText.gameObject.SetActive(value: false);
		LevelsListItems = new List<LevelsListItemController>();
		LevelsListScrollRect = GlobalCommons.Instance.CanvasBoundsController.UIRectTransform.Find("LevelsListScrollView").GetComponent<ScrollRect>();
		PeriodSelectorCG = GlobalCommons.Instance.CanvasBoundsController.UIRectTransform.Find("PeriodSelector").GetComponent<CanvasGroup>();
		PeriodSelectorCG.alpha = 0f;
		SelectionIndicator = PeriodSelectorCG.transform.Find("SelectionIndicator").GetComponent<Image>();
		SelectorButton = PeriodSelectorCG.transform.Find("SelectorButton").GetComponent<Button>();
		SelectorButton.onClick.AddListener(delegate
		{
			if (!LoadingInProgress)
			{
				SoundManager.instance.PlayButtonClickSound();
				switch (CurrentLevelListPeriodType)
				{
				case LevelListPeriodType.Weekly:
					CurrentLevelListPeriodType = LevelListPeriodType.Monthly;
					break;
				case LevelListPeriodType.Monthly:
					CurrentLevelListPeriodType = LevelListPeriodType.Weekly;
					break;
				}
				UpdatePeriodSelectorPosition();
				TestAndReload(CurrentLevelListType, null, 0f, forceReload: true);
			}
		});
		UpdatePeriodSelectorPosition();
		Transform transform = GlobalCommons.Instance.CanvasBoundsController.UIRectTransform.Find("TabButtons");
		LatestButton = transform.Find("LatestButton").GetComponent<Button>();
		HardestButton = transform.Find("HardestButton").GetComponent<Button>();
		MostPlayedButton = transform.Find("MostPlayedButton").GetComponent<Button>();
		MostLikedButton = transform.Find("MostLikedButton").GetComponent<Button>();
		RandomButton = transform.Find("RandomButton").GetComponent<Button>();
		MyLevelsButton = transform.Find("MyLevelsButton").GetComponent<Button>();
		Vector2 anchoredPosition = LatestButton.image.rectTransform.anchoredPosition;
		TabButtonUnselectedPosition = anchoredPosition.y;
		TabButtonSelectedPosition = TabButtonUnselectedPosition + 7f;
		TabButtons = new List<Button>
		{
			LatestButton,
			HardestButton,
			MostPlayedButton,
			MostLikedButton,
			RandomButton,
			MyLevelsButton
		};
		ScrollContentHolderRT = GameObject.Find("LevelsListScrollViewContent").GetComponent<RectTransform>();
		ViewportRT = ScrollContentHolderRT.transform.parent.GetComponent<RectTransform>();
		LevelsListScrollViewCanvasGroup = GameObject.Find("LevelsListScrollViewContent").GetComponent<CanvasGroup>();
		LevelsListScrollViewCanvasGroup.alpha = 0f;
		LevelsListLoadingSignRT = UnityEngine.Object.Instantiate(Prefabs.LevelsListLoadingSign).GetComponent<RectTransform>();
		LevelsListLoadingSignRT.transform.SetParent(ScrollContentHolderRT.transform, worldPositionStays: false);
		SetLevelListAndTabButtonsEnabled(val: false);
		LoadingSpinner = LoadingSpinnerController.InstantiateNewSpinner(base.transform);
		switch (CurrentLevelListType)
		{
		case LevelListType.Hardest:
			ProcessTabButtonClick(HardestButton, playSound: false);
			break;
		case LevelListType.Latest:
			ProcessTabButtonClick(LatestButton, playSound: false);
			break;
		case LevelListType.MostLiked:
			ProcessTabButtonClick(MostLikedButton, playSound: false);
			break;
		case LevelListType.MostPlayed:
			ProcessTabButtonClick(MostPlayedButton, playSound: false);
			break;
		case LevelListType.My:
			ProcessTabButtonClick(MyLevelsButton, playSound: false);
			break;
		case LevelListType.Random:
			ProcessTabButtonClick(RandomButton, playSound: false);
			break;
		}
		SoundManager.instance.ToggleMusic(SoundManager.MusicType.MenusMusic);
	}

	private void UpdatePeriodSelectorPosition()
	{
		SelectionIndicator.rectTransform.rotation = Quaternion.Euler(0f, 0f, (CurrentLevelListPeriodType != 0) ? 90f : 270f);
	}

	private void Update()
	{
		if (!LoadingInProgress && PageIndex < MaxPages && !TheEnd)
		{
			float verticalNormalizedPosition = LevelsListScrollRect.verticalNormalizedPosition;
			Vector2 sizeDelta = ScrollContentHolderRT.sizeDelta;
			if (verticalNormalizedPosition * (sizeDelta.y - 250f) < 45f && LoadLevelsList(CurrentLevelListType, fromTabClick: false))
			{
				UnityEngine.Debug.Log("Loading additional items...");
			}
		}
		LevelsListItems.ForEach(delegate(LevelsListItemController itm)
		{
			bool flag = true;
			float y = itm.ItemCoords.y;
			Vector2 anchoredPosition = ScrollContentHolderRT.anchoredPosition;
			float num = y + anchoredPosition.y;
			float? levelsListItemHeight = LevelsListItemHeight;
			if (levelsListItemHeight.HasValue && num > levelsListItemHeight.GetValueOrDefault())
			{
				flag = false;
			}
			else
			{
				float? levelsListItemHeight2 = LevelsListItemHeight;
				float? num2 = (!levelsListItemHeight2.HasValue) ? null : new float?(0f - ViewportRT.rect.height - levelsListItemHeight2.GetValueOrDefault());
				if (num2.HasValue && num < num2.GetValueOrDefault())
				{
					flag = false;
				}
			}
			if (itm.gameObject.activeInHierarchy != flag)
			{
				itm.gameObject.SetActive(flag);
			}
		});
	}

	private void PlayByIdButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		if (int.TryParse(LevelIdInputField.text, out int result))
		{
			LoadLevelMenu.ShowMenu(result, allowEditing: false);
		}
		else
		{
			GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("NonIntLevelIdTextError"));
		}
	}

	internal void ProcessLevelItemClick(UserLevelInfo userLevelInfo)
	{
		LastListType = CurrentLevelListType;
		LastLevelListPeriodType = CurrentLevelListPeriodType;
		SoundManager.instance.PlayButtonClickSound();
		LoadLevelMenu.ShowMenu(userLevelInfo.id, CurrentLevelListType == LevelListType.My);
	}

	private bool LoadLevelsList(LevelListType currentLevelListType, bool fromTabClick = true, bool forceReload = false)
	{
		if (LoadingInProgress)
		{
			return false;
		}
		if (InitialFetch)
		{
			InitialFetch = false;
		}
		else if (!forceReload && fromTabClick && currentLevelListType == CurrentLevelListType && currentLevelListType != LevelListType.Random)
		{
			return false;
		}
		if (fromTabClick)
		{
			PageIndex = 0;
			TheEnd = false;
			LevelsListItems.ForEach(delegate(LevelsListItemController itm)
			{
				UnityEngine.Object.Destroy(itm.gameObject);
			});
			LevelsListItems = new List<LevelsListItemController>();
			CurrentLevelListType = currentLevelListType;
			SetLevelListAndTabButtonsEnabled(val: false);
			string empty = string.Empty;
			switch (currentLevelListType)
			{
			case LevelListType.Hardest:
				empty = LocalizationManager.Instance.GetLocalizedText("LoadingUserLevels_Hardest");
				break;
			case LevelListType.Latest:
				empty = LocalizationManager.Instance.GetLocalizedText("LoadingUserLevels_Latest");
				break;
			case LevelListType.MostLiked:
				empty = LocalizationManager.Instance.GetLocalizedText("LoadingUserLevels_MostLiked");
				break;
			case LevelListType.MostPlayed:
				empty = LocalizationManager.Instance.GetLocalizedText("LoadingUserLevels_MostPlayed");
				break;
			case LevelListType.Random:
				empty = LocalizationManager.Instance.GetLocalizedText("LoadingUserLevels_Random");
				break;
			case LevelListType.My:
				empty = LocalizationManager.Instance.GetLocalizedText("LoadingUserLevels_My");
				break;
			default:
				throw new Exception("unknown level list type...");
			}
			LoadingSpinner.Show(empty);
		}
		else
		{
			PageIndex++;
		}
		LoadingInProgress = true;
		FireLevelListRequest((!fromTabClick) ? new ListLevelsRequest(currentLevelListType, CurrentLevelListPeriodType, 10, PageIndex, LoadLevelsListCallbackType.Additional) : new ListLevelsRequest(currentLevelListType, CurrentLevelListPeriodType, 10, PageIndex, LoadLevelsListCallbackType.Initial));
		return true;
	}

	private void FireLevelListRequest(ListLevelsRequest listLevelsRequest)
	{
		ListLevelsRequest listLevelsRequest2 = GlobalCommons.Instance.CachedWebRequests.CachedListLevelsRequests.Find((ListLevelsRequest rq) => rq.LoadLevelsListCallbackType == listLevelsRequest.LoadLevelsListCallbackType && rq.LevelListType == listLevelsRequest.LevelListType && rq.LevelListPeriodType == listLevelsRequest.LevelListPeriodType && rq.PageIndex == listLevelsRequest.PageIndex && rq.PageSize == listLevelsRequest.PageSize);
		if (listLevelsRequest2 == null)
		{
			UnityEngine.Debug.Log("No cached result found, calling API...");
			//GlobalCommons.Instance.CachedWebRequests.CachedListLevelsRequests.Add(listLevelsRequest.AssociateWWW(AsyncRestCaller.GetUserLevelsList(listLevelsRequest.LevelListType, listLevelsRequest.LevelListPeriodType, 10, listLevelsRequest.PageIndex, ProcessActualWebRequestResult)));
		}
		else
		{
			UnityEngine.Debug.Log("Cached result found, reusing...");
			ProcessLevelsListResultCallbalck(listLevelsRequest2.Request);
		}
	}

	private void ProcessActualWebRequestResult(UnityWebRequest request)
	{
		ListLevelsRequest listLevelsRequest = GlobalCommons.Instance.CachedWebRequests.CachedListLevelsRequests.Find((ListLevelsRequest rq) => rq.Request == request);
		if (listLevelsRequest != null)
		{
			listLevelsRequest.ResultTimestamp = DateTime.Now;
		}
		ProcessLevelsListResultCallbalck(request);
	}

	private void ProcessLevelsListResultCallbalck(UnityWebRequest request)
	{
		ListLevelsRequest listLevelsRequest = GlobalCommons.Instance.CachedWebRequests.CachedListLevelsRequests.Find((ListLevelsRequest rq) => rq.Request == request);
		if (listLevelsRequest != null)
		{
			switch (listLevelsRequest.LoadLevelsListCallbackType)
			{
			case LoadLevelsListCallbackType.Additional:
				StartCoroutine(ProcessLevelsAdditionalLoadResult(request));
				break;
			case LoadLevelsListCallbackType.Initial:
				StartCoroutine(ProcessLevelsInitialLoadResult(request));
				break;
			}
		}
	}

	private IEnumerator ProcessLevelsAdditionalLoadResult(UnityWebRequest request)
	{
		yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(1.25f, 1.5f));
		LoadingInProgress = false;
		if (request.error != null)
		{
			PageIndex--;
		}
		else
		{
			ProcessLoadedLevelsData(request);
		}
	}

	private IEnumerator ProcessLevelsInitialLoadResult(UnityWebRequest request)
	{
		yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(1f, 1.25f));
		LoadingSpinner.Hide();
		LoadingInProgress = false;
		if (request.error != null)
		{
			ErrorText.gameObject.SetActive(value: true);
			yield break;
		}
		SetLevelListAndTabButtonsEnabled(val: true);
		ProcessLoadedLevelsData(request);
		LevelsListScrollRect.verticalNormalizedPosition = 1f;
	}

	private void ProcessLoadedLevelsData(UnityWebRequest request)
	{
		float num = 50f;
		int num2 = -1;
		if (!string.IsNullOrEmpty(request.downloadHandler.text))
		{
			try
			{
				string text = request.downloadHandler.text;
				string json = "{ \"results\": " + text + "}";
				List<UserLevelInfo> list = new List<UserLevelInfo>(JsonUtility.FromJson<UserLevelInfoCollection>(json).results);
				for (int i = 0; i < list.Count; i++)
				{
					LevelsListItemController component = UnityEngine.Object.Instantiate(Prefabs.LevelsListItem).GetComponent<LevelsListItemController>();
					component.transform.SetParent(ScrollContentHolderRT.transform, worldPositionStays: false);
					component.Init(this, -22.3f - num * (float)LevelsListItems.Count);
					component.FeedLevelInfo(list[i]);
					if (!LevelsListItemHeight.HasValue)
					{
						LevelsListItemHeight = component.GetComponent<RectTransform>().rect.height;
					}
					LevelsListItems.Add(component);
				}
				num2 = list.Count;
				if (list.Count < 10)
				{
					TheEnd = true;
				}
			}
			catch (Exception)
			{
				TheEnd = true;
			}
		}
		else
		{
			TheEnd = true;
		}
		if (TheEnd && num2 == 0 && LevelsListItems.Count == 0 && CurrentLevelListType == LevelListType.My)
		{
			NoOwnLevelsText.gameObject.SetActive(value: true);
		}
		int num3 = LevelsListItems.Count;
		if (PageIndex >= MaxPages || TheEnd)
		{
			LevelsListLoadingSignRT.gameObject.SetActive(value: false);
		}
		else
		{
			LevelsListLoadingSignRT.gameObject.SetActive(value: true);
			RectTransform levelsListLoadingSignRT = LevelsListLoadingSignRT;
			Vector2 anchoredPosition = LevelsListLoadingSignRT.anchoredPosition;
			levelsListLoadingSignRT.anchoredPosition = new Vector2(anchoredPosition.x, -22.3f - num * (float)LevelsListItems.Count + 1f);
			num3++;
		}
		RectTransform scrollContentHolderRT = ScrollContentHolderRT;
		Vector2 sizeDelta = ScrollContentHolderRT.sizeDelta;
		scrollContentHolderRT.sizeDelta = new Vector2(sizeDelta.x, num * (float)num3 - 6f);
	}

	private void SetLevelListAndTabButtonsEnabled(bool val)
	{
		LevelsListScrollViewCanvasGroup.DOKill();
		LevelsListScrollViewCanvasGroup.DOFade((!val) ? 0f : 1f, 0.33f);
		LevelsListScrollViewCanvasGroup.interactable = val;
		TabButtons.ForEach(delegate(Button tb)
		{
			if (val)
			{
				tb.onClick.AddListener(delegate
				{
					ProcessTabButtonClick(tb);
				});
			}
			else
			{
				tb.onClick.RemoveAllListeners();
			}
		});
	}

	private void ProcessTabButtonClick(Button btn, bool playSound = true)
	{
		if (playSound)
		{
			SoundManager.instance.PlayButtonClickSound();
		}
		LevelListType listType = LevelListType.Latest;
		switch (btn.name)
		{
		case "LatestButton":
			listType = LevelListType.Latest;
			break;
		case "HardestButton":
			listType = LevelListType.Hardest;
			break;
		case "MostPlayedButton":
			listType = LevelListType.MostPlayed;
			break;
		case "MostLikedButton":
			listType = LevelListType.MostLiked;
			break;
		case "RandomButton":
			listType = LevelListType.Random;
			break;
		case "MyLevelsButton":
			listType = LevelListType.My;
			break;
		}
		TestAndReload(listType, btn, (!playSound) ? 0f : 0.2f);
	}

	private void TestAndReload(LevelListType listType, Button btn, float TabButtonsTweenTime, bool forceReload = false)
	{
		if (LoadLevelsList(listType, fromTabClick: true, forceReload))
		{
			PeriodSelectorCG.alpha = ((!LevelListTypesWithPeriod.Exists((LevelListType itm) => itm == listType)) ? 0f : 1f);
			NoOwnLevelsText.gameObject.SetActive(value: false);
			ErrorText.gameObject.SetActive(value: false);
			if (btn != null)
			{
				TabButtons.ForEach(delegate(Button tb)
				{
					if (tb == btn)
					{
						tb.image.rectTransform.DOAnchorPosY(TabButtonSelectedPosition, TabButtonsTweenTime);
						tb.transform.Find("ItemIcon").GetComponent<Image>().DOFade(1f, TabButtonsTweenTime);
					}
					else
					{
						tb.image.rectTransform.DOAnchorPosY(TabButtonUnselectedPosition, TabButtonsTweenTime);
						tb.transform.Find("ItemIcon").GetComponent<Image>().DOFade(0.5f, TabButtonsTweenTime);
					}
				});
			}
		}
	}

	private void MenuButtonClick()
	{
		LastListType = CurrentLevelListType;
		LastLevelListPeriodType = CurrentLevelListPeriodType;
		RectTransform component = MenuButton.GetComponent<RectTransform>();
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
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("LevelSelection");
	}
}
