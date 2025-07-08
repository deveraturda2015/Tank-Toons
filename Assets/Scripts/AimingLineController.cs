using System;
using UnityEngine;

public class AimingLineController : MonoBehaviour
{
	private LineRenderer lr;

	private GameObject aimPoint;

	private SpriteRenderer aimPointSR;

	private bool isActive;

	private int turnOnFilter;

	public const float AIMING_LINE_DISTANCE = 9f;

	private void Start()
	{
		lr = GetComponent<LineRenderer>();
		lr.SetWidth(0.06f, 0.03f);
		lr.sortingLayerName = "PlayerAimingLine";
		aimPoint = UnityEngine.Object.Instantiate(Prefabs.laserPointerPrefab);
		aimPoint.transform.parent = base.transform;
		aimPointSR = aimPoint.GetComponent<SpriteRenderer>();
		SetLineActive(val: false);
	}

	private void SetLineActive(bool val)
	{
		if (!Input.touchSupported)
		{
			val = false;
		}
		if (val)
		{
			turnOnFilter++;
		}
		else
		{
			turnOnFilter = 0;
		}
		bool enabled = (turnOnFilter > 1) ? true : false;
		aimPointSR.enabled = enabled;
		lr.enabled = enabled;
		isActive = val;
	}

	private void Update()
	{
		if (GameplayCommons.Instance.playersTankController.PlayerDead || GameplayCommons.Instance.weaponsController.ActiveGuidedRocket != null || !ShowLineForWeapon(GameplayCommons.Instance.weaponsController.SelectedWeapon) || GameplayCommons.Instance.GamePaused || !GameplayCommons.Instance.playersTankController.PlayerActive || GameplayCommons.Instance.levelStateController.GameplayStopped)
		{
			if (isActive)
			{
				SetLineActive(val: false);
			}
			return;
		}
		if (Input.touchSupported)
		{
			SetLineActive(GameplayCommons.Instance.touchesController.ShootTouchController.TouchActive);
		}
		else
		{
			SetLineActive(val: true);
		}
		Quaternion rotation = Quaternion.Euler(0f, 0f, 90f);
		Transform transform = GameplayCommons.Instance.playersTankController.TankTurret.transform;
		Vector3 v = rotation * transform.right;
		RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, v, 9f, LayerMasks.obstaclesEnemiesSpawnersDestrObstaclesLayerMask);
		Vector3[] array = new Vector3[2]
		{
			transform.position,
			transform.position + v.normalized * 9f
		};
		if (raycastHit2D.collider != null)
		{
			array[1] = raycastHit2D.point;
		}
		aimPoint.transform.position = array[1];
		lr.SetPositions(array);
	}

	public bool ShowLineForWeapon(WeaponTypes weaponType)
	{
		switch (weaponType)
		{
		case WeaponTypes.machinegun:
			return false;
		case WeaponTypes.shotgun:
			return false;
		case WeaponTypes.minigun:
			return false;
		case WeaponTypes.cannon:
			return false;
		case WeaponTypes.homingRocket:
			return false;
		case WeaponTypes.mines:
			return false;
		case WeaponTypes.guidedRocket:
			return false;
		case WeaponTypes.laser:
			return false;
		case WeaponTypes.railgun:
			return true;
		case WeaponTypes.ricochet:
			return true;
		case WeaponTypes.triple:
			return false;
		case WeaponTypes.shocker:
			return false;
		default:
			throw new Exception("unknown aiming line behavior for weapon type");
		}
	}
}
