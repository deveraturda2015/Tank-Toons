using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmationWindow : MonoBehaviour
{
	private Text HeaderText;

	private Button YesButton;

	private Button NoButton;

	private GameObject ControlsContainer;

	internal static bool Active;

	private CanvasGroup CanvasGroup;

	private void Initialize(string question, UnityAction yesEvent, UnityAction noEvent, float scale)
	{
		Active = true;
		ControlsContainer = base.transform.Find("ControlsContainer").gameObject;
		HeaderText = ControlsContainer.transform.Find("HeaderText").GetComponent<Text>();
		YesButton = ControlsContainer.transform.Find("YesButton").GetComponent<Button>();
		NoButton = ControlsContainer.transform.Find("NoButton").GetComponent<Button>();
		HeaderText.text = question;
		CanvasGroup = GetComponent<CanvasGroup>();
		CanvasGroup.alpha = 0f;
		CanvasGroup.DOFade(1f, 0.33f).OnCompleteWithCoroutine(delegate
		{
			YesButton.onClick.AddListener(yesEvent);
			YesButton.onClick.AddListener(ClosePanel);
			if (noEvent != null)
			{
				NoButton.onClick.AddListener(noEvent);
			}
			NoButton.onClick.AddListener(ClosePanel);
		});
		if (ControlsContainer != null && GlobalCommons.Instance.StateFaderController.GetCurrentCanvas() != null)
		{
			Vector2 referenceResolution = GlobalCommons.Instance.StateFaderController.GetCurrentCanvas().GetComponent<CanvasScaler>().referenceResolution;
			float y = referenceResolution.y;
			float num = y / 600f;
			Transform transform = ControlsContainer.transform;
			float x = num;
			float y2 = num;
			Vector3 localScale = ControlsContainer.transform.localScale;
			transform.localScale = new Vector3(x, y2, localScale.z);
		}
	}

	private void ClosePanel()
	{
		SoundManager.instance.PlayButtonClickSound();
		YesButton.onClick.RemoveAllListeners();
		NoButton.onClick.RemoveAllListeners();
		CanvasGroup.DOFade(0f, 0.33f).OnCompleteWithCoroutine(delegate
		{
			Active = false;
			UnityEngine.Object.Destroy(base.gameObject);
		});
	}

	internal static void ShowConfirmation(ShowConfirmationDialogRequest showConfirmationDialogRequest)
	{
		ConfirmationWindow component = UnityEngine.Object.Instantiate(Prefabs.ConfirmationWindow).GetComponent<ConfirmationWindow>();
		component.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
		component.Initialize(showConfirmationDialogRequest.Question, showConfirmationDialogRequest.YesEvent, showConfirmationDialogRequest.NoEvent, showConfirmationDialogRequest.Scale);
	}
}
