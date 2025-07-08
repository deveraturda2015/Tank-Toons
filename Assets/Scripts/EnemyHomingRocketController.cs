using UnityEngine;

internal class EnemyHomingRocketController : WeaponController
{
	private float lastTimeShot;

	private float shootInterval = 1.3f;

	private bool bossWeapon;

	private EnemyTankController enemyTankController;

	public WeaponTypes WeaponType => WeaponTypes.homingRocket;

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

	public EnemyHomingRocketController(EnemyTankController etc)
	{
		enemyTankController = etc;
	}

	public float Update(bool isShooting)
	{
		if (isShooting && Time.fixedTime - lastTimeShot >= shootInterval)
		{
			lastTimeShot = Time.fixedTime;
			enemyTankController.SetMaxTurretShiftFactor();
			GameObject gameObject = Object.Instantiate(Prefabs.EnemyHomingRocketPrefab, enemyTankController.TankBase.transform.position, Quaternion.identity);
			SoundManager.instance.PlayHomingMissileShotSound();
			GameplayCommons.Instance.effectsSpawner.SpawnMissileLaunchSmokeParticles(enemyTankController.TankBase.transform.position, enemyTankController.TankTurret.transform.rotation);
			gameObject.GetComponent<HomingRocket>().SetEnemysRocket(enemyTankController.TankTurret.transform.rotation, bossWeapon);
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
