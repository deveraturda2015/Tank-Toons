using System.Collections.Generic;
using UnityEngine;

public class TouchesController : MonoBehaviour
{
	public enum TankPointerType
	{
		movePointer,
		shootPointer
	}

	public enum TouchLocation
	{
		Move,
		Shoot,
		SelectWeapon,
		ZoomOut,
		Other
	}

	private TouchController moveTouchController;

	private TouchController shootTouchController;

	private WeaponSelectionTouchController weaponSelectionTouchController;

	private ZoomOutTouchController zoomOutTouchController;

	private List<TouchController> allTouchControllers;

	private Vector2 keyboardShootingVector = new Vector2(0f, 0f);

	internal TouchController MoveTouchController => moveTouchController;

	internal TouchController ShootTouchController => shootTouchController;

	public bool WeaponsMenuActive => weaponSelectionTouchController.TouchActive;

	public Vector2 MovementVector
	{
		get
		{
			if (!Input.touchSupported)
			{
				Vector2 result = new Vector2(0f, 0f);
				if (UnityEngine.Input.GetKey(KeyCode.A))
				{
					result = new Vector2(-1f, result.y);
				}
				else if (UnityEngine.Input.GetKey(KeyCode.D))
				{
					result = new Vector2(1f, result.y);
				}
				if (UnityEngine.Input.GetKey(KeyCode.W))
				{
					result = new Vector2(result.x, 1f);
				}
				else if (UnityEngine.Input.GetKey(KeyCode.S))
				{
					result = new Vector2(result.x, -1f);
				}
				return result;
			}
			return moveTouchController.GetNormalizedDirectionVector();
		}
	}

	public Vector2 ShootingVector
	{
		get
		{
			if (!Input.touchSupported)
			{
				Vector2? vector = null;
				vector = AutoAimHelper.UpdateAutoaimLogicAndGetVector();
				if (vector.HasValue && UnityEngine.Input.GetKey(KeyCode.U))
				{
					return vector.Value;
				}
				bool flag = true;
				bool flag2 = true;
				if (UnityEngine.Input.GetKey(KeyCode.J))
				{
					keyboardShootingVector = new Vector2(-1f, keyboardShootingVector.y);
				}
				else if (UnityEngine.Input.GetKey(KeyCode.L))
				{
					keyboardShootingVector = new Vector2(1f, keyboardShootingVector.y);
				}
				else
				{
					flag = false;
					keyboardShootingVector = new Vector2(keyboardShootingVector.x, keyboardShootingVector.y);
				}
				if (UnityEngine.Input.GetKey(KeyCode.I))
				{
					keyboardShootingVector = new Vector2(keyboardShootingVector.x, 1f);
				}
				else if (UnityEngine.Input.GetKey(KeyCode.K))
				{
					keyboardShootingVector = new Vector2(keyboardShootingVector.x, -1f);
				}
				else
				{
					flag2 = false;
					keyboardShootingVector = new Vector2(keyboardShootingVector.x, keyboardShootingVector.y);
				}
				if (flag && !flag2)
				{
					keyboardShootingVector = new Vector2(keyboardShootingVector.x, 0f);
				}
				if (flag2 && !flag)
				{
					keyboardShootingVector = new Vector2(0f, keyboardShootingVector.y);
				}
				return keyboardShootingVector;
			}
			return shootTouchController.GetNormalizedDirectionVector();
		}
	}

	private void Start()
	{
		if (GlobalCommons.Instance.globalGameStats.UseStaticControls)
		{
			moveTouchController = new StationeryAnalogTouchController(TankPointerType.movePointer);
			shootTouchController = new StationeryAnalogTouchController(TankPointerType.shootPointer);
		}
		else
		{
			moveTouchController = new AnalogTouchController(TankPointerType.movePointer);
			shootTouchController = new AnalogTouchController(TankPointerType.shootPointer);
		}
		shootTouchController.KeepLastAngle = true;
		weaponSelectionTouchController = new WeaponSelectionTouchController();
		zoomOutTouchController = new ZoomOutTouchController();
		allTouchControllers = new List<TouchController>
		{
			moveTouchController,
			shootTouchController,
			weaponSelectionTouchController,
			zoomOutTouchController
		};
	}

	public void ResetAllTouches()
	{
		for (int i = 0; i < allTouchControllers.Count; i++)
		{
			TouchController touchController = allTouchControllers[i];
			touchController.ForceEndTouch();
		}
	}

