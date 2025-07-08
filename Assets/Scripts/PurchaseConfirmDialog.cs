using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseConfirmDialog : MonoBehaviour
{
	private Button ConfirmPurchaseButton;

	private Button CancelPurchaseButton;

	private Button ShopButton;

	private Text HeaderText;

	private Text DescriptionText;

	private Text CostText;

	private UpgradesMenuController upgradesMenuController;

	private Button upgradeItemToConfirm;

	private int itemIndex;

	public const int armorIndex = -1;

	public const int speedIndex = -2;

	public Sprite[] WeaponIconSprites;

	public Sprite[] TankIconSprites;

	private Image PurchaseConfirmDialogImage;

	public Sprite inactivePurchaseButtonSprite;

	private CanvasGroup dialogCG;

	private float fadeTime = 0.15f;

	private float inScale;

	private float lastTimeSpawnedPrizeCoin;

	public void InitializeDialog(int itemCost, int itemIndex, UpgradesMenuController upgradesMenuController, Button upgradeItemToConfirm)
	{
		this.itemIndex = itemIndex;
		this.upgradeItemToConfirm = upgradeItemToConfirm;
		this.upgradesMenuController = upgradesMenuController;
		PurchaseConfirmDialogImage = base.transform.Find("PurchaseConfirmDialogImage").GetComponent<Image>();
		ConfirmPurchaseButton = PurchaseConfirmDialogImage.transform.Find("ConfirmPurchaseButton").GetComponent<Button>();
		CancelPurchaseButton = PurchaseConfirmDialogImage.transform.Find("CancelPurchaseButton").GetComponent<Button>();
		CancelPurchaseButton.onClick.AddListener(delegate
		{
			CancelPurchaseButtonClick();
		});
		ShopButton = PurchaseConfirmDialogImage.transform.Find("ShopButton").GetComponent<Button>();
		ShopButton.onClick.AddListener(delegate
		{
			ShopButtonClick();
		});
		if (GlobalCommons.Instance.globalGameStats.Money >= itemCost)
		{
			ShopButton.gameObject.SetActive(value: false);
			ConfirmPurchaseButton.onClick.AddListener(delegate
			{
				ConfirmPurchaseButtonClick();
			});
		}
		else
		{
			if (GlobalCommons.Instance.globalGameStats.LevelsCompleted >= 4)
			{
                //ShopButton.gameObject.SetActive(value: false);
                //if (Purchaser.Instance.IsInitialized())
                //{
                //	ShopButton.gameObject.SetActive(value: true);
                //}
                ShopButton.gameObject.SetActive(value: true);
            }
			else
			{
				ShopButton.gameObject.SetActive(value: false);
			}
			ConfirmPurchaseButton.image.sprite = inactivePurchaseButtonSprite;
			ConfirmPurchaseButton.onClick.AddListener(delegate
			{
				UnavailablePurchaseButtonClick();
			});
		}
		CostText = PurchaseConfirmDialogImage.transform.Find("CostText").GetComponent<Text>();
		CostText.text = itemCost.ToString();
		InitializeDialogItems();
		dialogCG = GetComponent<CanvasGroup>();
		dialogCG.alpha = 0f;
		dialogCG.DOFade(1f, fadeTime);
		Vector3 localScale = PurchaseConfirmDialogImage.transform.localScale;
		float x = localScale.x;
		Vector3 localScale2 = PurchaseConfirmDialogImage.transform.localScale;
		inScale = localScale2.x * 0.9f;
		Transform transform = PurchaseConfirmDialogImage.transform;
		float x2 = inScale;
		float y = inScale;
		Vector3 localScale3 = PurchaseConfirmDialogImage.transform.localScale;
		transform.localScale = new Vector3(x2, y, localScale3.z);
		Transform transform2 = PurchaseConfirmDialogImage.transform;
		float x3 = x;
		float y2 = x;
		Vector3 localScale4 = PurchaseConfirmDialogImage.transform.localScale;
		transform2.DOScale(new Vector3(x3, y2, localScale4.z), fadeTime);
		upgradesMenuController.UpgradeConfirmationActive = true;
	}

	private void UnavailablePurchaseButtonClick()
	{
		SoundManager.instance.PlayNotAvailableSound();
	}

	private void ShopButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		upgradesMenuController.effectsSpawner.DisableAllParticles();
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("ShopScene");
	}

	private void Update()
	{
		if (Time.fixedTime - lastTimeSpawnedPrizeCoin >= 0.1f)
		{
			lastTimeSpawnedPrizeCoin = Time.fixedTime;
			if (ShopButton.gameObject.activeInHierarchy)
			{
				upgradesMenuController.effectsSpawner.SpawnOverUICoinFlyoffEffect(ShopButton.transform.position, 0.5f);
			}
		}
	}

	private void InitializeDialogItems()
	{
		Image component = UnityEngine.Object.Instantiate(Prefabs.UpgradeWeaponItemImage, Vector3.zero, Quaternion.identity).GetComponent<Image>();
		component.transform.SetParent(PurchaseConfirmDialogImage.transform, worldPositionStays: false);
		component.GetComponent<RectTransform>().anchoredPosition = new Vector2(-108.5f, -10f);
		float num = 0.87f;
		Transform transform = component.transform;
		float x = num;
		float y = num;
		Vector3 localScale = component.transform.localScale;
		transform.localScale = new Vector3(x, y, localScale.z);
		int num2 = 0;
		if (itemIndex >= 0)
		{
			component.sprite = WeaponIconSprites[itemIndex];
			num2 = GlobalCommons.Instance.globalGameStats.WeaponsLevels[itemIndex];
		}
		else
		{
			component.sprite = TankIconSprites[-(itemIndex + 1)];
		}
		HeaderText = PurchaseConfirmDialogImage.transform.Find("HeaderText").GetComponent<Text>();
		DescriptionText = PurchaseConfirmDialogImage.transform.Find("DescriptionText").GetComponent<Text>();
		switch (upgradeItemToConfirm.name)
		{
		case "UpgradeTankSpeedButton":
			HeaderText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeSpeedHeaderTxt");
			DescriptionText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeSpeedDescriptionTxt");
			break;
		case "UpgradeTankArmorButton":
			HeaderText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeArmorHeaderTxt");
			DescriptionText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeArmorDescriptionTxt");
			break;
		case "UpgradeMachinegunButton":
			HeaderText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeMachinegunHeaderTxt");
			DescriptionText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeMachinegunDescriptionTxt");
			break;
		case "UpgradeShotgunButton":
			HeaderText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeShotgunHeaderTxt");
			if (num2 > 0)
			{
				DescriptionText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeShotgunPurchasedDescriptionTxt");
			}
			else
			{
				DescriptionText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeShotgunUnpurchaseeDescriptionTxt");
			}
			break;
		case "UpgradeMinigunButton":
			HeaderText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeMinigunHeaderTxt");
			if (num2 > 0)
			{
				DescriptionText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeMinigunPurchasedDescriptionTxt");
			}
			else
			{
				DescriptionText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeMinigunUnpurchaseeDescriptionTxt");
			}
			break;
		case "UpgradeCannonButton":
			HeaderText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeCannonHeaderTxt");
			if (num2 > 0)
			{
				DescriptionText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeCannonPurchasedDescriptionTxt");
			}
			else
			{
				DescriptionText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeCannonUnpurchaseeDescriptionTxt");
			}
			break;
		case "UpgradeHomingRocketButton":
			HeaderText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeHomingMslHeaderTxt");
			if (num2 > 0)
			{
				DescriptionText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeHomingMslPurchasedDescriptionTxt");
			}
			else
			{
				DescriptionText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeHomingMslUnpurchaseeDescriptionTxt");
			}
			break;
		case "UpgradeMineButton":
			HeaderText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeMinesHeaderTxt");
			if (num2 > 0)
			{
				DescriptionText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeMinesPurchasedDescriptionTxt");
			}
			else
			{
				DescriptionText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeMinesUnpurchaseeDescriptionTxt");
			}
			break;
		case "UpgradeGuidedRocketButton":
			HeaderText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeGuidedMslHeaderTxt");
			if (num2 > 0)
			{
				DescriptionText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeGuidedMslPurchasedDescriptionTxt");
			}
			else
			{
				DescriptionText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeGuidedMslUnpurchaseeDescriptionTxt");
			}
			break;
		case "UpgradeLaserButton":
			HeaderText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeLaserHeaderTxt");
			if (num2 > 0)
			{
				DescriptionText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeLaserPurchasedDescriptionTxt");
			}
			else
			{
				DescriptionText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeLaserUnpurchaseeDescriptionTxt");
			}
			break;
		case "UpgradeRailgunButton":
			HeaderText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeRailgunHeaderTxt");
			if (num2 > 0)
			{
				DescriptionText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeRailgunPurchasedDescriptionTxt");
			}
			else
			{
				DescriptionText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeRailgunUnpurchaseeDescriptionTxt");
			}
			break;
		case "UpgradeRicochetButton":
			HeaderText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeRicochetHeaderTxt");
			if (num2 > 0)
			{
				DescriptionText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeRicochetPurchasedDescriptionTxt");
			}
			else
			{
				DescriptionText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeRicochetUnpurchaseeDescriptionTxt");
			}
			break;
		case "UpgradeTripleButton":
			HeaderText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeTripleHeaderTxt");
			if (num2 > 0)
			{
				DescriptionText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeTriplePurchasedDescriptionTxt");
			}
			else
			{
				DescriptionText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeTripleUnpurchaseeDescriptionTxt");
			}
			break;
		case "UpgradeShockerButton":
			HeaderText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeShockerHeaderTxt");
			if (num2 > 0)
			{
				DescriptionText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeShockerPurchasedDescriptionTxt");
			}
			else
			{
				DescriptionText.text = LocalizationManager.Instance.GetLocalizedText("UpgradeShockerUnpurchaseeDescriptionTxt");
			}
			break;
		default:
			throw new Exception("unknown shop item type to confirm");
		}
		if (itemIndex >= 0 && num2 > 0)
		{
			HeaderText.text = HeaderText.text + " " + LocalizationManager.Instance.GetLocalizedText("LVPrefix") + (num2 + 1).ToString();
		}
	}

	private void ConfirmPurchaseButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		GameObject gameObject = GameObject.Find("UpgradeArmorTutorialCover");
		if (gameObject != null)
		{
			UnityEngine.Object.Destroy(gameObject.gameObject);
		}
		if (GlobalCommons.Instance.globalGameStats.WeaponUpgradeMessagePending)
		{
			GlobalCommons.Instance.globalGameStats.WeaponUpgradeMessagePending = false;
			UnityEngine.Object.Destroy(GameObject.Find("UpgradeMachinegunTutorialCover").gameObject);
		}
		dialogCG.DOKill();
		dialogCG.DOFade(0f, fadeTime).OnCompleteWithCoroutine(ProcessPurchase);
		PurchaseConfirmDialogImage.transform.DOKill();
		Transform transform = PurchaseConfirmDialogImage.transform;
		float x = inScale;
		float y = inScale;
		Vector3 localScale = PurchaseConfirmDialogImage.transform.localScale;
		transform.DOScale(new Vector3(x, y, localScale.z), fadeTime);
	}

	private void ProcessPurchase()
	{
		SoundManager.instance.PlayShopBuySound();
		if (itemIndex >= 0)
		{
			upgradesMenuController.ConfirmWeaponItemPurchase(upgradeItemToConfirm);
		}
		else
		{
			upgradesMenuController.ConfirmTankStatUpgrade(upgradeItemToConfirm);
		}
		upgradesMenuController.UpgradeConfirmationActive = false;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void CancelPurchaseButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		GameObject gameObject = GameObject.Find("UpgradeArmorTutorialCover");
		if (gameObject != null)
		{
			UnityEngine.Object.Destroy(gameObject.gameObject);
		}
		if (GlobalCommons.Instance.globalGameStats.WeaponUpgradeMessagePending)
		{
			GlobalCommons.Instance.globalGameStats.WeaponUpgradeMessagePending = false;
			UnityEngine.Object.Destroy(GameObject.Find("UpgradeMachinegunTutorialCover").gameObject);
		}
		dialogCG.DOKill();
		dialogCG.DOFade(0f, fadeTime).OnCompleteWithCoroutine(ProcessCancel);
		PurchaseConfirmDialogImage.transform.DOKill();
		Transform transform = PurchaseConfirmDialogImage.transform;
		float x = inScale;
		float y = inScale;
		Vector3 localScale = PurchaseConfirmDialogImage.transform.localScale;
		transform.DOScale(new Vector3(x, y, localScale.z), fadeTime);
	}

	private void ProcessCancel()
	{
		upgradesMenuController.UpgradeConfirmationActive = false;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void RewadButtonClick()
	{
        //bool isReady = AdManager.IsRewardedAdReady();
        //// Show it if it's ready
        //if (isReady)
        //{
        //    AdManager.ShowRewardedAd();
        //}
        SoundManager.instance.PlayButtonClickSound();
		//upgradesMenuController.effectsSpawner.DisableAllParticles();
		//int num = PlayRewardedAdSceneController.SetNextRewardValue(PlayRewardedAdSceneController.RewadResetLocation.Upgrades);
		//GlobalCommons.Instance.MessagesController.ShowConfirmationDialog(LocalizationManager.Instance.GetLocalizedText("WatchShortVideoForText") + Environment.NewLine + num.ToString() + " " + LocalizationManager.Instance.GetCoinsNumberEnding(num) + "?", ProceedToRewad, null, 0.75f);
	}

    void OnEnable()
    {
       // AdManager.RewardedAdCompleted += RewardedAdCompletedHandler;
    }
    // The event handler
    //void RewardedAdCompletedHandler(RewardedAdNetwork network, AdLocation location)
    //{
    //    upgradesMenuController.effectsSpawner.DisableAllParticles();
    //    int num = PlayRewardedAdSceneController.SetNextRewardValue(PlayRewardedAdSceneController.RewadResetLocation.Upgrades);
    //    GlobalCommons.Instance.MessagesController.ShowConfirmationDialog(LocalizationManager.Instance.GetLocalizedText("WatchShortVideoForText") + Environment.NewLine + num.ToString() + " " + LocalizationManager.Instance.GetCoinsNumberEnding(num) + "?", ProceedToRewad, null, 0.75f);

    //}
    // Unsubscribe
    void OnDisable()
    {
       // AdManager.RewardedAdCompleted -= RewardedAdCompletedHandler;
    }

    private void ProceedToRewad()
	{
		GlobalCommons.Instance.SceneToTransferTo = "Upgrades";
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("PlayRewardedAdScene");
	}
}
