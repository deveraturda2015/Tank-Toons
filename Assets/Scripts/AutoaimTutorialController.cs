using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AutoaimTutorialController : MonoBehaviour
{
	private Image TutorialBG;

	private Text headerText;

	private Text descriptionText;

	private Image AutoaimTutorialTouchCenter;

	private Image AutoaimTutorialTouch;

	private Image AutoaimCrosshair;

	private Image AutoaimTutorialTankBase;

	private Image AutoaimTutorialTankTurret;

	private Image AutoaimTutorialEnemyTankBase;

	private Image AutoaimTutorialEnemyTankTurret;

	private Image ThumbImage;

	private Image EnemyExplosion;

	private Image AutoaimTutorialEnemyTank2Base;

	private Image AutoaimTutorialEnemyTank2Turret;

	private Image EnemyExplosion2;

	private bool tutorialCompleted;

	private bool TutorialFullyInitialized;

	private float totalShootingTime;

	private float alphaFadeSpeed = 2f;

	private CanvasGroup canvasGroup;

	private void Start()
	{
		if (GlobalCommons.Instance.globalGameStats.TutorialCompleted || GlobalCommons.Instance.gameplayMode != GlobalCommons.GameplayModes.TutorialLevel)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		TutorialBG = GameObject.Find("AutoaimTutorialBG").GetComponent<Image>();
		TutorialBG.SetAlpha(0f);
		headerText = GameObject.Find("AutoaimTutorialText").GetComponent<Text>();
		headerText.SetAlpha(0f);
		descriptionText = GameObject.Find("AutoaimTutoriaDescriptionlText").GetComponent<Text>();
		descriptionText.SetAlpha(0f);
		headerText.text = LocalizationManager.Instance.GetLocalizedText((!GlobalCommons.Instance.globalGameStats.UseStaticControls) ? "AutoaimTutorialHeader" : "StaticAutoaimTutorialHeader");
		descriptionText.text = LocalizationManager.Instance.GetLocalizedText((!GlobalCommons.Instance.globalGameStats.UseStaticControls) ? "AutoaimTutorialDescr" : "StaticAutoaimTutorialDescr");
		AutoaimTutorialTouchCenter = TutorialBG.transform.Find("AutoaimTutorialTouchCenter").GetComponent<Image>();
		AutoaimTutorialTouchCenter.SetAlpha(0f);
		AutoaimTutorialTouch = TutorialBG.transform.Find("AutoaimTutorialTouch").GetComponent<Image>();
		AutoaimTutorialTouch.SetAlpha(0f);
		AutoaimCrosshair = TutorialBG.transform.Find("AutoaimCrosshair").GetComponent<Image>();
		AutoaimCrosshair.SetAlpha(0f);
		AutoaimTutorialTankBase = TutorialBG.transform.Find("AutoaimTutorialTankBase").GetComponent<Image>();
		AutoaimTutorialTankBase.SetAlpha(0f);
		AutoaimTutorialTankTurret = TutorialBG.transform.Find("AutoaimTutorialTankTurret").GetComponent<Image>();
		AutoaimTutorialTankTurret.SetAlpha(0f);
		AutoaimTutorialEnemyTankBase = TutorialBG.transform.Find("AutoaimTutorialEnemyTankBase").GetComponent<Image>();
		AutoaimTutorialEnemyTankBase.SetAlpha(0f);
		AutoaimTutorialEnemyTankTurret = TutorialBG.transform.Find("AutoaimTutorialEnemyTankTurret").GetComponent<Image>();
		AutoaimTutorialEnemyTankTurret.SetAlpha(0f);
		EnemyExplosion = TutorialBG.transform.Find("EnemyExplosion").GetComponent<Image>();
		EnemyExplosion.SetAlpha(0f);
		AutoaimTutorialEnemyTank2Base = TutorialBG.transform.Find("AutoaimTutorialEnemyTank2Base").GetComponent<Image>();
		AutoaimTutorialEnemyTank2Base.SetAlpha(0f);
		AutoaimTutorialEnemyTank2Turret = TutorialBG.transform.Find("AutoaimTutorialEnemyTank2Turret").GetComponent<Image>();
		AutoaimTutorialEnemyTank2Turret.SetAlpha(0f);
		EnemyExplosion2 = TutorialBG.transform.Find("EnemyExplosion2").GetComponent<Image>();
		EnemyExplosion2.SetAlpha(0f);
		ThumbImage = TutorialBG.transform.Find("ThumbImage").GetComponent<Image>();
		ThumbImage.SetAlpha(0f);
		GameplayCommons.Instance.tutorialController.SetAutoaimControllerGO(base.gameObject);
		base.gameObject.SetActive(value: false);
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

	public void Initialize()
	{
		TutorialFullyInitialized = true;
		Sequence sequence = DOTween.Sequence();
		sequence.Append(TutorialBG.DOFade(0.33f, 0.5f));
		sequence.Append(headerText.DOFade(1f, 0.2f));
		sequence.Append(descriptionText.DOFade(1f, 0.2f));
		float atPosition = sequence.Duration();
		sequence.Insert(atPosition, AutoaimTutorialEnemyTankBase.DOFade(1f, 0.2f));
		sequence.Insert(atPosition, AutoaimTutorialEnemyTankTurret.DOFade(1f, 0.2f));
		float atPosition2 = sequence.Duration();
		sequence.Insert(atPosition2, AutoaimTutorialEnemyTank2Base.DOFade(1f, 0.2f));
		sequence.Insert(atPosition2, AutoaimTutorialEnemyTank2Turret.DOFade(1f, 0.2f));
		float atPosition3 = sequence.Duration();
		sequence.Insert(atPosition3, AutoaimTutorialTankBase.DOFade(1f, 0.2f));
		sequence.Insert(atPosition3, AutoaimTutorialTankTurret.DOFade(1f, 0.2f));
		sequence.Append(ThumbImage.DOFade(1f, 0.2f));
		ThumbImage.transform.rotation = Quaternion.Euler(0f, 0f, -15f);
		Extensions.DelayedCallWithCoroutine(0.33f, ActivateAllBsis);
		Extensions.DelayedCallWithCoroutine(sequence.Duration() + 0.5f, StartAnimation);
	}

	private void StartAnimation()
	{
		if (!(ThumbImage == null))
		{
			Sequence sequence = DOTween.Sequence();
			float duration = 0.5f;
			Vector3 eulerAngles = ThumbImage.transform.rotation.eulerAngles;
			float z = eulerAngles.z;
			sequence.Append(ThumbImage.transform.DORotate(Vector3.zero, duration));
			Vector3 localScale = AutoaimCrosshair.transform.localScale;
			float x = localScale.x;
			sequence.Append(AutoaimCrosshair.transform.DOScale(x * 1.2f, 0f));
			sequence.Append(AutoaimCrosshair.GetComponent<RectTransform>().DOAnchorPos(AutoaimTutorialTankBase.GetComponent<RectTransform>().anchoredPosition, 0f));
			float atPosition = sequence.Duration();
			sequence.Insert(atPosition, AutoaimTutorialTouchCenter.DOFade(1f, 0.2f));
			sequence.Insert(atPosition, AutoaimCrosshair.DOFade(1f, 0.2f));
			sequence.Insert(atPosition, AutoaimCrosshair.transform.DOScale(x, 0.2f));
			float atPosition2 = sequence.Duration();
			sequence.Insert(atPosition2, AutoaimCrosshair.GetComponent<RectTransform>().DOAnchorPos(AutoaimTutorialEnemyTankBase.GetComponent<RectTransform>().anchoredPosition, 1f).SetEase(Ease.InOutCubic));
			sequence.Insert(atPosition2, AutoaimTutorialTankTurret.transform.DORotate(new Vector3(0f, 0f, 48f), 1f));
			RectTransform component = AutoaimTutorialTankTurret.GetComponent<RectTransform>();
			Vector2 anchoredPosition = component.anchoredPosition;
			Vector2 endValue = anchoredPosition + new Vector2(7f, -7f);
			for (int i = 0; i < 7; i++)
			{
				sequence.Append(component.DOAnchorPos(endValue, 0f));
				float atPosition3 = sequence.Duration();
				sequence.Insert(atPosition3, component.DOAnchorPos(anchoredPosition, 0.1f));
				sequence.Insert(atPosition3, AutoaimTutorialEnemyTankBase.transform.DOShakePosition(0.1f, 10f));
				sequence.Insert(atPosition3, AutoaimTutorialEnemyTankTurret.transform.DOShakePosition(0.1f, 10f));
			}
			sequence.Append(AutoaimTutorialEnemyTankBase.DOFade(0f, 0f));
			sequence.Append(AutoaimTutorialEnemyTankTurret.DOFade(0f, 0f));
			sequence.Append(EnemyExplosion.DOFade(1f, 0f));
			float atPosition4 = sequence.Duration();
			sequence.Insert(atPosition4, EnemyExplosion.DOFade(0f, 0.25f));
			sequence.Insert(atPosition4, AutoaimCrosshair.GetComponent<RectTransform>().DOAnchorPos(AutoaimTutorialEnemyTank2Base.GetComponent<RectTransform>().anchoredPosition, 1f).SetEase(Ease.InOutCubic));
			sequence.Insert(atPosition4, AutoaimTutorialTankTurret.transform.DORotate(new Vector3(0f, 0f, -48f), 1f));
			endValue = anchoredPosition + new Vector2(-7f, -7f);
			for (int j = 0; j < 7; j++)
			{
				sequence.Append(component.DOAnchorPos(endValue, 0f));
				float atPosition5 = sequence.Duration();
				sequence.Insert(atPosition5, component.DOAnchorPos(anchoredPosition, 0.1f));
				sequence.Insert(atPosition5, AutoaimTutorialEnemyTank2Base.transform.DOShakePosition(0.1f, 10f));
				sequence.Insert(atPosition5, AutoaimTutorialEnemyTank2Turret.transform.DOShakePosition(0.1f, 10f));
			}
			sequence.Append(AutoaimTutorialEnemyTank2Base.DOFade(0f, 0f));
			sequence.Append(AutoaimTutorialEnemyTank2Turret.DOFade(0f, 0f));
			sequence.Append(EnemyExplosion2.DOFade(1f, 0f));
			atPosition4 = sequence.Duration();
			sequence.Insert(atPosition4, EnemyExplosion2.DOFade(0f, 1f));
			sequence.Insert(atPosition4, AutoaimCrosshair.DOFade(0f, 0.5f));
			sequence.Insert(atPosition4, AutoaimTutorialTankTurret.transform.DORotate(Vector3.zero, 1f));
			sequence.Insert(atPosition4, ThumbImage.transform.DORotate(new Vector3(0f, 0f, z), duration));
			sequence.Insert(atPosition4, AutoaimTutorialTouchCenter.DOFade(0f, 0.3f));
			sequence.AppendInterval(0.5f);
			float atPosition6 = sequence.Duration();
			sequence.Insert(atPosition6, AutoaimTutorialEnemyTankBase.DOFade(1f, 1f));
			sequence.Insert(atPosition6, AutoaimTutorialEnemyTankTurret.DOFade(1f, 1f));
			sequence.Insert(atPosition6, AutoaimTutorialEnemyTank2Base.DOFade(1f, 1f));
			sequence.Insert(atPosition6, AutoaimTutorialEnemyTank2Turret.DOFade(1f, 1f));
			Extensions.DelayedCallWithCoroutine(sequence.Duration() + 0.1f, StartAnimation);
		}
	}

	private void Update()
	{
		SetAlphaAndCheckComplete();
	}

	private void SetAlphaAndCheckComplete()
	{
		if (!TutorialFullyInitialized)
		{
			return;
		}
		float num = 1f;
		num = ((!GameplayCommons.Instance.touchesController.ShootTouchController.ShowAutoaimCursor()) ? 1f : 0.25f);
		if (GameplayCommons.Instance.levelStateController.GameplayStopped)
		{
			tutorialCompleted = true;
		}
		if (GameplayCommons.Instance.enemiesTracker.AllEnemies.Count == 0)
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
}