	private void Update()
	{
		int touchCount = UnityEngine.Input.touchCount;
		for (int i = 0; i < touchCount; i++)
		{
			Touch touch = UnityEngine.Input.GetTouch(i);
			switch (touch.phase)
			{
			case TouchPhase.Began:
			{
				if (!(Time.timeScale > 0f))
				{
					break;
				}
				TouchLocation touchLocation = GetTouchLocation(touch.position);
				bool flag = false;
				Vector3 v = Camera.main.ScreenToWorldPoint(touch.position);
				if (GameplayCommons.Instance != null)
				{
					if (GameplayCommons.Instance.playersTankController != null && GameplayCommons.Instance.playersTankController.TankBase != null && Vector2.Distance(v, GameplayCommons.Instance.playersTankController.TankBase.transform.position) < 1.5f)
					{
						flag = true;
					}
					if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.TutorialLevel && touchLocation == TouchLocation.Shoot && !GameplayCommons.Instance.tutorialController.ShootingEnabled)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					TouchController touchController = GetControllerForLocation(touchLocation);
					if (touchController != null && !touchController.TouchActive)
					{
						touchController.ProcessTouchStart(touch);
					}
				}
				break;
			}
			case TouchPhase.Stationary:
				for (int k = 0; k < allTouchControllers.Count; k++)
				{
					TouchController touchController = allTouchControllers[k];
					if (touchController.TouchID == touch.fingerId)
					{
						touchController.PersistTouch(touch);
					}
				}
				break;
			case TouchPhase.Moved:
				for (int j = 0; j < allTouchControllers.Count; j++)
				{
					TouchController touchController = allTouchControllers[j];
					if (touchController.TouchID == touch.fingerId)
					{
						touchController.ProcessTouchMove(touch);
					}
				}
				break;
			case TouchPhase.Ended:
				ProcessTouchEnd(touch.fingerId, touch);
				break;
			case TouchPhase.Canceled:
				ProcessTouchEnd(touch.fingerId, touch);
				break;
			}
		}
		bool flag2 = shootTouchController.TouchActive;
		if (!Input.touchSupported)
		{
			if ((UnityEngine.Input.GetKey(KeyCode.J) || UnityEngine.Input.GetKey(KeyCode.I) || UnityEngine.Input.GetKey(KeyCode.L) || UnityEngine.Input.GetKey(KeyCode.K) || UnityEngine.Input.GetKey(KeyCode.U)) && (GlobalCommons.Instance.gameplayMode != GlobalCommons.GameplayModes.TutorialLevel || GameplayCommons.Instance.tutorialController.ShootingEnabled))
			{
				flag2 = true;
			}
			shootTouchController.ForceSetTouchHolding(flag2);
			if (UnityEngine.Input.GetKey(KeyCode.Space))
			{
				GameplayCommons.Instance.weaponSwitchController.FadeIn();
			}
			else
			{
				GameplayCommons.Instance.weaponSwitchController.FadeOut();
			}
			if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
			{
				GameplayCommons.Instance.cameraController.SetZoomState(zoomOut: true);
			}
			else
			{
				GameplayCommons.Instance.cameraController.SetZoomState(zoomOut: false);
			}
		}
		if (GameplayCommons.Instance.playersTankController.PlayerActive)
		{
			GameplayCommons.Instance.weaponsController.Update(flag2);
		}
		moveTouchController.UpdateControlsPosition();
		shootTouchController.UpdateControlsPosition();
	}

	private TouchLocation GetTouchLocation(Vector2 position)
	{
		if (GameplayCommons.Instance.gameplayUIController.CheckPauseButtonHit(position))
		{
			return TouchLocation.Other;
		}
		float num = (float)Screen.height / 4.8f / 2f * GameplayCommons.Instance.gameplayUIController.UpperButtonsScale;
		if (position.x < (float)(Screen.width / 2))
		{
			if (GameplayCommons.Instance.gameplayUIController.WeaponSelectButton.enabled)
			{
				Vector2 vector = RectTransformUtility.WorldToScreenPoint(Camera.main, GameplayCommons.Instance.gameplayUIController.WeaponSelectButton.transform.position);
				if (Mathf.Abs(position.x - vector.x) <= num && Mathf.Abs(position.y - vector.y) <= num)
				{
					return TouchLocation.SelectWeapon;
				}
				return TouchLocation.Move;
			}
			return TouchLocation.Move;
		}
		if (GameplayCommons.Instance.gameplayUIController.BinocularsButton.enabled)
		{
			Vector2 vector2 = RectTransformUtility.WorldToScreenPoint(Camera.main, GameplayCommons.Instance.gameplayUIController.BinocularsButton.transform.position);
			if (Mathf.Abs(position.x - vector2.x) <= num && Mathf.Abs(position.y - vector2.y) <= num)
			{
				return TouchLocation.ZoomOut;
			}
			return TouchLocation.Shoot;
		}
		return TouchLocation.Shoot;
	}

	private TouchController GetControllerForLocation(TouchLocation location)
	{
		switch (location)
		{
		case TouchLocation.Move:
			return moveTouchController;
		case TouchLocation.Shoot:
			return shootTouchController;
		case TouchLocation.SelectWeapon:
			return weaponSelectionTouchController;
		case TouchLocation.ZoomOut:
			return zoomOutTouchController;
		default:
			return null;
		}
	}

	private void ProcessTouchEnd(int touchId, Touch touch)
	{
		for (int i = 0; i < allTouchControllers.Count; i++)
		{
			TouchController touchController = allTouchControllers[i];
			if (touchController.TouchID == touchId)
			{
				touchController.EndTouch(touch);
			}
		}
	}
}
