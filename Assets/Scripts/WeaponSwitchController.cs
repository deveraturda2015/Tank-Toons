using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitchController : MonoBehaviour
{
	public enum WeaponSwitchControllerState
	{
		In,
		Out
	}

	private Image weaponsFadeImage;

	private Image weaponSelectorBackground;

	private RectTransform weaponSelectorBackgroundRT;

	internal WeaponSwitchControllerState CurrentState;

	private float faderFadeSpeed = 0.1f;

	private float panelFadeSpeed = 0.2f;

	private CanvasGroup panelCanvasGourp;

	private float maxFaderAlpha = 0.5f;

	private Image[] ammoBars;

	private Image[] noAmmoCovers;

	internal bool WasEverOpen;

	private Vector2 inPosition;

	private Vector2 outPosition;

	private float menuFadeSpeed = 0.15f;

	private float menuShiftAmount = 30f;

	private bool fadeOutFlag;

	private readonly Dictionary<string, WeaponTypes> buttonNameToWeaponType = new Dictionary<string, WeaponTypes>
	{
		{
			"MachinegunButton",
			WeaponTypes.machinegun
		},
		{
			"ShotgunButton",
			WeaponTypes.shotgun
		},
		{
			"MinigunButton",
			WeaponTypes.minigun
		},
		{
			"CannonButton",
			WeaponTypes.cannon
		},
		{
			"HomingRocketsButton",
			WeaponTypes.homingRocket
		},
		{
			"MinesButton",
			WeaponTypes.mines
		},
		{
			"GuidedRocketButton",
			WeaponTypes.guidedRocket
		},
		{
			"LaserButton",
			WeaponTypes.laser
		},
		{
			"RailgunButton",
			WeaponTypes.railgun
		},
		{
			"TripleButton",
			WeaponTypes.triple
		},
		{
			"ShockerButton",
			WeaponTypes.shocker
		},
		{
			"RicochetButton",
			WeaponTypes.ricochet
		}
	};

	private void Start()
	{
		weaponsFadeImage = GameObject.Find("WeaponsFadeImage").GetComponent<Image>();
		weaponsFadeImage.enabled = false;
		SetFaderAlpha(0f);
		weaponSelectorBackground = GameObject.Find("WeaponSelectorBackground").GetComponent<Image>();
		weaponSelectorBackgroundRT = weaponSelectorBackground.GetComponent<RectTransform>();
		Vector2 a = new Vector2(weaponSelectorBackgroundRT.rect.width, 0f - weaponSelectorBackgroundRT.rect.height + 181f);
		SetWeaponSelectorEnabled(val: false);
		panelCanvasGourp = weaponSelectorBackground.GetComponent<CanvasGroup>();
		panelCanvasGourp.alpha = 0f;
		CurrentState = WeaponSwitchControllerState.Out;
		noAmmoCovers = new Image[15];
		IEnumerator enumerator = weaponSelectorBackground.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				Button btn = transform.gameObject.GetComponent<Button>();
				if (btn != null)
				{
					btn.onClick.AddListener(delegate
					{
						WeaponSelectButtonClick(btn);
					});
					int num = (int)buttonNameToWeaponType[btn.name];
					WeaponTypes weaponTypes = (WeaponTypes)num;
					if (GlobalCommons.Instance.globalGameStats.WeaponsLevels[num] == 0)
					{
						GameObject gameObject = UnityEngine.Object.Instantiate(Prefabs.weaponItemCoverPrefab);
						gameObject.transform.SetParent(btn.transform.parent.transform, worldPositionStays: false);
						gameObject.transform.position = btn.transform.position;
						RectTransform component = gameObject.GetComponent<RectTransform>();
						RectTransform rectTransform = component;
						Vector2 anchoredPosition = component.anchoredPosition;
						float x = anchoredPosition.x;
						Vector2 anchoredPosition2 = component.anchoredPosition;
						rectTransform.anchoredPosition = new Vector2(x, anchoredPosition2.y + 5f);
					}
					else
					{
						GameObject gameObject2 = UnityEngine.Object.Instantiate(Prefabs.weaponItemNoAmmoCoverPrefab);
						gameObject2.transform.SetParent(btn.transform.parent.transform, worldPositionStays: false);
						gameObject2.transform.position = btn.transform.position;
						RectTransform component2 = gameObject2.GetComponent<RectTransform>();
						RectTransform rectTransform2 = component2;
						Vector2 anchoredPosition3 = component2.anchoredPosition;
						float x2 = anchoredPosition3.x;
						Vector2 anchoredPosition4 = component2.anchoredPosition;
						rectTransform2.anchoredPosition = new Vector2(x2, anchoredPosition4.y + 5f);
						noAmmoCovers[num] = gameObject2.GetComponent<Image>();
					}
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
		ammoBars = new Image[15]
		{
			weaponSelectorBackground.transform.Find("MachinegunAmmoBar").gameObject.GetComponent<Image>(),
			weaponSelectorBackground.transform.Find("ShotgunAmmoBar").gameObject.GetComponent<Image>(),
			weaponSelectorBackground.transform.Find("MinigunAmmoBar").gameObject.GetComponent<Image>(),
			weaponSelectorBackground.transform.Find("CannonAmmoBar").gameObject.GetComponent<Image>(),
			weaponSelectorBackground.transform.Find("HomingRocketAmmoBar").gameObject.GetComponent<Image>(),
			weaponSelectorBackground.transform.Find("MinesAmmoBar").gameObject.GetComponent<Image>(),
			weaponSelectorBackground.transform.Find("GuidedRocketAmmoBar").gameObject.GetComponent<Image>(),
			weaponSelectorBackground.transform.Find("LaserAmmoBar").gameObject.GetComponent<Image>(),
			weaponSelectorBackground.transform.Find("RailgunAmmoBar").gameObject.GetComponent<Image>(),
			null,
			null,
			null,
			weaponSelectorBackground.transform.Find("RicochetAmmoBar").gameObject.GetComponent<Image>(),
			weaponSelectorBackground.transform.Find("TripleAmmoBar").gameObject.GetComponent<Image>(),
			weaponSelectorBackground.transform.Find("ShockerAmmoBar").gameObject.GetComponent<Image>()
		};
		float num2 = (float)Screen.height / Screen.dpi;
		float value = 1f - (num2 - 2.48f) * 0.2f;
		value = Mathf.Clamp(value, 0.65f, 1f);
		Transform transform2 = weaponSelectorBackground.transform;
		float x3 = value;
		float y = value;
		Vector3 localScale = weaponSelectorBackground.transform.localScale;
		transform2.localScale = new Vector3(x3, y, localScale.z);
		menuShiftAmount *= value;
		inPosition = weaponSelectorBackgroundRT.anchoredPosition + a * (1f - value) / 2f;
		outPosition = new Vector2(inPosition.x + menuShiftAmount, inPosition.y - menuShiftAmount);
		weaponSelectorBackgroundRT.anchoredPosition = outPosition;
	}

	private void WeaponSelectButtonClick(Button btn)
	{
		if (PlayerBalance.LockedWeaponTypes.IndexOf(buttonNameToWeaponType[btn.name]) != -1)
		{
			SoundManager.instance.PlayNotAvailableSound();
			return;
		}
		Image component = btn.GetComponent<Image>();
		component.DOKill();
		component.SetAlpha(1f);
		Image target = component;
		Color color = component.color;
		float r = color.r;
		Color color2 = component.color;
		float g = color2.g;
		Color color3 = component.color;
		target.DOColor(new Color(r, g, color3.b, 0f), 0.2f).SetUpdate(isIndependentUpdate: true);
		GameplayCommons.Instance.weaponsController.SelectWeapon(buttonNameToWeaponType[btn.name], playSound: true);
	}

	private void SetWeaponSelectorEnabled(bool val)
	{
		weaponSelectorBackground.gameObject.SetActive(val);
		IEnumerator enumerator = weaponSelectorBackground.transform.GetEnumerator();
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
		switch (CurrentState)
		{
		case WeaponSwitchControllerState.In:
			if (!fadeOutFlag)
			{
				SoundManager.instance.PlaySwooshInSound();
				weaponsFadeImage.DOKill();
				panelCanvasGourp.DOKill();
				weaponSelectorBackgroundRT.DOKill();
				UpdateAmmoValues();
				weaponsFadeImage.enabled = true;
				SetWeaponSelectorEnabled(val: true);
				weaponsFadeImage.DOFade(maxFaderAlpha, menuFadeSpeed).SetUpdate(isIndependentUpdate: true);
				panelCanvasGourp.DOFade(1f, menuFadeSpeed).SetUpdate(isIndependentUpdate: true);
				weaponSelectorBackgroundRT.DOAnchorPos(inPosition, menuFadeSpeed).SetUpdate(isIndependentUpdate: true);
				fadeOutFlag = true;
			}
			break;
		case WeaponSwitchControllerState.Out:
			if (fadeOutFlag)
			{
				SoundManager.instance.PlayShooshOutSound();
				weaponsFadeImage.DOKill();
				panelCanvasGourp.DOKill();
				weaponSelectorBackgroundRT.DOKill();
				fadeOutFlag = false;
				weaponsFadeImage.DOFade(0f, menuFadeSpeed).SetUpdate(isIndependentUpdate: true);
				panelCanvasGourp.DOFade(0f, menuFadeSpeed).SetUpdate(isIndependentUpdate: true).OnCompleteWithCoroutine(FinalizeSelectorOut);
				weaponSelectorBackgroundRT.DOAnchorPos(outPosition, menuFadeSpeed).SetUpdate(isIndependentUpdate: true);
			}
			break;
		}
	}

	private void FinalizeSelectorOut()
	{
		weaponsFadeImage.enabled = false;
		SetWeaponSelectorEnabled(val: false);
	}

	private void UpdateAmmoValues()
	{
		for (int i = 0; i < ammoBars.Length; i++)
		{
			if (PlayerBalance.PlayerUnavailableWeaponIndexesList.Contains(i))
			{
				continue;
			}
			Image image = ammoBars[i];
			float num = 0f;
			if (GlobalCommons.Instance.globalGameStats.WeaponsLevels[i] > 0)
			{
				num = GameplayCommons.Instance.weaponsController.GetAmmoPercentageForWeapon((WeaponTypes)i);
				if (num == 0f)
				{
					noAmmoCovers[i].enabled = true;
				}
				else
				{
					noAmmoCovers[i].enabled = false;
				}
			}
			Transform transform = image.transform;
			float x = num;
			Vector3 localScale = image.transform.localScale;
			float y = localScale.y;
			Vector3 localScale2 = image.transform.localScale;
			transform.localScale = new Vector3(x, y, localScale2.z);
		}
	}

	private void SetFaderAlpha(float val)
	{
		Image image = weaponsFadeImage;
		Color color = weaponsFadeImage.color;
		float r = color.r;
		Color color2 = weaponsFadeImage.color;
		float g = color2.g;
		Color color3 = weaponsFadeImage.color;
		image.color = new Color(r, g, color3.b, val);
	}

	public void FadeIn()
	{
		if (CurrentState != 0 && !(GameplayCommons.Instance.weaponsController.ActiveGuidedRocket != null) && !GameplayCommons.Instance.levelStateController.GameplayStopped && !GameplayCommons.Instance.levelStateController.LevelCompletionPending && !GameplayCommons.Instance.GamePaused && !GameplayCommons.Instance.playersTankController.PlayerDead && (GlobalCommons.Instance.globalGameStats.WeaponsTutorialPending || GameplayCommons.Instance.gameplayUIController.WeaponsSelectEnabled))
		{
			CurrentState = WeaponSwitchControllerState.In;
			WasEverOpen = true;
			GameplayCommons.Instance.SetDesiredTimeScale(0f);
		}
	}

	public void FadeOut()
	{
		if (CurrentState != WeaponSwitchControllerState.Out)
		{
			CurrentState = WeaponSwitchControllerState.Out;
			GameplayCommons.Instance.SetDesiredTimeScale(1f);
		}
	}
}
