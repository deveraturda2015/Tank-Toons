using UnityEngine;

internal class ShockerController : WeaponController
{
	private float windupValue;

	private float windupMax = 0.2f;

	private bool bossWeapon;

	private ShockerBeam sb;

	private bool isPlayerController;

	private EnemyTankController enemyTankController;

	private bool didShot;

	public WeaponTypes WeaponType => WeaponTypes.shocker;

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

	public ShockerController(EnemyTankController etc)
	{
		enemyTankController = etc;
		if (etc == null)
		{
			isPlayerController = true;
			windupMax = 0.2f;
		}
		sb = Object.Instantiate(Prefabs.shockerBeamPrefab).GetComponent<ShockerBeam>();
		if (!isPlayerController)
		{
			sb.InitializeEnemyBeam(etc, bossWeapon);
		}
	}

	public float Update(bool isShooting)
	{
		if (isShooting)
		{
			if (!isPlayerController)
			{
				ActuallyPerformShot();
				return Time.deltaTime;
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
					ActuallyPerformShot();
					return Time.deltaTime;
				}
				didShot = false;
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
		sb.ProcessShooting(isShooting: false);
		return 0f;
	}

	private void ActuallyPerformShot()
	{
		if (!didShot)
		{
			SoundManager.instance.PlayLaserStartSound();
			GameplayCommons.Instance.cameraController.ShakeCamera(2f);
		}
		didShot = true;
		if (isPlayerController)
		{
			GameplayCommons.Instance.playersTankController.SetMaxTurretShiftFactor();
		}
		else
		{
			enemyTankController.SetMaxTurretShiftFactor();
		}
		sb.ProcessShooting(isShooting: true);
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
		return true;
	}

	public bool IsLoud()
	{
		return true;
	}

	public float GetAutoaimTurretRotationSpeedMod()
	{
		return 0.5f;
	}
}
