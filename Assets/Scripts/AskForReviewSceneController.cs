using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AskForReviewSceneController : MonoBehaviour
{
	private Button RateButton;

	private Button DoNotRateButton;

	private Button DoNotAskAgainCheckbox;

	private Text HeaderText;

	public Sprite CheckboxChecked;

	public Sprite CheckboxUnchecked;

	public Sprite PlayButtonSprite;

	private Image Tankman;

	public static bool ShownThisSession;

	private void Start()
	{
		ShownThisSession = true;
		Tankman = GlobalCommons.Instance.CanvasBoundsController.UIRectTransform.Find("Tankman").GetComponent<Image>();
		RateButton = GlobalCommons.Instance.CanvasBoundsController.UIRectTransform.Find("RateButton").gameObject.GetComponent<Button>();
		RateButton.onClick.AddListener(delegate
		{
			RateButtonClick();
		});
		DoNotRateButton = GlobalCommons.Instance.CanvasBoundsController.UIRectTransform.Find("DoNotRateButton").gameObject.GetComponent<Button>();
		DoNotRateButton.onClick.AddListener(delegate
		{
			DoNotRateButtonClick();
		});
		DoNotAskAgainCheckbox = GlobalCommons.Instance.CanvasBoundsController.UIRectTransform.Find("DoNotAskAgainCheckbox").gameObject.GetComponent<Button>();
		DoNotAskAgainCheckbox.onClick.AddListener(delegate
		{
			DoNotAskAgainCheckboxClick();
		});
		DoNotAskAgainCheckbox.GetComponent<Image>().sprite = CheckboxUnchecked;
		HeaderText = GlobalCommons.Instance.CanvasBoundsController.UIRectTransform.Find("HeaderText").gameObject.GetComponent<Text>();
		float num = 0.33f;
		for (int i = 1; i < 6; i++)
		{
			num += 0.1f;
			Image component = GlobalCommons.Instance.CanvasBoundsController.UIRectTransform.Find("RateStar" + i.ToString()).GetComponent<Image>();
			Vector3 localScale = component.transform.localScale;
			float x = localScale.x;
			Transform transform = component.transform;
			float x2 = x / 2f;
			float y = x / 2f;
			Vector3 localScale2 = component.transform.localScale;
			transform.localScale = new Vector3(x2, y, localScale2.z);
			component.SetAlpha(0f);
			component.transform.rotation = Quaternion.Euler(0f, 0f, 120f);
			float duration = 0.3f;
			component.transform.DOScale(x, duration).SetDelay(num);
			component.DOFade(1f, duration).SetDelay(num);
			component.transform.DORotate(Vector3.zero, duration).SetDelay(num);
		}
	}

	private void DoNotAskAgainCheckboxClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		if (DoNotAskAgainCheckbox.GetComponent<Image>().sprite == CheckboxUnchecked)
		{
			DoNotAskAgainCheckbox.GetComponent<Image>().sprite = CheckboxChecked;
		}
		else
		{
			DoNotAskAgainCheckbox.GetComponent<Image>().sprite = CheckboxUnchecked;
		}
	}

	private void RateButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		HeaderText.text = LocalizationManager.Instance.GetLocalizedText("ThankYouText") + "!";
		DoNotRateButton.gameObject.SetActive(value: false);
		GlobalCommons.Instance.globalGameStats.RatedGame = true;
		GlobalCommons.Instance.SaveGame();
		RateButton.GetComponent<Image>().sprite = PlayButtonSprite;
		RateButton.onClick.RemoveAllListeners();
		RateButton.onClick.AddListener(delegate
		{
			ConfirmRateButtonClick();
		});
		RectTransform component = RateButton.GetComponent<RectTransform>();
		RectTransform rectTransform = component;
		Vector2 anchoredPosition = component.anchoredPosition;
		rectTransform.anchoredPosition = new Vector2(0f, anchoredPosition.y);
		DoNotAskAgainCheckbox.gameObject.SetActive(value: false);
		GameObject.Find("DoNotAskAgainText").SetActive(value: false);
		Extensions.DelayedCallWithCoroutine(0.1f, TransferForRating);
	}

	private void TransferForRating()
	{
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.dtanks.toonsd");
	}

	private void ConfirmRateButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("Upgrades");
	}

	private void DoNotRateButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		if (DoNotAskAgainCheckbox.GetComponent<Image>().sprite == CheckboxChecked)
		{
			GlobalCommons.Instance.globalGameStats.RatedGame = true;
			GlobalCommons.Instance.SaveGame();
		}
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("Upgrades");
	}

	public static bool ReviewAvailable()
	{
		return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
	}
}
