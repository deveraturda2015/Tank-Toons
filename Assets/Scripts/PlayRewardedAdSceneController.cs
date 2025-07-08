using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayRewardedAdSceneController : MonoBehaviour
{
	internal enum RewadResetLocation
	{
		Upgrades,
		LevelResults
	}

	public EffectsSpawner effectsSpawner;

	private Text HeaderText;

	private Text ProfitText;

	private Image ProfintUnderlyingImage;

	private Button BackButton;

	private bool profitAnimationStarted;

	private int targetProfitValue;

	private int currentProfitValue;

	private int profitStepPerSec;

	private List<Button> PrizeButtons = new List<Button>();

	private int x2PrizeIndex;

	private int moneyToAdd;

	private bool selectedX2Prize;

	private float initialCoverImageAlpha;

	private Image CoverImage;

	private Canvas canvas;

	private float initialFixedDeltaTime;

	private static RewadResetLocation lastRewadResetLocation = RewadResetLocation.LevelResults;

	private static int NextRewardValue;

	private void Start()
	{
		initialFixedDeltaTime = Time.fixedDeltaTime;
		Time.fixedDeltaTime *= 0.2f;
		SoundManager.instance.StopMusic();
		canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
		effectsSpawner = new EffectsSpawner(EffectsSpawner.EffectsSpawnerPreset.RewardedVideoAndShopMenuAndPrizeScene);
		CoverImage = GlobalCommons.Instance.CanvasBoundsController.UIRectTransform.Find("CoverImage").GetComponent<Image>();
		Color color = CoverImage.color;
		initialCoverImageAlpha = color.a;
		Image coverImage = CoverImage;
		Color color2 = CoverImage.color;
		float r = color2.r;
		Color color3 = CoverImage.color;
		float g = color3.g;
		Color color4 = CoverImage.color;
		coverImage.color = new Color(r, g, color4.b, 1f);
		HeaderText = GlobalCommons.Instance.CanvasBoundsController.UIRectTransform.Find("AdResultDebugText").GetComponent<Text>();
		HeaderText.enabled = false;
		BackButton = GlobalCommons.Instance.CanvasBoundsController.UIRectTransform.Find("BackButton").GetComponent<Button>();
		BackButton.onClick.AddListener(delegate
		{
			BackButtonClick();
		});
		BackButton.GetComponent<Image>().enabled = false;
		ProfitText = GlobalCommons.Instance.CanvasBoundsController.UIRectTransform.Find("ProfitText").GetComponent<Text>();
		ProfitText.enabled = false;
		ProfintUnderlyingImage = GlobalCommons.Instance.CanvasBoundsController.UIRectTransform.Find("ProfintUnderlyingImage").GetComponent<Image>();
		ProfintUnderlyingImage.enabled = false;
		for (int i = 1; i < 4; i++)
		{
			InitializePrizeButton(i);
		}
		if (Application.isEditor)
		{
			moneyToAdd = 100500;
			HandleResult(AdsProcessor.AdDisplayResult.Finished);
		}
		else if (!GlobalCommons.Instance.AdsProcessor.ShowRewardedVideo(ProcessRewardedResult))
		{
			GlobalCommons.Instance.StateFaderController.ChangeSceneTo(GlobalCommons.Instance.SceneToTransferTo, immediate: true);
		}
	}

	private void ProcessRewardedResult(AdsProcessor.AdDisplayResult result)
	{
		switch (result)
		{
		case AdsProcessor.AdDisplayResult.Failed:
			HandleResult(AdsProcessor.AdDisplayResult.Failed);
			break;
		case AdsProcessor.AdDisplayResult.Finished:
			HandleResult(AdsProcessor.AdDisplayResult.Finished);
			break;
		case AdsProcessor.AdDisplayResult.Skipped:
			HandleResult(AdsProcessor.AdDisplayResult.Skipped);
			break;
		}
	}

	private void InitializePrizeButton(int buttonIndex)
	{
		Button component = GameObject.Find("Prize" + buttonIndex.ToString() + "Button").GetComponent<Button>();
		component.GetComponent<Image>().enabled = false;
		PrizeButtons.Add(component);
	}

	private void BackButtonClick()
	{
		Time.fixedDeltaTime = initialFixedDeltaTime;
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo(GlobalCommons.Instance.SceneToTransferTo, immediate: true);
	}

	private void Update()
	{
		effectsSpawner.Update();
		if (profitAnimationStarted && currentProfitValue < targetProfitValue)
		{
			SoundManager.instance.PlayCoinCountSound();
			effectsSpawner.SpawnOverUICoinFlyoffEffect(ProfitText.transform.position);
			int num = Mathf.FloorToInt((float)profitStepPerSec * Time.deltaTime);
			if (num < 1)
			{
				num = 1;
			}
			currentProfitValue += num;
			if (currentProfitValue > targetProfitValue)
			{
				currentProfitValue = targetProfitValue;
			}
			ProfitText.text = currentProfitValue.ToString();
			if (currentProfitValue == targetProfitValue)
			{
				Image component = BackButton.GetComponent<Image>();
				component.enabled = true;
				component.SetAlpha(0f);
				Image target = component;
				Color color = component.color;
				float r = color.r;
				Color color2 = component.color;
				float g = color2.g;
				Color color3 = component.color;
				target.DOColor(new Color(r, g, color3.b, 1f), 0.33f);
			}
		}
	}

	public void HandleResult(AdsProcessor.AdDisplayResult result)
	{
		Image coverImage = CoverImage;
		Color color = CoverImage.color;
		float r = color.r;
		Color color2 = CoverImage.color;
		float g = color2.g;
		Color color3 = CoverImage.color;
		coverImage.color = new Color(r, g, color3.b, initialCoverImageAlpha);
		HeaderText.enabled = true;
		switch (result)
		{
		case AdsProcessor.AdDisplayResult.Failed:
			HeaderText.text = LocalizationManager.Instance.GetLocalizedText("RewadSomethingWrong");
			BackButton.GetComponent<Image>().enabled = true;
			return;
		case AdsProcessor.AdDisplayResult.Finished:
			HeaderText.text = LocalizationManager.Instance.GetLocalizedText("ThankYouText") + "!" + Environment.NewLine + LocalizationManager.Instance.GetLocalizedText("RewadSelectPrizeBox");
			break;
		case AdsProcessor.AdDisplayResult.Skipped:
			if (GlobalCommons.Instance.globalGameStats.RewardedAdSkipWarningShown)
			{
				HeaderText.text = LocalizationManager.Instance.GetLocalizedText("RewadDoNotSkip");
				BackButton.GetComponent<Image>().enabled = true;
				return;
			}
			GlobalCommons.Instance.globalGameStats.RewardedAdSkipWarningShown = true;
			HeaderText.text = LocalizationManager.Instance.GetLocalizedText("RewadDoNotSkip") + Environment.NewLine + LocalizationManager.Instance.GetLocalizedText("RewadPretendNotSkip");
			break;
		}
		ShowPrizeBoxes();
	}

	private Color GetBoxColor()
	{
		return new Color(UnityEngine.Random.Range(0.85f, 1f), UnityEngine.Random.Range(0.85f, 1f), UnityEngine.Random.Range(0.85f, 1f));
	}

	private void ShowPrizeBoxes()
	{
		for (int i = 0; i < 3; i++)
		{
			Button button = PrizeButtons[i];
			Image buttonImage = button.GetComponent<Image>();
			buttonImage.enabled = true;
			buttonImage.color = GetBoxColor();
			buttonImage.SetAlpha(0f);
			float num = 70f;
			RectTransform rectTransform = buttonImage.rectTransform;
			Vector2 anchoredPosition = buttonImage.rectTransform.anchoredPosition;
			float x = anchoredPosition.x;
			Vector2 anchoredPosition2 = buttonImage.rectTransform.anchoredPosition;
			rectTransform.anchoredPosition = new Vector2(x, anchoredPosition2.y + num);
			float duration = 0.33f;
			float delay = 0.2f + (float)i * 0.2f;
			Tweener tweener = buttonImage.DOFade(1f, duration).SetDelay(delay);
			RectTransform rectTransform2 = buttonImage.rectTransform;
			Vector2 anchoredPosition3 = buttonImage.rectTransform.anchoredPosition;
			float x2 = anchoredPosition3.x;
			Vector2 anchoredPosition4 = buttonImage.rectTransform.anchoredPosition;
			Tweener tweener2 = rectTransform2.DOAnchorPos(new Vector2(x2, anchoredPosition4.y - num), duration).SetDelay(delay).SetEase(Ease.InQuart)
				.OnCompleteWithCoroutine(delegate
				{
					ProcessBoxFall(buttonImage);
				});
			if (i == 2)
			{
				tweener.OnCompleteWithCoroutine(FinalizePrizeBoxesInit);
			}
		}
	}

	private void ProcessBoxFall(Image boxImage)
	{
		float num = boxImage.rectTransform.rect.height * canvas.scaleFactor / (float)Screen.height * Camera.main.orthographicSize * 2f / 2f;
		float num2 = num * 2f;
		int num3 = 18;
		for (int i = 0; i <= num3; i++)
		{
			EffectsSpawner obj = effectsSpawner;
			Vector3 position = boxImage.transform.position;
			float x = position.x - num + (float)i * num2 / (float)num3;
			Vector3 position2 = boxImage.transform.position;
			float y = position2.y - num;
			Vector3 position3 = boxImage.transform.position;
			obj.SpawnSmoke(new Vector3(x, y, position3.z), 1, 0f);
		}
		SoundManager.instance.PlayPrizeDropSound();
	}

	private void FinalizePrizeBoxesInit()
	{
		for (int i = 0; i < 3; i++)
		{
			Button prizeButton = PrizeButtons[i];
			prizeButton.onClick.AddListener(delegate
			{
				PrizeButtonClick(prizeButton);
			});
		}
	}

	internal static int SetNextRewardValue(RewadResetLocation rewadResetLocation)
	{
		if (lastRewadResetLocation == RewadResetLocation.Upgrades && rewadResetLocation == RewadResetLocation.Upgrades)
		{
			return NextRewardValue;
		}
		lastRewadResetLocation = rewadResetLocation;
		int num = (GlobalCommons.Instance.globalGameStats.MaxLevelIncome <= 100) ? 100 : GlobalCommons.Instance.globalGameStats.MaxLevelIncome;
		NextRewardValue = Mathf.FloorToInt(Mathf.Ceil((float)num * UnityEngine.Random.Range(1.3f, 1.5f)) / 50f) * 50;
		if (GlobalCommons.Instance.globalGameStats.DoubleCoinsPurchased)
		{
			NextRewardValue *= 2;
		}
		return NextRewardValue;
	}

	private void PrizeButtonClick(Button btn)
	{
		HeaderText.text = string.Empty;
		int selectedIndex = int.Parse(btn.name.Substring(5, 1)) - 1;
		moneyToAdd = NextRewardValue;
		SoundManager.instance.PlayButtonClickSound();
		if (UnityEngine.Random.value > 0.5f || !GlobalCommons.Instance.globalGameStats.FirstRewadPrizePicked)
		{
			GlobalCommons.Instance.globalGameStats.FirstRewadPrizePicked = true;
			x2PrizeIndex = selectedIndex;
			moneyToAdd = Mathf.CeilToInt((float)moneyToAdd * 2f);
			selectedX2Prize = true;
			SoundManager.instance.PlayRewadWinSound();
		}
		else
		{
			List<int> list = new List<int>();
			list.Add(0);
			list.Add(1);
			list.Add(2);
			List<int> list2 = (from x in list
				where x != selectedIndex
				select x).ToList();
			x2PrizeIndex = list2[UnityEngine.Random.Range(0, list2.Count)];
		}
		float num = 0.2f;
		float num2 = 0.3f;
		float num3 = num2;
		for (int i = 0; i < 3; i++)
		{
			Button button = PrizeButtons[i];
			button.onClick.RemoveAllListeners();
			if (i == selectedIndex)
			{
				StartCoroutine(PopPrizeBox(i, num2));
				RectTransform component = button.GetComponent<RectTransform>();
				Vector3 localScale = component.transform.localScale;
				float x2 = localScale.x;
				Transform transform = component.transform;
				float x3 = x2 * 0.7f;
				float y = x2 * 0.7f;
				Vector3 localScale2 = component.transform.localScale;
				transform.localScale = new Vector3(x3, y, localScale2.z);
				RectTransform target = component;
				float x4 = x2 * 1.1f;
				float y2 = x2 * 1.1f;
				Vector3 localScale3 = component.localScale;
				target.DOScale(new Vector3(x4, y2, localScale3.z), num2);
			}
			else
			{
				num3 += num;
				StartCoroutine(PopPrizeBox(i, num3));
			}
		}
		Extensions.DelayedCallWithCoroutine(num3 + num, StartProfitCounting);
	}

	private void ShowImage(bool x2Img, Vector2 prizeBoxAnchoredPosition)
	{
		GameObject gameObject = (!x2Img) ? UnityEngine.Object.Instantiate(Prefabs.x1Image) : UnityEngine.Object.Instantiate(Prefabs.x2Image);
		gameObject.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
		RectTransform component = gameObject.GetComponent<RectTransform>();
		component.anchoredPosition = prizeBoxAnchoredPosition;
		Vector3 localScale = component.transform.localScale;
		float x = localScale.x;
		float num = (!x2Img) ? 0.5f : 0.7f;
		Transform transform = component.transform;
		float x2 = x * num;
		float y = x * num;
		Vector3 localScale2 = component.transform.localScale;
		transform.localScale = new Vector3(x2, y, localScale2.z);
		float num2 = (!x2Img) ? 0.8f : 1.1f;
		float duration = 0.2f;
		Sequence sequence = DOTween.Sequence();
		Sequence s = sequence;
		RectTransform target = component;
		float x3 = x * num2;
		float y2 = x * num2;
		Vector3 localScale3 = component.localScale;
		s.Append(target.DOScale(new Vector3(x3, y2, localScale3.z), 0.33f));
		Sequence s2 = sequence;
		RectTransform target2 = component;
		Vector2 anchoredPosition = component.anchoredPosition;
		s2.Append(target2.DOAnchorPosY(anchoredPosition.y + 15f, 1f));
		sequence.Insert(1.33f, gameObject.transform.DORotate(new Vector3(0f, 0f, 180f), duration));
		sequence.Insert(1.33f, gameObject.GetComponent<RectTransform>().DOScale(0f, duration));
	}

	private IEnumerator PopPrizeBox(int index, float delay)
	{
		yield return new WaitForSecondsRealtime(delay);
		Button prizeButton = PrizeButtons[index];
		prizeButton.GetComponent<RectTransform>().DOKill();
		ShowImage(index == x2PrizeIndex, prizeButton.GetComponent<RectTransform>().anchoredPosition);
		SoundManager.instance.PlayPrizeBoxPopSound();
		effectsSpawner.CreateExplosionEffect(prizeButton.transform.position, 1f, playSound: false, spawnDarkSmoke: false);
		int prizeParticlesCount = 25;
		Vector3 position4 = default(Vector3);
		for (int i = 0; i < prizeParticlesCount; i++)
		{
			float num = 1f;
			Vector3 position = prizeButton.transform.position;
			float x = position.x + UnityEngine.Random.Range(0f - num, num);
			Vector3 position2 = prizeButton.transform.position;
			float y = position2.y + UnityEngine.Random.Range(0f - num, num);
			Vector3 position3 = prizeButton.transform.position;
			position4 = new Vector3(x, y, position3.z);
			Rigidbody2D component = UnityEngine.Object.Instantiate(Prefabs.PrizeParticle, position4, Quaternion.identity).GetComponent<Rigidbody2D>();
			Vector2 a = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(100f, 300f);
			Vector2 force = (a + new Vector2(0f, 100f)) / 40f;
			component.AddForce(force, ForceMode2D.Impulse);
		}
		UnityEngine.Object.Destroy(prizeButton.gameObject);
	}

	private void StartProfitCounting()
	{
		if (selectedX2Prize)
		{
			HeaderText.text = LocalizationManager.Instance.GetLocalizedText("RewadDoubleAwesomeness");
		}
		else
		{
			HeaderText.text = LocalizationManager.Instance.GetLocalizedText("LevelsCompleteAwesomeText");
		}
		GlobalCommons.Instance.globalGameStats.IncreaseMoney(moneyToAdd);
		ProfitText.text = "0";
		targetProfitValue = moneyToAdd;
		profitStepPerSec = Mathf.FloorToInt((float)targetProfitValue / 1.5f);
		if (profitStepPerSec < 1)
		{
			profitStepPerSec = 1;
		}
		profitAnimationStarted = true;
		ProfitText.enabled = true;
		ProfitText.SetAlpha(0f);
		Text profitText = ProfitText;
		Color color = ProfitText.color;
		float r = color.r;
		Color color2 = ProfitText.color;
		float g = color2.g;
		Color color3 = ProfitText.color;
		profitText.DOColor(new Color(r, g, color3.b, 1f), 0.33f);
		ProfintUnderlyingImage.enabled = true;
		ProfintUnderlyingImage.SetAlpha(0f);
		Image profintUnderlyingImage = ProfintUnderlyingImage;
		Color color4 = ProfintUnderlyingImage.color;
		float r2 = color4.r;
		Color color5 = ProfintUnderlyingImage.color;
		float g2 = color5.g;
		Color color6 = ProfintUnderlyingImage.color;
		profintUnderlyingImage.DOColor(new Color(r2, g2, color6.b, 1f), 0.33f);
	}
}
