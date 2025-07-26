using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour
{
	private Button OKButton;

	public Sprite ItemCheckedSprite;

	public Sprite ItemUncheckedSprite;

	private Button ControlsButton;

	private Button PrivacyPolicyButton;

	private Button ScreenShakeButton;

	private Button ScreenFlashesButton;

	private Button EngineSoundButton;

	private Button FancyExplosionsButton;

	private void Start()
	{
		OKButton = base.transform.Find("OKButton").GetComponent<Button>();
		OKButton.onClick.AddListener(delegate
		{
			OKButtonClick();
		});
		ControlsButton = base.transform.Find("ControlsButton").GetComponent<Button>();
		ControlsButton.onClick.AddListener(ProcessControlsButtonClick);
		PrivacyPolicyButton = base.transform.Find("PrivacyPolicyButton").GetComponent<Button>();
		PrivacyPolicyButton.onClick.AddListener(ProcessPrivacyPolicyButtonClick);
		ScreenShakeButton = base.transform.Find("ScreenShakeButton").GetComponent<Button>();
		ScreenShakeButton.onClick.AddListener(delegate
		{
			ScreenShakeButtonClick();
		});
		ScreenFlashesButton = base.transform.Find("ScreenFlashesButton").GetComponent<Button>();
		ScreenFlashesButton.onClick.AddListener(delegate
		{
			ScreenFlashesButtonClick();
		});
		EngineSoundButton = base.transform.Find("EngineSoundButton").GetComponent<Button>();
		EngineSoundButton.onClick.AddListener(delegate
		{
			EngineSoundButtonClick();
		});
		FancyExplosionsButton = base.transform.Find("FancyExplosionsButton").GetComponent<Button>();
		FancyExplosionsButton.onClick.AddListener(delegate
		{
			FancyExplosionsButtonClick();
		});
		UpdateButtonsState();
	}

	private void ProcessPrivacyPolicyButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		Application.OpenURL("http://emittercritter.com/privacy/");
	}

	private void ProcessControlsButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		ControlsSettingsMenuController.Show();
	}

	private void UpdateButtonsState()
	{
		if (GlobalCommons.Instance.globalGameStats.ScreenShakeEnabled)
		{
			ScreenShakeButton.GetComponent<Image>().sprite = ItemCheckedSprite;
		}
		else
		{
			ScreenShakeButton.GetComponent<Image>().sprite = ItemUncheckedSprite;
		}
		if (GlobalCommons.Instance.globalGameStats.ScreenFlashesEnabled)
		{
			ScreenFlashesButton.GetComponent<Image>().sprite = ItemCheckedSprite;
		}
		else
		{
			ScreenFlashesButton.GetComponent<Image>().sprite = ItemUncheckedSprite;
		}
		if (GlobalCommons.Instance.globalGameStats.EngineSoundEnabled)
		{
			EngineSoundButton.GetComponent<Image>().sprite = ItemCheckedSprite;
		}
		else
		{
			EngineSoundButton.GetComponent<Image>().sprite = ItemUncheckedSprite;
		}
		if (GlobalCommons.Instance.globalGameStats.FancyExplosionsEffectEnabled)
		{
			FancyExplosionsButton.GetComponent<Image>().sprite = ItemCheckedSprite;
		}
		else
		{
			FancyExplosionsButton.GetComponent<Image>().sprite = ItemUncheckedSprite;
		}
	}

	private void FancyExplosionsButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.globalGameStats.FancyExplosionsEffectEnabled = !GlobalCommons.Instance.globalGameStats.FancyExplosionsEffectEnabled;
		UpdateButtonsState();
	}

	private void ScreenShakeButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.globalGameStats.ScreenShakeEnabled = !GlobalCommons.Instance.globalGameStats.ScreenShakeEnabled;
		UpdateButtonsState();
	}

	private void ScreenFlashesButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.globalGameStats.ScreenFlashesEnabled = !GlobalCommons.Instance.globalGameStats.ScreenFlashesEnabled;
		UpdateButtonsState();
	}

	private void EngineSoundButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.globalGameStats.EngineSoundEnabled = !GlobalCommons.Instance.globalGameStats.EngineSoundEnabled;
		UpdateButtonsState();
	}

	private void OKButtonClick()
	{
		AdmobManager.instance.ShowBanner();

		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.SaveGame();
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
