using UnityEngine;

public class HomingRocketController : WeaponController
{
	private float windupValue;

	private float windupMax = 0.2f;

	private float lastTimeShot;

	private float shootInterval = 0.33f;

	public WeaponTypes WeaponType => WeaponTypes.homingRocket;

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
		if (isShooting)
		{
			if (windupValue < windupMax)
			{
				windupValue += Time.deltaTime;
				if (windupValue > windupMax)
				{
					windupValue = windupMax;
				}
			}
			else if (Time.fixedTime - lastTimeShot >= shootInterval)
			{
				lastTimeShot = Time.fixedTime;
				GameplayCommons.Instance.playersTankController.SetMaxTurretShiftFactor();
				Object.Instantiate(Prefabs.homingRocketPrefab, GameplayCommons.Instance.playersTankController.TankBase.transform.position, Quaternion.identity);
				SoundManager.instance.PlayHomingMissileShotSound();
				GameplayCommons.Instance.effectsSpawner.SpawnMissileLaunchSmokeParticles(GameplayCommons.Instance.playersTankController.TankBase.transform.position, GameplayCommons.Instance.playersTankController.TankTurret.transform.rotation);
				GameplayCommons.Instance.cameraController.ShakeCamera(2f);
				return 1f;
			}
		}
		else
		{
			if (windupValue == windupMax)
			{
				GameplayCommons.Instance.effectsSpawner.SpawnSmoke(GameplayCommons.Instance.playersTankController.GetBarrelPoint(), 1, 0f);
			}
			if (windupValue > 0f)
			{
				windupValue -= Time.deltaTime;
			}
			if (windupValue < 0f)
			{
				windupValue = 0f;
			}
		}
		return 0f;
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
