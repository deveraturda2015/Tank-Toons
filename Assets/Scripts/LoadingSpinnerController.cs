using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSpinnerController : MonoBehaviour
{
	private Image SpinnerImage;

	private Text LoadingText;

	private CanvasGroup CanvasGroup;

	internal static LoadingSpinnerController InstantiateNewSpinner(Transform parentTransform)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Prefabs.LoadingSpinnerPrefab);
		gameObject.transform.SetParent(parentTransform, worldPositionStays: false);
		LoadingSpinnerController component = gameObject.GetComponent<LoadingSpinnerController>();
		component.Initialize();
		return component;
	}

	private void Initialize()
	{
		SpinnerImage = base.transform.Find("SpinnerImage").GetComponent<Image>();
		LoadingText = base.transform.Find("LoadingText").GetComponent<Text>();
		RectTransform component = GetComponent<RectTransform>();
		Vector2 anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
		component.anchoredPosition = new Vector2(0f, anchoredPosition.y);
		CanvasGroup = GetComponent<CanvasGroup>();
		CanvasGroup.alpha = 0f;
		base.gameObject.SetActive(value: false);
	}

	internal void Show(string text = null)
	{
		LoadingText.text = ((!string.IsNullOrEmpty(text)) ? text : string.Empty);
		base.gameObject.SetActive(value: true);
		CanvasGroup.DOKill();
		CanvasGroup.DOFade(1f, 0.33f);
	}

	internal void Hide()
	{
		CanvasGroup.DOKill();
		CanvasGroup.DOFade(0f, 0.33f).OnCompleteWithCoroutine(FinalizeHiding);
	}

	private void FinalizeHiding()
	{
		base.gameObject.SetActive(value: false);
	}

	private void Update()
	{
		Transform transform = SpinnerImage.transform;
		Vector3 eulerAngles = SpinnerImage.transform.rotation.eulerAngles;
		transform.rotation = Quaternion.Euler(0f, 0f, eulerAngles.z - Time.deltaTime * 360f);
	}
}
