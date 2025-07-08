using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ControlsSettingsMenuController : MonoBehaviour
{
	private Button OKButton;

	private Button AutoAimButton;

	private Button ShowGamepadsButton;

	private Text AutoAimDescription;

	public Sprite ItemCheckedSprite;

	public Sprite ItemUncheckedSprite;

	public Sprite LeftItemSelectedSprite;

	public Sprite RightItemSelectedSprite;

	private Button ControlsSwitchButton;

	private Image ControlsSwitchButtonImage;

	private Image SelectedItemIndicator;

	private Image DynamicPadCenter;

	private Image DynamicPadTop;

	private Image StaticPadCenter;

	private Image StaticPadTop;

	private Button SetupStaticControlsCentersButton;

	private Image ControlsSensitivityBar;

	private Image ControlsSensitivityIndicator;

	private Button ControlsSensitivityPlusButton;

	private Button ControlsSensitivityMinusButton;

	private Text DynamicControlsHeader;

	private Text StaticControlsHeader;

	private const float DYNAMICPADDEVIATION = 30f;

	private const float STICKMOVEMENTDEVIATION = 60f;

	private const float STICKMOVEMENTTWEENDURATION = 0.5f;

	private const float GAMEPADSFADEINOUTDURATION = 0.2f;

	private const float STICKMOVEDELAY = 0.3f;

	private Vector2 InitialDynamicGamepadCenter;

	private void Start()
	{
		DynamicPadCenter = base.transform.Find("DynamicPadCenter").GetComponent<Image>();
		DynamicPadTop = base.transform.Find("DynamicPadTop").GetComponent<Image>();
		StaticPadCenter = base.transform.Find("StaticPadCenter").GetComponent<Image>();
		StaticPadTop = base.transform.Find("StaticPadTop").GetComponent<Image>();
		DynamicControlsHeader = base.transform.Find("DynamicControlsHeader").GetComponent<Text>();
		StaticControlsHeader = base.transform.Find("StaticControlsHeader").GetComponent<Text>();
		ControlsSensitivityBar = base.transform.Find("ControlsSensitivityBar").GetComponent<Image>();
		ControlsSensitivityIndicator = ControlsSensitivityBar.transform.Find("ControlsSensitivityIndicator").GetComponent<Image>();
		ControlsSensitivityPlusButton = base.transform.Find("ControlsSensitivityPlusButton").GetComponent<Button>();
		ControlsSensitivityMinusButton = base.transform.Find("ControlsSensitivityMinusButton").GetComponent<Button>();
		ControlsSensitivityPlusButton.onClick.AddListener(ProcessSensitivityPlusClick);
		ControlsSensitivityMinusButton.onClick.AddListener(ProcessSensitivityMinusClick);
		DynamicPadCenter.SetAlpha(0f);
		DynamicPadTop.SetAlpha(0f);
		StaticPadCenter.SetAlpha(0f);
		StaticPadTop.SetAlpha(0f);
		ControlsSwitchButton = base.transform.Find("ControlsSwitchButton").GetComponent<Button>();
		ControlsSwitchButton.onClick.AddListener(ProcessControlsTypeSwitchClick);
		ControlsSwitchButtonImage = ControlsSwitchButton.GetComponent<Image>();
		SetupStaticControlsCentersButton = base.transform.Find("SetupStaticControlsCentersButton").GetComponent<Button>();
		SetupStaticControlsCentersButton.onClick.AddListener(ProcessSetupStaticControlsCentersButtonClick);
		OKButton = base.transform.Find("OKButton").GetComponent<Button>();
		OKButton.onClick.AddListener(delegate
		{
			OKButtonClick();
		});
		AutoAimButton = base.transform.Find("AutoAimButton").GetComponent<Button>();
		AutoAimButton.onClick.AddListener(delegate
		{
			AutoAimButtonClick();
		});
		ShowGamepadsButton = base.transform.Find("ShowGamepadsButton").GetComponent<Button>();
		ShowGamepadsButton.onClick.AddListener(delegate
		{
			ShowGamepadsButtonButtonClick();
		});
		SelectedItemIndicator = base.transform.Find("SelectedItemIndicator").GetComponent<Image>();
		AutoAimDescription = base.transform.Find("AutoAimDescription").GetComponent<Text>();
		AutoAimDescription.text = "+" + Mathf.CeilToInt(25f).ToString() + LocalizationManager.Instance.GetLocalizedText("SettingsAutoaimOffCoinsPercentage");
		UpdateButtonsState();
		UpdateSensitivityBarPosition(immediate: true);
		Extensions.DelayedCallWithCoroutine(0.5f, ShowGamepads);
	}

	private void ProcessSensitivityMinusClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.globalGameStats.ControlsSensitivityCoefficient -= 0.1f;
		GlobalCommons.Instance.globalGameStats.ControlsSensitivityCoefficient = Mathf.Clamp(GlobalCommons.Instance.globalGameStats.ControlsSensitivityCoefficient, 0.4f, 1.3f);
		UpdateSensitivityBarPosition();
	}

	private void ProcessSensitivityPlusClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.globalGameStats.ControlsSensitivityCoefficient += 0.1f;
		GlobalCommons.Instance.globalGameStats.ControlsSensitivityCoefficient = Mathf.Clamp(GlobalCommons.Instance.globalGameStats.ControlsSensitivityCoefficient, 0.4f, 1.3f);
		UpdateSensitivityBarPosition();
	}

	private void UpdateSensitivityBarPosition(bool immediate = false)
	{
		Vector2 sizeDelta = ControlsSensitivityBar.rectTransform.sizeDelta;
		float x = sizeDelta.x;
		ControlsSensitivityIndicator.rectTransform.DOKill();
		RectTransform rectTransform = ControlsSensitivityIndicator.rectTransform;
		float x2 = (0f - x) / 2f + (0.9f - (GlobalCommons.Instance.globalGameStats.ControlsSensitivityCoefficient - 0.4f)) / 0.9f * x;
		Vector2 anchoredPosition = ControlsSensitivityIndicator.rectTransform.anchoredPosition;
		rectTransform.DOAnchorPos(new Vector2(x2, anchoredPosition.y), (!immediate) ? 0.2f : 0f);
	}

	private void ProcessSetupStaticControlsCentersButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		StaticControlsCentersSettingsMenuController.Show();
	}

	private void ShowGamepads()
	{
		InitialDynamicGamepadCenter = DynamicPadCenter.rectTransform.anchoredPosition;
		StartDynamicGamepadsAnimation();
		StartStaticGamepadsAnimation();
	}

	private void StartStaticGamepadsAnimation()
	{
		StaticPadCenter.DOFade(1f, 0.2f);
		StaticPadTop.rectTransform.anchoredPosition = StaticPadCenter.rectTransform.anchoredPosition;
		StaticPadTop.DOFade(1f, 0.2f).OnCompleteWithCoroutine(ProcessStaticPadMoveOut);
	}

	private void ProcessStaticPadMoveOut()
	{
		float f = UnityEngine.Random.Range(0f, (float)Math.PI * 2f);
		StaticPadTop.rectTransform.DOAnchorPos(StaticPadTop.rectTransform.anchoredPosition + new Vector2(Mathf.Sin(f) * 60f, Mathf.Cos(f) * 60f), 0.5f).SetDelay(0.3f).OnCompleteWithCoroutine(ProcessStaticPadMoveBack);
	}

	private void ProcessStaticPadMoveBack()
	{
		StaticPadTop.rectTransform.DOAnchorPos(StaticPadCenter.rectTransform.anchoredPosition, 0.5f).SetDelay(0.3f).OnCompleteWithCoroutine(ProcessStaticPadMoveBackComplete);
	}

	private void ProcessStaticPadMoveBackComplete()
	{
		Extensions.DelayedCallWithCoroutine(0.3f, StartStaticGamepadsAnimation);
	}

	private void StartDynamicGamepadsAnimation()
	{
		DynamicPadCenter.rectTransform.anchoredPosition = InitialDynamicGamepadCenter + new Vector2(UnityEngine.Random.Range(-30f, 30f), UnityEngine.Random.Range(-30f, 30f));
		DynamicPadCenter.DOFade(1f, 0.2f);
		DynamicPadTop.rectTransform.anchoredPosition = DynamicPadCenter.rectTransform.anchoredPosition;
		DynamicPadTop.DOFade(1f, 0.2f).OnCompleteWithCoroutine(ProcessDynamicPadMoveOut);
	}

	private void ProcessDynamicPadMoveOut()
	{
		float f = UnityEngine.Random.Range(0f, (float)Math.PI * 2f);
		DynamicPadTop.rectTransform.DOAnchorPos(DynamicPadTop.rectTransform.anchoredPosition + new Vector2(Mathf.Sin(f) * 60f, Mathf.Cos(f) * 60f), 0.5f).SetDelay(0.3f).OnCompleteWithCoroutine(ProcessDynamicPadMoveBack);
	}

	private void ProcessDynamicPadMoveBack()
	{
		DynamicPadTop.rectTransform.DOAnchorPos(DynamicPadCenter.rectTransform.anchoredPosition, 0.5f).SetDelay(0.3f).OnCompleteWithCoroutine(ProcessDynamicPadMoveBackComplete);
	}

	private void ProcessDynamicPadMoveBackComplete()
	{
		DynamicPadCenter.DOFade(0f, 0.2f).SetDelay(0.3f);
		DynamicPadTop.DOFade(0f, 0.2f).SetDelay(0.3f).OnCompleteWithCoroutine(delegate
		{
			Extensions.DelayedCallWithCoroutine(0.3f, StartDynamicGamepadsAnimation);
		});
	}

	private void ProcessControlsTypeSwitchClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.globalGameStats.UseStaticControls = !GlobalCommons.Instance.globalGameStats.UseStaticControls;
		UpdateButtonsState();
	}

	private void ShowGamepadsButtonButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.globalGameStats.ShowGamePads = !GlobalCommons.Instance.globalGameStats.ShowGamePads;
		UpdateButtonsState();
	}

	private void OKButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.SaveGame();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void AutoAimButtonClick()
	{
		if (GlobalCommons.Instance.globalGameStats.TutorialCompleted)
		{
			SoundManager.instance.PlayButtonClickSound();
			GlobalCommons.Instance.globalGameStats.AutoAimEnabled = !GlobalCommons.Instance.globalGameStats.AutoAimEnabled;
			UpdateButtonsState();
		}
	}

	private void UpdateButtonsState()
	{
		if (GlobalCommons.Instance.globalGameStats.AutoAimEnabled)
		{
			AutoAimButton.GetComponent<Image>().sprite = ItemCheckedSprite;
		}
		else
		{
			AutoAimButton.GetComponent<Image>().sprite = ItemUncheckedSprite;
		}
		if (GlobalCommons.Instance.globalGameStats.ShowGamePads)
		{
			ShowGamepadsButton.GetComponent<Image>().sprite = ItemCheckedSprite;
		}
		else
		{
			ShowGamepadsButton.GetComponent<Image>().sprite = ItemUncheckedSprite;
		}
		SelectedItemIndicator.rectTransform.anchoredPosition = ((!GlobalCommons.Instance.globalGameStats.UseStaticControls) ? DynamicControlsHeader.rectTransform.anchoredPosition : StaticControlsHeader.rectTransform.anchoredPosition);
		ControlsSwitchButtonImage.sprite = ((!GlobalCommons.Instance.globalGameStats.UseStaticControls) ? LeftItemSelectedSprite : RightItemSelectedSprite);
	}

	internal static void Show()
	{
		ControlsSettingsMenuController component = UnityEngine.Object.Instantiate(Prefabs.ControlsSettingsMenu, Vector3.zero, Quaternion.identity).GetComponent<ControlsSettingsMenuController>();
		component.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
	}
}
