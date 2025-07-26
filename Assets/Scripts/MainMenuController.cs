using DG.Tweening;
using GooglePlayGames;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
	private Button playButton;

	private Button EditorButton;

	//private Image AwesomeText;

	private Image TanksText;

	private Button InfoButton;

	private Button ShareButton;

	private Button SettingsButton;

	public Button ShopButton;

	private GameObject ExclamationImageGO;

	private Button CloudSaveButton;

	internal static bool FailedLevelMessagePending;

	private Image playButtonImage;

	private GameObject LimitedOfferBlock;

	private Text LimitedOfferBlockTimerText;

	private Image LeftTank;

	private Image RightTank;

	private static bool firstLaunch = true;

	private bool checkShopEnable;

	public GameObject LoadingObj;

	public static bool LoadingscreenShown;

	private void Start()
	{
		GlobalCommons.Instance.globalGameStats.TutorialCompleted = true;

		if (!LoadingscreenShown)
		{
			LoadingObj.SetActive(true);
            LoadingscreenShown = true;

            StartCoroutine(CloseLoadingScreen());
		}
		else
		{
            LoadingObj.SetActive(false);
        }


        AdmobManager.instance.ShowBanner();

        float num = 80f;
	//	//AwesomeText = GameObject.Find("AwesomeText").GetComponent<Image>();
	//	//RectTransform rectTransform = AwesomeText.rectTransform;
	////	Vector2 anchoredPosition = AwesomeText.rectTransform.anchoredPosition;
	//	float x = anchoredPosition.x;
	//	//Vector2 anchoredPosition2 = AwesomeText.rectTransform.anchoredPosition;
	//	//rectTransform.anchoredPosition = new Vector2(x, anchoredPosition2.y + num);
	//	RectTransform rectTransform2 = AwesomeText.rectTransform;
	//	Vector2 anchoredPosition3 = AwesomeText.rectTransform.anchoredPosition;
	//	float x2 = anchoredPosition3.x;
	//	Vector2 anchoredPosition4 = AwesomeText.rectTransform.anchoredPosition;
	//	rectTransform2.DOAnchorPos(new Vector2(x2, anchoredPosition4.y - num), 0.33f);
	//	AwesomeText.SetAlpha(0f);
	//	AwesomeText.DOFade(1f, 0.33f);
		
		playButton = GameObject.Find("PlayButton").GetComponent<Button>();

		playButton.onClick.AddListener(delegate
		{
			PlayButtonClick();
		});

		playButtonImage = playButton.gameObject.GetComponent<Image>();
		EditorButton = GameObject.Find("EditorButton").GetComponent<Button>();
		EditorButton.onClick.AddListener(ProcessEditorButtonClick);
		LimitedOfferBlock = GlobalCommons.Instance.CanvasBoundsController.UIRectTransform.Find("LimitedOfferBlock").gameObject;
		LimitedOfferBlock.SetActive(value: false);
		InfoButton = GameObject.Find("InfoButton").GetComponent<Button>();

		InfoButton.onClick.AddListener(delegate
		{
			InfoButtonClick();
		});

		ShareButton = GameObject.Find("ShareButton").GetComponent<Button>();
		//if (NativeShare.IsCurrentPlatformSupported)
		//{
			ShareButton.onClick.AddListener(delegate
			{
				Share.instance.ShareLink();
			});
		//}
		//else
		//{
		//	ShareButton.gameObject.SetActive(value: false);
		//}
		//CloudSaveButton = GameObject.Find("ICloudButton").GetComponent<Button>();
		if (GlobalCommons.Instance.SaveManager != null)
		{
			GlobalCommons.Instance.SaveManager.Initialize();
			//CloudSaveButton.onClick.AddListener(delegate
			//{
			//	ProcessCloudSaveButtonClick();
			//});
			//if (!GlobalCommons.Instance.globalGameStats.CloudSaveTutorialShown && !GlobalCommons.Instance.globalGameStats.DidSaveToCloud && GlobalCommons.Instance.globalGameStats.LevelsCompleted >= 10)
			//{
			//	GlobalCommons.Instance.globalGameStats.CloudSaveTutorialShown = true;
			//	GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("CloudSaveTutorial"));
			//}
		}
		else
		{
			//UnityEngine.Object.Destroy(CloudSaveButton.gameObject);
		}
		SettingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();
		//ShopButton = GameObject.Find("ShopButton").GetComponent<Button>();
		ExclamationImageGO = ShopButton.transform.Find("ExclamationImage").gameObject;
		ExclamationImageGO.SetActive(value: false);		


		if (!GlobalCommons.Instance.globalGameStats.TutorialCompleted)
		{
			RectTransform component = GameObject.Find("MuteSoundButton").GetComponent<RectTransform>();
			RectTransform component2 = GameObject.Find("MuteMusicButton").GetComponent<RectTransform>();
			RectTransform component3 = SettingsButton.GetComponent<RectTransform>();
			component2.anchoredPosition = component.anchoredPosition;
			component.anchoredPosition = component3.anchoredPosition;
			SettingsButton.gameObject.SetActive(value: false);
			ShopButton.gameObject.SetActive(value: false);
			EditorButton.gameObject.SetActive(value: false);
		}
		else
		{
			//SettingsButton.onClick.AddListener(delegate
			//{
			//	SettingsButtonClick();
			//});
			//if (ProcessSHopEnableCheck())
			//{
			//	ShopButton.onClick.AddListener(ProcessShopButtonClick);
			//}
			//else
			//{
			//	ShopButton.gameObject.SetActive(value: false);
			//	checkShopEnable = true;
			//}
		}


		if (EditorButton.gameObject.activeInHierarchy)
		{
			playButton.image.rectTransform.anchoredPosition += new Vector2(-75f, 0f);
			//EditorButton.image.rectTransform.anchoredPosition += new Vector2(-75f, 0f);
			EditorButton.image.SetAlpha(0f);
			EditorButton.image.DOFade(1f, 0.33f).SetDelay(0.4f);
		}
		playButtonImage.SetAlpha(0f);
		playButtonImage.DOFade(1f, 0.33f).SetDelay(0.3f);
		SoundManager.instance.ToggleMusic(SoundManager.MusicType.MenusMusic);
		if (!firstLaunch)
		{
			GlobalCommons.Instance.SaveGame();
		}
		else
		{
			firstLaunch = false;
		}
		LeftTank = GameObject.Find("LeftTank").GetComponent<Image>();
		RightTank = GameObject.Find("RightTank").GetComponent<Image>();
		float num2 = 50f;
		float num3 = 50f;
		float delay = 0.05f;
		float delay2 = 0.1f;
		float duration = 1f;
		float num4 = 0.75f;
		Ease ease = Ease.InOutQuint;
		RectTransform rectTransform5 = LeftTank.rectTransform;
		Vector2 anchoredPosition9 = LeftTank.rectTransform.anchoredPosition;
		float x5 = anchoredPosition9.x - num3;
		Vector2 anchoredPosition10 = LeftTank.rectTransform.anchoredPosition;
		rectTransform5.anchoredPosition = new Vector2(x5, anchoredPosition10.y - num2);
		LeftTank.SetAlpha(0f);
		LeftTank.rectTransform.localScale = new Vector3(num4, num4, num4);
		LeftTank.DOFade(1f, duration).SetDelay(delay).SetEase(ease);
		RectTransform rectTransform6 = LeftTank.rectTransform;
		Vector2 anchoredPosition11 = LeftTank.rectTransform.anchoredPosition;
		float x6 = anchoredPosition11.x + num3;
		Vector2 anchoredPosition12 = LeftTank.rectTransform.anchoredPosition;
		rectTransform6.DOAnchorPos(new Vector2(x6, anchoredPosition12.y + num2), duration).SetDelay(delay).SetEase(ease);
		LeftTank.rectTransform.DOScale(new Vector3(1f, 1f, 1f), duration).SetDelay(delay).SetEase(ease);
		RectTransform rectTransform7 = RightTank.rectTransform;
		Vector2 anchoredPosition13 = RightTank.rectTransform.anchoredPosition;
		float x7 = anchoredPosition13.x + num3;
		Vector2 anchoredPosition14 = RightTank.rectTransform.anchoredPosition;
		rectTransform7.anchoredPosition = new Vector2(x7, anchoredPosition14.y - num2);
		RightTank.SetAlpha(0f);
		RightTank.rectTransform.localScale = new Vector3(0f - num4, num4, num4);
		RightTank.DOFade(1f, duration).SetDelay(delay2).SetEase(ease);
		RectTransform rectTransform8 = RightTank.rectTransform;
		Vector2 anchoredPosition15 = RightTank.rectTransform.anchoredPosition;
		float x8 = anchoredPosition15.x - num3;
		Vector2 anchoredPosition16 = RightTank.rectTransform.anchoredPosition;
		rectTransform8.DOAnchorPos(new Vector2(x8, anchoredPosition16.y + num2), duration).SetDelay(delay2).SetEase(ease);
		RightTank.rectTransform.DOScale(new Vector3(-1f, 1f, 1f), duration).SetDelay(delay2).SetEase(ease);
		if (FailedLevelMessagePending)
		{
			FailedLevelMessagePending = false;
			GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("UnknownItemsUpdateGame"));
		}
	}

	IEnumerator CloseLoadingScreen()
	{
		yield return new WaitForSeconds(4);
		LoadingObj.SetActive(false);
    }

	private void ProcessEditorButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		RectTransform component = EditorButton.GetComponent<RectTransform>();
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
		GlobalCommons.Instance.gameplayMode = GlobalCommons.GameplayModes.EditorLevel;
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("LevelEditor");
	}

	private bool ProcessSHopEnableCheck()
	{
		return  GlobalCommons.Instance.globalGameStats.LevelsCompleted >= 0;
	}

	public void ProcessShopButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		RectTransform component = ShopButton.GetComponent<RectTransform>();
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
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("ShopScene");
	}

	private void ProcessCloudSaveButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		switch (Application.platform)
		{
		case RuntimePlatform.Android:
			if (Social.Active != null /* && PlayGamesPlatform.Instance.IsAuthenticated()*/)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(Prefabs.ICloudMenu, Vector3.zero, Quaternion.identity);
				gameObject.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
			}
			else
			{
				GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("LogInToText") + "Google Play Services" + Environment.NewLine + LocalizationManager.Instance.GetLocalizedText("MessageToEnableGameSaving"));
			}
			break;
		case RuntimePlatform.IPhonePlayer:
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Prefabs.ICloudMenu, Vector3.zero, Quaternion.identity);
			gameObject.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
			break;
		}
		}
	}

	private void Update()
	{
		if (checkShopEnable && ProcessSHopEnableCheck())
		{
			ShopButton.gameObject.SetActive(value: true);
			ShopButton.onClick.AddListener(ProcessShopButtonClick);
			checkShopEnable = false;
		}
		if (LimitedOfferBlock.activeInHierarchy)
		{
			if (GlobalCommons.Instance.PromotionController.PromoTimeLeft.HasValue)
			{
				if (LimitedOfferBlockTimerText != null)
				{
					LimitedOfferBlockTimerText.text = $"{GlobalCommons.Instance.PromotionController.PromoTimeLeft.Value.Hours:00}" + ":" + $"{GlobalCommons.Instance.PromotionController.PromoTimeLeft.Value.Minutes:00}" + ":" + $"{GlobalCommons.Instance.PromotionController.PromoTimeLeft.Value.Seconds:00}";
				}
			}
			else
			{
				LimitedOfferBlock.SetActive(value: false);
				ExclamationImageGO.SetActive(value: false);
			}
		}
		else if (Time.timeSinceLevelLoad > 1f  && GlobalCommons.Instance.PromotionController.PromoTimeLeft.HasValue && GlobalCommons.Instance.PromotionController.PromoTimeLeft.Value > TimeSpan.FromMinutes(5.0))
		{
			LimitedOfferBlock.gameObject.SetActive(value: true);
			RectTransform component = LimitedOfferBlock.GetComponent<RectTransform>();
			Vector2 anchoredPosition = component.anchoredPosition;
			Vector2 vector2 = component.anchoredPosition = anchoredPosition + new Vector2(0f, -100f);
			component.DOAnchorPos(anchoredPosition, 0.5f);
			RectTransform component2 = component.Find("Tankman").GetComponent<RectTransform>();
			Vector2 anchoredPosition2 = component2.anchoredPosition;
			Vector2 vector4 = component2.anchoredPosition = anchoredPosition2 + new Vector2(0f, -100f);
			component2.DOAnchorPos(anchoredPosition2, 0.5f).SetDelay(0.5f);
			Button component3 = component.Find("BgImage").GetComponent<Button>();
			component3.onClick.AddListener(ProcessPromoClick);
			LimitedOfferBlockTimerText = component.Find("TimerText").GetComponent<Text>();
			ExclamationImageGO.gameObject.SetActive(value: true);
			Image component4 = ExclamationImageGO.GetComponent<Image>();
			component4.SetAlpha(0f);
			component4.DOFade(1f, 0.2f);
		}
		if (Application.platform == RuntimePlatform.Android && UnityEngine.Input.GetKeyDown(KeyCode.Escape) && UnityEngine.Object.FindObjectOfType<ConfirmationWindow>() == null)
		{
			GlobalCommons.Instance.MessagesController.ShowConfirmationDialog(LocalizationManager.Instance.GetLocalizedText("ConfirmExitGame"), delegate
			{
				Application.Quit();
			}, null);
		}
	}

	private void ProcessPromoClick()
	{
		RectTransform component = LimitedOfferBlock.GetComponent<RectTransform>();
		Button component2 = component.Find("BgImage").GetComponent<Button>();
		component2.onClick.RemoveAllListeners();
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("ShopScene");
	}

	private void InfoButtonClick()
	{

		GameObject gameObject = UnityEngine.Object.Instantiate(Prefabs.CreditsMenu, Vector3.zero, Quaternion.identity);
		SoundManager.instance.PlayButtonClickSound();
		gameObject.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);

        AdmobManager.instance.HideBanner();

    }

    public void SettingsButtonClick()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Prefabs.SettingsMenu, Vector3.zero, Quaternion.identity);
		SoundManager.instance.PlayButtonClickSound();
		gameObject.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);

        AdmobManager.instance.HideBanner();

    }

    private void PlayButtonClick()
	{
		AdmobManager.instance.HideBanner();

		bool isEditor = Application.isEditor;
		SoundManager.instance.PlayButtonClickSound();
		RectTransform component = playButton.GetComponent<RectTransform>();
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

		if (GlobalCommons.Instance.globalGameStats.TutorialCompleted)
		{
			GlobalCommons.Instance.StateFaderController.ChangeSceneTo("Upgrades");
			return;
		}

		GlobalCommons.Instance.globalGameStats.UseStaticControls = GlobalCommons.Instance.UseStaticControlsAsDefault;
		SoundManager.instance.FadeOutMusic();
		GlobalCommons.Instance.ActualSelectedLevel = 1;
		GlobalCommons.Instance.gameplayMode = GlobalCommons.GameplayModes.TutorialLevel;
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("LoadingScene");
	}
}
