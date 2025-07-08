using UnityEngine;

internal class EnemyRicochetController : WeaponController
{
	private float lastTimeShot;

	private float shootInterval = 0.8f;

	private bool bossWeapon;

	private EnemyTankController enemyTankController;

	public WeaponTypes WeaponType => WeaponTypes.ricochet;

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

	public float ShootAngleTreshhold => 30f;

	public EnemyRicochetController(EnemyTankController etc)
	{
		enemyTankController = etc;
	}

	public float Update(bool isShooting)
	{
		if (isShooting && Time.fixedTime - lastTimeShot >= shootInterval)
		{
			lastTimeShot = Time.fixedTime;
			enemyTankController.SetMaxTurretShiftFactor();
			Vector3 barrelPoint = enemyTankController.GetBarrelPoint();
			GameObject gameObject = Object.Instantiate(Prefabs.RicochetBulletPrefab, barrelPoint, Quaternion.identity);
			gameObject.GetComponent<RicochetBullet>().Initialize(enemyTankController.TankTurret.transform.right, isPlayerCannon: false, bossWeapon);
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
