using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveCompletePanelController : MonoBehaviour
{
	private enum CurrentState
	{
		Suspended,
		Shown,
		Disappearing
	}

	private Image ObjectiveCompleteBackground;

	private Text ObjectiveCompleteRewardText;

	private CanvasGroup panelCanvasGourp;

	private float panelFadeSpeed = 0.05f;

	private CurrentState currentState;

	private float stateTimeStamp;

	private float initialShowDelayTimeout = 0.5f;

	private float showTimeout = 0.5f;

	public Sprite[] TargetSprites;

	private bool profitCountCompleted;

	private Image ObjectiveCompletedImage;

	private bool profitAnimationStarted;

	private int targetProfitValue;

	private int currentProfitValue;

	private int profitStepPerSec;

	private RectTransform panelRT;

	private void Start()
	{
		panelRT = GetComponent<RectTransform>();
		stateTimeStamp = Time.fixedTime;
		ObjectiveCompleteBackground = GameObject.Find("ObjectiveCompleteBackground").GetComponent<Image>();
		SetCurrentTargetDisplayEnabled(val: false);
		panelCanvasGourp = ObjectiveCompleteBackground.GetComponent<CanvasGroup>();
		panelCanvasGourp.alpha = 0f;
		ObjectiveCompletedImage = ObjectiveCompleteBackground.transform.Find("ObjectiveCompletedImage").GetComponent<Image>();
		ObjectiveCompleteRewardText = ObjectiveCompleteBackground.transform.Find("ObjectiveCompleteRewardText").GetComponent<Text>();
	}

	private void SetCurrentTargetDisplayEnabled(bool val)
	{
		ObjectiveCompleteBackground.gameObject.SetActive(val);
		IEnumerator enumerator = ObjectiveCompleteBackground.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				transform.gameObject.SetActive(val);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	private void Update()
	{
		float duration = 0.33f;
		float num = 40f;
		float num2 = 25f;
		switch (currentState)
		{
		case CurrentState.Shown:
			if (profitCountCompleted && Time.fixedTime - stateTimeStamp >= showTimeout)
			{
				currentState = CurrentState.Disappearing;
				stateTimeStamp = Time.fixedTime;
			}
			if (!ObjectiveCompleteBackground.gameObject.activeInHierarchy)
			{
				SetCurrentTargetDisplayEnabled(val: true);
				LittleTarget previousTarget = GlobalCommons.Instance.globalGameStats.LittleTargetsTracker.PreviousTarget;
				ObjectiveCompletedImage.sprite = TargetSprites[(int)previousTarget.TargetType];
				ObjectiveCompleteRewardText.text = "0";
				panelCanvasGourp.DOFade(1f, duration);
				RectTransform rectTransform = panelRT;
				Vector2 anchoredPosition2 = panelRT.anchoredPosition;
				float x = anchoredPosition2.x;
				Vector2 anchoredPosition3 = panelRT.anchoredPosition;
				rectTransform.anchoredPosition = new Vector2(x, anchoredPosition3.y + num);
				RectTransform rectTransform2 = panelRT;
				Vector3 localScale = panelRT.localScale;
				rectTransform2.localScale = new Vector3(0.75f, 0.75f, localScale.z);
				RectTransform target2 = panelRT;
				Vector2 anchoredPosition4 = panelRT.anchoredPosition;
				target2.DOAnchorPosY(anchoredPosition4.y - num, duration);
				panelRT.DOScale(1f, duration);
				panelRT.rotation = Quaternion.Euler(0f, 0f, num2);
				panelRT.DORotate(Vector3.zero, duration);
				targetProfitValue = GlobalCommons.Instance.globalGameStats.LittleTargetsTracker.PrevLittleTargetCoinsBonus;
				profitStepPerSec = Mathf.FloorToInt((float)targetProfitValue / 1.5f);
				if (profitStepPerSec < 1)
				{
					profitStepPerSec = 1;
				}
				profitAnimationStarted = true;
				SoundManager.instance.PlayRewadWinSound();
			}
			break;
		case CurrentState.Disappearing:
			if (panelCanvasGourp.alpha == 1f)
			{
				RectTransform target = panelRT;
				Vector2 anchoredPosition = panelRT.anchoredPosition;
				target.DOAnchorPosY(anchoredPosition.y + num, duration);
				panelRT.DOScale(0.75f, duration);
				panelRT.DORotate(new Vector3(0f, 0f, 0f - num2), duration);
			}
			if (panelCanvasGourp.alpha > 0f)
			{
				float num3 = panelCanvasGourp.alpha - panelFadeSpeed;
				if (num3 < 0f)
				{
					num3 = 0f;
				}
				panelCanvasGourp.alpha = num3;
				if (num3 == 0f)
				{
					DisableAndRemove();
				}
			}
			break;
		}
		if (profitAnimationStarted && currentProfitValue < targetProfitValue)
		{
			SoundManager.instance.PlayCoinCountSound();
			EffectsSpawner effectsSpawner = GameplayCommons.Instance.effectsSpawner;
			Vector3 position = ObjectiveCompleteRewardText.transform.position;
			float x2 = position.x - 0.7f;
			Vector3 position2 = ObjectiveCompleteRewardText.transform.position;
			float y = position2.y;
			Vector3 position3 = ObjectiveCompleteRewardText.transform.position;
			effectsSpawner.SpawnOverUICoinFlyoffEffect(new Vector3(x2, y, position3.z), 0.6f);
			int num4 = Mathf.FloorToInt((float)profitStepPerSec * Time.deltaTime);
			if (num4 < 1)
			{
				num4 = 1;
			}
			currentProfitValue += num4;
			if (currentProfitValue > targetProfitValue)
			{
				currentProfitValue = targetProfitValue;
			}
			ObjectiveCompleteRewardText.text = currentProfitValue.ToString();
			if (currentProfitValue == targetProfitValue)
			{
				stateTimeStamp = Time.fixedTime;
				profitCountCompleted = true;
			}
		}
	}

	public void Activate()
	{
		if (currentState == CurrentState.Suspended)
		{
			currentState = CurrentState.Shown;
			stateTimeStamp = Time.fixedTime;
		}
	}

	private void DisableAndRemove()
	{
		IEnumerator enumerator = ObjectiveCompleteBackground.gameObject.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				UnityEngine.Object.Destroy(transform.gameObject);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		UnityEngine.Object.Destroy(ObjectiveCompleteBackground.gameObject);
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
