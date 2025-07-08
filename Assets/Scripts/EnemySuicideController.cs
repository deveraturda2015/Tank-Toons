public class EnemySuicideController : WeaponController
{
	public WeaponTypes WeaponType => WeaponTypes.suicide;

	public float MinimumPlayerDistanceWhileChasing => 0f;

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

	public EnemySuicideController(EnemyTankController etc)
	{
	}

	public float Update(bool isShooting)
	{
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
