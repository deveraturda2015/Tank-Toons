using UnityEngine;

internal class EnemyCannonController : WeaponController
{
	private float lastTimeShot;

	private float shootInterval = 0.8f;

	private bool bossWeapon;

	private EnemyTankController enemyTankController;

	public WeaponTypes WeaponType => WeaponTypes.cannon;

	public float MinimumPlayerDistanceWhileChasing => 2.5f;

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

	public EnemyCannonController(EnemyTankController etc)
	{
		enemyTankController = etc;
	}

	public float Update(bool isShooting)
	{
		if (isShooting && Time.fixedTime - lastTimeShot >= shootInterval)
		{
			SoundManager.instance.PlaySleeveFallSound();
			GameplayCommons.Instance.effectsSpawner.SpawnProjectileSleevesEffect(enemyTankController.TankTurret.transform, EffectsSpawner.SleeveType.Cannon);
			enemyTankController.SetMaxTurretShiftFactor();
			lastTimeShot = Time.fixedTime;
			SoundManager.instance.PlayCannonShotSound();
			Vector3 barrelPoint = enemyTankController.GetBarrelPoint();
			GameObject gameObject = Object.Instantiate(Prefabs.cannonBulletPrefab, barrelPoint, Quaternion.identity);
			gameObject.GetComponent<CannonBullet>().Initialize(10f, 0.04f, enemyTankController.TankTurret.transform.right, isPlayerCannon: false, bossWeapon);
			GameplayCommons.Instance.effectsSpawner.SpawnSmoke(barrelPoint, 2, 0f);
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
