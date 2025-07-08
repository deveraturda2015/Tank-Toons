using UnityEngine;

public class RicochetController : WeaponController
{
	private bool lastShootingState;

	public WeaponTypes WeaponType => WeaponTypes.ricochet;

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
			SoundManager.instance.PlayRicochetShotSound();
			GameplayCommons.Instance.playersTankController.SetMaxTurretShiftFactor();
			Vector3 barrelPoint = GameplayCommons.Instance.playersTankController.GetBarrelPoint();
			GameObject gameObject = Object.Instantiate(Prefabs.RicochetBulletPrefab, barrelPoint, Quaternion.identity);
			gameObject.GetComponent<RicochetBullet>().Initialize(GameplayCommons.Instance.playersTankController.TankTurret.transform.right, isPlayerCannon: true);
			GameplayCommons.Instance.effectsSpawner.SpawnSmoke(GameplayCommons.Instance.playersTankController.GetBarrelPoint(), 2, 0f);
			result = 1f;
			GameplayCommons.Instance.cameraController.ShakeCamera();
		}
		lastShootingState = isShooting;
		return result;
	}

	public bool IsLoud()
	{
		return true;
	}

	public float GetAutoaimTurretRotationSpeedMod()
	{
		return 1f;
	}

	public bool EligibleForDownwardsNoAmmoSelect()
	{
		return false;
	}

	public bool ShowCursor()
	{
		return true;
	}

	public bool BouncesCursor()
	{
		return true;
	}
}
