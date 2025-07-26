using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FightSign : MonoBehaviour
{
	private RectTransform rt;

	private float appearTimeStamp;

	private float appearDuration = 0.33f;

	private float disappearDuration = 0.33f;

	private float initialScale;

	private Image signImage;

	private Sequence mySequence;

	private float hideTS;

	private void Start()
	{
		signImage = GetComponent<Image>();
		appearTimeStamp = Time.fixedTime;
		rt = GetComponent<RectTransform>();
		rt.anchoredPosition = new Vector2(0f, 5f);
		Vector3 localScale = rt.localScale;
		initialScale = localScale.x * 0.75f;
		float num = 1.25f;
		RectTransform rectTransform = rt;
		Vector3 localScale2 = rt.localScale;
		float x = localScale2.x * num;
		Vector3 localScale3 = rt.localScale;
		float y = localScale3.y * num;
		Vector3 localScale4 = rt.localScale;
		rectTransform.localScale = new Vector3(x, y, localScale4.z);
		mySequence = DOTween.Sequence();
		mySequence.SetUpdate(isIndependentUpdate: true);
		mySequence.Append(rt.DOScale(initialScale, appearDuration));
		mySequence.AppendInterval(1.5f);
		hideTS = mySequence.Duration();
		mySequence.Insert(hideTS, rt.DORotate(new Vector3(0f, 0f, -30f), disappearDuration));
		mySequence.Insert(hideTS, rt.DOScale(initialScale / 2f, disappearDuration));
		Sequence s = mySequence;
		float atPosition = hideTS;
		Image target = signImage;
		Color color = signImage.color;
		float r = color.r;
		Color color2 = signImage.color;
		float g = color2.g;
		Color color3 = signImage.color;
		s.Insert(atPosition, target.DOColor(new Color(r, g, color3.b, 0f), disappearDuration));
		mySequence.OnCompleteWithCoroutine(CleanupAndRemove);
		SoundManager.instance.StopMusic();
		SoundManager.instance.ToggleMusic(SoundManager.MusicType.GameplayMusic, levelResolutionIsCompleted: false, doFadeIn: false);
	}

	private void Update()
	{
		if (GameplayCommons.Instance.GamePaused && mySequence.fullPosition < hideTS)
		{
			mySequence.fullPosition = hideTS;
		}
	}

	private void CleanupAndRemove()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
