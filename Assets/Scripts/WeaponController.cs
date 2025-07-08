internal interface WeaponController
{
	WeaponTypes WeaponType
	{
		get;
	}

	float MinimumPlayerDistanceWhileChasing
	{
		get;
	}

	bool InstantlyForgetPlayerOnSightLoss
	{
		get;
	}

	bool BossWeapon
	{
		get;
		set;
	}

	float ShootAngleTreshhold
	{
		get;
	}

	float Update(bool isShooting);

	bool BouncesCursor();

	bool ShowCursor();

	bool EligibleForDownwardsNoAmmoSelect();

	bool IsLoud();

	float GetAutoaimTurretRotationSpeedMod();
}
