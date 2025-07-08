using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrizeSceneController : MonoBehaviour
{
	private SpriteRenderer prizeSR;

	private Rigidbody2D prizeRB;

	public EffectsSpawner effectsSpawner;

	private GameObject LeftWall;

	private GameObject RightWall;

	private GameObject FloorGO;

	private TextMeshProUGUI CountdownText;

	private RectTransform CountdownTextRT;

	private TextMeshProUGUI AwesomeText;

	private float countdownScaleBig = 1.2f;

	private float countdownScaleSmall = 1f;

	private float countdownScaleTime = 0.5f;

	private int currentComboValue;

	public Sprite prizeNormal;

	public Sprite prizeFlash;

	private int FlashTimer;

	private int prizeHP;

	private int prizeHPMax = 7;

	private int punchCoins;

	private int coinsGot;

	private Text ProfitText;

	private Button BackButton;

	private TextMeshProUGUI ResultsAwesomeText;

	private CanvasGroup ResultsBlockCG;

	private bool profitAnimationStarted;

	private int currentProfitValue;

	private int profitStepPerSec;

	private float particleDeviation = 1f;

	private List<Vector2> prevPrizePositions = new List<Vector2>();

	private const int prevPositionsToCompensate = 2;

	public Sprite[] prizeSprites;

	private Sprite currentPrizeSprite;

	private bool setBoundsPositions;

	private float initialFixedDeltaTime;

	private void Start()
	{
		initialFixedDeltaTime = Time.fixedDeltaTime;
		Time.fixedDeltaTime *= 0.2f;
		currentPrizeSprite = prizeSprites[0];
		SoundManager.instance.FadeOutMusic();
		effectsSpawner = new EffectsSpawner(EffectsSpawner.EffectsSpawnerPreset.RewardedVideoAndShopMenuAndPrizeScene);
		ResultsBlockCG = GameObject.Find("ResultsBlock").GetComponent<CanvasGroup>();
		ResultsBlockCG.gameObject.SetActive(value: false);
		ProfitText = ResultsBlockCG.gameObject.transform.Find("ProfitText").GetComponent<Text>();
		ResultsAwesomeText = ResultsBlockCG.gameObject.transform.Find("ResultsAwesomeText").GetComponent<TextMeshProUGUI>();
		BackButton = ResultsBlockCG.gameObject.transform.Find("BackButton").GetComponent<Button>();
		BackButton.onClick.AddListener(delegate
		{
			BackButtonClick();
		});
		int num = GlobalCommons.Instance.globalGameStats.MaxLevelIncome;
		int num2 = Mathf.CeilToInt(12.195f);
		if (num < num2)
		{
			num = num2;
		}
		punchCoins = Mathf.CeilToInt((float)num * 2.33f / (float)prizeHPMax);
		prizeHP = prizeHPMax;
		prizeSR = GameObject.Find("Prize").GetComponent<SpriteRenderer>();
		prizeRB = prizeSR.GetComponent<Rigidbody2D>();
		CountdownText = GameObject.Find("CountdownText").GetComponent<TextMeshProUGUI>();
		CountdownTextRT = CountdownText.GetComponent<RectTransform>();
		CountdownText.text = "4";
		AwesomeText = GameObject.Find("AwesomeText").GetComponent<TextMeshProUGUI>();
		CountdownText.SetAlpha(0f);
		AwesomeText.SetAlpha(0f);
		GlobalCommons.Instance.SaveGame();
	}

	private void BackButtonClick()
	{
		Time.fixedDeltaTime = initialFixedDeltaTime;
		GlobalCommons.Instance.globalGameStats.IncreaseMoney(coinsGot);
		GlobalCommons.Instance.globalGameStats.PrizeLevel++;
		GlobalCommons.Instance.globalGameStats.LastTimeGotPrize = DateTime.Now;
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("Upgrades");
	}

	private void Update()
	{
		if (!setBoundsPositions)
		{
			setBoundsPositions = true;
			LeftWall = GameObject.Find("LeftWall");
			Transform transform = LeftWall.transform;
			float x = (0f - Camera.main.orthographicSize) * Camera.main.aspect - 50f;
			Vector3 position = LeftWall.transform.position;
			float y = position.y;
			Vector3 position2 = LeftWall.transform.position;
			transform.position = new Vector3(x, y, position2.z);
			RightWall = GameObject.Find("RightWall");
			Transform transform2 = RightWall.transform;
			float x2 = Camera.main.orthographicSize * Camera.main.aspect + 50f;
			Vector3 position3 = RightWall.transform.position;
			float y2 = position3.y;
			Vector3 position4 = RightWall.transform.position;
			transform2.position = new Vector3(x2, y2, position4.z);
			FloorGO = GameObject.Find("FloorGO");
			Transform transform3 = FloorGO.transform;
			Vector3 position5 = FloorGO.transform.position;
			float x3 = position5.x;
			float y3 = 0f - Camera.main.orthographicSize - 50f;
			Vector3 position6 = FloorGO.transform.position;
			transform3.position = new Vector3(x3, y3, position6.z);
		}
		effectsSpawner.Update();
		if (prizeHP > 0)
		{
			ProcessTouches();
			if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
			{
				effectsSpawner.CreateFlareEffect(Vector3.zero, 0.5f);
				PunchPrizeBox();
			}
			if (FlashTimer > 0)
			{
				FlashTimer--;
				if (prizeSR.sprite != prizeFlash)
				{
					prizeSR.sprite = prizeFlash;
				}
			}
			else if (prizeSR.sprite != prizeNormal)
			{
				prizeSR.sprite = currentPrizeSprite;
			}
		}
		if (profitAnimationStarted && currentProfitValue < coinsGot)
		{
			SoundManager.instance.PlayCoinCountSound();
			effectsSpawner.SpawnOverUICoinFlyoffEffect(ProfitText.transform.position);
			int num = Mathf.FloorToInt((float)profitStepPerSec * Time.deltaTime);
			if (num < 1)
			{
				num = 1;
			}
			currentProfitValue += num;
			if (currentProfitValue > coinsGot)
			{
				currentProfitValue = coinsGot;
			}
			ProfitText.text = currentProfitValue.ToString();
			if (currentProfitValue == coinsGot)
			{
				Image component = BackButton.GetComponent<Image>();
				BackButton.gameObject.SetActive(value: true);
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

	private void ProcessTouches()
	{
		prevPrizePositions.Add(prizeSR.transform.position);
		if (prevPrizePositions.Count > 2)
		{
			prevPrizePositions.RemoveAt(0);
		}
		int touchCount = UnityEngine.Input.touchCount;
		for (int i = 0; i < touchCount; i++)
		{
			Touch touch = UnityEngine.Input.GetTouch(i);
			if (touch.phase != 0)
			{
				continue;
			}
			Vector3 v = Camera.main.ScreenToWorldPoint(touch.position);
			effectsSpawner.CreateFlareEffect(new Vector3(v.x, v.y, 0f), 0.5f);
			for (int j = 0; j < prevPrizePositions.Count; j++)
			{
				Vector2 b = prevPrizePositions[j];
				if (Vector2.Distance(v, b) <= 2f)
				{
					PunchPrizeBox();
					return;
				}
				SoundManager.instance.PlayShooshOutSound();
			}
		}
	}

	private void PunchPrizeBox()
	{
		coinsGot += punchCoins + Mathf.CeilToInt((float)punchCoins / (float)prizeHPMax * (float)currentComboValue);
		prizeRB.velocity = Vector2.zero;
		float num = -6f;
		Vector3 position = prizeRB.transform.position;
		if (position.x < 0f)
		{
			float num2 = num;
			Vector3 position2 = prizeRB.transform.position;
			num = num2 + 6f * ((0f - position2.x) / GlobalCommons.Instance.horizontalCameraBorderWithCompensation);
		}
		float num3 = 6f;
		Vector3 position3 = prizeRB.transform.position;
		if (position3.x > 0f)
		{
			float num4 = num3;
			Vector3 position4 = prizeRB.transform.position;
			num3 = num4 - 6f * (position4.x / GlobalCommons.Instance.horizontalCameraBorderWithCompensation);
		}
		prizeRB.AddForce(new Vector2(UnityEngine.Random.Range(num, num3), 12.5f) * 2f, ForceMode2D.Impulse);
		prizeRB.angularVelocity = UnityEngine.Random.Range(200f, 600f);
		if (UnityEngine.Random.value > 0.5f)
		{
			prizeRB.angularVelocity *= -1f;
		}
		currentComboValue++;
		int num5 = 4 + Mathf.FloorToInt(currentComboValue / 2);
		for (int i = 0; i < num5; i++)
		{
			Rigidbody2D component = UnityEngine.Object.Instantiate(Prefabs.PrizeCoin, prizeRB.transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
			Vector2 a = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(1000f, 2000f);
			component.GetComponent<PrizeCoin>().InitCoin(this);
			component.AddForce(a + new Vector2(0f, 2250f));
		}
		UpdateComboText();
		FlashTimer = 3;
		int num6 = 5;
		int num7 = 40;
		prizeHP--;
		if (prizeHP == 0)
		{
			SoundManager.instance.PlayPrizeBreakSound();
			num6 = num7;
			UnityEngine.Object.Destroy(prizeRB.gameObject);
			AwesomeText.DOKill();
			CountdownText.DOKill();
			AwesomeText.DOFade(0f, 0.5f);
			if (GlobalCommons.Instance.globalGameStats.DoubleCoinsPurchased)
			{
				coinsGot *= 2;
			}
			CountdownText.DOFade(0f, 0.5f).OnCompleteWithCoroutine(FadeInResults);
			effectsSpawner.CreateExplosionEffect(prizeRB.transform.position, 1f, playSound: false, spawnDarkSmoke: false);
		}
		else
		{
			SoundManager.instance.PlayPrizePunchSound();
			float num8 = (float)prizeHP / (float)prizeHPMax;
			if (num8 > 0.75f)
			{
				currentPrizeSprite = prizeSprites[0];
			}
			else if (num8 > 0.5f)
			{
				currentPrizeSprite = prizeSprites[1];
			}
			else if (num8 > 0.5f)
			{
				currentPrizeSprite = prizeSprites[2];
			}
			else
			{
				currentPrizeSprite = prizeSprites[3];
			}
		}
		Vector3 position8 = default(Vector3);
		for (int j = 0; j < num6; j++)
		{
			float num9 = (num6 != num7) ? particleDeviation : (particleDeviation * 1.66f);
			Vector3 position5 = prizeRB.transform.position;
			float x = position5.x + UnityEngine.Random.Range(0f - num9, num9);
			Vector3 position6 = prizeRB.transform.position;
			float y = position6.y + UnityEngine.Random.Range(0f - num9, num9);
			Vector3 position7 = prizeRB.transform.position;
			position8 = new Vector3(x, y, position7.z);
			Rigidbody2D component2 = UnityEngine.Object.Instantiate(Prefabs.PrizeParticle, position8, Quaternion.identity).GetComponent<Rigidbody2D>();
			Vector2 a2 = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(100f, 300f);
			Vector2 vector = (a2 + new Vector2(0f, 100f)) / 40f;
			if (num6 == num7)
			{
				vector *= UnityEngine.Random.Range(1.2f, 2f);
			}
			component2.AddForce(vector, ForceMode2D.Impulse);
		}
	}

	private void FadeInResults()
	{
		ResultsBlockCG.gameObject.SetActive(value: true);
		ResultsBlockCG.alpha = 0f;
		ResultsBlockCG.DOFade(1f, 0.5f);
		BackButton.gameObject.SetActive(value: false);
		profitStepPerSec = Mathf.FloorToInt((float)coinsGot / 1.5f);
		if (profitStepPerSec < 1)
		{
			profitStepPerSec = 1;
		}
		profitAnimationStarted = true;
		SoundManager.instance.PlayRewadWinSound();
		if (currentComboValue == prizeHPMax)
		{
			ResultsAwesomeText.text = LocalizationManager.Instance.GetLocalizedText("PrizePerfectUnboxing");
		}
		else
		{
			ResultsAwesomeText.text = LocalizationManager.Instance.GetLocalizedText("PrizeAwesome");
		}
	}

	private void UpdateComboText()
	{
		CountdownText.DOKill();
		CountdownText.transform.DOKill();
		CountdownText.fontSize = 100f;
		CountdownTextRT.anchoredPosition = new Vector2(0f, 100f);
		if (CountdownText.text != LocalizationManager.Instance.GetLocalizedText("PrizeOuch"))
		{
			Transform transform = CountdownText.transform;
			float x = countdownScaleBig;
			float y = countdownScaleBig;
			Vector3 localScale = CountdownText.transform.localScale;
			transform.localScale = new Vector3(x, y, localScale.z);
			CountdownText.transform.DOScale(countdownScaleSmall, countdownScaleTime);
		}
		if (currentComboValue == 0)
		{
			CountdownText.SetAlpha(0.5f);
			AwesomeText.SetAlpha(0f);
			CountdownText.text = LocalizationManager.Instance.GetLocalizedText("PrizeOuch");
			return;
		}
		AwesomeText.SetAlpha(0.5f);
		string text = string.Empty;
		if (currentComboValue >= 10)
		{
			text = LocalizationManager.Instance.GetLocalizedText("PrizeAwesome");
		}
		else if (currentComboValue >= 5)
		{
			text = LocalizationManager.Instance.GetLocalizedText("PrizeGreat");
		}
		else if (currentComboValue >= 2)
		{
			text = LocalizationManager.Instance.GetLocalizedText("PrizeNice");
		}
		else if (currentComboValue == 1)
		{
			text = string.Empty;
		}
		if (text != AwesomeText.text)
		{
			AwesomeText.text = text;
			Transform transform2 = AwesomeText.transform;
			float x2 = countdownScaleBig;
			float y2 = countdownScaleBig;
			Vector3 localScale2 = AwesomeText.transform.localScale;
			transform2.localScale = new Vector3(x2, y2, localScale2.z);
			AwesomeText.transform.DOScale(countdownScaleSmall, countdownScaleTime);
		}
		if (currentComboValue > 1)
		{
			CountdownText.text = currentComboValue.ToString() + LocalizationManager.Instance.GetLocalizedText("GameplayUICombo") + "!";
			CountdownText.SetAlpha(0.5f);
		}
		else
		{
			CountdownText.SetAlpha(0f);
		}
	}

	public void ProcessPrizeFloorTouch(Collision2D col)
	{
		SoundManager.instance.PlayPrizeDropSound();
		currentComboValue = 0;
		if (prizeHP != prizeHPMax)
		{
			UpdateComboText();
		}
		for (int i = 0; i < col.contacts.Length; i++)
		{
			effectsSpawner.SpawnSmoke(col.contacts[i].point, 2, 0f);
		}
	}
}
