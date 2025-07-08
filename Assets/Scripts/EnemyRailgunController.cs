using UnityEngine;

public class EnemyRailgunController : WeaponController
{
	private float lastTimeShot;

	private float shootInterval = 0.8f;

	private bool bossWeapon;

	private EnemyTankController enemyTankController;

	public WeaponTypes WeaponType => WeaponTypes.railgun;

	public float MinimumPlayerDistanceWhileChasing => 1.8f;

	public bool InstantlyForgetPlayerOnSightLoss => false;

	public bool BossWeapon
	{
		get
		{
			return bossWeapon;
		}
		set
		{
			bossWeapon = value;
		}
	}

	public float ShootAngleTreshhold => 20f;

	public EnemyRailgunController(EnemyTankController etc)
	{
		enemyTankController = etc;
	}

	public float Update(bool isShooting)
	{
		if (isShooting && Time.fixedTime - lastTimeShot >= shootInterval)
		{
			SoundManager.instance.PlayRailgunShotSound();
			lastTimeShot = Time.fixedTime;
			RailgunBeam component = Object.Instantiate(Prefabs.railgunBeamPrefab).GetComponent<RailgunBeam>();
			component.InitializeBeam(enemyTankController, isPlayerBeam: false, bossWeapon);
			GameplayCommons.Instance.effectsSpawner.SpawnSmoke(enemyTankController.GetBarrelPoint(), 2, 0f);
			enemyTankController.SetMaxTurretShiftFactor();
		}
		return 0f;
	}

	public bool BouncesCursor()
	{
		return false;
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
