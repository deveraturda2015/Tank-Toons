using System;
using System.Collections.Generic;

public class EnemiesBalance
{
	internal static float bossDamageCoeff = 2f;

	private static Dictionary<WeaponTypes, float> EnemiesHPDict = new Dictionary<WeaponTypes, float>
	{
		{
			WeaponTypes.machinegun,
			10f
		},
		{
			WeaponTypes.shotgun,
			20f
		},
		{
			WeaponTypes.minigun,
			25f
		},
		{
			WeaponTypes.cannon,
			40f
		},
		{
			WeaponTypes.homingRocket,
			60f
		},
		{
			WeaponTypes.suicide,
			40f
		},
		{
			WeaponTypes.laser,
			80f
		},
		{
			WeaponTypes.railgun,
			110f
		},
		{
			WeaponTypes.ricochet,
			140f
		},
		{
			WeaponTypes.triple,
			170f
		},
		{
			WeaponTypes.shocker,
			210f
		},
		{
			WeaponTypes.gold,
			10f
		}
	};

	internal static float BossHPCoeff
	{
		get
		{
			if (GlobalCommons.Instance.ActualSelectedLevel >= 75)
			{
				return 20f;
			}
			if (GlobalCommons.Instance.ActualSelectedLevel >= 60)
			{
				return 16f;
			}
			if (GlobalCommons.Instance.ActualSelectedLevel >= 38)
			{
				return 12f;
			}
			if (GlobalCommons.Instance.ActualSelectedLevel >= 18)
			{
				return 8f;
			}
			return 6f;
		}
	}

	internal static float GetEnemyHP(WeaponTypes enemyWeaponType)
	{
		if (!EnemiesHPDict.ContainsKey(enemyWeaponType))
		{
			throw new Exception("unknown enemy type hp init");
		}
		return EnemiesHPDict[enemyWeaponType];
	}

	internal static float GetEnemyDamage(WeaponTypes weaponType)
	{
		int num = 0;
		switch (weaponType)
		{
		case WeaponTypes.machinegun:
			return 1f + (float)(GlobalCommons.Instance.SelectedLevelBalanceFactor - 1) * 0.1f;
		case WeaponTypes.shotgun:
			return 1f + (float)(GlobalCommons.Instance.SelectedLevelBalanceFactor - 1) * 0.1f;
		case WeaponTypes.minigun:
			return 1f + (float)(GlobalCommons.Instance.SelectedLevelBalanceFactor - 1) * 0.1f;
		case WeaponTypes.cannon:
			num = ((GlobalCommons.Instance.SelectedLevelBalanceFactor < 6) ? 6 : GlobalCommons.Instance.SelectedLevelBalanceFactor);
			return 18f + (float)(num - 6) * 0.5f;
		case WeaponTypes.homingRocket:
			num = ((GlobalCommons.Instance.SelectedLevelBalanceFactor < 16) ? 16 : GlobalCommons.Instance.SelectedLevelBalanceFactor);
			return 24f + (float)(num - 16) * 0.5f;
		case WeaponTypes.suicide:
			return GlobalBalance.GetExplBarrelDamage() / 2f;
		case WeaponTypes.laser:
			num = ((GlobalCommons.Instance.SelectedLevelBalanceFactor < 40) ? 40 : GlobalCommons.Instance.SelectedLevelBalanceFactor);
			return 68.3f + (float)(num - 40) * 2f;
		case WeaponTypes.railgun:
			num = ((GlobalCommons.Instance.SelectedLevelBalanceFactor < 40) ? 40 : GlobalCommons.Instance.SelectedLevelBalanceFactor);
			return 100f + (float)(num - 40) * 2.5f;
		case WeaponTypes.ricochet:
			num = ((GlobalCommons.Instance.SelectedLevelBalanceFactor < 40) ? 40 : GlobalCommons.Instance.SelectedLevelBalanceFactor);
			return 100f + (float)(num - 40) * 2.5f;
		case WeaponTypes.triple:
			return 1f + (float)(GlobalCommons.Instance.SelectedLevelBalanceFactor - 1) * 0.1f;
		case WeaponTypes.shocker:
			num = ((GlobalCommons.Instance.SelectedLevelBalanceFactor < 40) ? 40 : GlobalCommons.Instance.SelectedLevelBalanceFactor);
			return 68.3f + (float)(num - 40) * 2f;
		case WeaponTypes.gold:
			return 0f;
		default:
			throw new Exception("unknown enemy weapon type damage init");
		}
	}
}
