using UnityEngine;

public class RailgunController : WeaponController
{
	private bool lastShootingState;

	private Transform prevPlayerTransform;

	public WeaponTypes WeaponType => WeaponTypes.railgun;

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
			SoundManager.instance.PlayRailgunShotSound();
			GameplayCommons.Instance.playersTankController.SetMaxTurretShiftFactor();
			RailgunBeam component = Object.Instantiate(Prefabs.railgunBeamPrefab).GetComponent<RailgunBeam>();
			component.InitializeBeam(null, isPlayerBeam: true);
			GameplayCommons.Instance.effectsSpawner.SpawnSmoke(GameplayCommons.Instance.playersTankController.GetBarrelPoint(), 2, 0f);
			result = 1f;
			GameplayCommons.Instance.cameraController.ShakeCamera();
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
		return true;
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
