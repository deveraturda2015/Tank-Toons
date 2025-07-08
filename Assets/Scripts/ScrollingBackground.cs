using DG.Tweening;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
	private RectTransform BottomBackgroundImageRT;

	public float MaxShiftX = 110f;

	public float MaxShiftY = 59f;

	private Vector2 rotationTransitionTimeBounds = new Vector2(3f, 4f);

	private Vector2 rotationBounds = new Vector2(-5f, 5f);

	private Ease transitionsEasing = Ease.InOutQuint;

	private float maxScale;

	private float minScale;

	private void Start()
	{
		BottomBackgroundImageRT = GameObject.Find("BottomBackgroundImage").GetComponent<RectTransform>();
		Vector3 localScale = BottomBackgroundImageRT.localScale;
		minScale = localScale.x;
		maxScale = minScale * 1.33f;
		ShiftBGX();
		ShiftBGY();
		ShiftRotation();
		ShiftScale();
	}

	private void ShiftScale()
	{
		float num = UnityEngine.Random.Range(minScale, maxScale);
		Vector3 localScale = BottomBackgroundImageRT.localScale;
		float duration = Mathf.Abs(localScale.x - num) * 40f;
		BottomBackgroundImageRT.DOScale(num, duration).SetEase(transitionsEasing).OnCompleteWithCoroutine(ShiftScale);
	}

	private void ShiftBGX()
	{
		float num;
		float num2;
		do
		{
			num = UnityEngine.Random.Range(0f - MaxShiftX, MaxShiftX);
			Vector2 anchoredPosition = BottomBackgroundImageRT.anchoredPosition;
			num2 = Mathf.Abs(anchoredPosition.x - num);
		}
		while (num2 < MaxShiftX / 2f);
		float duration = num2 / 10f;
		BottomBackgroundImageRT.DOAnchorPosX(num, duration).SetEase(transitionsEasing).OnCompleteWithCoroutine(ShiftBGX);
	}

	private void ShiftBGY()
	{
		float num;
		float num2;
		do
		{
			num = UnityEngine.Random.Range(0f - MaxShiftY, MaxShiftY);
			Vector2 anchoredPosition = BottomBackgroundImageRT.anchoredPosition;
			num2 = Mathf.Abs(anchoredPosition.y - num);
		}
		while (num2 < MaxShiftY / 2f);
		float duration = num2 / 10f;
		BottomBackgroundImageRT.DOAnchorPosY(num, duration).SetEase(transitionsEasing).OnCompleteWithCoroutine(ShiftBGY);
	}

	private void ShiftRotation()
	{
		BottomBackgroundImageRT.DORotate(new Vector3(0f, 0f, UnityEngine.Random.Range(rotationBounds.x, rotationBounds.y)), UnityEngine.Random.Range(rotationTransitionTimeBounds.x, rotationTransitionTimeBounds.y)).SetEase(transitionsEasing).OnCompleteWithCoroutine(ShiftRotation);
	}
}
