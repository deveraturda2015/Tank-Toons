using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EditorLevelCompletedMenu : MonoBehaviour
{
	private Button OKButton;

	private Image LevelCompleteImage;

	private Button ShareButton;

	private Button RestartButton;

	public Sprite LevelFailedImage;

	private bool levelCompleted = true;

	public Sprite InactiveShareSprite;

	public Sprite ActiveShareSprite;

	private Text resulutionText;

	private void Start()
	{
		if (!GameplayCommons.Instance.playersTankController.PlayerDead)
		{
			SoundManager.instance.ToggleMusic(SoundManager.MusicType.JingleMusic, levelResolutionIsCompleted: true);
		}
		GlobalCommons.Instance.LastSelectedWeapon = GameplayCommons.Instance.weaponsController.SelectedWeapon;
		OKButton = base.transform.Find("OKButton").GetComponent<Button>();
		OKButton.onClick.AddListener(delegate
		{
			OkButtonClick();
		});
		OKButton.image.SetAlpha(0f);
		ShareButton = base.transform.Find("ShareButton").GetComponent<Button>();
		ShareButton.onClick.AddListener(delegate
		{
			ShareButtonClick();
		});
		ShareButton.image.SetAlpha(0f);
		resulutionText = base.transform.Find("ResolutionText").GetComponent<Text>();
		resulutionText.SetAlpha(0f);
		RestartButton = base.transform.Find("RestartButton").GetComponent<Button>();
		RestartButton.onClick.AddListener(delegate
		{
			RestartButtonClick();
		});
		RestartButton.image.SetAlpha(0f);
		LevelCompleteImage = base.transform.Find("LevelCompleteImage").GetComponent<Image>();
		LevelCompleteImage.SetAlpha(0f);
		if (GameplayCommons.Instance.playersTankController.HPPercentage <= 0f)
		{
			LevelCompleteImage.sprite = LevelFailedImage;
			resulutionText.text = LocalizationManager.Instance.GetLocalizedText("LevelFailed");
			levelCompleted = false;
			ShareButton.image.sprite = InactiveShareSprite;
		}
		else
		{
			LevelEditorController.CanShareLevel = true;
		}
		Image component = GetComponent<Image>();
		component.SetAlpha(0f);
		component.DOFade(0.75f, 0.5f).OnCompleteWithCoroutine(CompleteBGFade);
	}

	private void RestartButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		if (AdsProcessor.GetInterstitialAdsAllowedOnLevelRestart())
		{
			GlobalCommons.Instance.SceneToTransferTo = "LoadingScene";
			GlobalCommons.Instance.StateFaderController.ChangeSceneTo("PlayAdScene");
		}
		else
		{
			GlobalCommons.Instance.StateFaderController.ChangeSceneTo("LoadingScene");
		}
	}

	private void ShareButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		if (levelCompleted)
		{
			//LevelEditorController.OpenUploadMenu = true;
			SoundManager.instance.PlayButtonClickSound();
			//if (AdsProcessor.GetInterstitialAdsAllowedAfterLevelFinish())
			//{
			//	GlobalCommons.Instance.SceneToTransferTo = "LevelEditor";
			//	GlobalCommons.Instance.StateFaderController.ChangeSceneTo("MainMenu");
			//}
			//else
			//{
			//	GlobalCommons.Instance.StateFaderController.ChangeSceneTo("LevelEditor");
			//}
			Share.instance.ShareLink();
		}
		else
		{
			GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("EditorCompleteToShare"));
		}
	}

	private void OkButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		if (AdsProcessor.GetInterstitialAdsAllowedAfterLevelFinish())
		{
			GlobalCommons.Instance.SceneToTransferTo = "LevelEditor";
			GlobalCommons.Instance.StateFaderController.ChangeSceneTo("MainMenu");
		}
		else
		{
			GlobalCommons.Instance.StateFaderController.ChangeSceneTo("LevelEditor");
		}
	}

	private void CompleteBGFade()
	{
		LevelCompleteImage.DOFade(1f, 0.2f);
		resulutionText.DOFade(1f, 0.2f).SetDelay(0.1f);
		BumpImageUpwards(RestartButton.image, 0.2f, 0.2f);
		BumpImageUpwards(ShareButton.image, 0.3f, 0.2f);
		BumpImageUpwards(OKButton.image, 0.4f, 0.2f);
	}

	private void BumpImageUpwards(Image img, float delay, float duration)
	{
		float num = 30f;
		img.DOFade(1f, duration).SetDelay(delay);
		RectTransform rectTransform = img.rectTransform;
		Vector2 anchoredPosition = img.rectTransform.anchoredPosition;
		float x = anchoredPosition.x;
		Vector2 anchoredPosition2 = img.rectTransform.anchoredPosition;
		rectTransform.anchoredPosition = new Vector2(x, anchoredPosition2.y - num);
		RectTransform rectTransform2 = img.rectTransform;
		Vector2 anchoredPosition3 = img.rectTransform.anchoredPosition;
		rectTransform2.DOAnchorPosY(anchoredPosition3.y + num, duration).SetEase(Ease.OutCubic).SetDelay(delay);
	}

	private void Update()
	{
	}
}
