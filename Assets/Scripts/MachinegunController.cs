using UnityEngine;

public class MachinegunController : WeaponController
{
	private float windupValue;

	private float windupMax = 0.2f;

	private float lastTimeShot;

	private float shootInterval = 0.11f;

	private bool bossWeapon;

	private bool isPlayerController;

	private EnemyTankController enemyTankController;

	private bool didShot;

	public WeaponTypes WeaponType => WeaponTypes.machinegun;

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

	public MachinegunController(EnemyTankController etc)
	{
		enemyTankController = etc;
		if (etc == null)
		{
			isPlayerController = true;
			shootInterval = PlayerBalance.MachinegunShootIntervals[GlobalCommons.Instance.globalGameStats.WeaponsLevels[0]];
		}
		else
		{
			isPlayerController = false;
		}
	}

	public float Update(bool isShooting)
	{
		if (isShooting)
		{
			if (!isPlayerController)
			{
				ActuallyPerformShot();
				return 1f;
			}
			if (!GlobalCommons.Instance.globalGameStats.AutoAimEnabled && windupValue < windupMax)
			{
				windupValue += Time.deltaTime;
				if (windupValue > windupMax)
				{
					windupValue = windupMax;
				}
				didShot = false;
			}
			else
			{
				float num = Vector2.Angle(GameplayCommons.Instance.playersTankController.TankTurret.transform.up, GameplayCommons.Instance.touchesController.ShootingVector);
				if (num < 10f)
				{
					if (GameplayCommons.Instance.touchesController.ShootTouchController.IsAutoaiming() && !didShot)
					{
						didShot = true;
						return 0f;
					}
					if (ActuallyPerformShot())
					{
						return 1f;
					}
				}
				else
				{
					didShot = false;
				}
			}
		}
		else
		{
			if (windupValue > 0f)
			{
				windupValue -= Time.deltaTime;
			}
			if (windupValue < 0f)
			{
				windupValue = 0f;
			}
			if (didShot)
			{
				SoundManager.instance.PlaySleeveFallSound();
				if (isPlayerController)
				{
					GameplayCommons.Instance.effectsSpawner.SpawnSmoke(GameplayCommons.Instance.playersTankController.GetBarrelPoint(), 1, 0f);
				}
				didShot = false;
			}
		}
		return 0f;
	}

	private bool ActuallyPerformShot()
	{
		if (Time.fixedTime - lastTimeShot >= shootInterval)
		{
			if (isPlayerController)
			{
				GameplayCommons.Instance.effectsSpawner.SpawnProjectileSleevesEffect(GameplayCommons.Instance.playersTankController.TankTurret.transform, EffectsSpawner.SleeveType.Machinegun);
			}
			else
			{
				GameplayCommons.Instance.effectsSpawner.SpawnProjectileSleevesEffect(enemyTankController.TankTurret.transform, EffectsSpawner.SleeveType.Machinegun);
			}
			lastTimeShot = Time.fixedTime;
			didShot = true;
			float bulletDamage;
			Vector3 barrelPoint;
			if (isPlayerController)
			{
				bulletDamage = 1f;
				barrelPoint = GameplayCommons.Instance.playersTankController.GetBarrelPoint();
				GameplayCommons.Instance.playersTankController.SetMaxTurretShiftFactor();
			}
			else
			{
				bulletDamage = EnemiesBalance.GetEnemyDamage(WeaponType);
				enemyTankController.SetMaxTurretShiftFactor();
				barrelPoint = enemyTankController.GetBarrelPoint();
			}
			if (isPlayerController)
			{
				GameplayCommons.Instance.effectsSpawner.CreateShotFlareEffect(barrelPoint);
			}
			SoundManager.instance.PlayMachinegunShotSound();
			GameplayCommons.Instance.gameObjectPools.InitializeSimpleBullet(barrelPoint, 10f, 0.04f, enemyTankController, bulletDamage);
			return true;
		}
		return false;
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
		return true;
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
