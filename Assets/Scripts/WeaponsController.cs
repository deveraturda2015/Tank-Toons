using UnityEngine;

internal class WeaponsController
{
	private WeaponTypes selectedWeapon;

	private WeaponController[] weaponsControllers;

	private float[] weaponsAmmo;

	private float[] weaponsAmmoMax;

	private int[] weaponsLevels;

	public GuidedRocket ActiveGuidedRocket;

	public float lastTimeGuidedRocketExploded = -100f;

	public Vector3 lastTimeGuidedRocketExplosionCoords = Vector3.zero;

	public static float guidedRocketExplosionCameraTieout = 0.5f;

	public bool GuidedRocketAftershotPeriodActive => Time.fixedTime - lastTimeGuidedRocketExploded <= guidedRocketExplosionCameraTieout;

	public WeaponController SelectedWeaponController => weaponsControllers[(int)selectedWeapon];

	public WeaponTypes SelectedWeapon => selectedWeapon;

	public WeaponsController()
	{
		MachinegunController machinegunController = new MachinegunController(null);
		ShotgunController shotgunController = new ShotgunController(null);
		MinigunController minigunController = new MinigunController(null);
		CannonController cannonController = new CannonController();
		HomingRocketController homingRocketController = new HomingRocketController();
		LaserController laserController = new LaserController(null);
		GuidedMissilesController guidedMissilesController = new GuidedMissilesController();
		RailgunController railgunController = new RailgunController();
		MinesController minesController = new MinesController();
		RicochetController ricochetController = new RicochetController();
		TripleController tripleController = new TripleController(null);
		ShockerController shockerController = new ShockerController(null);
		weaponsControllers = new WeaponController[15]
		{
			machinegunController,
			shotgunController,
			minigunController,
			cannonController,
			homingRocketController,
			minesController,
			guidedMissilesController,
			laserController,
			railgunController,
			null,
			null,
			null,
			ricochetController,
			tripleController,
			shockerController
		};
		weaponsAmmo = new float[GlobalCommons.Instance.globalGameStats.WeaponsLevels.Length];
		weaponsAmmoMax = new float[weaponsAmmo.Length];
		weaponsLevels = GlobalCommons.Instance.globalGameStats.WeaponsLevels;
		for (int i = 0; i < weaponsLevels.Length; i++)
		{
			int num = weaponsLevels[i];
			weaponsAmmo[i] = (weaponsAmmoMax[i] = PlayerBalance.AmmoMax[i][num]);
		}
		selectedWeapon = GlobalCommons.Instance.LastSelectedWeapon;
	}

	public void Update(bool isShooting)
	{
		RicochetBullet.ProcessedVisibilityThisFrame = false;
		if (GameplayCommons.Instance.GamePaused || GameplayCommons.Instance.playersTankController.PlayerDead || GameplayCommons.Instance.levelStateController.GameplayStopped)
		{
			isShooting = false;
		}
		bool flag = false;
		if (weaponsAmmo[(int)selectedWeapon] > 0f)
		{
			flag = true;
		}
		if (!flag && selectedWeapon == WeaponTypes.guidedRocket && GameplayCommons.Instance.weaponsController.ActiveGuidedRocket != null)
		{
			flag = true;
		}
		if (flag)
		{
			float num = weaponsControllers[(int)selectedWeapon].Update(isShooting);
			if (num > 0f)
			{
				BushController.ResetHiddenTimestamp();
				weaponsAmmo[(int)selectedWeapon] -= num;
				if (weaponsAmmo[(int)selectedWeapon] < 0f)
				{
					weaponsAmmo[(int)selectedWeapon] = 0f;
				}
				GameplayCommons.Instance.gameplayUIController.UpdatePlayerAmmo(GetAmmoPercentageForWeapon(selectedWeapon));
				if (SelectedWeaponController.BouncesCursor())
				{
					GameplayCommons.Instance.touchesController.ShootTouchController.BouncePointer();
					GameplayCommons.Instance.autoaimPointerController.BouncePointer();
				}
			}
		}
		else
		{
			int num2 = (int)selectedWeapon;
			do
			{
				num2--;
			}
			while (num2 != 0 && (weaponsControllers[num2] == null || !weaponsControllers[num2].EligibleForDownwardsNoAmmoSelect() || !(weaponsAmmo[num2] > 0f)));
			SelectWeapon((WeaponTypes)num2);
		}
	}

	public void CollectAmmoBonus(WeaponTypes weaponType)
	{
		int num = Mathf.FloorToInt(weaponsAmmo[(int)weaponType] + Mathf.Ceil(weaponsAmmoMax[(int)weaponType] / 3f));
		if ((float)num > weaponsAmmoMax[(int)weaponType])
		{
			num = Mathf.FloorToInt(weaponsAmmoMax[(int)weaponType]);
		}
		weaponsAmmo[(int)weaponType] = num;
		GameplayCommons.Instance.gameplayUIController.UpdatePlayerAmmo(GetAmmoPercentageForWeapon(selectedWeapon));
	}

	public float GetAmmoPercentageForWeapon(WeaponTypes? weaponType = default(WeaponTypes?))
	{
		WeaponTypes value = selectedWeapon;
		if (weaponType.HasValue)
		{
			value = weaponType.Value;
		}
		if (value == WeaponTypes.machinegun)
		{
			return 1f;
		}
		return weaponsAmmo[(int)value] / weaponsAmmoMax[(int)value];
	}

	internal void ResetGuidedRocket(GuidedRocket guidedRocket)
	{
		ActiveGuidedRocket = null;
		lastTimeGuidedRocketExploded = Time.fixedTime;
		lastTimeGuidedRocketExplosionCoords = guidedRocket.transform.position;
	}

	public void SelectWeapon(WeaponTypes type, bool playSound = false)
	{
		weaponsControllers[(int)selectedWeapon].Update(isShooting: false);
		if (weaponsLevels[(int)type] > 0 && weaponsAmmo[(int)type] > 0f)
		{
			SoundManager.instance.PlayWeaponSwitchSound();
			selectedWeapon = type;
			GameplayCommons.Instance.playersTankController.SetTurretSprite(doBounce: true);
			GameplayCommons.Instance.gameplayUIController.UpdateWeaponSelectButtonFrame((int)type);
			GameplayCommons.Instance.gameplayUIController.UpdatePlayerAmmo(GetAmmoPercentageForWeapon(type));
		}
		else if (playSound)
		{
			SoundManager.instance.PlayNotAvailableSound();
		}
	}
}
