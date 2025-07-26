using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesMenuController : MonoBehaviour
{
	private Button playButton;

	private Button menuButton;

	private Button ShopButton;

	public EffectsSpawner effectsSpawner;

	private Button monehButton;

	private Button MaxButton;

	private Button MinButton;

	private Button RewadButtonUpg;

	private Button achievementsButton;

	private Button PrizeButton;

	private Button GameCenterButton;

	private Text PrizeTimeLeftText;

	private float lastTimeSpawnedPrizeCoin;

	private float lastTimeSpawnedRewadCoin;

	private Button upgradeTankSpeedButton;

	private Button upgradeTankArmorButton;

	private Text playerFundsText;

	public bool upgradeMachinegunTutorActive;

	public bool upgradeArmorTutorActive;

	public bool prizeTutorActive;

	private bool someTutorialShown;

	private int maxWeaponUpgradeLevel;

	private int maxSpeedUpgradeLevel;

	private int maxArmorUpgradeLevel;

	private bool rewadAvaileble;

	private ScrollRect WeaponsScroll;

	private RectTransform WeaponsScrollContentRT;

	public Sprite LockedButtonSprite;

	public Sprite NotReadyRewadButtonSprite;

	private string purchaseIndicatorName = "purchaseIndicator";

	public bool UpgradeConfirmationActive;

	private List<Button> allUpgradeButtons = new List<Button>();

	public GameObject LeaderboardUI;
	public GameObject RewardAdsConfirmationUI;
	public GameObject NoAdsUI;

	private static string[] upgradeWeaponButtonNames = new string[15]
	{
		"UpgradeMachinegunButton",
		"UpgradeShotgunButton",
		"UpgradeMinigunButton",
		"UpgradeCannonButton",
		"UpgradeHomingRocketButton",
		"UpgradeMineButton",
		"UpgradeGuidedRocketButton",
		"UpgradeLaserButton",
		"UpgradeRailgunButton",
		"UpgradeSuicideButton",
		"UpgradeGoldButton",
		"UpgradeRandomButton",
		"UpgradeRicochetButton",
		"UpgradeTripleButton",
		"UpgradeShockerButton"
	};

	public Sprite[] WeaponIconSprites;

	private int currentMoneyVal;

	internal static bool AnyUpgradeAvailable
	{
		get
		{
			for (int i = 0; i < GlobalCommons.Instance.globalGameStats.WeaponsLevels.Length; i++)
			{
				if (!PlayerBalance.PlayerUnavailableWeaponIndexesList.Contains(i))
				{
					WeaponTypes weaponTypes = (WeaponTypes)i;
					int num = GlobalCommons.Instance.globalGameStats.WeaponsLevels[i];
					int num2 = PlayerBalance.WeaponsUpgradesCost[0].Length;
					int num3 = (num < num2) ? PlayerBalance.WeaponsUpgradesCost[i][num] : 0;
					if (num < num2 && GlobalCommons.Instance.globalGameStats.Money >= PlayerBalance.WeaponsUpgradesCost[i][num])
					{
						return true;
					}
				}
			}
			if (GlobalCommons.Instance.globalGameStats.TankArmorLevel < PlayerBalance.armorUpgradeCost.Length)
			{
				int num4 = PlayerBalance.armorUpgradeCost[GlobalCommons.Instance.globalGameStats.TankArmorLevel];
				if (num4 <= GlobalCommons.Instance.globalGameStats.Money)
				{
					return true;
				}
			}
			if (GlobalCommons.Instance.globalGameStats.TankSpeedLevel < PlayerBalance.speedUpgradeCost.Length)
			{
				int num5 = PlayerBalance.speedUpgradeCost[GlobalCommons.Instance.globalGameStats.TankSpeedLevel];
				if (num5 <= GlobalCommons.Instance.globalGameStats.Money)
				{
					return true;
				}
			}
			return false;
		}
	}

	private void Start()
	{
		AdmobManager.instance.HideBanner();

		effectsSpawner = new EffectsSpawner(EffectsSpawner.EffectsSpawnerPreset.UpgradesShop);
		maxWeaponUpgradeLevel = PlayerBalance.WeaponsUpgradesCost[0].Length;
		maxSpeedUpgradeLevel = PlayerBalance.speedUpgradeCost.Length;
		maxArmorUpgradeLevel = PlayerBalance.armorUpgradeCost.Length;
		playButton = GameObject.Find("PlayButton").GetComponent<Button>();
		playButton.onClick.AddListener(delegate
		{
			PlayButtonClick();
		});
		menuButton = GameObject.Find("MenuButton").GetComponent<Button>();
		menuButton.onClick.AddListener(delegate
		{
			MenuButtonClick();
		});
		monehButton = GameObject.Find("MonehButton").GetComponent<Button>();
		monehButton.onClick.AddListener(delegate
		{
			MonehButtonClick();
		});
		achievementsButton = GameObject.Find("AchievementsButton").GetComponent<Button>();
		achievementsButton.onClick.AddListener(delegate
		{
			AchievementsButtonClick();
		});

		PrizeButton = GameObject.Find("PrizeButton").GetComponent<Button>();
		if (GlobalCommons.Instance.globalGameStats.LevelsCompleted >= 2)
		{
			PrizeButton.onClick.AddListener(delegate
			{
				PrizeButtonClick();
			});
			PrizeTimeLeftText = PrizeButton.transform.Find("PrizeTimeLeftText").GetComponent<Text>();
			UpdatePrizeTimerText();
		}
		else
		{
			PrizeButton.image.sprite = LockedButtonSprite;
			PrizeButton.onClick.AddListener(delegate
			{
				LockedButtonClick();
			});
			PrizeTimeLeftText = PrizeButton.transform.Find("PrizeTimeLeftText").GetComponent<Text>();
			UnityEngine.Object.Destroy(PrizeTimeLeftText);
		}
		//ShopButton = GameObject.Find("ShopButton").GetComponent<Button>();
		//if (GlobalCommons.Instance.globalGameStats.LevelsCompleted >= 4)
		//{
		//	ShopButton.onClick.AddListener(delegate
		//	{
		//		ShopButtonClick();
		//	});
		//}
		//else
		//{
		//	ShopButton.image.sprite = LockedButtonSprite;
		//	ShopButton.onClick.AddListener(delegate
		//	{
		//		LockedButtonClick();
		//	});
		//}

		RewadButtonUpg = GameObject.Find("RewadButtonUpg").GetComponent<Button>();
		//if ((GlobalCommons.Instance.globalGameStats.LevelsCompleted >= 4) ? true : false)
		//{
		//	if (GlobalCommons.Instance.AdsProcessor.CanShowRewardedAd() ? true : false)
		//	{
		//RewadButtonUpg.onClick.AddListener(delegate
		//{
		//	OpenRewardUI();
		//});
		rewadAvaileble = true;
		//	}
		//	else
		//	{
		//		UnityEngine.Object.Destroy(RewadButtonUpg.gameObject);
		//	}
		//}
		//else
		//{
		//	UnityEngine.Object.Destroy(RewadButtonUpg.gameObject);
		//}


		//GameCenterButton = GameObject.Find("GameCenterButton").GetComponent<Button>();
		//GameCenterButton.onClick.AddListener(delegate
		//{
		//	GameCenterButtonClick();
		//});

		MaxButton = GameObject.Find("MaxButton").GetComponent<Button>();
		MaxButton.onClick.AddListener(delegate
		{
			MaxButtonClick();
		});
		MinButton = GameObject.Find("MinButton").GetComponent<Button>();
		MinButton.onClick.AddListener(delegate
		{
			MinButtonClick();
		});
		playerFundsText = GameObject.Find("PlayerFundsText").GetComponent<Text>();
		WeaponsScroll = GameObject.Find("WeaponsScroll").GetComponent<ScrollRect>();
		WeaponsScrollContentRT = GameObject.Find("WeaponsScrollContent").GetComponent<RectTransform>();
		upgradeTankSpeedButton = GameObject.Find("UpgradeTankSpeedButton").GetComponent<Button>();
		upgradeTankArmorButton = GameObject.Find("UpgradeTankArmorButton").GetComponent<Button>();
		upgradeTankSpeedButton.onClick.AddListener(delegate
		{
			UpgradeTankButtonClick(upgradeTankSpeedButton);
		});
		upgradeTankArmorButton.onClick.AddListener(delegate
		{
			UpgradeTankButtonClick(upgradeTankArmorButton);
		});
		allUpgradeButtons.Add(upgradeTankSpeedButton);
		allUpgradeButtons.Add(upgradeTankArmorButton);
		Canvas component = GameObject.Find("Canvas").GetComponent<Canvas>();
		int num = 0;
		int num2 = 0;
		float num3 = 150f;
		float num4 = 135f;
		float num5 = 80f;
		float num6 = -210f;
		float num7 = 0.85f;
		for (int i = 0; i < upgradeWeaponButtonNames.Length; i++)
		{
			if (!PlayerBalance.PlayerUnavailableWeaponIndexesList.Contains(i))
			{
				Button upgradeWeaponItemButton = UnityEngine.Object.Instantiate(Prefabs.UpgradeWeaponItem, Vector3.zero, Quaternion.identity).GetComponent<Button>();
				upgradeWeaponItemButton.transform.SetParent(WeaponsScrollContentRT.transform, worldPositionStays: false);
				Transform transform = upgradeWeaponItemButton.transform;
				float x = num7;
				float y = num7;
				Vector3 localScale = upgradeWeaponItemButton.transform.localScale;
				transform.localScale = new Vector3(x, y, localScale.z);
				upgradeWeaponItemButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 * (float)num + num5 - num3, (0f - num4) * (float)num2 + num6 + num4);
				upgradeWeaponItemButton.name = upgradeWeaponButtonNames[i];
				upgradeWeaponItemButton.onClick.AddListener(delegate
				{
					UpgradeWeaponButtonClick(upgradeWeaponItemButton);
				});
				allUpgradeButtons.Add(upgradeWeaponItemButton);
				Image component2 = UnityEngine.Object.Instantiate(Prefabs.UpgradeWeaponItemImage, Vector3.zero, Quaternion.identity).GetComponent<Image>();
				component2.transform.SetParent(upgradeWeaponItemButton.transform, worldPositionStays: false);
				component2.rectTransform.anchoredPosition = new Vector2(-17f, 17f);
				component2.sprite = WeaponIconSprites[i];
				Text component3 = UnityEngine.Object.Instantiate(Prefabs.WeaponItemCostText, Vector3.zero, Quaternion.identity).GetComponent<Text>();
				Transform transform2 = component3.transform;
				float x2 = 1f / num7;
				float y2 = 1f / num7;
				Vector3 localScale2 = component3.transform.localScale;
				transform2.localScale = new Vector3(x2, y2, localScale2.z);
				component3.transform.SetParent(upgradeWeaponItemButton.transform, worldPositionStays: false);
				component3.name = "CostText";
				component3.GetComponent<RectTransform>().anchoredPosition = new Vector2(4f, -49f);
				num++;
				if (num == 2)
				{
					num = 0;
					num2++;
				}
			}
		}
		SoundManager.instance.ToggleMusic(SoundManager.MusicType.MenusMusic);
		currentMoneyVal = GlobalCommons.Instance.globalGameStats.Money;
		playerFundsText.text = currentMoneyVal.ToString();
		monehButton.GetComponent<Image>().SetAlpha(0f);
		MaxButton.GetComponent<Image>().SetAlpha(0f);
		MinButton.gameObject.SetActive(value: false);
		UnityEngine.Object.Destroy(monehButton.gameObject);
		UnityEngine.Object.Destroy(MaxButton.gameObject);
		UnityEngine.Object.Destroy(MinButton.gameObject);
		SaveGame();
		ProcessUpgradeMachinegunTutorial();
		ProcessPrizeTutorial();
		ProcessArmorTutorial();
		UpdateUpgradeButtons();
		DebugHelper.Log("play time in minutes is: " + GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(GameStatistics.Stat.SecondsSpentPlaying) / 60);

        AdmobManager.instance.HideBanner();

    }

    private void LockedButtonClick()
	{
		SoundManager.instance.PlayNotAvailableSound();
	}

	public void RewadButtonClick()
	{
		AdmobManager.instance.ShowRewardedAd(() =>
		{
			GlobalCommons.Instance.globalGameStats.IncreaseMoney(1000);
			GlobalCommons.Instance.SaveGame();
		});

        //bool isReady = AdManager.IsRewardedAdReady();
        //// Show it if it's ready
        //if (isReady)
        //{
        //    AdManager.ShowRewardedAd();
        //}
        AdmobManager.instance.HideBanner();

        SoundManager.instance.PlayButtonClickSound();

		CloseRewardUI();
		//int num = PlayRewardedAdSceneController.SetNextRewardValue(PlayRewardedAdSceneController.RewadResetLocation.Upgrades);
		//GlobalCommons.Instance.MessagesController.ShowConfirmationDialog(LocalizationManager.Instance.GetLocalizedText("WatchShortVideoForText") + Environment.NewLine + 100 + " " + LocalizationManager.Instance.GetCoinsNumberEnding(100) + "?", ProceedToRewad, null, 0.75f);
	}

	public void OpenRewardUI()
	{
		SoundManager.instance.PlayButtonClickSound();

		if (AdmobManager.RewardAdAvailable)
		{
			RewardAdsConfirmationUI.SetActive(true);
		}
		else
		{
			NoAdsUI.SetActive(true);
		}
	}

	public void CloseRewardUI()
	{
		SoundManager.instance.PlayButtonClickSound();

		RewardAdsConfirmationUI.SetActive(false);
		NoAdsUI.SetActive(false);
	}


	void OnEnable()
    {
       // AdManager.RewardedAdCompleted += RewardedAdCompletedHandler;
    }
    // The event handler
    //void RewardedAdCompletedHandler(RewardedAdNetwork network, AdLocation location)
    //{
    //    SoundManager.instance.PlayButtonClickSound();
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
		effectsSpawner.DisableAllParticles();
		GlobalCommons.Instance.SceneToTransferTo = "Upgrades";
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("PlayRewardedAdScene");
	}

	private void Update()
	{
		effectsSpawner.Update();
		UpdatePrizeTimerText();
		if (RewadButtonUpg != null && !GlobalCommons.Instance.globalGameStats.isPayingPlayer && Time.timeSinceLevelLoad > 0.5f && Time.fixedTime - lastTimeSpawnedRewadCoin >= 0.3f && !upgradeMachinegunTutorActive && !upgradeArmorTutorActive && !UpgradeConfirmationActive && UnityEngine.Object.FindObjectsOfType<ConfirmationWindow>().Length == 0 && UnityEngine.Object.FindObjectsOfType<SimpleUIMessageController>().Length == 0)
		{
			lastTimeSpawnedRewadCoin = Time.fixedTime;
			effectsSpawner.SpawnOverUICoinFlyoffEffect(RewadButtonUpg.transform.position, 0.5f);
		}
		if (currentMoneyVal != GlobalCommons.Instance.globalGameStats.Money)
		{
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
			else
			{
				currentMoneyVal -= num;
			}
			playerFundsText.text = currentMoneyVal.ToString();
		}
	}

	private void UpdatePrizeTimerText()
	{
		if (PrizeButton == null || PrizeTimeLeftText == null)
		{
			return;
		}
		TimeSpan timeLeftForPrize = GlobalBalance.GetTimeLeftForPrize();
		if (timeLeftForPrize > TimeSpan.Zero)
		{
			PrizeTimeLeftText.text = $"{(int)timeLeftForPrize.TotalHours:0}:{timeLeftForPrize.Minutes:00}:{timeLeftForPrize.Seconds:00}";
			return;
		}
		if (Time.timeSinceLevelLoad > 0.5f && Time.fixedTime - lastTimeSpawnedPrizeCoin >= 0.1f && !upgradeMachinegunTutorActive && !upgradeArmorTutorActive && !UpgradeConfirmationActive && UnityEngine.Object.FindObjectsOfType<ConfirmationWindow>().Length == 0 && UnityEngine.Object.FindObjectsOfType<SimpleUIMessageController>().Length == 0)
		{
			lastTimeSpawnedPrizeCoin = Time.fixedTime;
			effectsSpawner.SpawnOverUICoinFlyoffEffect(PrizeButton.transform.position, 0.5f);
		}
		PrizeTimeLeftText.text = LocalizationManager.Instance.GetLocalizedText("PrizeReady");
	}

	private void MaxButtonClick()
	{
		GlobalCommons.Instance.globalGameStats.MaximizeProgressValues();
		SoundManager.instance.PlayButtonClickSound();
		effectsSpawner.DisableAllParticles();
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("Upgrades");
	}

	private void MinButtonClick()
	{
		GlobalCommons.Instance.globalGameStats.MinimizeProgressValues();
		SoundManager.instance.PlayButtonClickSound();
		effectsSpawner.DisableAllParticles();
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("Upgrades");
	}

	private void ProcessPrizeTutorial()
	{
		Image component = GameObject.Find("PrizeTutorialCover").GetComponent<Image>();
		if (!GlobalCommons.Instance.globalGameStats.PrizeTutorialShown && GlobalBalance.GetTimeLeftForPrize() <= TimeSpan.Zero && GlobalCommons.Instance.globalGameStats.LevelsCompleted >= 2 && !someTutorialShown)
		{
			someTutorialShown = true;
			GlobalCommons.Instance.globalGameStats.PrizeTutorialShown = true;
			component.transform.SetSiblingIndex(component.transform.parent.childCount - 1);
			PrizeButton.transform.SetSiblingIndex(PrizeButton.transform.parent.childCount - 1);
			prizeTutorActive = true;
			SoundManager.instance.PlayRewadWinSound();
		}
		else
		{
			UnityEngine.Object.Destroy(component.gameObject);
		}
	}

	private void ProcessArmorTutorial()
	{
		Image component = GameObject.Find("UpgradeArmorTutorialCover").GetComponent<Image>();
		if (!GlobalCommons.Instance.globalGameStats.ArmorUpgradeTutorialShown && GlobalCommons.Instance.globalGameStats.TankArmorLevel == 0 && GlobalCommons.Instance.globalGameStats.Money >= PlayerBalance.armorUpgradeCost[0] && !someTutorialShown)
		{
			someTutorialShown = true;
			upgradeArmorTutorActive = true;
			GlobalCommons.Instance.globalGameStats.ArmorUpgradeTutorialShown = true;
			component.transform.SetSiblingIndex(component.transform.parent.childCount - 1);
			Button component2 = GameObject.Find("UpgradeArmorTutorialButton").GetComponent<Button>();
			component2.onClick.AddListener(delegate
			{
				ProcessArmorTutorialButtonClick();
			});
			GameObject.Find("ArmorTutorCostText").GetComponent<Text>().text = PlayerBalance.armorUpgradeCost[0].ToString();
		}
		else
		{
			UnityEngine.Object.Destroy(component.gameObject);
		}
	}

	private void ProcessArmorTutorialButtonClick()
	{
		upgradeArmorTutorActive = false;
		UpdateUpgradeButtons();
		UpgradeTankButtonClick(upgradeTankArmorButton);
	}

	private bool ProcessUpgradeMachinegunTutorial()
	{
		Image component = GameObject.Find("UpgradeMachinegunTutorialCover").GetComponent<Image>();
		if (GlobalCommons.Instance.globalGameStats.WeaponUpgradeMessagePending)
		{
			component.transform.SetSiblingIndex(component.transform.parent.childCount - 1);
			GameObject gameObject = GameObject.Find("UpgradeMachinegunButton");
			gameObject.transform.SetParent(component.transform, worldPositionStays: true);
			upgradeMachinegunTutorActive = true;
			int num = PlayerBalance.WeaponsUpgradesCost[0][1] - GlobalCommons.Instance.globalGameStats.Money;
			if (num > 0)
			{
				GlobalCommons.Instance.globalGameStats.IncreaseMoney(num);
			}
			someTutorialShown = true;
			return true;
		}
		UnityEngine.Object.Destroy(component.gameObject);
		return false;
	}

	private void SaveGame()
	{
		GlobalCommons.Instance.SaveGame();
	}

	private void UpgradeTankButtonClick(Button btn)
	{
		if (btn.name == "UpgradeTankSpeedButton")
		{
			if (GlobalCommons.Instance.globalGameStats.TankSpeedLevel < maxSpeedUpgradeLevel)
			{
				SoundManager.instance.PlayButtonClickSound();
				int itemCost = PlayerBalance.speedUpgradeCost[GlobalCommons.Instance.globalGameStats.TankSpeedLevel];
				PurchaseConfirmDialog component = UnityEngine.Object.Instantiate(Prefabs.PurchaseConfirmDialog, Vector3.zero, Quaternion.identity).GetComponent<PurchaseConfirmDialog>();
				component.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
				component.InitializeDialog(itemCost, -2, this, btn);
			}
			else
			{
				SoundManager.instance.PlayNotAvailableSound();
			}
		}
		else if (btn.name == "UpgradeTankArmorButton")
		{
			if (GlobalCommons.Instance.globalGameStats.TankArmorLevel < maxArmorUpgradeLevel)
			{
				SoundManager.instance.PlayButtonClickSound();
				int itemCost2 = PlayerBalance.armorUpgradeCost[GlobalCommons.Instance.globalGameStats.TankArmorLevel];
				PurchaseConfirmDialog component2 = UnityEngine.Object.Instantiate(Prefabs.PurchaseConfirmDialog, Vector3.zero, Quaternion.identity).GetComponent<PurchaseConfirmDialog>();
				component2.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
				component2.InitializeDialog(itemCost2, -1, this, btn);
			}
			else
			{
				SoundManager.instance.PlayNotAvailableSound();
			}
		}
	}

	public void ConfirmTankStatUpgrade(Button btn)
	{
		if (btn.name == "UpgradeTankSpeedButton")
		{
			if (GlobalCommons.Instance.globalGameStats.TankSpeedLevel < maxSpeedUpgradeLevel)
			{
				int num = PlayerBalance.speedUpgradeCost[GlobalCommons.Instance.globalGameStats.TankSpeedLevel];
				if (GlobalCommons.Instance.globalGameStats.Money >= num)
				{
					GlobalCommons.Instance.globalGameStats.DecreaseMoney(num);
					GlobalCommons.Instance.globalGameStats.UpgradeTankSpeed();
				}
			}
		}
		else if (btn.name == "UpgradeTankArmorButton" && GlobalCommons.Instance.globalGameStats.TankArmorLevel < maxArmorUpgradeLevel)
		{
			int num2 = PlayerBalance.armorUpgradeCost[GlobalCommons.Instance.globalGameStats.TankArmorLevel];
			if (GlobalCommons.Instance.globalGameStats.Money >= num2)
			{
				GlobalCommons.Instance.globalGameStats.DecreaseMoney(num2);
				GlobalCommons.Instance.globalGameStats.UpgradeTankArmor();
			}
		}
		effectsSpawner.SpawnPurchaseEffect(btn.transform.position);
		UpdateUpgradeButtons();
	}

	public void ConfirmWeaponItemPurchase(Button btn)
	{
		PerfomrWeaponItemUpgrade(btn);
		effectsSpawner.SpawnPurchaseEffect(btn.transform.position);
		UpdateUpgradeButtons();
	}

	private void UpgradeWeaponButtonClick(Button btn)
	{
		if (btn.transform.parent != WeaponsScrollContentRT.transform)
		{
			btn.transform.SetParent(WeaponsScrollContentRT.transform, worldPositionStays: true);
		}
		upgradeMachinegunTutorActive = false;
		int num = Array.IndexOf(upgradeWeaponButtonNames, btn.name);
		WeaponTypes item = (WeaponTypes)num;
		if (PlayerBalance.LockedWeaponTypes.IndexOf(item) != -1)
		{
			SoundManager.instance.PlayNotAvailableSound();
			return;
		}
		int num2 = GlobalCommons.Instance.globalGameStats.WeaponsLevels[num];
		if (num2 < maxWeaponUpgradeLevel)
		{
			SoundManager.instance.PlayButtonClickSound();
			PurchaseConfirmDialog component = UnityEngine.Object.Instantiate(Prefabs.PurchaseConfirmDialog, Vector3.zero, Quaternion.identity).GetComponent<PurchaseConfirmDialog>();
			component.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
			component.InitializeDialog(PlayerBalance.WeaponsUpgradesCost[num][num2], num, this, btn);
		}
		else
		{
			SoundManager.instance.PlayNotAvailableSound();
		}
	}

	private void PerfomrWeaponItemUpgrade(Button btn)
	{
		int num = Array.IndexOf(upgradeWeaponButtonNames, btn.name);
		int num2 = GlobalCommons.Instance.globalGameStats.WeaponsLevels[num];
		int availableWeaponsCount = GlobalCommons.Instance.globalGameStats.AvailableWeaponsCount;
		if (num2 >= maxWeaponUpgradeLevel)
		{
			return;
		}
		int num3 = PlayerBalance.WeaponsUpgradesCost[num][num2];
		if (GlobalCommons.Instance.globalGameStats.Money >= num3)
		{
			GlobalCommons.Instance.globalGameStats.DecreaseMoney(num3);
			GlobalCommons.Instance.globalGameStats.WeaponsLevels[num] = GlobalCommons.Instance.globalGameStats.WeaponsLevels[num] + 1;
			if (availableWeaponsCount == 1 && GlobalCommons.Instance.globalGameStats.AvailableWeaponsCount == 2)
			{
				GlobalCommons.Instance.globalGameStats.WeaponsTutorialPending = true;
			}
		}
	}

	private void UpdateUpgradeButtons()
	{
		for (int i = 0; i < allUpgradeButtons.Count; i++)
		{
			UpdateUpgradeButton(allUpgradeButtons[i]);
		}
	}

	private void AddButtonPurchaseIndicator(Button btn, Vector2 shift)
	{
		if (!upgradeArmorTutorActive)
		{
			Image component = UnityEngine.Object.Instantiate(Prefabs.ItemPurchaseAvailableIndicator, btn.transform.position, Quaternion.identity).GetComponent<Image>();
			component.name = purchaseIndicatorName;
			Transform transform = component.transform;
			Vector3 localScale = btn.transform.localScale;
			float x = 1f / localScale.x;
			Vector3 localScale2 = btn.transform.localScale;
			float y = 1f / localScale2.y;
			Vector3 localScale3 = component.transform.localScale;
			transform.localScale = new Vector3(x, y, localScale3.z);
			component.transform.SetParent(btn.transform, worldPositionStays: false);
			component.rectTransform.anchoredPosition = new Vector2(shift.x, shift.y);
		}
	}

	private void UpdateUpgradeButton(Button btn, bool initialUpdate = false)
	{
		bool flag = false;
		Vector3? vector = null;
		bool flag2 = false;
		Transform transform = btn.transform.Find(purchaseIndicatorName);
		if ((bool)transform)
		{
			vector = transform.transform.position;
			UnityEngine.Object.Destroy(transform.gameObject);
		}
		int num = Array.IndexOf(upgradeWeaponButtonNames, btn.name);
		if (num != -1)
		{
			WeaponTypes item = (WeaponTypes)num;
			int num2 = GlobalCommons.Instance.globalGameStats.WeaponsLevels[num];
			int num3 = (num2 < maxWeaponUpgradeLevel) ? PlayerBalance.WeaponsUpgradesCost[num][num2] : 0;
			bool flag3 = num3 <= GlobalCommons.Instance.globalGameStats.Money;
			IEnumerator enumerator = btn.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform transform2 = (Transform)enumerator.Current;
					if (transform2.name == "levelIndicator")
					{
						UnityEngine.Object.Destroy(transform2.gameObject);
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
			for (int i = 0; i < maxWeaponUpgradeLevel; i++)
			{
				Image image = (num2 <= i) ? UnityEngine.Object.Instantiate(Prefabs.WeaponLevelIndicatorInactive, Vector3.zero, Quaternion.identity).GetComponent<Image>() : UnityEngine.Object.Instantiate(Prefabs.WeaponLevelIndicator, Vector3.zero, Quaternion.identity).GetComponent<Image>();
				image.transform.SetParent(btn.transform, worldPositionStays: false);
				image.name = "levelIndicator";
				image.GetComponent<RectTransform>().anchoredPosition = new Vector2(57.5f, -35f + (float)i * 15.8f);
			}
			if (num2 == 0)
			{
				Image image2 = (!flag3) ? UnityEngine.Object.Instantiate(Prefabs.UnpurchasedWeaponCover, Vector3.zero, Quaternion.identity).GetComponent<Image>() : UnityEngine.Object.Instantiate(Prefabs.UnpurchasedWeaponCoverNew, Vector3.zero, Quaternion.identity).GetComponent<Image>();
				image2.transform.SetParent(btn.transform, worldPositionStays: false);
				image2.name = "levelIndicator";
				image2.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				float num4 = 0.95f;
				Transform transform3 = image2.transform;
				float x = num4;
				float y = num4;
				Vector3 localScale = image2.transform.localScale;
				transform3.localScale = new Vector3(x, y, localScale.z);
			}
			string text = "MAX";
			if (num2 < maxWeaponUpgradeLevel)
			{
				text = num3.ToString();
				if (flag3 && PlayerBalance.LockedWeaponTypes.IndexOf(item) == -1)
				{
					AddButtonPurchaseIndicator(btn, new Vector2(-60f, 60f));
					flag2 = true;
				}
			}
			RectTransform component = btn.transform.Find("CostText").GetComponent<RectTransform>();
			btn.transform.Find("CostText").GetComponent<Text>().text = text;
			Transform transform4 = btn.transform.Find("UnavailableCover");
			if (PlayerBalance.LockedWeaponTypes.IndexOf(item) != -1)
			{
				Image component2 = transform4.gameObject.GetComponent<Image>();
				component2.transform.SetAsLastSibling();
			}
			else if (transform4 != null)
			{
				UnityEngine.Object.Destroy(transform4.gameObject);
			}
		}
		else
		{
			flag = true;
			int num5 = 0;
			switch (btn.name)
			{
			case "UpgradeTankSpeedButton":
			{
				string text2 = "MAX";
				if (GlobalCommons.Instance.globalGameStats.TankSpeedLevel < maxSpeedUpgradeLevel)
				{
					int num7 = PlayerBalance.speedUpgradeCost[GlobalCommons.Instance.globalGameStats.TankSpeedLevel];
					text2 = num7.ToString();
					if (num7 <= GlobalCommons.Instance.globalGameStats.Money)
					{
						AddButtonPurchaseIndicator(btn, new Vector2(-35f, 50f));
						flag2 = true;
					}
				}
				btn.transform.Find("CostText").GetComponent<Text>().text = text2;
				num5 = GlobalCommons.Instance.globalGameStats.TankSpeedLevel;
				break;
			}
			case "UpgradeTankArmorButton":
			{
				string text2 = "MAX";
				if (GlobalCommons.Instance.globalGameStats.TankArmorLevel < maxArmorUpgradeLevel)
				{
					int num6 = PlayerBalance.armorUpgradeCost[GlobalCommons.Instance.globalGameStats.TankArmorLevel];
					text2 = num6.ToString();
					if (num6 <= GlobalCommons.Instance.globalGameStats.Money)
					{
						AddButtonPurchaseIndicator(btn, new Vector2(-35f, 50f));
						flag2 = true;
					}
				}
				btn.transform.Find("CostText").GetComponent<Text>().text = text2;
				num5 = GlobalCommons.Instance.globalGameStats.TankArmorLevel;
				break;
			}
			}
			IEnumerator enumerator2 = btn.transform.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					Transform transform5 = (Transform)enumerator2.Current;
					if (transform5.name == "levelIndicator")
					{
						UnityEngine.Object.Destroy(transform5.gameObject);
					}
				}
			}
			finally
			{
				IDisposable disposable2;
				if ((disposable2 = (enumerator2 as IDisposable)) != null)
				{
					disposable2.Dispose();
				}
			}
			float num8 = 0.58f;
			if (btn.name != "UpgradeTankArmorButton")
			{
				for (int j = 0; j < maxSpeedUpgradeLevel; j++)
				{
					Image image3 = (num5 <= j) ? UnityEngine.Object.Instantiate(Prefabs.TankLevelIndicatorInactive, Vector3.zero, Quaternion.identity).GetComponent<Image>() : UnityEngine.Object.Instantiate(Prefabs.TankLevelIndicator, Vector3.zero, Quaternion.identity).GetComponent<Image>();
					image3.transform.SetParent(btn.transform, worldPositionStays: false);
					image3.name = "levelIndicator";
					Transform transform6 = image3.transform;
					float x2 = num8;
					float y2 = num8;
					Vector3 localScale2 = image3.transform.localScale;
					transform6.localScale = new Vector3(x2, y2, localScale2.z);
					float num9 = 0f;
					int num10 = j;
					if (j > 4)
					{
						num10 = j - 5;
						num9 = 1f;
					}
					image3.GetComponent<RectTransform>().anchoredPosition = new Vector2(-19.4f + (float)num10 * 10.45f, -39f - num9 * 11f);
				}
			}
			else
			{
				for (int k = 0; k < maxArmorUpgradeLevel; k++)
				{
					Image image4 = (num5 <= k) ? UnityEngine.Object.Instantiate(Prefabs.TankLevelIndicatorInactive, Vector3.zero, Quaternion.identity).GetComponent<Image>() : UnityEngine.Object.Instantiate(Prefabs.TankLevelIndicator, Vector3.zero, Quaternion.identity).GetComponent<Image>();
					image4.transform.SetParent(btn.transform, worldPositionStays: false);
					image4.name = "levelIndicator";
					Transform transform7 = image4.transform;
					float x3 = num8;
					float y3 = num8;
					Vector3 localScale3 = image4.transform.localScale;
					transform7.localScale = new Vector3(x3, y3, localScale3.z);
					float num11 = 0f;
					int num12 = k;
					num12 = k - 5 * Mathf.FloorToInt(k / 5);
					num11 = Mathf.FloorToInt(k / 5);
					image4.GetComponent<RectTransform>().anchoredPosition = new Vector2(-20.3f + (float)num12 * 10.45f, -33f - num11 * 11f);
				}
			}
		}
		if (vector.HasValue && !flag2)
		{
			Vector3[] array = new Vector3[4];
			WeaponsScroll.GetComponent<RectTransform>().GetWorldCorners(array);
			Vector3 value = vector.Value;
			if (flag || (value.x >= array[0].x && value.x <= array[2].x && value.y >= array[0].y && value.y <= array[1].y))
			{
				effectsSpawner.SpawnUpgradeAvailableIndicatorFalloff(vector.Value);
			}
		}
	}

	private void MenuButtonClick()
	{
		RectTransform component = menuButton.GetComponent<RectTransform>();
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
		effectsSpawner.DisableAllParticles();
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("MainMenu");
	}

	private void MonehButtonClick()
	{
		GlobalCommons.Instance.globalGameStats.IncreaseMoney(100500);
		UpdateUpgradeButtons();
		SaveGame();
	}

	private void PlayButtonClick()
	{
		RectTransform component = playButton.GetComponent<RectTransform>();
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
		effectsSpawner.DisableAllParticles();
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("LevelSelection");
	}

	private void AchievementsButtonClick()
	{
		RectTransform component = achievementsButton.GetComponent<RectTransform>();
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
		effectsSpawner.DisableAllParticles();
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("AchievementsMenu");
	}

	private void ShopButtonClick()
	{
		RectTransform component = ShopButton.GetComponent<RectTransform>();
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
		effectsSpawner.DisableAllParticles();
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("ShopScene");
	}

	private void PrizeButtonClick()
	{
		if ((GlobalBalance.GetTimeLeftForPrize() < TimeSpan.Zero) ? true : false)
		{
			RectTransform component = PrizeButton.GetComponent<RectTransform>();
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
			effectsSpawner.DisableAllParticles();
			GlobalCommons.Instance.StateFaderController.ChangeSceneTo("PrizeScene");
		}
		else
		{
			SoundManager.instance.PlayNotAvailableSound();
		}
	}

	public void GameCenterButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();

		//switch (Application.platform)
		//{
		//case RuntimePlatform.Android:
		//	GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("LogInToText") + "Google Play Services" + Environment.NewLine + LocalizationManager.Instance.GetLocalizedText("ToViewLeaderboardsText"), 0.8f);
		//	break;
		//case RuntimePlatform.IPhonePlayer:
		//	GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("LogInToText") + "Game Center" + Environment.NewLine + LocalizationManager.Instance.GetLocalizedText("ToViewLeaderboardsText"), 0.8f);
		//	break;
		//default:
		//	GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("LogInText") + Environment.NewLine + LocalizationManager.Instance.GetLocalizedText("ToViewLeaderboardsText"), 0.8f);
		//	break;
		//}
		//GooglePlayGamesManager.Instance.LeaderBoardPostScore("CgkIlumC0I8GEAIQAA", currentMoneyVal);

		LeaderboardUI.SetActive(false);
	}

	public void OpenLeaderboardWindow()
	{
		LeaderboardUI.SetActive(true);
	}

	public void OpenRate()
	{
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("AskForReviewScene");

	}
}
