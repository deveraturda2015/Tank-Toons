using UnityEngine;

public class GoldenTankWeaponController : WeaponController
{
	private EnemyTankController etc;

	private float coinsDispenseDistanceSquared = 16f;

	private float lastTimeDispensedCoin;

	private float coinsLeft;

	private float secondsToDispense = 3f;

	private float dispensePerSecond;

	public WeaponTypes WeaponType => WeaponTypes.gold;

	public float MinimumPlayerDistanceWhileChasing => 22.5f;

	public bool InstantlyForgetPlayerOnSightLoss => true;

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

	public GoldenTankWeaponController(EnemyTankController etc)
	{
		this.etc = etc;
		lastTimeDispensedCoin = -100500f;
		coinsLeft = MoneyLootCounter.GetEnemyMoneyLoot() * 12;
		dispensePerSecond = coinsLeft / secondsToDispense;
	}

	public float Update(bool isShooting)
	{
		if (etc.SeesPlayer && coinsLeft > 0f && !GameplayCommons.Instance.levelStateController.IsFreezeActive && ((Vector2)(GameplayCommons.Instance.playersTankController.TankBase.transform.position - etc.TankBase.transform.position)).sqrMagnitude <= coinsDispenseDistanceSquared)
		{
			float f = Time.deltaTime * dispensePerSecond;
			int num = Mathf.CeilToInt(f);
			if (num < 5)
			{
				num = 5;
			}
			BonusesSpawner.SpawnCoinsBonus(etc.TankBase.transform.position, num);
			coinsLeft -= num;
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
