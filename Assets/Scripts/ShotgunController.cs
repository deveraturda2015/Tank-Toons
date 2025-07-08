using UnityEngine;

public class ShotgunController : WeaponController
{
	private float windupValue;

	private float windupMax = 0.2f;

	private float lastTimeShot;

	private float shootInterval = 0.7f;

	private int bulletsCount = 10;

	private bool bossWeapon;

	private bool didShot;

	private bool isPlayerController;

	private EnemyTankController enemyTankController;

	public WeaponTypes WeaponType => WeaponTypes.shotgun;

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

	public ShotgunController(EnemyTankController etc)
	{
		enemyTankController = etc;
		if (etc == null)
		{
			isPlayerController = true;
			shootInterval = 0.5f;
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
			didShot = false;
		}
		return 0f;
	}

	private bool ActuallyPerformShot()
	{
		if (Time.fixedTime - lastTimeShot >= shootInterval)
		{
			if (isPlayerController)
			{
				GameplayCommons.Instance.effectsSpawner.SpawnProjectileSleevesEffect(GameplayCommons.Instance.playersTankController.TankTurret.transform, EffectsSpawner.SleeveType.Shotgun);
			}
			else
			{
				GameplayCommons.Instance.effectsSpawner.SpawnProjectileSleevesEffect(enemyTankController.TankTurret.transform, EffectsSpawner.SleeveType.Shotgun);
			}
			SoundManager.instance.PlaySleeveFallSound();
			didShot = true;
			lastTimeShot = Time.fixedTime;
			Vector3 zero = Vector3.zero;
			Vector3 barrelPoint = GameplayCommons.Instance.playersTankController.GetBarrelPoint();
			float num;
			float angleFix;
			if (isPlayerController)
			{
				num = PlayerBalance.shotguDamageValues[GlobalCommons.Instance.globalGameStats.WeaponsLevels[1]];
				zero = barrelPoint;
				GameplayCommons.Instance.playersTankController.SetMaxTurretShiftFactor();
				angleFix = 25f;
				GameplayCommons.Instance.cameraController.ShakeCamera(2f);
				Vector3 eulerAngles = GameplayCommons.Instance.playersTankController.TankTurret.transform.rotation.eulerAngles;
				GameplayCommons.Instance.effectsSpawner.SpawnMissileLaunchSmokeParticles(zero, Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z + 180f));
			}
			else
			{
				num = EnemiesBalance.GetEnemyDamage(WeaponType);
				if (bossWeapon)
				{
					num *= EnemiesBalance.bossDamageCoeff;
				}
				enemyTankController.SetMaxTurretShiftFactor();
				zero = enemyTankController.GetBarrelPoint();
				angleFix = 40f;
			}
			for (int i = 0; i < bulletsCount; i++)
			{
				GameplayCommons.Instance.gameObjectPools.InitializeSimpleBullet(zero, angleFix, 0.2f, enemyTankController, num);
			}
			GameplayCommons.Instance.effectsSpawner.SpawnSmoke(zero, 2, 0f);
			SoundManager.instance.PlayShotgunShotSound();
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
