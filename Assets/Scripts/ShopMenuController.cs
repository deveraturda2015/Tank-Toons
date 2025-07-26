using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenuController : MonoBehaviour
{
	private Button backButton;

	private Button RestorePurchasesBtn;

	private Text PlayerFundsText;

	private CanvasGroup mainCanvasGroup;

	public EffectsSpawner effectsSpawner;

	private Text NoAdsPrice;

	private Button PurchaseNoAdsButton;

	private string doubleCoinsEnabledString;

	private int currentMoneyVal;

	private GameObject thankYouMessage;

	public Sprite[] moneyPackSprites;

	public Sprite tankBankItemSprite;

	private List<Image> flareSpawnPoints;

	private float flareSpawnTimestamp = float.MaxValue;

	private Vector2 flaresSpawnInterval = new Vector2(1.6f, 3.2f);

	internal static bool PromoMode;

	private void Start()
	{
		AdmobManager.instance.HideBanner();

		if (GlobalCommons.Instance.PromotionController.PromoTimeLeft.HasValue)
		{
			PromoMode = true;
		}
		doubleCoinsEnabledString = LocalizationManager.Instance.GetLocalizedText("Purchased");
		effectsSpawner = new EffectsSpawner(EffectsSpawner.EffectsSpawnerPreset.RewardedVideoAndShopMenuAndPrizeScene);
		backButton = GameObject.Find("BackButton").GetComponent<Button>();
		backButton.onClick.AddListener(delegate
		{
			BackButtonClick();
		});
		RestorePurchasesBtn = GameObject.Find("RestorePurchasesBtn").GetComponent<Button>();
		if (Application.platform == RuntimePlatform.IPhonePlayer && !GlobalCommons.Instance.globalGameStats.DoubleCoinsPurchased)
		{
			RestorePurchasesBtn.onClick.AddListener(delegate
			{
				RestorePurchasesBtnClick();
			});
		}
		else
		{
			UnityEngine.Object.Destroy(RestorePurchasesBtn.gameObject);
		}
		currentMoneyVal = GlobalCommons.Instance.globalGameStats.Money;
		PlayerFundsText = GameObject.Find("PlayerFundsText").GetComponent<Text>();
		PlayerFundsText.text = currentMoneyVal.ToString();
		mainCanvasGroup = GetComponent<CanvasGroup>();
		//InitPricesAndValues();
		GlobalCommons.Instance.SaveGame();
		flareSpawnTimestamp = Time.fixedTime + UnityEngine.Random.Range(flaresSpawnInterval.x, flaresSpawnInterval.y);
	}

	private void Update()
	{
		if (thankYouMessage != null)
		{
			return;
		}
		effectsSpawner.Update();
		if (Time.fixedTime > flareSpawnTimestamp)
		{
			flareSpawnTimestamp = Time.fixedTime + UnityEngine.Random.Range(flaresSpawnInterval.x, flaresSpawnInterval.y);
			Vector3 position = flareSpawnPoints[UnityEngine.Random.Range(0, flareSpawnPoints.Count)].transform.position;
			position = new Vector3(position.x + UnityEngine.Random.Range(-0.7f, 0.7f), position.y + UnityEngine.Random.Range(-0.3f, 0.3f), position.z);
			effectsSpawner.SpawnOverUIFlare(position);
		}
		if (currentMoneyVal != GlobalCommons.Instance.globalGameStats.Money)
		{
			SoundManager.instance.PlayCoinCountSound();
			effectsSpawner.SpawnOverUICoinFlyoffEffect(PlayerFundsText.transform.position);
			int num = Math.Abs(currentMoneyVal - GlobalCommons.Instance.globalGameStats.Money);
			num /= 6;
			if (num < 1)
			{
				num = 1;
			}
			if (currentMoneyVal < GlobalCommons.Instance.globalGameStats.Money)
			{
				currentMoneyVal += num;
			}
			if (currentMoneyVal > GlobalCommons.Instance.globalGameStats.Money)
			{
				currentMoneyVal = GlobalCommons.Instance.globalGameStats.Money;
			}
			PlayerFundsText.text = currentMoneyVal.ToString();
		}
	}

	private void ShowThankYouMessage()
	{
		if (!(thankYouMessage != null))
		{
			thankYouMessage = UnityEngine.Object.Instantiate(Prefabs.ThankYouMessage, Vector3.zero, Quaternion.identity);
			thankYouMessage.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
		}
	}

	//private void InitPricesAndValues()
	//{
	//	flareSpawnPoints = new List<Image>();
	//	int num = 6;
	//	while (num-- > 0)
	//	{
	//		Button shopButton = UnityEngine.Object.Instantiate(Prefabs.ShopItemBtn).GetComponent<Button>();
	//		shopButton.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
	//		Image component = shopButton.transform.Find("CoinImage").GetComponent<Image>();
	//		Text component2 = shopButton.transform.Find("CostText").GetComponent<Text>();
	//		Image component3 = shopButton.transform.Find("ItemImage").GetComponent<Image>();
	//		flareSpawnPoints.Add(component3);
	//		Text component4 = shopButton.transform.Find("ItemValue").GetComponent<Text>();
	//		Text component5 = shopButton.transform.Find("BottomText").GetComponent<Text>();
	//		int num3 = num;
	//		num3 = ((num != 5) ? (num + 1) : 0);
	//		shopButton.GetComponent<RectTransform>().anchoredPosition = new Vector2((float)(num3 % 3 - 1) * 230f, 100f - (float)(num3 / 3) * 200f);

	//		if (num == 5)
	//		{
	//			PurchaseNoAdsButton = shopButton;
	//			NoAdsPrice = component2;
	//			shopButton.name = "PurchaseNoAdsButton";
	//			if (GlobalCommons.Instance.globalGameStats.DoubleCoinsPurchased)
	//			{
	//				component2.text = doubleCoinsEnabledString;
	//			}
	//			else
	//			{
	//				component2.text = Purchaser.Instance.GetLocalizedPrice(Purchaser.PRODUCT_DOUBLE_COINS);
	//				shopButton.onClick.AddListener(delegate
	//				{
	//					PurchaseNoAdsButtonClick();
	//				});
	//			}
	//			component3.sprite = tankBankItemSprite;
	//			component4.text = LocalizationManager.Instance.GetLocalizedText("TankBankNameText");
	//			RectTransform rectTransform = component4.rectTransform;
	//			Vector2 anchoredPosition = component4.rectTransform.anchoredPosition;
	//			float x = anchoredPosition.x;
	//			Vector2 anchoredPosition2 = component4.rectTransform.anchoredPosition;
	//			rectTransform.anchoredPosition = new Vector2(x, anchoredPosition2.y + 4f);
	//			component.enabled = false;
	//			component3.SetNativeSize();
	//			component3.transform.DOScale(0.65f, 0f);
	//			RectTransform rectTransform2 = component3.rectTransform;
	//			Vector2 anchoredPosition3 = component3.rectTransform.anchoredPosition;
	//			float x2 = anchoredPosition3.x;
	//			Vector2 anchoredPosition4 = component3.rectTransform.anchoredPosition;
	//			rectTransform2.anchoredPosition = new Vector2(x2, anchoredPosition4.y);
	//			Text text = component5;
	//			text.text = LocalizationManager.Instance.GetLocalizedText("ShopDoubleCoinsText");
	//		}
	//		else
	//		{
	//			UnityEngine.Object.Destroy(component5.gameObject);
	//			shopButton.name = "PurchasePack" + (num + 1).ToString() + "Button";
	//			component2.text = Purchaser.Instance.GetLocalizedPrice(Purchaser.Instance.GetCoinsPackItemNameWithIndex(num + 1)).ToString();
	//			component3.sprite = moneyPackSprites[num];
	//			component4.text = ((!PromoMode) ? GlobalBalance.goldPacksAmount[num] : (GlobalBalance.goldPacksAmount[num] * 2)).ToString();
	//			RectTransform rectTransform3 = component4.rectTransform;
	//			Vector2 anchoredPosition5 = component4.rectTransform.anchoredPosition;
	//			float x3 = anchoredPosition5.x + 15f;
	//			Vector2 anchoredPosition6 = component4.rectTransform.anchoredPosition;
	//			rectTransform3.anchoredPosition = new Vector2(x3, anchoredPosition6.y);
	//			RectTransform rectTransform4 = component.rectTransform;
	//			float num4 = (0f - LayoutUtility.GetPreferredWidth(component4.rectTransform)) / 2f - 21f;
	//			Vector2 anchoredPosition7 = component4.rectTransform.anchoredPosition;
	//			float x4 = num4 + anchoredPosition7.x;
	//			Vector2 anchoredPosition8 = component.rectTransform.anchoredPosition;
	//			rectTransform4.anchoredPosition = new Vector2(x4, anchoredPosition8.y);
	//			shopButton.onClick.AddListener(delegate
	//			{
	//				PurchaseCoinsPackButtonClick(shopButton);
	//			});
	//			component3.SetNativeSize();
	//			component3.transform.DOScale(0.65f, 0f);
	//		}
	//	}
	//}

	private void SetShopEnabled(bool val)
	{
		UnityEngine.Object.FindObjectOfType<GraphicRaycaster>().enabled = val;
	}

	private void BackButtonClick()
	{
		RectTransform component = backButton.GetComponent<RectTransform>();
		Vector3 localScale = component.transform.localScale;
		float x = localScale.x;
		Transform transform = component.transform;
		float x2 = x * 0.7f;
		float y = x * 0.7f;
		Vector3 localScale2 = component.transform.localScale;
		transform.localScale = new Vector3(x2, y, localScale2.z);
		RectTransform target = component;
		float x3 = x * 1.1f;
		float y2 = x * 1.1f;
		Vector3 localScale3 = component.localScale;
		target.DOScale(new Vector3(x3, y2, localScale3.z), 0.17f);
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("MainMenu");
	}

	private void RestorePurchasesBtnClick()
	{
		Purchaser.instance.RestorePurchases();
	}

	public void ProcessCoinsPurchase()
	{
		SetShopEnabled(val: true);
		ShowThankYouMessage();
	}

	public void ProcessDoubleCoinsPurchase()
	{
		SetShopEnabled(val: true);
		NoAdsPrice.text = doubleCoinsEnabledString;
		PurchaseNoAdsButton.onClick.RemoveAllListeners();
		ShowThankYouMessage();
	}

	public void ProcessPurchaseFailed()
	{
		SetShopEnabled(val: true);
		GameObject gameObject = UnityEngine.Object.Instantiate(Prefabs.PurchaseFailedMessage, Vector3.zero, Quaternion.identity);
		gameObject.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
	}

	private void PurchaseCoinsPackButtonClick(Button btn)
	{
		Debug.Log("asd");
		int index = int.Parse(btn.name[12].ToString());
		SetShopEnabled(val: false);
		//Purchaser.Instance.BuyProduct(Purchaser.Instance.GetCoinsPackItemNameWithIndex(index));
	}

	private void PurchaseNoAdsButtonClick()
	{
		Debug.Log("ddd");

		SetShopEnabled(val: false);
		//Purchaser.Instance.BuyProduct(Purchaser.PRODUCT_DOUBLE_COINS);
	}
}
