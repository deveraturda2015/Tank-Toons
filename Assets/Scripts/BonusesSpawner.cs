using System;
using System.Collections.Generic;
using UnityEngine;

public class BonusesSpawner
{
	private const int healthId = 1;

	private const int ammoId = 5;

	private static bool IsNoCoinsAndNoMapRevealMode()
	{
		if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.SurvivalLevel || GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.EditorLevel || GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.CustomLevel)
		{
			return true;
		}
		return false;
	}

	public static void SpawnCoinsBonus(Vector3 coords, int moneyEquivalent, bool increaseCoinCount = false)
	{
		if (IsNoCoinsAndNoMapRevealMode())
		{
			return;
		}
		int num = Mathf.FloorToInt((float)moneyEquivalent / 50f);
		moneyEquivalent -= num * 50;
		int num2 = Mathf.FloorToInt((float)moneyEquivalent / 20f);
		moneyEquivalent -= num2 * 20;
		int num3 = Mathf.FloorToInt((float)moneyEquivalent / 5f);
		int num4 = num + num2 + num3;
		int num5 = 3;
		if (increaseCoinCount)
		{
			num5 = 12;
		}
		while (num4 < num5)
		{
			if (num2 > 0)
			{
				num2--;
				num3 += 4;
			}
			else
			{
				if (num <= 0)
				{
					break;
				}
				num--;
				num2 += 2;
			}
			num4 = num + num2 + num3;
		}
		do
		{
			Vector3 position = new Vector3(coords.x, coords.y, GameplayCommons.Instance.coinZIndex);
			GameplayCommons.Instance.coinZIndex -= 0.001f;
			BonusController.BonusType coinType = BonusController.BonusType.bronzeCoinBonus;
			if (num > 0)
			{
				num--;
				coinType = BonusController.BonusType.platinumCoinBonus;
			}
			else if (num2 > 0)
			{
				num2--;
				coinType = BonusController.BonusType.goldenCoinBonus;
			}
			else if (num3 > 0)
			{
				num3--;
				coinType = BonusController.BonusType.bronzeCoinBonus;
			}
			GameplayCommons.Instance.gameObjectPools.InitializeCoin(position, coinType);
		}
		while (num3 > 0 || num2 > 0 || num > 0);
	}

	public static void SpawnRandomBonus(Vector3 coords, List<BonusController.BonusType> typesList = null)
	{
		List<WeaponTypes> eligibleWeaponAmmoBonuses = GetEligibleWeaponAmmoBonuses();
		if (typesList == null)
		{
			typesList = new List<BonusController.BonusType>();
			if (GlobalCommons.Instance.globalGameStats.LevelsCompleted > 6 && GameplayCommons.Instance.playersTankController.HPPercentage > 0.5f)
			{
				typesList.Add(BonusController.BonusType.bombBonus);
			}
			if (!IsNoCoinsAndNoMapRevealMode())
			{
				typesList.Add(BonusController.BonusType.coinBonus);
				typesList.Add(BonusController.BonusType.coinBonus);
				typesList.Add(BonusController.BonusType.coinBonus);
				typesList.Add(BonusController.BonusType.coinBonus);
				typesList.Add(BonusController.BonusType.coinBonus);
				if (GameplayCommons.Instance.visibilityController.UncoverPercentage > 0.25f)
				{
					typesList.Add(BonusController.BonusType.revealMapBonus);
					typesList.Add(BonusController.BonusType.revealMapBonus);
					typesList.Add(BonusController.BonusType.revealMapBonus);
				}
			}
			else
			{
				typesList.Add(BonusController.BonusType.freezeBonus);
				typesList.Add(BonusController.BonusType.freezeBonus);
				typesList.Add(BonusController.BonusType.invisibilityBonus);
				typesList.Add(BonusController.BonusType.invisibilityBonus);
				typesList.Add(BonusController.BonusType.doubleDamageBonus);
				typesList.Add(BonusController.BonusType.doubleDamageBonus);
			}
			List<EnemyTankController> allEnemies = GameplayCommons.Instance.enemiesTracker.AllEnemies;
			for (int i = 0; i < allEnemies.Count; i++)
			{
				EnemyTankController enemyTankController = allEnemies[i];
				Vector3 position = enemyTankController.TankBase.transform.position;
				if (Mathf.Abs(position.x - coords.x) < 7f)
				{
					Vector3 position2 = enemyTankController.TankBase.transform.position;
					if (Mathf.Abs(position2.y - coords.y) < 7f)
					{
						typesList.Add(BonusController.BonusType.freezeBonus);
						typesList.Add(BonusController.BonusType.invisibilityBonus);
						typesList.Add(BonusController.BonusType.doubleDamageBonus);
						typesList.Add(BonusController.BonusType.freezeBonus);
						typesList.Add(BonusController.BonusType.invisibilityBonus);
						typesList.Add(BonusController.BonusType.doubleDamageBonus);
						break;
					}
				}
			}
			if (GameplayCommons.Instance.playersTankController.HPPercentage <= 0.25f && GameplayCommons.Instance.playersTankController.HPPercentage > 0.1f)
			{
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
			}
			else if (GameplayCommons.Instance.playersTankController.HPPercentage <= 0.1f)
			{
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
			}
			else if (GameplayCommons.Instance.playersTankController.HPPercentage < 0.9f)
			{
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
				typesList.Add(BonusController.BonusType.healthBonus);
			}
			if (eligibleWeaponAmmoBonuses.Count > 0)
			{
				typesList.Add(WeaponTypeToBonusType(eligibleWeaponAmmoBonuses[UnityEngine.Random.Range(0, eligibleWeaponAmmoBonuses.Count)]));
				typesList.Add(WeaponTypeToBonusType(eligibleWeaponAmmoBonuses[UnityEngine.Random.Range(0, eligibleWeaponAmmoBonuses.Count)]));
				typesList.Add(WeaponTypeToBonusType(eligibleWeaponAmmoBonuses[UnityEngine.Random.Range(0, eligibleWeaponAmmoBonuses.Count)]));
			}
		}
		if (typesList.Count > 0)
		{
			BonusController.BonusType bonusType = typesList[UnityEngine.Random.Range(0, typesList.Count)];
			if (!IsNoCoinsAndNoMapRevealMode() && GameplayCommons.Instance.enemiesTracker.AllEnemies.Count == 0)
			{
				bonusType = BonusController.BonusType.coinBonus;
			}
			if ((Array.IndexOf(BonusController.activeBonusTypes, bonusType) != -1 && CheckActiveBonusActiveOrPresent()) || (bonusType == BonusController.BonusType.revealMapBonus && GameplayCommons.Instance.enemiesTracker.CheckRevealMapBonusPresent()))
			{
				bonusType = GetFailsafeBonusType(eligibleWeaponAmmoBonuses);
			}
			if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.TutorialLevel)
			{
				bonusType = BonusController.BonusType.coinBonus;
			}
			InstantiateBonus(coords, bonusType);
		}
	}

	private static BonusController.BonusType GetFailsafeBonusType(List<WeaponTypes> eligibleAmmoBonuses)
	{
		if (!IsNoCoinsAndNoMapRevealMode())
		{
			return BonusController.BonusType.coinBonus;
		}
		if (UnityEngine.Random.value > 0.8f)
		{
			return BonusController.BonusType.bombBonus;
		}
		if (eligibleAmmoBonuses.Count > 0 && UnityEngine.Random.value > 0.25f)
		{
			return WeaponTypeToBonusType(eligibleAmmoBonuses[UnityEngine.Random.Range(0, eligibleAmmoBonuses.Count)]);
		}
		return BonusController.BonusType.healthBonus;
	}

	private static bool CheckActiveBonusActiveOrPresent()
	{
		return false;
	}

	private static void InstantiateBonus(Vector3 coords, BonusController.BonusType bonusToInstantiate)
	{
		switch (bonusToInstantiate)
		{
		case BonusController.BonusType.bombBonus:
			UnityEngine.Object.Instantiate(Prefabs.bombBonusPrefab, coords, Quaternion.identity);
			break;
		case BonusController.BonusType.coinBonus:
			SpawnCoinsBonus(coords, MoneyLootCounter.GetCrateCoinsLoot(), increaseCoinCount: true);
			break;
		case BonusController.BonusType.CannonAmmoBonus:
			UnityEngine.Object.Instantiate(Prefabs.cannonAmmoBonus, coords, Quaternion.identity);
			break;
		case BonusController.BonusType.freezeBonus:
			UnityEngine.Object.Instantiate(Prefabs.freezeBonusPrefab, coords, Quaternion.identity);
			break;
		case BonusController.BonusType.revealMapBonus:
			UnityEngine.Object.Instantiate(Prefabs.revealMapBonusPrefab, coords, Quaternion.identity);
			break;
		case BonusController.BonusType.GuidedRocketAmmoBonus:
			UnityEngine.Object.Instantiate(Prefabs.guidedRocketAmmoBonus, coords, Quaternion.identity);
			break;
		case BonusController.BonusType.healthBonus:
			UnityEngine.Object.Instantiate(Prefabs.healthBonusPrefab, coords, Quaternion.identity);
			break;
		case BonusController.BonusType.HomingRocketAmmoBonus:
			UnityEngine.Object.Instantiate(Prefabs.homingRocketAmmoBonus, coords, Quaternion.identity);
			break;
		case BonusController.BonusType.invisibilityBonus:
			UnityEngine.Object.Instantiate(Prefabs.invisibilityBonusPrefab, coords, Quaternion.identity);
			break;
		case BonusController.BonusType.doubleDamageBonus:
			UnityEngine.Object.Instantiate(Prefabs.doubleDamageBonusPrefab, coords, Quaternion.identity);
			break;
		case BonusController.BonusType.LaserAmmoBonus:
			UnityEngine.Object.Instantiate(Prefabs.laserAmmoBonus, coords, Quaternion.identity);
			break;
		case BonusController.BonusType.MinesAmmoBonus:
			UnityEngine.Object.Instantiate(Prefabs.minesAmmoBonus, coords, Quaternion.identity);
			break;
		case BonusController.BonusType.MinigunAmmoBonus:
			UnityEngine.Object.Instantiate(Prefabs.minigunAmmoBonus, coords, Quaternion.identity);
			break;
		case BonusController.BonusType.RailgunAmmoBonus:
			UnityEngine.Object.Instantiate(Prefabs.railgunAmmoBonus, coords, Quaternion.identity);
			break;
		case BonusController.BonusType.ShotgunAmmoBonus:
			UnityEngine.Object.Instantiate(Prefabs.shotgunAmmoBonus, coords, Quaternion.identity);
			break;
		case BonusController.BonusType.RicochetAmmoBonus:
			UnityEngine.Object.Instantiate(Prefabs.RicochetAmmoBonus, coords, Quaternion.identity);
			break;
		case BonusController.BonusType.TripleAmmoBonus:
			UnityEngine.Object.Instantiate(Prefabs.TripleAmmoBonus, coords, Quaternion.identity);
			break;
		case BonusController.BonusType.ShockAmmoBonus:
			UnityEngine.Object.Instantiate(Prefabs.ShockAmmoBonus, coords, Quaternion.identity);
			break;
		default:
			throw new Exception("uneligible random bonus type");
		}
	}

	private static BonusController.BonusType WeaponTypeToBonusType(WeaponTypes wType)
	{
		switch (wType)
		{
		case WeaponTypes.cannon:
			return BonusController.BonusType.CannonAmmoBonus;
		case WeaponTypes.guidedRocket:
			return BonusController.BonusType.GuidedRocketAmmoBonus;
		case WeaponTypes.homingRocket:
			return BonusController.BonusType.HomingRocketAmmoBonus;
		case WeaponTypes.laser:
			return BonusController.BonusType.LaserAmmoBonus;
		case WeaponTypes.mines:
			return BonusController.BonusType.MinesAmmoBonus;
		case WeaponTypes.minigun:
			return BonusController.BonusType.MinigunAmmoBonus;
		case WeaponTypes.railgun:
			return BonusController.BonusType.RailgunAmmoBonus;
		case WeaponTypes.shotgun:
			return BonusController.BonusType.ShotgunAmmoBonus;
		case WeaponTypes.ricochet:
			return BonusController.BonusType.RicochetAmmoBonus;
		case WeaponTypes.triple:
			return BonusController.BonusType.TripleAmmoBonus;
		case WeaponTypes.shocker:
			return BonusController.BonusType.ShockAmmoBonus;
		default:
			throw new Exception("uneligible weapon ammo bonus type: " + wType.ToString());
		}
	}

	public static List<WeaponTypes> GetEligibleWeaponAmmoBonuses()
	{
		List<WeaponTypes> list = new List<WeaponTypes>();
		int[] weaponsLevels = GlobalCommons.Instance.globalGameStats.WeaponsLevels;
		for (int i = 1; i < weaponsLevels.Length; i++)
		{
			if (!PlayerBalance.PlayerUnavailableWeaponIndexesList.Contains(i) && weaponsLevels[i] > 0 && (IsNoCoinsAndNoMapRevealMode() || GameplayCommons.Instance.weaponsController.GetAmmoPercentageForWeapon((WeaponTypes)i) < 0.66f))
			{
				list.Add((WeaponTypes)i);
			}
		}
		return list;
	}
}
