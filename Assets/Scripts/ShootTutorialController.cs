using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShootTutorialController : MonoBehaviour
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

	private Image TutorialBG;

	private Image ShootTutorialTouch;

	private Image TutorialTouchCenter;

	private float stateSwitchTimeStamp = -1f;

	private float showBGTimeOffset = 1f;

	private float BGfadeInTime = 0.5f;

	private float targetBGalpha = 0.33f;

	private float showTouchTimeOffset = 1f;

	private float touchFadeInTime = 0.5f;

	private float moveToPositionTimeOffset = 0.66f;

	private float moveToPotisionTime = 0.33f;

	private Vector2 lastPlayerPosition;

	private float playerDistanceTravelled;

	private float alphaFadeSpeed = 2f;

	private States currentState;

	private Vector2 initialTouchPointerPosition;

	private Vector2? touchImageMoveCoords;

	private bool tutorialCompleted;

	private CanvasGroup canvasGroup;

	public bool Activated;

	private float turretShiftFactor;

	private float lastTimeShot;

	private float shotInterval = 0.1f;

	private float totalShootingTime;

	private Text ShootTutorialText;

	private Text ShootTutoriaDescriptionlText;

	private bool TutorialFullyInitialized => currentState == States.moveTouchPointerToInitialPosition || currentState == States.moveTouchPointerToRandomPoint || currentState == States.None;

	private void Start()
	{
		if (GlobalCommons.Instance.globalGameStats.TutorialCompleted || GlobalCommons.Instance.gameplayMode != GlobalCommons.GameplayModes.TutorialLevel)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		TutorialBG = GameObject.Find("ShootTutorialBG").GetComponent<Image>();
		ShootTutorialText = GameObject.Find("ShootTutorialText").GetComponent<Text>();
		ShootTutoriaDescriptionlText = GameObject.Find("ShootTutoriaDescriptionlText").GetComponent<Text>();
		ShootTutorialText.text = LocalizationManager.Instance.GetLocalizedText((!GlobalCommons.Instance.globalGameStats.UseStaticControls) ? "ShootTutorialHeader" : "StaticShootTutorialHeader");
		ShootTutoriaDescriptionlText.text = LocalizationManager.Instance.GetLocalizedText((!GlobalCommons.Instance.globalGameStats.UseStaticControls) ? "ShootTutorialDescr" : "StaticShootTutorialDescr");
		ShootTutorialTouch = GameObject.Find("ShootTutorialTouch").GetComponent<Image>();
		initialTouchPointerPosition = ShootTutorialTouch.rectTransform.anchoredPosition;
		TutorialTouchCenter = GameObject.Find("ShootTutorialTouchCenter").GetComponent<Image>();
		if (GlobalCommons.Instance.globalGameStats.UseStaticControls)
		{
			UnityEngine.Object.Destroy(ShootTutorialTouch.gameObject);
			UnityEngine.Object.Destroy(TutorialTouchCenter.gameObject);
		}
		if (ShootTutorialTouch != null)
		{
			ShootTutorialTouch.enabled = false;
			TutorialTouchCenter.enabled = false;
		}
		TutorialBG.enabled = false;
		ShootTutorialText.enabled = false;
		ShootTutoriaDescriptionlText.enabled = false;
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

	private void Update()
	{
		if (!Activated)
		{
			return;
		}
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
				ActivateAllBsis();
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
				GameplayCommons.Instance.tutorialController.ShootingEnabled = true;
				currentState = ((!(ShootTutorialTouch != null)) ? States.None : States.touchPointerFadeIn);
				stateSwitchTimeStamp = Time.fixedTime;
				if (ShootTutorialTouch != null)
				{
					ShootTutorialTouch.enabled = true;
					ShootTutorialTouch.SetAlpha(0f);
					TutorialTouchCenter.enabled = true;
					TutorialTouchCenter.SetAlpha(0f);
					ShootTutorialTouch.DOFade(1f, 0.2f).SetDelay(0.3f);
					TutorialTouchCenter.DOFade(1f, 0.2f).SetDelay(0.3f);
				}
				ShootTutorialText.enabled = true;
				ShootTutorialText.SetAlpha(0f);
				ShootTutoriaDescriptionlText.enabled = true;
				ShootTutoriaDescriptionlText.SetAlpha(0f);
				ShootTutorialText.DOFade(1f, 0.2f);
				ShootTutoriaDescriptionlText.DOFade(1f, 0.2f).SetDelay(0.1f);
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
				ShootTutorialTouch.rectTransform.anchoredPosition = Vector2.Lerp(initialTouchPointerPosition, touchImageMoveCoords.Value, num3);
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
				ShootTutorialTouch.rectTransform.anchoredPosition = Vector2.Lerp(touchImageMoveCoords.Value, initialTouchPointerPosition, num2);
				if (num2 == 1f)
				{
					touchImageMoveCoords = null;
					stateSwitchTimeStamp = Time.fixedTime;
					currentState = States.moveTouchPointerToRandomPoint;
				}
			}
			break;
		}
		SetAlphaAndCheckComplete();
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
			if (GameplayCommons.Instance.touchesController.ShootTouchController.TouchActive)
			{
				num = 0.25f;
				totalShootingTime += Time.deltaTime;
				if (totalShootingTime > 1.33f)
				{
					tutorialCompleted = true;
				}
				Image shootTutorialTouch = ShootTutorialTouch;
				Color color = ShootTutorialTouch.color;
				shootTutorialTouch.SetAlpha(Mathf.MoveTowards(color.a, 0f, Time.deltaTime * alphaFadeSpeed * 2f));
				Image tutorialTouchCenter = TutorialTouchCenter;
				Color color2 = TutorialTouchCenter.color;
				tutorialTouchCenter.SetAlpha(Mathf.MoveTowards(color2.a, 0f, Time.deltaTime * alphaFadeSpeed * 2f));
			}
			else
			{
				num = 1f;
				Image shootTutorialTouch2 = ShootTutorialTouch;
				Color color3 = ShootTutorialTouch.color;
				shootTutorialTouch2.SetAlpha(Mathf.MoveTowards(color3.a, 1f, Time.deltaTime * alphaFadeSpeed * 2f));
				Image tutorialTouchCenter2 = TutorialTouchCenter;
				Color color4 = TutorialTouchCenter.color;
				tutorialTouchCenter2.SetAlpha(Mathf.MoveTowards(color4.a, 1f, Time.deltaTime * alphaFadeSpeed * 2f));
			}
		}
		else
		{
			tutorialCompleted = true;
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
			UnityEngine.Object.Destroy(base.gameObject);
			GameplayCommons.Instance.tutorialController.ShowDestroyBlocksTutorial();
			GameplayCommons.Instance.gameplayUIController.ShowBinocularsButton();
		}
	}

	private void RotateGameobjectToVector(Vector2 directionVector, GameObject obj)
	{
		float z = Mathf.Atan2(directionVector.y, directionVector.x) * 57.29578f - 90f;
		obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, Quaternion.Euler(0f, 0f, z), 15f * Time.deltaTime);
	}
}
