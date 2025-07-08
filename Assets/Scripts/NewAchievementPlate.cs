using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NewAchievementPlate : MonoBehaviour
{
	public Sprite[] AchievementsSprites;

	private Image NewAchievementImage;

	private Vector2 initialPosition;

	private CanvasGroup cg;

	private RectTransform rt;

	private float fadeInTime = 0.33f;

	private float fadeOutTime = 0.33f;

	private float showDelay = 1f;

	private float horizontalShift = -50f;

	public void InitializePlate(AchievementsTracker.Achievement achievement)
	{
		NewAchievementImage = base.transform.Find("NewAchievementImage").GetComponent<Image>();
		NewAchievementImage.sprite = AchievementsSprites[(int)achievement];
		cg = GetComponent<CanvasGroup>();
		cg.alpha = 0f;
		rt = GetComponent<RectTransform>();
		initialPosition = rt.anchoredPosition;
		RectTransform rectTransform = rt;
		Vector2 anchoredPosition = rt.anchoredPosition;
		float x = anchoredPosition.x + horizontalShift;
		Vector2 anchoredPosition2 = rt.anchoredPosition;
		rectTransform.anchoredPosition = new Vector2(x, anchoredPosition2.y);
		cg.DOFade(1f, fadeInTime).SetUpdate(isIndependentUpdate: true);
		rt.DOAnchorPos(initialPosition, fadeInTime).OnCompleteWithCoroutine(ProcessInComplete).SetUpdate(isIndependentUpdate: true);
	}

	private void ProcessInComplete()
	{
		SoundManager.instance.PlayNewAchievementSound();
		cg.DOFade(0f, fadeOutTime).SetDelay(showDelay).SetUpdate(isIndependentUpdate: true);
		RectTransform target = rt;
		Vector2 anchoredPosition = rt.anchoredPosition;
		float x = anchoredPosition.x + horizontalShift;
		Vector2 anchoredPosition2 = rt.anchoredPosition;
		target.DOAnchorPos(new Vector2(x, anchoredPosition2.y), fadeOutTime).SetDelay(showDelay).OnCompleteWithCoroutine(ProcessOutComplete)
			.SetUpdate(isIndependentUpdate: true);
	}

	private void ProcessOutComplete()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
