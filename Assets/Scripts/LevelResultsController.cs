using DG.Tweening;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LevelResultsController : MonoBehaviour
{
	private Image levelResultsFadeImage;

	private GameObject levelResultsBackground;

	private Image ResolutionImage;

	private Text TimerText;

	private Text ProfitImage;

	private Image ProfitBGImage;

	private float faderFadeSpeed = 0.05f;

	private float panelFadeSpeed = 0.05f;

	private CanvasGroup panelCanvasGourp;

	private float maxFaderAlpha = 0.7f;

	private bool controllerEnabled;

	private Button continueButton;

	private Button LikeButton;

	private Button ReportButton;

	private Button restartButton;

	private Button rewardedAdButton;

	private Button TankobankButton;

	private Text earningsText;

	private Text NewUpgradesAvailableText;

	private float scaleFactor = 0.999999f;

	public Sprite levelFailedResolutionImage;

	public Sprite gameOverResolutionImage;

	public Sprite newLevelCompleteResolutionImage;

	private bool profitAnimationStarted;

	private int targetProfitValue;

	private int currentProfitValue;

	private int profitStepPerSec;

	private int OfferCoinsDispenseCtr;

	private Vector2 earningsTextPosition;

	private bool rewadEligible;

	private bool tankobankEligible;

	internal bool tankobankDialogOpen;

	private List<float> fireworksTimestamps;

	private bool enableTimestampSet;

	private float enableTimestamp;

	private bool doCountProfit = true;

	private float initialControlButtonsDistance;

	private Button ShareButton;

	public bool IsEnabled => controllerEnabled;

	private void Start()
	{
		levelResultsFadeImage = GameObject.Find("LevelResultsFadeImage").GetComponent<Image>();
		if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.EditorLevel)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			UnityEngine.Object.Destroy(levelResultsFadeImage.gameObject);
			ShareButton = GameObject.Find("ShareProgressButton").GetComponent<Button>();
			UnityEngine.Object.Destroy(ShareButton.gameObject);
			return;
		}
		levelResultsFadeImage.enabled = false;
		SetFaderAlpha(0f);
		levelResultsBackground = GameObject.Find("LevelResultsBackground");
		SetLevelResultsEnabled(val: false);
		panelCanvasGourp = levelResultsBackground.GetComponent<CanvasGroup>();
		panelCanvasGourp.alpha = 0f;
		SetScale(scaleFactor);
		restartButton = levelResultsBackground.transform.Find("RestartButton").GetComponent<Button>();
		if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.TutorialLevel)
		{
			UnityEngine.Object.Destroy(restartButton.gameObject);
		}
		else
		{
			restartButton.onClick.AddListener(delegate
			{
				RestartButtonClick();
			});
		}
		rewardedAdButton = levelResultsBackground.transform.Find("RewardedAdButton").GetComponent<Button>();
		TankobankButton = levelResultsBackground.transform.Find("TankobankButton").GetComponent<Button>();
		LikeButton = levelResultsBackground.transform.Find("LikeButton").GetComponent<Button>();
		LikeButton.gameObject.SetActive(value: false);
		ReportButton = levelResultsBackground.transform.Find("ReportButton").GetComponent<Button>();
		ReportButton.gameObject.SetActive(value: false);
		continueButton = levelResultsBackground.transform.Find("ContinueButton").GetComponent<Button>();
		continueButton.onClick.AddListener(delegate
		{
			ContinueButtonClick();
		});
		RectTransform component = continueButton.GetComponent<RectTransform>();
		Vector2 anchoredPosition = component.anchoredPosition;
		initialControlButtonsDistance = anchoredPosition.x;
		ShareButton = GameObject.Find("ShareProgressButton").GetComponent<Button>();
		ShareButton.onClick.AddListener(delegate
		{
			Share.instance.ShareLink();
		});
		earningsText = levelResultsBackground.transform.Find("EarningsText").GetComponent<Text>();
		NewUpgradesAvailableText = levelResultsBackground.transform.Find("NewUpgradesAvailableText").GetComponent<Text>();
		ResolutionImage = levelResultsBackground.transform.Find("ResolutionImage").GetComponent<Image>();
		TimerText = levelResultsBackground.transform.Find("TimerText").GetComponent<Text>();
		ProfitImage = levelResultsBackground.transform.Find("ProfitImage").GetComponent<Text>();
		ProfitBGImage = levelResultsBackground.transform.Find("ProfitBGImage").GetComponent<Image>();
		ResolutionImage.enabled = false;
		ProfitImage.enabled = false;
		continueButton.GetComponent<Image>().enabled = false;
		ShareButton.gameObject.SetActive(value: false);
		if ((bool)restartButton)
		{
			restartButton.GetComponent<Image>().enabled = false;
		}
		earningsText.enabled = false;
		TimerText.enabled = false;
		NewUpgradesAvailableText.enabled = false;
		rewardedAdButton.gameObject.SetActive(value: false);
		TankobankButton.gameObject.SetActive(value: false);
	}

	private void SetScale(float val)
	{
		Transform transform = panelCanvasGourp.transform;
		Vector3 localScale = panelCanvasGourp.transform.localScale;
		transform.localScale = new Vector3(val, val, localScale.z);
	}

	private void ContinueButtonClick()
	{
		//bool isReady = AdManager.IsInterstitialAdReady();
		//// Show it if it's ready
		//if (isReady)
		//{
		//    AdManager.ShowInterstitialAd();
		//}
		AdmobManager.instance.ShowInterstitialAd();

        GameplayCommons.Instance.effectsSpawner.DisableAllParticles();
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.GameplayModes gameplayMode = GlobalCommons.Instance.gameplayMode;
		if (gameplayMode == GlobalCommons.GameplayModes.CustomLevel)
		{
			if (AdsProcessor.GetInterstitialAdsAllowedAfterLevelFinish())
			{
				GlobalCommons.Instance.SceneToTransferTo = "UserLevels";
				GlobalCommons.Instance.StateFaderController.ChangeSceneTo("Upgrades");
			}
			else
			{
				GlobalCommons.Instance.StateFaderController.ChangeSceneTo("Upgrades");
			}
		}
		else if (GameplayCommons.Instance.LastLevelCompleted)
		{
			GlobalCommons.Instance.StateFaderController.ChangeSceneTo("AllLevelsCompleteScene");
		}
		else if (!AskForReviewSceneController.ShownThisSession && AskForReviewSceneController.ReviewAvailable() && !GlobalCommons.Instance.globalGameStats.RatedGame && GlobalCommons.Instance.globalGameStats.AskForReviewFactor >= 2 && GlobalCommons.Instance.globalGameStats.WeaponsLevels[1] > 0)
		{
			GlobalCommons.Instance.StateFaderController.ChangeSceneTo("AskForReviewScene");
		}
		else if (AdsProcessor.GetInterstitialAdsAllowedAfterLevelFinish())
		{
			GlobalCommons.Instance.SceneToTransferTo = "Upgrades";
			GlobalCommons.Instance.StateFaderController.ChangeSceneTo("Upgrades");
		}
		else
		{
			GlobalCommons.Instance.StateFaderController.ChangeSceneTo("Upgrades");
		}
	}

	private void TankobankButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		GameObject gameObject = UnityEngine.Object.Instantiate(Prefabs.TankoBankDialog, Vector3.zero, Quaternion.identity);
		gameObject.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
		tankobankDialogOpen = true;
	}

	private void RewardedAdButtonClick()
	{
		if(AdmobManager.RewardAdAvailable)
		{			
			AdmobManager.instance.ShowRewardedAd(() =>
			{
				GlobalCommons.Instance.globalGameStats.IncreaseMoney(100);
				GlobalCommons.Instance.SaveGame();
			});
		}
		//bool isReady = AdManager.IsRewardedAdReady();
		//// Show it if it's ready
		//if (isReady)
		//{
		//    AdManager.ShowRewardedAd();
		//}
		//      GameplayCommons.Instance.effectsSpawner.DisableAllParticles();
		//int num = PlayRewardedAdSceneController.SetNextRewardValue(PlayRewardedAdSceneController.RewadResetLocation.Upgrades);
		//GlobalCommons.Instance.MessagesController.ShowConfirmationDialog(LocalizationManager.Instance.GetLocalizedText("WatchShortVideoForText") + Environment.NewLine + num.ToString() + " " + LocalizationManager.Instance.GetCoinsNumberEnding(num) + "?", ProceedToRewad, null, 0.75f);
	}

    void OnEnable()
    {
       // AdManager.RewardedAdCompleted += RewardedAdCompletedHandler;
    }
    // The event handler
    //void RewardedAdCompletedHandler(RewardedAdNetwork network, AdLocation location)
    //{
    //    GameplayCommons.Instance.effectsSpawner.DisableAllParticles();
    //    int num = PlayRewardedAdSceneController.SetNextRewardValue(PlayRewardedAdSceneController.RewadResetLocation.Upgrades);
    //    GlobalCommons.Instance.MessagesController.ShowConfirmationDialog(LocalizationManager.Instance.GetLocalizedText("WatchShortVideoForText") + Environment.NewLine + num.ToString() + " " + LocalizationManager.Instance.GetCoinsNumberEnding(num) + "?", ProceedToRewad, null, 0.75f);
    //}
    // Unsubscribe
    void OnDisable()
    {
      //  AdManager.RewardedAdCompleted -= RewardedAdCompletedHandler;
    }

    private void ProceedToRewad()
	{
		GlobalCommons.Instance.SceneToTransferTo = "Upgrades";
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("PlayRewardedAdScene");
	}

	private void RestartButtonClick()
	{
		//bool isReady = AdManager.IsInterstitialAdReady();
		//// Show it if it's ready
		//if (isReady)
		//{
		//    AdManager.ShowInterstitialAd();
		//}
		AdmobManager.instance.ShowInterstitialAd();

        GameplayCommons.Instance.effectsSpawner.DisableAllParticles();
		GameplayCommons.Instance.SetDesiredTimeScale(1f);
		SoundManager.instance.PlayButtonClickSound();
		if (AdsProcessor.GetInterstitialAdsAllowedOnLevelRestart())
		{
			GlobalCommons.Instance.SceneToTransferTo = "LoadingScene";
			GlobalCommons.Instance.StateFaderController.ChangeSceneTo("Upgrades");
		}
		else
		{
			GlobalCommons.Instance.StateFaderController.ChangeSceneTo("LoadingScene");
		}
	}

	private void SetFaderAlpha(float val)
	{
		Image image = levelResultsFadeImage;
		Color color = levelResultsFadeImage.color;
		float r = color.r;
		Color color2 = levelResultsFadeImage.color;
		float g = color2.g;
		Color color3 = levelResultsFadeImage.color;
		image.color = new Color(r, g, color3.b, val);
	}

	private void SetLevelResultsEnabled(bool val)
	{
		levelResultsBackground.SetActive(val);
	}

	private void Update()
	{
		if (!controllerEnabled)
		{
			return;
		}
		if (!enableTimestampSet)
		{
			enableTimestamp = Time.fixedTime;
			enableTimestampSet = true;
		}
		if (fireworksTimestamps.Count > 0 && Time.fixedTime - enableTimestamp > fireworksTimestamps[0])
		{
			fireworksTimestamps.RemoveAt(0);
			SoundManager.instance.PlayFireworkSound();
			GameplayCommons.Instance.cameraController.ShakeCamera(2f);
			EffectsSpawner effectsSpawner = GameplayCommons.Instance.effectsSpawner;
			Vector3 position = Camera.main.transform.position;
			float x = position.x - GlobalCommons.Instance.horizontalCameraBorderWithCompensation / 2f + UnityEngine.Random.value * GlobalCommons.Instance.horizontalCameraBorderWithCompensation;
			Vector3 position2 = Camera.main.transform.position;
			effectsSpawner.SpawnFireworksEffect(new Vector3(x, position2.y - GlobalCommons.Instance.verticalCameraBorderWithCompensation / 2f + UnityEngine.Random.value * GlobalCommons.Instance.verticalCameraBorderWithCompensation, 0f));
		}
		if (!levelResultsFadeImage.enabled)
		{
			levelResultsFadeImage.enabled = true;
			SetLevelResultsEnabled(val: true);
		}
		Color color = levelResultsFadeImage.color;
		if (color.a < maxFaderAlpha)
		{
			Color color2 = levelResultsFadeImage.color;
			float num = color2.a + faderFadeSpeed;
			if (num > maxFaderAlpha)
			{
				num = maxFaderAlpha;
			}
			SetFaderAlpha(num);
		}
		if (panelCanvasGourp.alpha < 1f)
		{
			float num = panelCanvasGourp.alpha + panelFadeSpeed;
			if (num > 1f)
			{
				num = 1f;
			}
			panelCanvasGourp.alpha = num;
		}
		if (scaleFactor < 1f)
		{
			scaleFactor += 0.01f;
			if (scaleFactor > 1f)
			{
				scaleFactor = 1f;
			}
			SetScale(scaleFactor);
			if (scaleFactor == 1f)
			{
				ProcessControlsFadeIn();
			}
		}
		if (!profitAnimationStarted)
		{
			return;
		}
		if (currentProfitValue < targetProfitValue)
		{
			float num2 = 3f;
			earningsText.rectTransform.anchoredPosition = new Vector2(earningsTextPosition.x + UnityEngine.Random.Range(0f - num2, num2), earningsTextPosition.y + UnityEngine.Random.Range(0f - num2, num2));
			GameplayCommons.Instance.effectsSpawner.SpawnOverUICoinFlyoffEffect(earningsText.transform.position);
			SoundManager.instance.PlayCoinCountSound();
			int num3 = Mathf.FloorToInt((float)profitStepPerSec * Time.deltaTime);
			if (num3 < 1)
			{
				num3 = 1;
			}
			currentProfitValue += num3;
			if (currentProfitValue > targetProfitValue)
			{
				currentProfitValue = targetProfitValue;
			}
			earningsText.text = currentProfitValue.ToString();
			if (currentProfitValue == targetProfitValue)
			{
				EnableMenuButtons();
			}
			return;
		}
		earningsText.rectTransform.anchoredPosition = earningsTextPosition;
		if (tankobankDialogOpen)
		{
			return;
		}
		if (OfferCoinsDispenseCtr < 5)
		{
			OfferCoinsDispenseCtr++;
			return;
		}
		OfferCoinsDispenseCtr = 0;
		if (tankobankEligible)
		{
			GameplayCommons.Instance.effectsSpawner.SpawnOverUICoinFlyoffEffect(TankobankButton.transform.position, 0.5f);
		}
		if (rewadEligible && !GlobalCommons.Instance.globalGameStats.isPayingPlayer)
		{
			GameplayCommons.Instance.effectsSpawner.SpawnOverUICoinFlyoffEffect(rewardedAdButton.transform.position, 0.5f);
		}
	}

	private void EnableMenuButtons()
	{
		float num = 0f;
		if ((bool)restartButton)
		{
			num = 0.1f;
			Image component = restartButton.GetComponent<Image>();
			component.enabled = true;
			component.SetAlpha(0f);
			Image target = component;
			Color color = component.color;
			float r = color.r;
			Color color2 = component.color;
			float g = color2.g;
			Color color3 = component.color;
			target.DOColor(new Color(r, g, color3.b, 1f), 0.2f);
		}
		Image component2 = continueButton.GetComponent<Image>();
		component2.enabled = true;
		component2.SetAlpha(0f);
		Image target2 = component2;
		Color color4 = component2.color;
		float r2 = color4.r;
		Color color5 = component2.color;
		float g2 = color5.g;
		Color color6 = component2.color;
		target2.DOColor(new Color(r2, g2, color6.b, 1f), 0.2f).SetDelay(num);
		if (UpgradesMenuController.AnyUpgradeAvailable && GlobalCommons.Instance.gameplayMode != GlobalCommons.GameplayModes.TutorialLevel)
		{
			NewUpgradesAvailableText.enabled = true;
			NewUpgradesAvailableText.SetAlpha(0f);
			Extensions.DelayedCallWithCoroutine(num + 0.2f, delegate
			{
				BlinkNewUpgradesAvailableText();
			});
		}
		if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.CustomLevel)
		{
			ReportButton.gameObject.SetActive(value: true);
			ReportButton.image.SetAlpha(0f);
			ReportButton.image.DOFade(1f, 0.2f).SetDelay(num + 0.1f);
			ReportButton.onClick.AddListener(ProcessReportClick);
			LikeButton.gameObject.SetActive(value: true);
			LikeButton.image.SetAlpha(0f);
			LikeButton.image.DOFade(1f, 0.2f).SetDelay(num + 0.1f);
			LikeButton.onClick.AddListener(LikeButtonClick);
			Vector2 anchoredPosition = LikeButton.image.rectTransform.anchoredPosition;
			float x = anchoredPosition.x;
			Vector2 anchoredPosition2 = ReportButton.image.rectTransform.anchoredPosition;
			float num2 = Mathf.Abs(x - anchoredPosition2.x);
			RectTransform rectTransform = continueButton.image.rectTransform;
			Vector2 anchoredPosition3 = continueButton.image.rectTransform.anchoredPosition;
			float x2 = anchoredPosition3.x + num2;
			Vector2 anchoredPosition4 = continueButton.image.rectTransform.anchoredPosition;
			rectTransform.anchoredPosition = new Vector2(x2, anchoredPosition4.y);
			RectTransform rectTransform2 = restartButton.image.rectTransform;
			Vector2 anchoredPosition5 = restartButton.image.rectTransform.anchoredPosition;
			float x3 = anchoredPosition5.x - num2;
			Vector2 anchoredPosition6 = restartButton.image.rectTransform.anchoredPosition;
			rectTransform2.anchoredPosition = new Vector2(x3, anchoredPosition6.y);
		}
		//if (NativeShare.IsCurrentPlatformSupported && GlobalCommons.Instance.gameplayMode != GlobalCommons.GameplayModes.TutorialLevel)
		//{
		//	ShareButton.gameObject.SetActive(value: true);
		//	Image component3 = ShareButton.GetComponent<Image>();
		//	component3.enabled = true;
		//	component3.SetAlpha(0f);
		//	Image target3 = component3;
		//	Color color7 = component3.color;
		//	float r3 = color7.r;
		//	Color color8 = component3.color;
		//	float g3 = color8.g;
		//	Color color9 = component3.color;
		//	target3.DOColor(new Color(r3, g3, color9.b, 1f), 0.2f).SetDelay(num);
		//}
		InitializeOfferButtons();
	}

	private void ProcessReportClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.MessagesController.ShowConfirmationDialog(LocalizationManager.Instance.GetLocalizedText("ConfirmLevelReport"), delegate
		{
			GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("ThanksForLevelReport"));
			ReportButton.onClick.RemoveAllListeners();
			//AsyncRestCaller.ReportLevel(int.Parse(LevelEditorController.LoadedCustomLevelID), delegate
			//{
			//});
		}, null);
	}

	private void BlinkNewUpgradesAvailableText()
	{
		float endValue = 0f;
		Color color = NewUpgradesAvailableText.color;
		if (color.a == 0f)
		{
			endValue = 1f;
		}
		NewUpgradesAvailableText.DOFade(endValue, 0.75f).OnCompleteWithCoroutine(BlinkNewUpgradesAvailableText);
	}

	private void LikeButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		LikeButton.onClick.RemoveAllListeners();
		//AsyncRestCaller.LikeLevel(int.Parse(LevelEditorController.LoadedCustomLevelID), LikeCallback);
		UnityEngine.Object.FindObjectOfType<Canvas>().GetComponent<GraphicRaycaster>().enabled = false;
	}

	private void LikeCallback(UnityWebRequest request)
	{
		UnityEngine.Object.FindObjectOfType<Canvas>().GetComponent<GraphicRaycaster>().enabled = true;
		if (request.error != null)
		{
			GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("APIErrorMessage"));
			return;
		}
		UnityEngine.Debug.Log(request.downloadHandler.text);
		if (request.responseCode == 200)
		{
			GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("SuccessfulVoteMsg"));
		}
		else
		{
			GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("AlreadyVotedMsg"));
		}
	}

	private void InitializeOfferButtons()
	{
		if (!rewadEligible && !tankobankEligible)
		{
			return;
		}
		RectTransform component;
		Image component2;
		if (rewadEligible)
		{
			rewardedAdButton.onClick.AddListener(delegate
			{
				RewardedAdButtonClick();
			});
			rewardedAdButton.gameObject.SetActive(value: true);
			component = rewardedAdButton.GetComponent<RectTransform>();
			component2 = rewardedAdButton.GetComponent<Image>();
			Text component3 = GameObject.Find("RewadEarningsText").GetComponent<Text>();
			component3.text = PlayRewardedAdSceneController.SetNextRewardValue(PlayRewardedAdSceneController.RewadResetLocation.LevelResults).ToString();
		}
		else
		{
			TankobankButton.onClick.AddListener(delegate
			{
				TankobankButtonClick();
			});
			TankobankButton.gameObject.SetActive(value: true);
			Image component4 = TankobankButton.GetComponent<Image>();
			component4.SetAlpha(0f);
			Image target = component4;
			Color color = component4.color;
			float r = color.r;
			Color color2 = component4.color;
			float g = color2.g;
			Color color3 = component4.color;
			target.DOColor(new Color(r, g, color3.b, 1f), 0.4f).SetDelay(1f);
			component = TankobankButton.GetComponent<RectTransform>();
			component2 = TankobankButton.GetComponent<Image>();
			float shiftAmount = -9f;
			ShiftRectTransformY(TankobankButton.GetComponent<RectTransform>(), shiftAmount);
			ShiftRectTransformY(continueButton.GetComponent<RectTransform>(), shiftAmount);
			if ((bool)restartButton)
			{
				ShiftRectTransformY(restartButton.GetComponent<RectTransform>(), shiftAmount);
			}
		}
		RectTransform component5 = continueButton.GetComponent<RectTransform>();
		RectTransform component6 = restartButton.GetComponent<RectTransform>();
		float num = 95f;
		RectTransform rectTransform = component5;
		float x = num;
		Vector2 anchoredPosition = component5.anchoredPosition;
		rectTransform.anchoredPosition = new Vector2(x, anchoredPosition.y);
		RectTransform rectTransform2 = component;
		Vector2 anchoredPosition2 = component5.anchoredPosition;
		rectTransform2.anchoredPosition = new Vector2(0f, anchoredPosition2.y + 7f);
		RectTransform rectTransform3 = component6;
		float x2 = 0f - num;
		Vector2 anchoredPosition3 = component6.anchoredPosition;
		rectTransform3.anchoredPosition = new Vector2(x2, anchoredPosition3.y);
		Transform transform = component.transform;
		Vector3 localScale = component.transform.localScale;
		float x3 = localScale.x * 1.2f;
		Vector3 localScale2 = component.transform.localScale;
		float y = localScale2.y * 1.2f;
		Vector3 localScale3 = component.transform.localScale;
		transform.localScale = new Vector3(x3, y, localScale3.z);
		component2.SetAlpha(0f);
		Image target2 = component2;
		Color color4 = component2.color;
		float r2 = color4.r;
		Color color5 = component2.color;
		float g2 = color5.g;
		Color color6 = component2.color;
		target2.DOColor(new Color(r2, g2, color6.b, 1f), 0.2f);
	}

	private void ProcessControlsFadeIn()
	{
		ResolutionImage.enabled = false;
		ProfitImage.enabled = false;
		continueButton.GetComponent<Image>().enabled = false;
		if ((bool)restartButton)
		{
			restartButton.GetComponent<Image>().enabled = false;
		}
		if (doCountProfit)
		{
			earningsText.enabled = false;
		}
		switch (GlobalCommons.Instance.gameplayMode)
		{
		case GlobalCommons.GameplayModes.CustomLevel:
		{
			TimerText.enabled = true;
			TimerText.SetAlpha(0f);
			Text timerText = TimerText;
			Color color = TimerText.color;
			float r = color.r;
			Color color2 = TimerText.color;
			float g = color2.g;
			Color color3 = TimerText.color;
			timerText.DOColor(new Color(r, g, color3.b, 1f), 0.5f).SetDelay(0.15f);
			break;
		}
		case GlobalCommons.GameplayModes.TutorialLevel:
		{
			RectTransform component = continueButton.GetComponent<RectTransform>();
			RectTransform rectTransform = component;
			Vector2 anchoredPosition = component.anchoredPosition;
			rectTransform.anchoredPosition = new Vector2(0f, anchoredPosition.y);
			break;
		}
		}
		if (doCountProfit)
		{
			ProfitImage.enabled = true;
			ProfitImage.SetAlpha(0f);
			Text profitImage = ProfitImage;
			Color color4 = ProfitImage.color;
			float r2 = color4.r;
			Color color5 = ProfitImage.color;
			float g2 = color5.g;
			Color color6 = ProfitImage.color;
			profitImage.DOColor(new Color(r2, g2, color6.b, 1f), 0.5f).SetDelay(0.33f);
			if (GlobalCommons.Instance.globalGameStats.DoubleCoinsPurchased)
			{
				ProfitImage.text += " x2";
			}
			earningsText.enabled = true;
		}
		float duration = 0.5f;
		ResolutionImage.enabled = true;
		CanvasGroup component2 = ResolutionImage.GetComponent<CanvasGroup>();
		component2.alpha = 0f;
		component2.DOFade(1f, duration);
		ResolutionImage.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
		ResolutionImage.transform.DOScale(1f, duration);
		if (targetProfitValue > 0)
		{
			profitAnimationStarted = true;
		}
		else
		{
			EnableMenuButtons();
		}
	}

	private bool AreOffersAvailable()
	{
		if ((GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.ArenaLevel || GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.SurvivalLevel || GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.RegularLevel) && GameplayCommons.Instance.levelStateController.currentGameStats.MoneyCollected > 100 && GlobalCommons.Instance.globalGameStats.LevelsCompleted >= 4 && !GameplayCommons.Instance.LastLevelCompleted)
		{
			return true;
		}
		return false;
	}

	private void InitializeOffer()
	{
		if (!AreOffersAvailable())
		{
			return;
		}
		if (false || (!GlobalCommons.Instance.globalGameStats.DoubleCoinsPurchased && UnityEngine.Random.value > 0.72f))
		{
			tankobankEligible = true;
		}
		if (rewadEligible && tankobankEligible)
		{
			if (UnityEngine.Random.value > 0.8f)
			{
				rewadEligible = false;
			}
			else
			{
				tankobankEligible = false;
			}
		}
	}

	public void ProcessDoubleCoinsPurchase()
	{
		tankobankEligible = false;
		TankobankButton.gameObject.SetActive(value: false);
		RectTransform component = continueButton.GetComponent<RectTransform>();
		RectTransform component2 = restartButton.GetComponent<RectTransform>();
		RectTransform rectTransform = component;
		float x = initialControlButtonsDistance;
		Vector2 anchoredPosition = component.anchoredPosition;
		rectTransform.anchoredPosition = new Vector2(x, anchoredPosition.y);
		RectTransform rectTransform2 = component2;
		float x2 = 0f - initialControlButtonsDistance;
		Vector2 anchoredPosition2 = component2.anchoredPosition;
		rectTransform2.anchoredPosition = new Vector2(x2, anchoredPosition2.y);
	}

	public void FadeIn()
	{
		if (controllerEnabled)
		{
			return;
		}
		if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.CustomLevel || GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.EditorLevel)
		{
			doCountProfit = false;
		}
		GlobalCommons.Instance.LastSelectedWeapon = GameplayCommons.Instance.weaponsController.SelectedWeapon;
		if (!GameplayCommons.Instance.playersTankController.PlayerDead)
		{
			SoundManager.instance.ToggleMusic(SoundManager.MusicType.JingleMusic, levelResolutionIsCompleted: true);
			if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.CustomLevel && GlobalCommons.Instance.globalGameStats.PlayerID != LevelEditorController.LoadedCustomLevelCreatorID)
			{
				//AsyncRestCaller.TrackLevelWin();
			}
		}
		if (GlobalCommons.Instance.gameplayMode != GlobalCommons.GameplayModes.TutorialLevel)
		{
			GlobalCommons.Instance.globalGameStats.AchievementsTracker.ProcessAfterLevelCheck();
		}
		LevelStateController.ProcessLevelEarnings();
		if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.TutorialLevel)
		{
			int num = PlayerBalance.WeaponsUpgradesCost[0][1] - GameplayCommons.Instance.levelStateController.currentGameStats.MoneyCollected;
			if (num > 0)
			{
				GameplayCommons.Instance.levelStateController.currentGameStats.PickupMoney(num, showUIText: false);
			}
		}
		if (GlobalCommons.Instance.globalGameStats.DoubleCoinsPurchased)
		{
			GameplayCommons.Instance.levelStateController.currentGameStats.PickupMoney(GameplayCommons.Instance.levelStateController.currentGameStats.MoneyCollected, showUIText: false);
		}
		bool flag = false;
		if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.RegularLevel && GlobalCommons.Instance.globalGameStats.LevelsCompleted == GlobalCommons.Instance.ActualSelectedLevel - 1)
		{
			flag = true;
		}
		if (!GameplayCommons.Instance.playersTankController.PlayerDead)
		{
			if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.RegularLevel && GlobalCommons.Instance.globalGameStats.LevelsCompleted == GlobalCommons.Instance.ActualSelectedLevel - 1)
			{
				GlobalCommons.Instance.globalGameStats.LevelsCompleted++;
				GameplayCommons.Instance.NewRegularLevelCompleted = true;
				while (GlobalCommons.Instance.globalGameStats.CompletedLevelsProgressIndicators.Count < GlobalCommons.Instance.ActualSelectedLevel + 1)
				{
					GlobalCommons.Instance.globalGameStats.CompletedLevelsProgressIndicators.Add(0);
				}
				GlobalCommons.Instance.globalGameStats.CompletedLevelsProgressIndicators[GlobalCommons.Instance.ActualSelectedLevel] = GlobalCommons.Instance.globalGameStats.ProgressIndicator;
				GlobalCommons.Instance.globalGameStats.timesLastLevelAttempted = 1;
				if (GlobalCommons.Instance.globalGameStats.LevelsCompleted == 10)
				{
					GlobalCommons.Instance.globalGameStats.ArenaUnlockedMessagePending = true;
				}
				if (GlobalCommons.Instance.globalGameStats.LevelsCompleted == 20)
				{
					GlobalCommons.Instance.globalGameStats.SurvivalUnlockedMessagePending = true;
				}
				if (GlobalCommons.Instance.globalGameStats.LevelsCompleted % 15 == 0)
				{
					LevelSelectionMenuController.lastSelectedScreen = -1;
				}
			}
			else if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.TutorialLevel && !GlobalCommons.Instance.globalGameStats.TutorialCompleted)
			{
				GlobalCommons.Instance.globalGameStats.TutorialCompleted = true;
			}
		}
		else
		{
			if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.RegularLevel && GlobalCommons.Instance.globalGameStats.LevelsCompleted == GlobalCommons.Instance.ActualSelectedLevel - 1)
			{
				GlobalCommons.Instance.globalGameStats.timesLastLevelAttempted++;
			}
		}
		InitializeOffer();
		InitFireworks();
		GlobalCommons.Instance.globalGameStats.IncreaseScore(Mathf.FloorToInt((float)GameplayCommons.Instance.levelStateController.currentGameStats.MoneyCollected * 0.66f));
		if ((GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.RegularLevel || GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.ArenaLevel || GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.TutorialLevel) && GlobalCommons.Instance.globalGameStats.MaxLevelIncome < GameplayCommons.Instance.levelStateController.currentGameStats.MoneyCollected)
		{
			GlobalCommons.Instance.globalGameStats.MaxLevelIncome = GameplayCommons.Instance.levelStateController.currentGameStats.MoneyCollected;
		}
		if (doCountProfit)
		{
			targetProfitValue = GameplayCommons.Instance.levelStateController.currentGameStats.MoneyCollected;
			profitStepPerSec = Mathf.FloorToInt((float)targetProfitValue / 1.5f);
			if (profitStepPerSec < 1)
			{
				profitStepPerSec = 1;
			}
			earningsText.text = "0";
		}
		else
		{
			targetProfitValue = 0;
			profitStepPerSec = 1;
			earningsText.text = "0";
			UnityEngine.Object.Destroy(ProfitBGImage.gameObject);
			UnityEngine.Object.Destroy(earningsText.gameObject);
		}
		controllerEnabled = true;
		RectTransform component = ResolutionImage.GetComponent<RectTransform>();
		RectTransform component2 = ProfitBGImage.GetComponent<RectTransform>();
		RectTransform component3 = earningsText.GetComponent<RectTransform>();
		RectTransform component4 = TimerText.GetComponent<RectTransform>();
		Text component5 = ResolutionImage.transform.Find("ResolutionText").GetComponent<Text>();
		if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.SurvivalLevel)
		{
			TimerText.enabled = false;
			ResolutionImage.sprite = gameOverResolutionImage;
			component5.text = LocalizationManager.Instance.GetLocalizedText("GameOverText");
			int stat = GameplayCommons.Instance.levelStateController.currentGameStats.GameStatistics.GetStat(GameStatistics.Stat.SecondsSpentPlaying);
			int num2 = Mathf.FloorToInt((float)stat / 60f);
			stat -= num2 * 60;
			string text = num2.ToString();
			if (text.Length == 1)
			{
				text = "0" + text;
			}
			string text2 = stat.ToString();
			if (text2.Length == 1)
			{
				text2 = "0" + text2;
			}
			ProfitImage.text = LocalizationManager.Instance.GetLocalizedText("SurvivalKillsTxt") + GameplayCommons.Instance.levelStateController.currentGameStats.GameStatistics.GetStat(GameStatistics.Stat.TanksDestroyed).ToString();
		}
		else
		{
			if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.CustomLevel)
			{
				TimerText.text = LocalizationManager.Instance.GetLocalizedText("CustomLevelPlayedId") + LevelEditorController.LoadedCustomLevelID;
				RectTransform rectTransform = TimerText.rectTransform;
				Vector2 anchoredPosition = TimerText.rectTransform.anchoredPosition;
				float x = anchoredPosition.x;
				Vector2 anchoredPosition2 = TimerText.rectTransform.anchoredPosition;
				rectTransform.anchoredPosition = new Vector2(x, anchoredPosition2.y - 75f);
			}
			else
			{
				TimerText.enabled = false;
			}
			if (GameplayCommons.Instance.playersTankController.PlayerDead)
			{
				ResolutionImage.sprite = levelFailedResolutionImage;
				component5.text = LocalizationManager.Instance.GetLocalizedText("LevelFailed");
			}
			else if (GameplayCommons.Instance.NewRegularLevelCompleted)
			{
				ResolutionImage.sprite = newLevelCompleteResolutionImage;
				component5.text = LocalizationManager.Instance.GetLocalizedText("NewLevelComplete");
			}
		}
		if (GameplayCommons.Instance.playersTankController.PlayerDead)
		{
			GlobalCommons.Instance.lastLevelResolution = GlobalCommons.LevelResolution.Failed;
		}
		else
		{
			GlobalCommons.Instance.lastLevelResolution = GlobalCommons.LevelResolution.Completed;
		}
		if (flag)
		{
			ProgressIndicatorHelper.ModifyProgressIndicator();
		}
		if (SocialWorker.Instance.SocialAuthenticated)
		{
			SocialWorker.Instance.ReportHiScore();
		}
		if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.SurvivalLevel && SocialWorker.Instance.SocialAuthenticated)
		{
			SocialWorker.Instance.ReportSurvivalEnemiesCrushed();
		}
		earningsTextPosition = earningsText.rectTransform.anchoredPosition;
		if (GameplayCommons.Instance.LastLevelCompleted)
		{
			UnityEngine.Object.Destroy(restartButton.gameObject);
		}
		PlayRewardedAdSceneController.SetNextRewardValue(PlayRewardedAdSceneController.RewadResetLocation.LevelResults).ToString();
	}

	private void InitFireworks()
	{
		fireworksTimestamps = new List<float>();
		if (!GameplayCommons.Instance.NewRegularLevelCompleted && 0 == 0 && ((GlobalCommons.Instance.gameplayMode != GlobalCommons.GameplayModes.CustomLevel && GlobalCommons.Instance.gameplayMode != GlobalCommons.GameplayModes.EditorLevel) || !(GameplayCommons.Instance.playersTankController.HPPercentage > 0f)))
		{
			return;
		}
		for (int i = 0; i < 6; i++)
		{
			float num = UnityEngine.Random.Range(0.1f, 0.4f);
			if (fireworksTimestamps.Count == 0)
			{
				fireworksTimestamps.Add(num);
			}
			else
			{
				fireworksTimestamps.Add(num + fireworksTimestamps[fireworksTimestamps.Count - 1]);
			}
		}
	}

	private void ShiftRectTransformY(RectTransform rt, float shiftAmount)
	{
		Vector2 anchoredPosition = rt.anchoredPosition;
		float x = anchoredPosition.x;
		Vector2 anchoredPosition2 = rt.anchoredPosition;
		rt.anchoredPosition = new Vector2(x, anchoredPosition2.y + shiftAmount);
	}

	private void WriteStatsFile()
	{
		string path = "Statistics.txt";
		if (!File.Exists(path))
		{
			using (StreamWriter streamWriter = File.CreateText(path))
			{
				streamWriter.WriteLine("Created stats file...");
			}
		}
		using (StreamWriter streamWriter2 = File.AppendText(path))
		{
			streamWriter2.WriteLine("--------------------------");
			streamWriter2.WriteLine("--------------------------");
			streamWriter2.WriteLine("NEW ENTRY");
			streamWriter2.WriteLine("Game mode: " + GlobalCommons.Instance.gameplayMode.ToString());
			streamWriter2.WriteLine("Level number: " + GlobalCommons.Instance.ActualSelectedLevel.ToString());
			streamWriter2.WriteLine("Level completed: " + GameplayCommons.Instance.levelStateController.LevelCompletionPending.ToString());
			streamWriter2.WriteLine("Story levels completed: " + GlobalCommons.Instance.globalGameStats.LevelsCompleted);
			streamWriter2.WriteLine("STATS:");
			streamWriter2.WriteLine("Collected coins: " + GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(GameStatistics.Stat.CoinsCollected));
			streamWriter2.WriteLine("Destroyed spawners: " + GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(GameStatistics.Stat.SpawnersDestroyed));
			streamWriter2.WriteLine("Destroyed tanks: " + GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(GameStatistics.Stat.TanksDestroyed));
			streamWriter2.WriteLine("Destroyed towers: " + GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(GameStatistics.Stat.TowersDestroyed));
			streamWriter2.WriteLine("Destroyed bricks: " + GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(GameStatistics.Stat.WallsDestroyed));
			streamWriter2.WriteLine("Exploded barrels: " + GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(GameStatistics.Stat.BarrelsExploded));
			streamWriter2.WriteLine("Destroyed crates: " + GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(GameStatistics.Stat.BonusCratesPopped));
			streamWriter2.WriteLine("overall levels completed: " + GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(GameStatistics.Stat.LevelsCompleted));
			streamWriter2.WriteLine("Enemy rockets dodged: " + GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(GameStatistics.Stat.EnemyRocketsDodged));
			streamWriter2.WriteLine("Best Combo: " + GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(GameStatistics.Stat.BiggestCombo));
			streamWriter2.WriteLine("Mines enemy exploded: " + GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(GameStatistics.Stat.TimesHitEnemyWithMine));
			streamWriter2.WriteLine("Times hit enemy with guided missile: " + GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(GameStatistics.Stat.TimesHitEnemyWithGuidedMissile));
			streamWriter2.WriteLine("Accuracy: " + (float)GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(GameStatistics.Stat.TimesHit) / (float)(GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(GameStatistics.Stat.TimesMissed) + GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(GameStatistics.Stat.TimesHit)));
			streamWriter2.WriteLine("TimePlayed: " + GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(GameStatistics.Stat.SecondsSpentPlaying));
			streamWriter2.WriteLine("HP percentage: " + GameplayCommons.Instance.playersTankController.HPPercentage);
			streamWriter2.WriteLine("Simultaneous railgun hits: " + GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(GameStatistics.Stat.RailgunSimultaneousHits));
			streamWriter2.WriteLine("Total money collected: " + GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(GameStatistics.Stat.TotalMoneyCollected));
			streamWriter2.WriteLine("Total money spent: " + GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(GameStatistics.Stat.TotalMoneySpent));
			streamWriter2.WriteLine("Weapons levels: " + string.Join(",", (from p in GlobalCommons.Instance.globalGameStats.WeaponsLevels
				select p.ToString()).ToArray()));
			streamWriter2.WriteLine("Tank speed: " + GlobalCommons.Instance.globalGameStats.TankSpeedLevel.ToString() + ", Tank Armor: " + GlobalCommons.Instance.globalGameStats.TankArmorLevel.ToString());
			streamWriter2.WriteLine("Current objective index: " + GlobalCommons.Instance.globalGameStats.LittleTargetsTracker.CurrentTargetIndex.ToString());
			streamWriter2.WriteLine("END STATS");
		}
		using (StreamReader streamReader = File.OpenText(path))
		{
			string empty = string.Empty;
			while ((empty = streamReader.ReadLine()) != null)
			{
				Console.WriteLine(empty);
			}
		}
	}
}
