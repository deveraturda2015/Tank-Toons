using System.Collections.Generic;
using UnityEngine;

public class MinesController : WeaponController
{
	private bool lastShootingState;

	public static List<GameObject> AllMines = new List<GameObject>();

	public static float MinesProximityTreshhold = 0.4f;

	public WeaponTypes WeaponType => WeaponTypes.mines;

	public float MinimumPlayerDistanceWhileChasing => 1.8f;

	public bool InstantlyForgetPlayerOnSightLoss => false;

	public bool BossWeapon
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public float ShootAngleTreshhold => 20f;

	public MinesController()
	{
		AllMines = new List<GameObject>();
	}

	public float Update(bool isShooting)
	{
		isShooting = GameplayCommons.Instance.touchesController.ShootTouchController.TouchHolding;
		float result = 0f;
		if (!isShooting && lastShootingState && !GameplayCommons.Instance.touchesController.WeaponsMenuActive)
		{
			Vector3 position = GameplayCommons.Instance.playersTankController.TankBase.transform.position;
			bool flag = false;
			for (int i = 0; i < AllMines.Count; i++)
			{
				GameObject gameObject = AllMines[i];
				Vector3 position2 = gameObject.transform.position;
				if (Mathf.Abs(position2.x - position.x) <= MinesProximityTreshhold)
				{
					Vector3 position3 = gameObject.transform.position;
					if (Mathf.Abs(position3.y - position.y) <= MinesProximityTreshhold)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				GameplayCommons.Instance.playersTankController.SetTurretSprite(doBounce: true);
				SoundManager.instance.PlayMinePlaceSound();
				GameObject item = Object.Instantiate(Prefabs.minePrefab, position, Quaternion.identity);
				result = 1f;
				AllMines.Add(item);
			}
			else
			{
				SoundManager.instance.PlayCantPlaceMineSound();
			}
		}
		lastShootingState = isShooting;
		return result;
	}

	public bool BouncesCursor()
	{
		return true;
	}

	public bool ShowCursor()
	{
		return false;
	}

	public bool EligibleForDownwardsNoAmmoSelect()
	{
		return false;
	}

	public bool IsLoud()
	{
		return false;
	}

	public float GetAutoaimTurretRotationSpeedMod()
	{
		return 1f;
	}
}
