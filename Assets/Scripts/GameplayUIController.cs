using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
	private Image playerHPBar;

	private Image[] playerHPBars;

	private Image PlayerAmmoBar;

	private Image[] playerAmmoBars;

	private Image FreezeBonusBG;

	private Image FreezeBonusTimeBar;

	private Image FreezeBonusIcon;

	private Image InvisibilityBonusBG;

	private Image InvisibilityBonusTimeBar;

	private Image InvisibilityBonusIcon;

	private Image DoubleDamageBonusBG;

	private Image DoubleDamageBonusTimeBar;

	private Image DoubleDamageBonusIcon;

	private Button pauseButton;

	private Image pauseButtonImage;

	private Image screenFlashImage;

	private Image ComboTextBG;

	private Text comboText;

	private Image ProfitTextBG;

	private Text profitText;

	private Image PlayerHiddenImage;

	private int profitTextShakeFactor;

	private Vector3 profitTextDefaultLocalPosition;

	private float profitTextDisplayTimestamp = -100f;

	private float profitShowTimeout = 2f;

	private int comboTextShakeFactor;

	private Vector3 comboTextDefaultLocalPosition;

	private float screenFlashFactor;

	private float screenFlashFactorDecrease = 5f;

	private CanvasGroup upperUIItems;

	private float dimmedUIGroupAlphaVal = 0.33f;

	private float uiGroupAlphaChangeSpeed = 4f;

	internal Image BinocularsButton;

	private Vector2 binocularsButtonTargetPosition;

	private Vector2 binocularsButtonShiftedPosition;

	private bool binocularsButtonOutComplete;

	private float binocularsButtonStartTransitionTimestamp;

	private float binocularsButtonTransitionTime = 0.4f;

	private float binocularsButtonActivationDelay = 0.5f;

	private bool waitForWeaponsTutorial;

	internal Image WeaponSelectButton;

	private Vector2 weaponSelectButtonTargetPosition;

	private Vector2 weaponSelectButtonShiftedPosition;

	private bool weaponSelectButtonOutComplete;

	private bool weaponTutorialComplete;

	private float weaponSelectButtonStartTransitionTimestamp;

	private float weaponSelectButtonTransitionTime = 0.4f;

	private float weaponSelectButtonActivationDelay = 0.25f;

	private CanvasGroup bottomUIItems;

	private Vector2 bottomUIItemsTargetPosition;

	private Vector2 bottomUIItemsShiftedPosition;

	private bool bottomUIItemsOutComplete;

	private float bottomUIItemsStartTransitionTimestamp;

	private float bottomUIItemsTransitionTime = 0.4f;

	private float bottomUIItemsActivationDelay = 0.75f;

	private bool allUIItemsOutComplete;

	private Image WeaponSelectionTutorialImage;

	private Text WeaponSelectText;

	public Sprite[] weaponSelectButtonSprites;

	private float targetPlayerHPPercentage = 1f;

	private float targetPlayerAmmoPercentage = 1f;

	private long updateIndex;

	internal float UpperButtonsScale;

	private int multipleBarsCount = 10;

	private bool useMultipleBars;

	private float BGPlatesMaxAlpha = 0.25f;

	public bool BinucularsEnabled => binocularsButtonOutComplete;

	public bool WeaponsSelectEnabled => weaponSelectButtonOutComplete;

	private float ComboTextAlpha
	{
		get
		{
			Color color = comboText.color;
			return color.a;
		}
		set
		{
			Color color = comboText.color;
			if (color.a != value)
			{
				Text text = comboText;
				Color color2 = comboText.color;
				float r = color2.r;
				Color color3 = comboText.color;
				float g = color3.g;
				Color color4 = comboText.color;
				text.color = new Color(r, g, color4.b, value);
			}
			bool flag = (value != 0f) ? true : false;
			if (comboText.enabled != flag)
			{
				comboText.enabled = flag;
				ComboTextBG.enabled = flag;
			}
			Color color5 = comboText.color;
			float num = color5.a;
			if (num > BGPlatesMaxAlpha)
			{
				num = BGPlatesMaxAlpha;
			}
			ComboTextBG.SetAlpha(num);
		}
	}

	private float ProfitTextAlpha
	{
		get
		{
			Color color = profitText.color;
			return color.a;
		}
		set
		{
			Color color = profitText.color;
			if (color.a != value)
			{
				Text text = profitText;
				Color color2 = profitText.color;
				float r = color2.r;
				Color color3 = profitText.color;
				float g = color3.g;
				Color color4 = profitText.color;
				text.color = new Color(r, g, color4.b, value);
			}
			bool flag = (value != 0f) ? true : false;
			if (profitText.enabled != flag)
			{
				profitText.enabled = flag;
				ProfitTextBG.enabled = flag;
			}
			float num = value;
			if (num > BGPlatesMaxAlpha)
			{
				num = BGPlatesMaxAlpha;
			}
			ProfitTextBG.SetAlpha(num);
		}
	}

	private void Start()
	{
		if (Screen.height >= 1080)
		{
			useMultipleBars = true;
		}
		WeaponSelectionTutorialImage = GameObject.Find("WeaponSelectionTutorialImage").GetComponent<Image>();
		if (GlobalCommons.Instance.globalGameStats.WeaponsTutorialPending)
		{
			WeaponSelectText = WeaponSelectionTutorialImage.transform.Find("WeaponSelectText").GetComponent<Text>();
			WeaponSelectText.SetAlpha(0f);
			WeaponSelectionTutorialImage.SetAlpha(0f);
			WeaponSelectionTutorialImage.enabled = false;
			WeaponSelectText.enabled = false;
		}
		else
		{
			UnityEngine.Object.Destroy(WeaponSelectionTutorialImage.gameObject);
		}
		BinocularsButton = GameObject.Find("BinocularsButton").GetComponent<Image>();
		BinocularsButton.enabled = false;
		WeaponSelectButton = GameObject.Find("WeaponSelectButton").GetComponent<Image>();
		WeaponSelectButton.enabled = false;
		PositionUpperButtons();
		screenFlashImage = GameObject.Find("ScreenFlashImage").GetComponent<Image>();
		screenFlashImage.SetAlpha(0.01f);
		PlayerHiddenImage = GameObject.Find("PlayerHiddenImage").GetComponent<Image>();
		PlayerHiddenImage.enabled = false;
		upperUIItems = GameObject.Find("UpperUIItems").GetComponent<CanvasGroup>();
		playerHPBar = GameObject.Find("PlayerHPBar").GetComponent<Image>();
		PlayerAmmoBar = GameObject.Find("PlayerAmmoBar").GetComponent<Image>();
		if (useMultipleBars)
		{
			playerHPBar.enabled = false;
			PlayerAmmoBar.enabled = false;
			playerHPBars = FillBars(playerHPBar, playerHPBars);
			playerAmmoBars = FillBars(PlayerAmmoBar, playerAmmoBars);
		}
		pauseButton = GameObject.Find("PauseButton").GetComponent<Button>();
		pauseButton.onClick.AddListener(delegate
		{
			PauseButtonClick();
		});
		pauseButtonImage = pauseButton.GetComponent<Image>();
		FreezeBonusTimeBar = GameObject.Find("FreezeBonusTimeBar").GetComponent<Image>();
		FreezeBonusIcon = GameObject.Find("FreezeBonusIcon").GetComponent<Image>();
		FreezeBonusBG = GameObject.Find("FreezeBonusBG").GetComponent<Image>();
		FreezeBonusTimeBar.gameObject.SetActive(value: false);
		FreezeBonusIcon.gameObject.SetActive(value: false);
		FreezeBonusBG.gameObject.SetActive(value: false);
		DoubleDamageBonusTimeBar = GameObject.Find("DoubleDamageBonusTimeBar").GetComponent<Image>();
		DoubleDamageBonusIcon = GameObject.Find("DoubleDamageBonusIcon").GetComponent<Image>();
		DoubleDamageBonusBG = GameObject.Find("DoubleDamageBonusBG").GetComponent<Image>();
		DoubleDamageBonusTimeBar.gameObject.SetActive(value: false);
		DoubleDamageBonusIcon.gameObject.SetActive(value: false);
		DoubleDamageBonusBG.gameObject.SetActive(value: false);
		InvisibilityBonusTimeBar = GameObject.Find("InvisibilityBonusTimeBar").GetComponent<Image>();
		InvisibilityBonusIcon = GameObject.Find("InvisibilityBonusIcon").GetComponent<Image>();
		InvisibilityBonusBG = GameObject.Find("InvisibilityBonusBG").GetComponent<Image>();
		InvisibilityBonusTimeBar.gameObject.SetActive(value: false);
		InvisibilityBonusIcon.gameObject.SetActive(value: false);
		InvisibilityBonusBG.gameObject.SetActive(value: false);
		ComboTextBG = GlobalCommons.Instance.CanvasBoundsController.UIRectTransform.Find("ComboTextBG").GetComponent<Image>();
		comboText = GlobalCommons.Instance.CanvasBoundsController.UIRectTransform.Find("ComboText").GetComponent<Text>();
		comboTextDefaultLocalPosition = comboText.rectTransform.anchoredPosition;
		comboText.enabled = false;
		ComboTextAlpha = 0f;
		ProfitTextBG = GlobalCommons.Instance.CanvasBoundsController.UIRectTransform.Find("ProfitTextBG").GetComponent<Image>();
		profitText = GlobalCommons.Instance.CanvasBoundsController.UIRectTransform.Find("ProfitText").GetComponent<Text>();
		profitTextDefaultLocalPosition = profitText.rectTransform.anchoredPosition;
		profitText.enabled = false;
		ProfitTextAlpha = 0f;
		bottomUIItems = GameObject.Find("BottomUIItems").GetComponent<CanvasGroup>();
		bottomUIItems.gameObject.SetActive(value: false);
		if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.TutorialLevel)
		{
			bottomUIItemsActivationDelay = float.MaxValue;
			binocularsButtonActivationDelay = float.MaxValue;
			weaponSelectButtonActivationDelay = float.MaxValue;
		}
		else if (GlobalCommons.Instance.globalGameStats.AvailableWeaponsCount == 1)
		{
			weaponSelectButtonActivationDelay = float.MaxValue;
		}
		UpdateWeaponSelectButtonFrame((int)GameplayCommons.Instance.weaponsController.SelectedWeapon);
	}

	private Image[] FillBars(Image referenceBar, Image[] barsArray)
	{
		Vector2 sizeDelta = referenceBar.rectTransform.sizeDelta;
		float x = sizeDelta.x;
		float num = x / (float)multipleBarsCount;
		float num2 = num * 0.7f;
		float num3 = num - num2;
		float num4 = num + num3 / (float)(multipleBarsCount + 1);
		barsArray = new Image[multipleBarsCount];
		for (int i = 0; i < multipleBarsCount; i++)
		{
			Image component = UnityEngine.Object.Instantiate(referenceBar.gameObject).GetComponent<Image>();
			component.enabled = true;
			component.gameObject.name = referenceBar.name + "SmallBar" + i.ToString();
			component.transform.SetParent(referenceBar.transform.parent, worldPositionStays: false);
			RectTransform rectTransform = component.rectTransform;
			float x2 = num2;
			Vector2 sizeDelta2 = component.rectTransform.sizeDelta;
			rectTransform.sizeDelta = new Vector2(x2, sizeDelta2.y);
			RectTransform rectTransform2 = component.rectTransform;
			Vector2 anchoredPosition = component.rectTransform.anchoredPosition;
			float x3 = anchoredPosition.x + (float)i * num4;
			Vector2 anchoredPosition2 = component.rectTransform.anchoredPosition;
			rectTransform2.anchoredPosition = new Vector2(x3, anchoredPosition2.y);
			barsArray[i] = component;
		}
		return barsArray;
	}

	private void PositionUpperButtons()
	{
		float num = (float)Screen.height / Screen.dpi;
		Vector2 referenceResolution = GameObject.Find("Canvas").GetComponent<CanvasScaler>().referenceResolution;
		float y = referenceResolution.y;
		DebugHelper.Log("asdf: " + WeaponSelectButton.rectTransform.rect.height);
		DebugHelper.Log("asdf: " + y);
		float num2 = 3.2f * Screen.dpi;
		float num3 = num2 / (float)Screen.height;
		float num4 = y - num3 * y;
		DebugHelper.Log("asdf: " + num4);
		float num5 = num4;
		Vector2 anchoredPosition = WeaponSelectButton.rectTransform.anchoredPosition;
		if (num5 > anchoredPosition.y)
		{
			RectTransform rectTransform = WeaponSelectButton.rectTransform;
			Vector2 anchoredPosition2 = WeaponSelectButton.rectTransform.anchoredPosition;
			rectTransform.anchoredPosition = new Vector2(anchoredPosition2.x, 0f - num4);
			RectTransform rectTransform2 = BinocularsButton.rectTransform;
			Vector2 anchoredPosition3 = BinocularsButton.rectTransform.anchoredPosition;
			rectTransform2.anchoredPosition = new Vector2(anchoredPosition3.x, 0f - num4);
		}
		UpperButtonsScale = 1f - (num - 2.48f) * 0.2f;
		UpperButtonsScale = Mathf.Clamp(UpperButtonsScale, 0.725f, 1f);
		Vector2 a = new Vector2(WeaponSelectButton.rectTransform.rect.width, 0f);
		Transform transform = WeaponSelectButton.transform;
		float upperButtonsScale = UpperButtonsScale;
		float upperButtonsScale2 = UpperButtonsScale;
		Vector3 localScale = WeaponSelectButton.transform.localScale;
		transform.localScale = new Vector3(upperButtonsScale, upperButtonsScale2, localScale.z);
		WeaponSelectButton.rectTransform.anchoredPosition = WeaponSelectButton.rectTransform.anchoredPosition - a * (1f - UpperButtonsScale) / 2f;
		a = new Vector2(BinocularsButton.rectTransform.rect.width, 0f);
		Transform transform2 = BinocularsButton.transform;
		float upperButtonsScale3 = UpperButtonsScale;
		float upperButtonsScale4 = UpperButtonsScale;
		Vector3 localScale2 = BinocularsButton.transform.localScale;
		transform2.localScale = new Vector3(upperButtonsScale3, upperButtonsScale4, localScale2.z);
		BinocularsButton.rectTransform.anchoredPosition = BinocularsButton.rectTransform.anchoredPosition + a * (1f - UpperButtonsScale) / 2f;
	}

	public void UpdateWeaponSelectButtonFrame(int index)
	{
		WeaponSelectButton.sprite = weaponSelectButtonSprites[index];
	}

	public void ShowBinocularsButton()
	{
		binocularsButtonActivationDelay = 0f;
	}

	public bool CheckPauseButtonHit(Vector2 coords)
	{
		return RectTransformUtility.RectangleContainsScreenPoint(pauseButtonImage.rectTransform, coords, Camera.main);
	}

	private void PauseButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		GameplayCommons.Instance.TogglePause();
	}

	private void SetFlasherAlpha(float val)
	{
		Image image = screenFlashImage;
		Color color = screenFlashImage.color;
		float r = color.r;
		Color color2 = screenFlashImage.color;
		float g = color2.g;
		Color color3 = screenFlashImage.color;
		image.color = new Color(r, g, color3.b, val);
	}

	private void ProcessBinocularsOut()
	{
		if (GameplayCommons.Instance.playersTankController.PlayerActive && !(Time.fixedTime < GameplayCommons.Instance.playerActivationTime + binocularsButtonActivationDelay) && !BinocularsButton.enabled)
		{
			BinocularsButton.enabled = true;
			BinocularsButton.SetAlpha(0f);
			binocularsButtonTargetPosition = BinocularsButton.rectTransform.anchoredPosition;
			RectTransform rectTransform = BinocularsButton.rectTransform;
			Vector2 anchoredPosition = BinocularsButton.rectTransform.anchoredPosition;
			float x = anchoredPosition.x + (float)(Screen.width / 15);
			Vector2 anchoredPosition2 = BinocularsButton.rectTransform.anchoredPosition;
			rectTransform.anchoredPosition = new Vector2(x, anchoredPosition2.y + (float)(Screen.height / 15));
			BinocularsButton.rectTransform.DOAnchorPos(binocularsButtonTargetPosition, binocularsButtonTransitionTime);
			BinocularsButton.DOFade(1f, binocularsButtonTransitionTime).OnCompleteWithCoroutine(FinalizeBinocularsOut);
		}
	}

	private void FinalizeBinocularsOut()
	{
		binocularsButtonOutComplete = true;
	}

	private void ProcessWeaponsButtonOut()
	{
		if (GameplayCommons.Instance.playersTankController.PlayerActive && !(Time.fixedTime < GameplayCommons.Instance.playerActivationTime + weaponSelectButtonActivationDelay) && !WeaponSelectButton.enabled)
		{
			WeaponSelectButton.enabled = true;
			if (GlobalCommons.Instance.globalGameStats.WeaponsTutorialPending)
			{
				WeaponSelectText.enabled = true;
				WeaponSelectText.DOFade(1f, 0.25f);
				WeaponSelectionTutorialImage.enabled = true;
				WeaponSelectionTutorialImage.DOFade(1f, 0.25f);
				waitForWeaponsTutorial = true;
			}
			WeaponSelectButton.SetAlpha(0f);
			weaponSelectButtonTargetPosition = WeaponSelectButton.rectTransform.anchoredPosition;
			RectTransform rectTransform = WeaponSelectButton.rectTransform;
			Vector2 anchoredPosition = WeaponSelectButton.rectTransform.anchoredPosition;
			float x = anchoredPosition.x - (float)(Screen.width / 15);
			Vector2 anchoredPosition2 = WeaponSelectButton.rectTransform.anchoredPosition;
			rectTransform.anchoredPosition = new Vector2(x, anchoredPosition2.y + (float)(Screen.height / 15));
			WeaponSelectButton.rectTransform.DOAnchorPos(weaponSelectButtonTargetPosition, weaponSelectButtonTransitionTime);
			WeaponSelectButton.DOFade(1f, weaponSelectButtonTransitionTime).OnCompleteWithCoroutine(FinalizeWeaponsButtonOut);
		}
	}

	private void FinalizeWeaponsButtonOut()
	{
		weaponSelectButtonOutComplete = true;
	}

	private void ProcessBottomUIItemsOut()
	{
		if (GameplayCommons.Instance.playersTankController.PlayerActive && !(Time.fixedTime < GameplayCommons.Instance.playerActivationTime + bottomUIItemsActivationDelay))
		{
			RectTransform component = bottomUIItems.GetComponent<RectTransform>();
			if (!bottomUIItems.gameObject.activeInHierarchy)
			{
				bottomUIItems.gameObject.SetActive(value: true);
				bottomUIItems.alpha = 0f;
				bottomUIItemsTargetPosition = component.anchoredPosition;
				RectTransform rectTransform = component;
				Vector2 anchoredPosition = component.anchoredPosition;
				float x = anchoredPosition.x;
				Vector2 anchoredPosition2 = component.anchoredPosition;
				rectTransform.anchoredPosition = new Vector2(x, anchoredPosition2.y - (float)(Screen.height / 15));
				component.DOAnchorPos(bottomUIItemsTargetPosition, bottomUIItemsTransitionTime);
				bottomUIItems.DOFade(1f, bottomUIItemsTransitionTime).OnCompleteWithCoroutine(FinalizeBottomUIItemsOut);
			}
		}
	}

	private void FinalizeBottomUIItemsOut()
	{
		bottomUIItemsOutComplete = true;
	}

	public void UpdatePlayerHP(float playerHPPercentage)
	{
		if (useMultipleBars)
		{
			float num = 0f;
			float num2 = 1f / (float)multipleBarsCount;
			float num3 = 0f;
			float num4 = 0.05f;
			for (int i = 0; i < multipleBarsCount; i++)
			{
				Image image = playerHPBars[i];
				bool flag = false;
				if (playerHPPercentage > 0f && playerHPPercentage >= num)
				{
					flag = true;
				}
				if (image.enabled != flag)
				{
					if (image.enabled)
					{
						GameplayCommons.Instance.effectsSpawner.SpawnOverUIHPBarFlyoffEffect(image.transform.position);
					}
					image.enabled = flag;
					if (image.enabled)
					{
						image.transform.DOKill();
						Transform transform = image.transform;
						Vector3 localScale = image.transform.localScale;
						float x = localScale.x;
						Vector3 localScale2 = image.transform.localScale;
						transform.localScale = new Vector3(x, 0f, localScale2.z);
						image.transform.DOScaleY(1f, 0.1f).SetUpdate(isIndependentUpdate: true).SetDelay(num3);
						num3 += num4;
					}
				}
				num += num2;
			}
		}
		else
		{
			targetPlayerHPPercentage = playerHPPercentage;
		}
	}

	public void UpdatePlayerAmmo(float ammoPercentage)
	{
		if (useMultipleBars)
		{
			float num = 0f;
			float num2 = 1f / (float)multipleBarsCount;
			float num3 = 0f;
			float num4 = 0.05f;
			for (int i = 0; i < multipleBarsCount; i++)
			{
				Image image = playerAmmoBars[i];
				bool flag = false;
				if (ammoPercentage > 0f && ammoPercentage >= num)
				{
					flag = true;
				}
				if (image.enabled != flag)
				{
					if (image.enabled)
					{
						GameplayCommons.Instance.effectsSpawner.SpawnOverUIAmmoBarFlyoffEffect(image.transform.position);
					}
					image.enabled = flag;
					if (image.enabled)
					{
						image.transform.DOKill();
						Transform transform = image.transform;
						Vector3 localScale = image.transform.localScale;
						float x = localScale.x;
						Vector3 localScale2 = image.transform.localScale;
						transform.localScale = new Vector3(x, 0f, localScale2.z);
						image.transform.DOScaleY(1f, 0.1f).SetUpdate(isIndependentUpdate: true).SetDelay(num3);
						num3 += num4;
					}
				}
				num += num2;
			}
		}
		else
		{
			targetPlayerAmmoPercentage = ammoPercentage;
		}
	}

	private void UpdateAmmoAndHPBars()
	{
		Vector3 localScale = playerHPBar.transform.localScale;
		if (localScale.x != targetPlayerHPPercentage)
		{
			Transform transform = playerHPBar.transform;
			Vector3 localScale2 = playerHPBar.transform.localScale;
			float x = FMath.MoveFloatStepClamp(localScale2.x, targetPlayerHPPercentage, 0.033f);
			Vector3 localScale3 = playerHPBar.transform.localScale;
			float y = localScale3.y;
			Vector3 localScale4 = playerHPBar.transform.localScale;
			transform.localScale = new Vector3(x, y, localScale4.z);
		}
		Vector3 localScale5 = PlayerAmmoBar.transform.localScale;
		if (localScale5.x != targetPlayerAmmoPercentage)
		{
			Transform transform2 = PlayerAmmoBar.transform;
			Vector3 localScale6 = PlayerAmmoBar.transform.localScale;
			float x2 = FMath.MoveFloatStepClamp(localScale6.x, targetPlayerAmmoPercentage, 0.033f);
			Vector3 localScale7 = PlayerAmmoBar.transform.localScale;
			float y2 = localScale7.y;
			Vector3 localScale8 = PlayerAmmoBar.transform.localScale;
			transform2.localScale = new Vector3(x2, y2, localScale8.z);
		}
	}

	private void Update()
	{
		updateIndex++;
		if (updateIndex == 2)
		{
			screenFlashImage.SetAlpha(1f);
			screenFlashImage.enabled = false;
		}
		if (GameplayCommons.Instance.playersTankController == null)
		{
			return;
		}
		if (!useMultipleBars)
		{
			UpdateAmmoAndHPBars();
		}
		if (screenFlashFactor > 0f)
		{
			if (!screenFlashImage.enabled)
			{
				screenFlashImage.enabled = true;
			}
			SetFlasherAlpha(screenFlashFactor);
			screenFlashFactor = ((Time.timeScale != 1f) ? (screenFlashFactor - 0.083f) : (screenFlashFactor - screenFlashFactorDecrease * Time.deltaTime));
			if (screenFlashFactor < 0f)
			{
				screenFlashFactor = 0f;
			}
		}
		else if (screenFlashImage.enabled)
		{
			screenFlashImage.enabled = false;
		}
		if (GameplayCommons.Instance.levelStateController.IsDoubleDamageActive)
		{
			if (!DoubleDamageBonusTimeBar.gameObject.activeInHierarchy)
			{
				DoubleDamageBonusTimeBar.gameObject.SetActive(value: true);
				DoubleDamageBonusIcon.gameObject.SetActive(value: true);
				DoubleDamageBonusBG.gameObject.SetActive(value: true);
			}
			Transform transform = DoubleDamageBonusTimeBar.transform;
			float activeBonusPercentage = GameplayCommons.Instance.levelStateController.GetActiveBonusPercentage(BonusController.BonusType.doubleDamageBonus);
			Vector3 localScale = DoubleDamageBonusTimeBar.transform.localScale;
			float y = localScale.y;
			Vector3 localScale2 = DoubleDamageBonusTimeBar.transform.localScale;
			transform.localScale = new Vector3(activeBonusPercentage, y, localScale2.z);
		}
		else if (DoubleDamageBonusTimeBar.gameObject.activeInHierarchy)
		{
			if (!GameplayCommons.Instance.levelStateController.GameplayStopped)
			{
				GameplayCommons.Instance.effectsSpawner.CreateFlareEffect(DoubleDamageBonusBG.transform.position);
				if (!GameplayCommons.Instance.levelResultsController.IsEnabled)
				{
					SoundManager.instance.PlayBonusWearoffSound();
				}
			}
			DoubleDamageBonusTimeBar.gameObject.SetActive(value: false);
			DoubleDamageBonusIcon.gameObject.SetActive(value: false);
			DoubleDamageBonusBG.gameObject.SetActive(value: false);
		}
		if (GameplayCommons.Instance.levelStateController.IsFreezeActive)
		{
			if (!FreezeBonusTimeBar.gameObject.activeInHierarchy)
			{
				FreezeBonusTimeBar.gameObject.SetActive(value: true);
				FreezeBonusIcon.gameObject.SetActive(value: true);
				FreezeBonusBG.gameObject.SetActive(value: true);
			}
			Transform transform2 = FreezeBonusTimeBar.transform;
			float activeBonusPercentage2 = GameplayCommons.Instance.levelStateController.GetActiveBonusPercentage(BonusController.BonusType.freezeBonus);
			Vector3 localScale3 = FreezeBonusTimeBar.transform.localScale;
			float y2 = localScale3.y;
			Vector3 localScale4 = FreezeBonusTimeBar.transform.localScale;
			transform2.localScale = new Vector3(activeBonusPercentage2, y2, localScale4.z);
		}
		else if (FreezeBonusTimeBar.gameObject.activeInHierarchy)
		{
			if (!GameplayCommons.Instance.levelStateController.GameplayStopped)
			{
				if (!GameplayCommons.Instance.levelResultsController.IsEnabled)
				{
					SoundManager.instance.PlayBonusWearoffSound();
				}
				GameplayCommons.Instance.effectsSpawner.CreateFlareEffect(FreezeBonusBG.transform.position);
			}
			FreezeBonusTimeBar.gameObject.SetActive(value: false);
			FreezeBonusIcon.gameObject.SetActive(value: false);
			FreezeBonusBG.gameObject.SetActive(value: false);
		}
		if (GameplayCommons.Instance.levelStateController.IsInvisibilityActive)
		{
			if (!InvisibilityBonusTimeBar.gameObject.activeInHierarchy)
			{
				InvisibilityBonusTimeBar.gameObject.SetActive(value: true);
				InvisibilityBonusIcon.gameObject.SetActive(value: true);
				InvisibilityBonusBG.gameObject.SetActive(value: true);
			}
			Transform transform3 = InvisibilityBonusTimeBar.transform;
			float activeBonusPercentage3 = GameplayCommons.Instance.levelStateController.GetActiveBonusPercentage(BonusController.BonusType.invisibilityBonus);
			Vector3 localScale5 = InvisibilityBonusTimeBar.transform.localScale;
			float y3 = localScale5.y;
			Vector3 localScale6 = InvisibilityBonusTimeBar.transform.localScale;
			transform3.localScale = new Vector3(activeBonusPercentage3, y3, localScale6.z);
		}
		else if (InvisibilityBonusTimeBar.gameObject.activeInHierarchy)
		{
			if (!GameplayCommons.Instance.levelStateController.GameplayStopped)
			{
				if (!GameplayCommons.Instance.levelResultsController.IsEnabled)
				{
					SoundManager.instance.PlayBonusWearoffSound();
				}
				GameplayCommons.Instance.effectsSpawner.CreateFlareEffect(InvisibilityBonusBG.transform.position);
			}
			InvisibilityBonusTimeBar.gameObject.SetActive(value: false);
			InvisibilityBonusIcon.gameObject.SetActive(value: false);
			InvisibilityBonusBG.gameObject.SetActive(value: false);
		}
		UpdateComboText();
		UpdateProfitText();
		if (allUIItemsOutComplete)
		{
			SetUIItemsAlpha();
			return;
		}
		if (!binocularsButtonOutComplete)
		{
			ProcessBinocularsOut();
		}
		if (!weaponSelectButtonOutComplete)
		{
			ProcessWeaponsButtonOut();
		}
		else if (!weaponTutorialComplete)
		{
			ProcessWeaponsTutorial();
		}
		if (!bottomUIItemsOutComplete)
		{
			ProcessBottomUIItemsOut();
		}
		if (((binocularsButtonOutComplete && weaponSelectButtonOutComplete && bottomUIItemsOutComplete) || (GlobalCommons.Instance.globalGameStats.AvailableWeaponsCount == 1 && bottomUIItemsOutComplete && binocularsButtonOutComplete)) && (weaponTutorialComplete || !waitForWeaponsTutorial))
		{
			allUIItemsOutComplete = true;
		}
	}

	private void ProcessWeaponsTutorial()
	{
		if (GlobalCommons.Instance.globalGameStats.WeaponsTutorialPending)
		{
			if (GameplayCommons.Instance.weaponSwitchController.WasEverOpen)
			{
				WeaponSelectionTutorialImage.DOKill();
				Color color = WeaponSelectionTutorialImage.color;
				float a = color.a;
				a -= 0.05f;
				if (a < 0f)
				{
					a = 0f;
				}
				WeaponSelectionTutorialImage.SetAlpha(a);
				WeaponSelectText.SetAlpha(a);
				if (a == 0f)
				{
					UnityEngine.Object.Destroy(WeaponSelectionTutorialImage.gameObject);
					weaponTutorialComplete = true;
					GlobalCommons.Instance.globalGameStats.WeaponsTutorialPending = false;
				}
			}
		}
		else
		{
			weaponTutorialComplete = true;
		}
	}

	private void SetUIItemsAlpha()
	{
		float target = 1f;
		float target2 = 1f;
		float target3 = 1f;
		float target4 = 1f;
		if (GameplayCommons.Instance.cameraController.IsZoomedOut)
		{
			target = dimmedUIGroupAlphaVal;
			target2 = dimmedUIGroupAlphaVal;
			target4 = dimmedUIGroupAlphaVal;
			target3 = dimmedUIGroupAlphaVal;
		}
		if (GameplayCommons.Instance.touchesController.ShootTouchController.TouchActive)
		{
			Vector2 normalized = GameplayCommons.Instance.touchesController.ShootTouchController.RawDirectionVectorWithTimeoutToFiltered.normalized;
			if (Math.Abs(normalized.x) < Math.Abs(normalized.y))
			{
				if (normalized.y > 0f)
				{
					target = dimmedUIGroupAlphaVal;
				}
				if (normalized.y < 0f)
				{
					target2 = dimmedUIGroupAlphaVal;
				}
			}
			if (normalized.y > 0.12f && normalized.x > 0.3f)
			{
				target3 = dimmedUIGroupAlphaVal;
			}
			if (normalized.y > 0.12f && normalized.x < -0.3f)
			{
				target4 = dimmedUIGroupAlphaVal;
			}
		}
		if (GameplayCommons.Instance.GamePaused || (GameplayCommons.Instance.levelStateController.GameplayStopped ? true : false))
		{
			target = 0f;
			target2 = 0f;
			target4 = 0f;
			target3 = 0f;
		}
		ModifyCanvasGroupAlpha(upperUIItems, target);
		ModifyCanvasGroupAlpha(bottomUIItems, target2);
		ModifyImageAlpha(BinocularsButton, target3);
		if (weaponSelectButtonOutComplete)
		{
			ModifyImageAlpha(WeaponSelectButton, target4);
		}
		if (BushController.PlayerHidden)
		{
			if (!PlayerHiddenImage.enabled)
			{
				PlayerHiddenImage.enabled = true;
			}
			ModifyImageAlpha(PlayerHiddenImage, 1f);
			return;
		}
		Color color = PlayerHiddenImage.color;
		if (color.a > 0f)
		{
			ModifyImageAlpha(PlayerHiddenImage, 0f, faster: true);
		}
		else if (PlayerHiddenImage.enabled)
		{
			PlayerHiddenImage.enabled = false;
		}
	}

	private void ModifyImageAlpha(Image img, float target, bool faster = false)
	{
		Color color = img.color;
		float num = color.a;
		float num2 = uiGroupAlphaChangeSpeed;
		if (faster)
		{
			num2 *= 3f;
		}
		if (num > target)
		{
			num -= Time.deltaTime * num2;
			if (num < target)
			{
				num = target;
			}
		}
		else if (num < target)
		{
			num += Time.deltaTime * num2;
			if (num > target)
			{
				num = target;
			}
		}
		float num3 = num;
		Color color2 = img.color;
		if (num3 != color2.a)
		{
			img.SetAlpha(num);
		}
	}

	private void ModifyCanvasGroupAlpha(CanvasGroup cg, float target)
	{
		if (cg.alpha > target)
		{
			cg.alpha -= Time.deltaTime * uiGroupAlphaChangeSpeed;
			if (cg.alpha < target)
			{
				cg.alpha = target;
			}
		}
		else if (cg.alpha < target)
		{
			cg.alpha += Time.deltaTime * uiGroupAlphaChangeSpeed;
			if (cg.alpha > target)
			{
				cg.alpha = target;
			}
		}
	}

	internal void FlashScreen()
	{
		if (GlobalCommons.Instance.globalGameStats.ScreenFlashesEnabled)
		{
			bool isEditor = Application.isEditor;
			if (0 == 0)
			{
				screenFlashFactor = 1f;
			}
		}
	}

	public void ShakeComboText()
	{
		comboTextShakeFactor = 80;
	}

	public void ShakeProfitText()
	{
		profitTextShakeFactor = 80;
	}

	private void UpdateProfitText()
	{
		if (profitTextShakeFactor > 0)
		{
			profitTextShakeFactor -= 10;
			if (profitTextShakeFactor < 0)
			{
				profitTextShakeFactor = 0;
			}
			float num = (float)profitTextShakeFactor * 0.03f;
			float num2 = num;
			float num3 = num;
			if ((double)UnityEngine.Random.value > 0.5)
			{
				num2 *= -1f;
			}
			if ((double)UnityEngine.Random.value > 0.5)
			{
				num3 *= -1f;
			}
			profitText.rectTransform.anchoredPosition = new Vector3(profitTextDefaultLocalPosition.x + num2, profitTextDefaultLocalPosition.y + num3, profitTextDefaultLocalPosition.z);
			RectTransform rectTransform = ProfitTextBG.rectTransform;
			Vector2 anchoredPosition = profitText.rectTransform.anchoredPosition;
			float x = anchoredPosition.x;
			Vector2 anchoredPosition2 = profitText.rectTransform.anchoredPosition;
			rectTransform.anchoredPosition = new Vector2(x, anchoredPosition2.y);
			float num4 = num;
			if ((double)UnityEngine.Random.value > 0.5)
			{
				num4 *= -1f;
			}
			profitText.rectTransform.localEulerAngles = new Vector3(0f, 0f, num4);
			ProfitTextBG.rectTransform.localEulerAngles = profitText.rectTransform.localEulerAngles;
		}
		if (Time.fixedTime - profitTextDisplayTimestamp < profitShowTimeout)
		{
			if (ProfitTextAlpha != 1f)
			{
				ProfitTextAlpha = 1f;
			}
		}
		else if (ProfitTextAlpha > 0f)
		{
			float num5 = ProfitTextAlpha - 3f * Time.deltaTime;
			if (num5 < 0f)
			{
				num5 = 0f;
			}
			ProfitTextAlpha = num5;
		}
	}

	public void ShowProfitText(int moneyCollected)
	{
		ShakeProfitText();
		profitTextDisplayTimestamp = Time.fixedTime;
		ProfitTextAlpha = 1f;
		profitText.text = moneyCollected.ToString() + LocalizationManager.Instance.GetLocalizedText("GameplayUIProfit");
		RectTransform rectTransform = ProfitTextBG.rectTransform;
		Vector2 anchoredPosition = profitText.rectTransform.anchoredPosition;
		float x = anchoredPosition.x;
		Vector2 anchoredPosition2 = profitText.rectTransform.anchoredPosition;
		rectTransform.anchoredPosition = new Vector2(x, anchoredPosition2.y);
		ProfitTextBG.rectTransform.sizeDelta = new Vector2(LayoutUtility.GetPreferredWidth(profitText.rectTransform) + 12f, LayoutUtility.GetPreferredHeight(profitText.rectTransform));
	}

	private void UpdateComboText()
	{
		if (comboTextShakeFactor > 0)
		{
			comboTextShakeFactor -= 10;
			if (comboTextShakeFactor < 0)
			{
				comboTextShakeFactor = 0;
			}
			float num = (float)comboTextShakeFactor * 0.05f;
			float num2 = num;
			float num3 = num;
			if ((double)UnityEngine.Random.value > 0.5)
			{
				num2 *= -1f;
			}
			if ((double)UnityEngine.Random.value > 0.5)
			{
				num3 *= -1f;
			}
			comboText.rectTransform.anchoredPosition = new Vector3(comboTextDefaultLocalPosition.x + num2, comboTextDefaultLocalPosition.y + num3, comboTextDefaultLocalPosition.z);
			RectTransform rectTransform = ComboTextBG.rectTransform;
			Vector2 anchoredPosition = comboText.rectTransform.anchoredPosition;
			float x = anchoredPosition.x;
			Vector2 anchoredPosition2 = comboText.rectTransform.anchoredPosition;
			rectTransform.anchoredPosition = new Vector2(x, anchoredPosition2.y);
			float num4 = (float)comboTextShakeFactor * 0.15f;
			if ((double)UnityEngine.Random.value > 0.5)
			{
				num4 *= -1f;
			}
			comboText.rectTransform.localEulerAngles = new Vector3(0f, 0f, num4);
			ComboTextBG.rectTransform.localEulerAngles = comboText.rectTransform.localEulerAngles;
		}
		if (GameplayCommons.Instance.levelStateController.ComboController.DisplayComboCounter)
		{
			if (ComboTextAlpha != 1f)
			{
				ComboTextAlpha = 1f;
			}
			string text = GameplayCommons.Instance.levelStateController.ComboController.CurrentComboValue.ToString() + LocalizationManager.Instance.GetLocalizedText("GameplayUICombo");
			if (comboText.text != text)
			{
				comboText.text = text;
				ComboTextBG.rectTransform.sizeDelta = new Vector2(LayoutUtility.GetPreferredWidth(comboText.rectTransform) + 12f, LayoutUtility.GetPreferredHeight(comboText.rectTransform));
			}
		}
		else if (ComboTextAlpha > 0f)
		{
			float num5 = ComboTextAlpha - 3f * Time.deltaTime;
			if (num5 < 0f)
			{
				num5 = 0f;
			}
			ComboTextAlpha = num5;
		}
	}
}
