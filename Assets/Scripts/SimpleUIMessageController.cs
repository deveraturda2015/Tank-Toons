using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SimpleUIMessageController : MonoBehaviour
{
	private Button OKButton;

	private GameObject ControlsContainer;

	internal static bool Active;

	private CanvasGroup CanvasGroup;

	private void InitializeMessage(string text, float scale)
	{
		Active = true;
		ControlsContainer = base.transform.Find("ControlsContainer").gameObject;
		Text component = ControlsContainer.transform.Find("MessageText").GetComponent<Text>();
		component.text = text;
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
		CanvasGroup = GetComponent<CanvasGroup>();
		CanvasGroup.alpha = 0f;
		CanvasGroup.DOFade(1f, 0.33f).OnCompleteWithCoroutine(delegate
		{
			OKButton = ControlsContainer.transform.Find("OKButton").GetComponent<Button>();
			OKButton.onClick.AddListener(delegate
			{
				OKButtonClick();
			});
		});
	}

	private void OKButtonClick()
	{
		OKButton.onClick.RemoveAllListeners();
		SoundManager.instance.PlayButtonClickSound();
		CanvasGroup.DOFade(0f, 0.33f).OnCompleteWithCoroutine(delegate
		{
			Active = false;
			UnityEngine.Object.Destroy(base.gameObject);
		});
	}

	public static void ShowMessage(ShowSimpleMessageRequest ShowSimpleMessageRequest)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Prefabs.SimpleUIMessage, Vector3.zero, Quaternion.identity);
		gameObject.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
		gameObject.GetComponent<SimpleUIMessageController>().InitializeMessage(ShowSimpleMessageRequest.MessageText, ShowSimpleMessageRequest.Scale);
	}
}
