using UnityEngine;

public class CannonController : WeaponController
{
	private float windupValue;

	private float windupMax = 0.2f;

	private float lastTimeShot;

	private float shootInterval = 0.8f;

	private bool didShot;

	public WeaponTypes WeaponType => WeaponTypes.cannon;

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
			else
			{
				float num = Vector2.Angle(GameplayCommons.Instance.playersTankController.TankTurret.transform.up, GameplayCommons.Instance.touchesController.ShootingVector);
				if (Time.fixedTime - lastTimeShot >= shootInterval && num < 3f)
				{
					lastTimeShot = Time.fixedTime;
					SoundManager.instance.PlaySleeveFallSound();
					GameplayCommons.Instance.effectsSpawner.SpawnProjectileSleevesEffect(GameplayCommons.Instance.playersTankController.TankTurret.transform, EffectsSpawner.SleeveType.Cannon);
					GameplayCommons.Instance.playersTankController.SetMaxTurretShiftFactor();
					Vector3 barrelPoint = GameplayCommons.Instance.playersTankController.GetBarrelPoint();
					GameObject gameObject = Object.Instantiate(Prefabs.cannonBulletPrefab, barrelPoint, Quaternion.identity);
					gameObject.GetComponent<CannonBullet>().Initialize(0f, 0f, GameplayCommons.Instance.playersTankController.TankTurret.transform.right, isPlayerCannon: true);
					SoundManager.instance.PlayCannonShotSound();
					GameplayCommons.Instance.cameraController.ShakeCamera(2f);
					Vector3 eulerAngles = GameplayCommons.Instance.playersTankController.TankTurret.transform.rotation.eulerAngles;
					GameplayCommons.Instance.effectsSpawner.SpawnMissileLaunchSmokeParticles(barrelPoint, Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z + 180f));
					didShot = true;
					return 1f;
				}
			}
		}
		else
		{
			if (windupValue == windupMax && didShot)
			{
				didShot = false;
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
