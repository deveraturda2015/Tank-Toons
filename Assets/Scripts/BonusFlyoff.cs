using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BonusFlyoff : MonoBehaviour
{
	private Image BonusFlyoffImage;

	private const float IMAGESCALE = 1f;

	private const float TWEENTIME = 0.66f;

	private float rotationFactor = 20f;

	private void Update()
	{
		BonusFlyoffImage.rectTransform.Rotate(new Vector3(0f, 0f, 10f));
	}

	public void InitFlyoff(Vector3 bonusPosition, Vector2 targetPosition, Sprite bonusSprite)
	{
		if (UnityEngine.Random.value > 0.5f)
		{
			rotationFactor *= -1f;
		}
		BonusFlyoffImage = GetComponent<Image>();
		base.transform.SetParent(GameplayCommons.Instance.canvas.transform);
		RectTransform canvasRT = GameplayCommons.Instance.canvasRT;
		Vector2 vector = Camera.main.WorldToViewportPoint(bonusPosition);
		float x = vector.x;
		Vector2 sizeDelta = canvasRT.sizeDelta;
		float num = x * sizeDelta.x;
		Vector2 sizeDelta2 = canvasRT.sizeDelta;
		float x2 = num - sizeDelta2.x * 0.5f;
		float y = vector.y;
		Vector2 sizeDelta3 = canvasRT.sizeDelta;
		Vector2 anchoredPosition = new Vector2(x2, y * sizeDelta3.y);
		BonusFlyoffImage.rectTransform.anchoredPosition = anchoredPosition;
		BonusFlyoffImage.SetNativeSize();
		GameplayCommons.Instance.effectsSpawner.CreateHitEffect(base.transform.position);
		BonusFlyoffImage.sprite = bonusSprite;
		Transform transform = BonusFlyoffImage.transform;
		Vector3 localScale = BonusFlyoffImage.transform.localScale;
		transform.localScale = new Vector3(1f, 1f, localScale.z);
		BonusFlyoffImage.rectTransform.DOAnchorPos(targetPosition, 0.66f).SetEase(Ease.OutCubic);
		BonusFlyoffImage.rectTransform.DOScale(0f, 0.66f).SetEase(Ease.OutCubic).OnCompleteWithCoroutine(ProcessTweenComplete);
	}

	private void ProcessTweenComplete()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
