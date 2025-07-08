using UnityEngine;
using UnityEngine.UI;

public class FeedbackMenuController : MonoBehaviour
{
	private Button OKButton;

	private Button FeedbackButton;

	private InputField InputField;

	private void Start()
	{
		OKButton = base.transform.Find("OKButton").GetComponent<Button>();
		FeedbackButton = base.transform.Find("FeedbackButton").GetComponent<Button>();
		SetButtonListenersState(val: true);
		InputField = base.transform.Find("InputField").GetComponent<InputField>();
	}

	private void SetButtonListenersState(bool val)
	{
		if (val)
		{
			OKButton.onClick.AddListener(OkClick);
			FeedbackButton.onClick.AddListener(FeedbackButtonClick);
		}
		else
		{
			OKButton.onClick.RemoveAllListeners();
			FeedbackButton.onClick.RemoveAllListeners();
		}
	}

	private void OkClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void FeedbackButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		if (string.IsNullOrEmpty(InputField.text) || InputField.text == LocalizationManager.Instance.GetLocalizedText("FeedbackTextPlaceholder"))
		{
			GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("EmptyFeedback"));
		}
		else
		{
			SetButtonListenersState(val: false);
		}
	}

	private void ProcessFeedbackResult(WWW request)
	{
		if (request.error != null || request.text != "success")
		{
			GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("FeedbackError"));
			SetButtonListenersState(val: true);
		}
		else
		{
			GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("FeedbackThankYou"));
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	internal static void Show()
	{
		FeedbackMenuController component = UnityEngine.Object.Instantiate(Prefabs.FeedbackMenu).GetComponent<FeedbackMenuController>();
		component.transform.SetParent(UnityEngine.Object.FindObjectOfType<Canvas>().transform, worldPositionStays: false);
	}
}
