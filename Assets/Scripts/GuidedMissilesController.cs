using UnityEngine;

public class GuidedMissilesController : WeaponController
{
	private bool lastShootingState;

	public WeaponTypes WeaponType => WeaponTypes.guidedRocket;

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

	public float Update(bool isShooting)
	{
		isShooting = GameplayCommons.Instance.touchesController.ShootTouchController.TouchHolding;
		float result = 0f;
		if (!isShooting && lastShootingState && !GameplayCommons.Instance.touchesController.WeaponsMenuActive)
		{
			if (GameplayCommons.Instance.weaponsController.ActiveGuidedRocket == null)
			{
				GameObject gameObject = Object.Instantiate(Prefabs.guidedRocketPrefab, GameplayCommons.Instance.playersTankController.TankBase.transform.position, Quaternion.identity);
				GameplayCommons.Instance.weaponsController.ActiveGuidedRocket = gameObject.GetComponent<GuidedRocket>();
				GameplayCommons.Instance.playersTankController.SetMaxTurretShiftFactor();
				SoundManager.instance.PlayHomingMissileShotSound();
				GameplayCommons.Instance.effectsSpawner.SpawnMissileLaunchSmokeParticles(GameplayCommons.Instance.playersTankController.TankBase.transform.position, GameplayCommons.Instance.playersTankController.TankTurret.transform.rotation);
				GameplayCommons.Instance.cameraController.ShakeCamera(3f);
				result = 1f;
			}
			else
			{
				GameplayCommons.Instance.weaponsController.ActiveGuidedRocket.DestroyRocket();
			}
		}
		lastShootingState = isShooting;
		return result;
	}

	public bool BouncesCursor()
	{
		return false;
	}

	public bool ShowCursor()
	{
		if (GameplayCommons.Instance.weaponsController.ActiveGuidedRocket == null)
		{
			return true;
		}
		return false;
	}

	public bool EligibleForDownwardsNoAmmoSelect()
	{
		return false;
	}

	public bool IsLoud()
	{
		return true;
	}

	public float GetAutoaimTurretRotationSpeedMod()
	{
		return 1f;
	}
}
