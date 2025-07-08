using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MoveTutorialController : MonoBehaviour
{
	private enum States
	{
		initialWait,
		bgFadeIn,
		touchPointerFadeIn,
		moveTouchPointerToRandomPoint,
		moveTouchPointerToInitialPosition,
		None
	}

    public Image TutorialBG;

    public Image MoveTutorialTouch;

    public Image TutorialTouchCenter;

    public float stateSwitchTimeStamp = -1f;

    public float showBGTimeOffset = 1f;

    public float BGfadeInTime = 0.5f;

    public float targetBGalpha = 0.33f;

    public float showTouchTimeOffset = 1f;

    public float touchFadeInTime = 0.5f;

    public float moveToPositionTimeOffset = 0.66f;

    public float moveToPotisionTime = 0.33f;

    public Vector2 lastPlayerPosition;

    public float playerDistanceTravelled;

	public const float BORDER_FADE_TIME = 1f;

	public const float BORDER_MAX_ALPHA = 1f;

    public float alphaFadeSpeed = 2f;

    private States currentState;

    public Vector2 initialTouchPointerPosition;

    public Vector2? touchImageMoveCoords;

    public bool tutorialCompleted;

    public CanvasGroup canvasGroup;

    public Text MoveTutorialText;

    public Text MoveTutoriaDescriptionlText;

    public bool TutorialFullyInitialized => currentState == States.moveTouchPointerToInitialPosition || currentState == States.moveTouchPointerToRandomPoint || currentState == States.None;

	private void Start()
	{
		if (GlobalCommons.Instance.globalGameStats.TutorialCompleted || GlobalCommons.Instance.gameplayMode != GlobalCommons.GameplayModes.TutorialLevel)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}

		TutorialBG = GameObject.Find("TutorialBG").GetComponent<Image>();
		MoveTutorialText = GameObject.Find("MoveTutorialText").GetComponent<Text>();
		MoveTutoriaDescriptionlText = GameObject.Find("MoveTutoriaDescriptionlText").GetComponent<Text>();
		MoveTutorialText.text = LocalizationManager.Instance.GetLocalizedText((!GlobalCommons.Instance.globalGameStats.UseStaticControls) ? "MoveTutorialHeader" : "StaticMoveTutorialHeader");
		MoveTutoriaDescriptionlText.text = LocalizationManager.Instance.GetLocalizedText((!GlobalCommons.Instance.globalGameStats.UseStaticControls) ? "MoveTutorialDescr" : "StaticMoveTutorialDescr");
		MoveTutorialTouch = GameObject.Find("MoveTutorialTouch").GetComponent<Image>();
		initialTouchPointerPosition = MoveTutorialTouch.rectTransform.anchoredPosition;
		TutorialTouchCenter = GameObject.Find("TutorialTouchCenter").GetComponent<Image>();
		if (GlobalCommons.Instance.globalGameStats.UseStaticControls)
		{
			UnityEngine.Object.Destroy(MoveTutorialTouch.gameObject);
			UnityEngine.Object.Destroy(TutorialTouchCenter.gameObject);
		}
		if (MoveTutorialTouch != null)
		{
			MoveTutorialTouch.enabled = false;
			TutorialTouchCenter.enabled = false;
		}
		TutorialBG.enabled = false;
		MoveTutorialText.enabled = false;
		MoveTutoriaDescriptionlText.enabled = false;
		IEnumerator enumerator = TutorialBG.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				if (transform.name.Contains("CornerImage"))
				{
					Image component = transform.GetComponent<Image>();
					component.SetAlpha(0f);
				}
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
		canvasGroup = GetComponent<CanvasGroup>();
	}

	private void Update()
	{
		float num = Time.fixedTime - stateSwitchTimeStamp;
		switch (currentState)
		{
		case States.initialWait:
			if (GameplayCommons.Instance.playersTankController.PlayerActive)
			{
				lastPlayerPosition = GameplayCommons.Instance.playersTankController.TankBase.transform.position;
				stateSwitchTimeStamp = Time.fixedTime;
				TutorialBG.enabled = true;
				TutorialBG.SetAlpha(0f);
				currentState = States.bgFadeIn;
				Extensions.DelayedCallWithCoroutine(0.3f, ActivateAllBsis);
			}
			break;
		case States.bgFadeIn:
		{
			if (!(num >= showBGTimeOffset - BGfadeInTime))
			{
				break;
			}
			float num5 = (1f - (showBGTimeOffset - num) / BGfadeInTime) * targetBGalpha;
			if (num5 > targetBGalpha)
			{
				num5 = targetBGalpha;
			}
			TutorialBG.SetAlpha(num5);
			if (num5 == targetBGalpha)
			{
				currentState = ((!(MoveTutorialTouch != null)) ? States.None : States.touchPointerFadeIn);
				stateSwitchTimeStamp = Time.fixedTime;
				if (MoveTutorialTouch != null)
				{
					MoveTutorialTouch.enabled = true;
					MoveTutorialTouch.SetAlpha(0f);
					TutorialTouchCenter.enabled = true;
					TutorialTouchCenter.SetAlpha(0f);
					MoveTutorialTouch.DOFade(1f, 0.2f).SetDelay(0.3f);
					TutorialTouchCenter.DOFade(1f, 0.2f).SetDelay(0.3f);
				}
				MoveTutorialText.enabled = true;
				MoveTutorialText.SetAlpha(0f);
				MoveTutoriaDescriptionlText.enabled = true;
				MoveTutoriaDescriptionlText.SetAlpha(0f);
				MoveTutorialText.DOFade(1f, 0.2f);
				MoveTutoriaDescriptionlText.DOFade(1f, 0.2f).SetDelay(0.1f);
			}
			break;
		}
		case States.touchPointerFadeIn:
			if (num >= showTouchTimeOffset - touchFadeInTime)
			{
				float num4 = 1f - (showTouchTimeOffset - num) / touchFadeInTime;
				if (num4 > 1f)
				{
					num4 = 1f;
				}
				if (num4 == 1f)
				{
					currentState = States.moveTouchPointerToRandomPoint;
					stateSwitchTimeStamp = Time.fixedTime;
				}
			}
			break;
		case States.moveTouchPointerToRandomPoint:
			if (!touchImageMoveCoords.HasValue)
			{
				touchImageMoveCoords = initialTouchPointerPosition + UnityEngine.Random.insideUnitCircle.normalized * 30f;
			}
			if (num >= moveToPositionTimeOffset - moveToPotisionTime)
			{
				float num3 = 1f - (moveToPositionTimeOffset - num) / moveToPotisionTime;
				if (num3 > 1f)
				{
					num3 = 1f;
				}
				MoveTutorialTouch.rectTransform.anchoredPosition = Vector2.Lerp(initialTouchPointerPosition, touchImageMoveCoords.Value, num3);
				if (num3 == 1f)
				{
					stateSwitchTimeStamp = Time.fixedTime;
					currentState = States.moveTouchPointerToInitialPosition;
				}
			}
                break;
		case States.moveTouchPointerToInitialPosition:
			if (num >= moveToPositionTimeOffset - moveToPotisionTime)
			{
				float num2 = 1f - (moveToPositionTimeOffset - num) / moveToPotisionTime;
				if (num2 > 1f)
				{
					num2 = 1f;
				}
				MoveTutorialTouch.rectTransform.anchoredPosition = Vector2.Lerp(touchImageMoveCoords.Value, initialTouchPointerPosition, num2);
				if (num2 == 1f)
				{
					touchImageMoveCoords = null;
					stateSwitchTimeStamp = Time.fixedTime;
					currentState = States.moveTouchPointerToRandomPoint;
				}
			}
                break;
		}
		CountPlayerDistanceTravelled();
		SetAlphaAndCheckComplete();
	}

	private void ActivateAllBsis()
	{
		BorderSlideImage[] componentsInChildren = TutorialBG.transform.GetComponentsInChildren<BorderSlideImage>();
		foreach (BorderSlideImage borderSlideImage in componentsInChildren)
		{
			borderSlideImage.Activate();
		}
		IEnumerator enumerator = TutorialBG.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				if (transform.name.Contains("CornerImage"))
				{
					Image component = transform.GetComponent<Image>();
					component.DOFade(1f, 0.5f);
				}
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

	private void SetAlphaAndCheckComplete()
	{
		if (!TutorialFullyInitialized)
		{
			return;
		}
		float num = 1f;
		if (Input.touchSupported)
		{
			if (GameplayCommons.Instance.touchesController.MoveTouchController.TouchActive)
			{
				Image moveTutorialTouch = MoveTutorialTouch;
				Color color = MoveTutorialTouch.color;
				moveTutorialTouch.SetAlpha(Mathf.MoveTowards(color.a, 0f, Time.deltaTime * alphaFadeSpeed * 2f));
				Image tutorialTouchCenter = TutorialTouchCenter;
				Color color2 = TutorialTouchCenter.color;
				tutorialTouchCenter.SetAlpha(Mathf.MoveTowards(color2.a, 0f, Time.deltaTime * alphaFadeSpeed * 2f));
				num = 0.25f;
			}
			else
			{
				num = 1f;
				Image moveTutorialTouch2 = MoveTutorialTouch;
				Color color3 = MoveTutorialTouch.color;
				moveTutorialTouch2.SetAlpha(Mathf.MoveTowards(color3.a, 1f, Time.deltaTime * alphaFadeSpeed * 2f));
				Image tutorialTouchCenter2 = TutorialTouchCenter;
				Color color4 = TutorialTouchCenter.color;
				tutorialTouchCenter2.SetAlpha(Mathf.MoveTowards(color4.a, 1f, Time.deltaTime * alphaFadeSpeed * 2f));
			}
		}
		if (tutorialCompleted)
		{
			num = 0f;
		}
		if (canvasGroup.alpha < num)
		{
			canvasGroup.alpha += Time.deltaTime * alphaFadeSpeed;
			if (canvasGroup.alpha > num)
			{
				canvasGroup.alpha = num;
			}
		}
		else if (canvasGroup.alpha > num)
		{
			canvasGroup.alpha -= Time.deltaTime * alphaFadeSpeed;
			if (canvasGroup.alpha < num)
			{
				canvasGroup.alpha = num;
			}
		}
		if (canvasGroup.alpha == 0f)
		{
			ShootTutorialController component = GameObject.Find("ShootTutorial").GetComponent<ShootTutorialController>();
			component.Activated = true;
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void CountPlayerDistanceTravelled()
	{
		if (TutorialFullyInitialized && !tutorialCompleted)
		{
			Vector2 vector = (Vector2)GameplayCommons.Instance.playersTankController.TankBase.transform.position - lastPlayerPosition;
			lastPlayerPosition = GameplayCommons.Instance.playersTankController.TankBase.transform.position;
			playerDistanceTravelled += vector.magnitude;
			if (playerDistanceTravelled >= 5f)
			{
				tutorialCompleted = true;
			}
		}
	}

	private void RotateGameobjectToVector(Vector2 directionVector, GameObject obj)
	{
		float z = Mathf.Atan2(directionVector.y, directionVector.x) * 57.29578f - 90f;
		obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, Quaternion.Euler(0f, 0f, z), 15f * Time.deltaTime);
	}
}
