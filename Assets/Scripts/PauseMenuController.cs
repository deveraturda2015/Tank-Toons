using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
	private Image pauseFadeImage;

	private Image pauseMenuBackground;

	private CanvasGroup pauseMenuCanvasGroup;

	private Button resumeButton;

	private Button restartButton;

	private Button exitToMenuButton;

	private Button MuteMusicButton;

	private Button MuteSoundButton;

	private Image MuteMusicButtonImage;

	private Image MuteSoundButtonImage;

	private float initialScale;

	private void Start()
	{
		pauseFadeImage = GameObject.Find("PauseFadeImage").GetComponent<Image>();
		resumeButton = GameObject.Find("ResumeButton").GetComponent<Button>();
		resumeButton.onClick.AddListener(delegate
		{
			ResumeButtonClick();
		});
		restartButton = GameObject.Find("PauseMenuBackground").transform.Find("RestartButton").GetComponent<Button>();
		restartButton.onClick.AddListener(delegate
		{
			RestartButtonClick();
		});
		exitToMenuButton = GameObject.Find("ExitToMenuButton").GetComponent<Button>();
		exitToMenuButton.onClick.AddListener(delegate
		{
			ExitToMenuButtonClick();
		});
		MuteMusicButton = GameObject.Find("MuteMusicButton").GetComponent<Button>();
		MuteSoundButton = GameObject.Find("MuteSoundButton").GetComponent<Button>();
		pauseMenuBackground = GameObject.Find("PauseMenuBackground").GetComponent<Image>();
		pauseMenuCanvasGroup = pauseMenuBackground.GetComponent<CanvasGroup>();
		Vector3 localScale = pauseMenuCanvasGroup.transform.localScale;
		initialScale = localScale.x;
		if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.TutorialLevel)
		{
			restartButton.gameObject.SetActive(value: false);
			exitToMenuButton.gameObject.SetActive(value: false);
		}
		MuteMusicButtonImage = MuteMusicButton.GetComponent<Image>();
		MuteSoundButtonImage = MuteSoundButton.GetComponent<Image>();
		pauseMenuBackground.gameObject.SetActive(value: false);
		MuteSoundButton.gameObject.SetActive(value: false);
		MuteMusicButton.gameObject.SetActive(value: false);
		pauseFadeImage.gameObject.SetActive(value: false);
	}

	private void Update()
	{
	}

	private void ResumeButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		GameplayCommons.Instance.TogglePause();
	}

	private void RestartButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		SetPauseMenuEnabled(val: false);
		GameplayCommons.Instance.SetDesiredTimeScale(1f);
		GlobalCommons.Instance.lastLevelResolution = GlobalCommons.LevelResolution.Failed;
		GlobalCommons.Instance.LastSelectedWeapon = GameplayCommons.Instance.weaponsController.SelectedWeapon;
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

	private void ExitToMenuButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		SetPauseMenuEnabled(val: false);
		GameplayCommons.Instance.SetDesiredTimeScale(1f);
		GlobalCommons.Instance.lastLevelResolution = GlobalCommons.LevelResolution.Failed;
		GlobalCommons.Instance.LastSelectedWeapon = GameplayCommons.Instance.weaponsController.SelectedWeapon;
		switch (GlobalCommons.Instance.gameplayMode)
		{
		case GlobalCommons.GameplayModes.ArenaLevel:
			GlobalCommons.Instance.StateFaderController.ChangeSceneTo("LevelSelection");
			break;
		case GlobalCommons.GameplayModes.EditorLevel:
			GlobalCommons.Instance.StateFaderController.ChangeSceneTo("LevelEditor");
			break;
		case GlobalCommons.GameplayModes.CustomLevel:
			GlobalCommons.Instance.StateFaderController.ChangeSceneTo("LevelSelection");
			break;
		case GlobalCommons.GameplayModes.RegularLevel:
			GlobalCommons.Instance.StateFaderController.ChangeSceneTo("Upgrades");
			break;
		case GlobalCommons.GameplayModes.SurvivalLevel:
			GlobalCommons.Instance.StateFaderController.ChangeSceneTo("LevelSelection");
			break;
		default:
			GlobalCommons.Instance.StateFaderController.ChangeSceneTo("Upgrades");
			break;
		}
	}

	public void SetState(bool pauseState)
	{
		SetPauseMenuEnabled(pauseState);
		
	}

	private void SetPauseMenuEnabled(bool val)
	{
		float duration = 0.17f;
		float num = 0.85f * initialScale;
		if (val)
		{
			pauseMenuBackground.gameObject.SetActive(val);
			MuteSoundButton.gameObject.SetActive(val);
			MuteMusicButton.gameObject.SetActive(val);
			pauseFadeImage.gameObject.SetActive(val);
			pauseMenuCanvasGroup.DOKill();
			pauseMenuCanvasGroup.alpha = 0f;
			pauseMenuCanvasGroup.DOFade(1f, duration).SetUpdate(isIndependentUpdate: true);
			MuteSoundButtonImage.DOKill();
			MuteMusicButtonImage.DOKill();
			pauseMenuBackground.DOKill();
			pauseFadeImage.DOKill();
			MuteSoundButtonImage.SetAlpha(0f);
			MuteMusicButtonImage.SetAlpha(0f);
			pauseMenuBackground.SetAlpha(0f);
			pauseFadeImage.SetAlpha(0f);
			MuteSoundButtonImage.DOFade(1f, duration).SetUpdate(isIndependentUpdate: true);
			MuteMusicButtonImage.DOFade(1f, duration).SetUpdate(isIndependentUpdate: true);
			pauseMenuBackground.DOFade(1f, duration).SetUpdate(isIndependentUpdate: true);
			pauseFadeImage.DOFade(0.5f, duration).SetUpdate(isIndependentUpdate: true);
			pauseMenuCanvasGroup.transform.DOKill();
			Transform transform = pauseMenuCanvasGroup.transform;
			float x = num;
			float y = num;
			Vector3 localScale = pauseMenuCanvasGroup.transform.localScale;
			transform.localScale = new Vector3(x, y, localScale.z);
			Transform transform2 = pauseMenuCanvasGroup.transform;
			float x2 = initialScale;
			float y2 = initialScale;
			Vector3 localScale2 = pauseMenuCanvasGroup.transform.localScale;
			transform2.DOScale(new Vector3(x2, y2, localScale2.z), duration).SetUpdate(isIndependentUpdate: true);
		}
		else
		{
			pauseMenuCanvasGroup.DOKill();
			pauseMenuCanvasGroup.DOFade(0f, duration).SetUpdate(isIndependentUpdate: true).OnCompleteWithCoroutine(FinishFadeout);
			MuteSoundButtonImage.DOKill();
			MuteMusicButtonImage.DOKill();
			pauseMenuBackground.DOKill();
			pauseFadeImage.DOKill();
			MuteSoundButtonImage.DOFade(0f, duration).SetUpdate(isIndependentUpdate: true);
			MuteMusicButtonImage.DOFade(0f, duration).SetUpdate(isIndependentUpdate: true);
			pauseMenuBackground.DOFade(0f, duration).SetUpdate(isIndependentUpdate: true);
			pauseFadeImage.DOFade(0f, duration).SetUpdate(isIndependentUpdate: true);
			pauseMenuCanvasGroup.transform.DOKill();
			Transform transform3 = pauseMenuCanvasGroup.transform;
			float x3 = num;
			float y3 = num;
			Vector3 localScale3 = pauseMenuCanvasGroup.transform.localScale;
			transform3.DOScale(new Vector3(x3, y3, localScale3.z), duration).SetUpdate(isIndependentUpdate: true);
		}
	}

	private void FinishFadeout()
	{
		pauseMenuBackground.gameObject.SetActive(value: false);
		MuteSoundButton.gameObject.SetActive(value: false);
		MuteMusicButton.gameObject.SetActive(value: false);
		pauseFadeImage.gameObject.SetActive(value: false);
	}
}
