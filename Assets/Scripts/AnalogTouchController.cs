using DG.Tweening;
using System;
using UnityEngine;

internal class AnalogTouchController : TouchController
{
	private const int INACTIVE_TOUCH_ID = -1;

	private bool touchActive;

	private bool touchHolding;

	internal int touchId = -1;

	public Vector2 initialTouchCoords;

	public Vector2 lastInitialTouchCoords;

	public Vector2 internalDirectionVector;

	public Vector2 directionVectorToReport;

	public bool keepLastAngle;

	private float bounceScale;

	private float timeTapped;

	private float rawFilterTimeout = 0.3f;

	private GameObject tankPointer;

	private SpriteRenderer tankPointerSR;

	private bool padDisplayState;

	private SpriteRenderer analogPadTop;

	private SpriteRenderer analogPadCenter;

	private Camera cam;

	private bool pointerVisibilityState;

	private TouchesController.TankPointerType pointerType;

	private float initialPointerScale;

	private int upperCursorLayerID;

	private int lowerCursorLayerID;

	private bool leftInitialTouchPosition;

	private float deadZoneToUse;

	private const float PAD_ALPHA = 0.66f;

	private const float PAD_SCALE = 3f;

	private const float PAD_APPEARTIME = 0.2f;

	private const float PAD_DISAPPEARTIME = 0.33f;

	private DateTime? LastTimeTouchCompleted;

	public override bool KeepLastAngle
	{
		set
		{
			keepLastAngle = value;
		}
	}

	public override int TouchID => touchId;

	public override bool TouchActive
	{
		get
		{
			if (GameplayCommons.Instance.touchesController.WeaponsMenuActive)
			{
				return false;
			}
			return touchActive;
		}
	}

	public override bool TouchHolding => touchHolding && !GameplayCommons.Instance.playersTankController.PlayerDead && !GameplayCommons.Instance.levelStateController.GameplayStopped;

	public Vector2 AnalogTopCoords
	{
		get
		{
			if (internalDirectionVector.magnitude < GlobalCommons.Instance.AnalogMax)
			{
				return new Vector2(initialTouchCoords.x + internalDirectionVector.x, initialTouchCoords.y + internalDirectionVector.y);
			}
			Vector2 vector = internalDirectionVector.normalized * GlobalCommons.Instance.AnalogMax;
			return new Vector2(initialTouchCoords.x + vector.x, initialTouchCoords.y + vector.y);
		}
	}

	public override Vector2 RawDirectionVectorWithTimeoutToFiltered
	{
		get
		{
			if (Time.fixedTime - timeTapped >= rawFilterTimeout)
			{
				return GetAnalogueVector(normalized: true);
			}
			return GetAnalogueVector(normalized: false);
		}
	}

	public AnalogTouchController(TouchesController.TankPointerType pointerType)
	{
		upperCursorLayerID = SortingLayer.NameToID("TutorialText");
		lowerCursorLayerID = SortingLayer.NameToID("TankShootPointer");
		this.pointerType = pointerType;
		if (GlobalCommons.Instance.globalGameStats.ShowGamePads)
		{
			switch (pointerType)
			{
			case TouchesController.TankPointerType.movePointer:
				analogPadCenter = UnityEngine.Object.Instantiate(Prefabs.AnalogPadMoving).GetComponent<SpriteRenderer>();
				break;
			case TouchesController.TankPointerType.shootPointer:
				analogPadCenter = UnityEngine.Object.Instantiate(Prefabs.AnalogPadAiming).GetComponent<SpriteRenderer>();
				break;
			}
			analogPadTop = UnityEngine.Object.Instantiate(Prefabs.AnalogPadTop).GetComponent<SpriteRenderer>();
			analogPadTop.transform.localScale = new Vector3(3f, 3f, 3f);
			analogPadCenter.transform.localScale = new Vector3(3f, 3f, 3f);
			analogPadTop.SetAlpha(0f);
			analogPadCenter.SetAlpha(0f);
		}
		switch (pointerType)
		{
		case TouchesController.TankPointerType.movePointer:
			deadZoneToUse = GlobalCommons.Instance.analogDeadzone;
			tankPointer = UnityEngine.Object.Instantiate(Prefabs.tankMovePointerPrefab);
			break;
		case TouchesController.TankPointerType.shootPointer:
			if (GlobalCommons.Instance.globalGameStats.AutoAimEnabled)
			{
				deadZoneToUse = GlobalCommons.Instance.analogDeadzone * 1.55f;
			}
			else
			{
				deadZoneToUse = GlobalCommons.Instance.analogDeadzone * 1.25f;
			}
			tankPointer = UnityEngine.Object.Instantiate(Prefabs.tankShootPointerPrefab);
			break;
		}
		tankPointerSR = tankPointer.GetComponent<SpriteRenderer>();
		tankPointerSR.SetAlpha(0f);
		cam = UnityEngine.Object.FindObjectOfType<Camera>();
		if (pointerType == TouchesController.TankPointerType.shootPointer)
		{
			float num = 1f;
			Transform transform = tankPointer.transform;
			Vector3 localScale = tankPointer.transform.localScale;
			float x = localScale.x * num;
			Vector3 localScale2 = tankPointer.transform.localScale;
			float y = localScale2.y * num;
			Vector3 localScale3 = tankPointer.transform.localScale;
			transform.localScale = new Vector3(x, y, localScale3.z);
		}
		Vector3 localScale4 = tankPointer.transform.localScale;
		initialPointerScale = localScale4.x;
		bounceScale = initialPointerScale * 1.25f;
	}

	public override void UpdateControlsPosition()
	{
		bool flag = false;
		if (!GameplayCommons.Instance.playersTankController.PlayerDead && touchActive && !GameplayCommons.Instance.GamePaused && !GameplayCommons.Instance.touchesController.WeaponsMenuActive && !GameplayCommons.Instance.levelStateController.GameplayStopped && GameplayCommons.Instance.LevelFullyInitialized)
		{
			Vector3 vector = (!(GameplayCommons.Instance.weaponsController.ActiveGuidedRocket == null)) ? GameplayCommons.Instance.weaponsController.ActiveGuidedRocket.transform.position : GameplayCommons.Instance.playersTankController.TankBase.transform.position;
			Vector2 vector2 = Camera.main.ScreenToWorldPoint(initialTouchCoords + internalDirectionVector) - Camera.main.ScreenToWorldPoint(initialTouchCoords);
			if (vector2.magnitude > 1f)
			{
				if (tankPointerSR.sortingLayerID != upperCursorLayerID)
				{
					tankPointerSR.sortingLayerID = upperCursorLayerID;
				}
			}
			else if (tankPointerSR.sortingLayerID != lowerCursorLayerID)
			{
				tankPointerSR.sortingLayerID = lowerCursorLayerID;
			}
			Transform transform = tankPointer.transform;
			float x = vector.x + vector2.x;
			float y = vector.y + vector2.y;
			Vector3 position = tankPointer.transform.position;
			transform.position = new Vector3(x, y, position.z);
			switch (pointerType)
			{
			case TouchesController.TankPointerType.movePointer:
			{
				float z = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f - 90f;
				tankPointer.transform.rotation = Quaternion.Euler(0f, 0f, z);
				if (!GameplayCommons.Instance.playersTankController.PlayerDead && !GameplayCommons.Instance.weaponsController.GuidedRocketAftershotPeriodActive)
				{
					flag = true;
				}
				break;
			}
			case TouchesController.TankPointerType.shootPointer:
				if (!GameplayCommons.Instance.playersTankController.PlayerDead && GameplayCommons.Instance.weaponsController.ActiveGuidedRocket == null && !GameplayCommons.Instance.levelStateController.GameplayStopped && GameplayCommons.Instance.weaponsController.SelectedWeaponController.ShowCursor() && (!GlobalCommons.Instance.globalGameStats.AutoAimEnabled || leftInitialTouchPosition))
				{
					flag = true;
				}
				break;
			default:
				throw new Exception("No behavior specified for analog touch cursor");
			}
		}
		if (pointerVisibilityState != flag)
		{
			pointerVisibilityState = flag;
			if (flag)
			{
				tankPointerSR.DOKill();
				tankPointerSR.DOFade(1f, 0.05f).SetUpdate(isIndependentUpdate: true);
			}
			else
			{
				tankPointerSR.DOKill();
				tankPointerSR.DOFade(0f, 0.25f).SetUpdate(isIndependentUpdate: true);
			}
		}
	}

	public override bool IsAutoaiming()
	{
		if (pointerType != TouchesController.TankPointerType.shootPointer)
		{
			throw new Exception();
		}
		return GlobalCommons.Instance.globalGameStats.AutoAimEnabled && touchActive && !leftInitialTouchPosition;
	}

	public override bool ShowAutoaimCursor()
	{
		if (pointerType != TouchesController.TankPointerType.shootPointer)
		{
			throw new Exception();
		}
		return GlobalCommons.Instance.globalGameStats.AutoAimEnabled && touchHolding && !leftInitialTouchPosition && GameplayCommons.Instance.weaponsController.SelectedWeaponController.ShowCursor();
	}

	public void Update()
	{
		if (!GlobalCommons.Instance.globalGameStats.ShowGamePads)
		{
			return;
		}
		if (touchId != -1)
		{
			if (!padDisplayState)
			{
				analogPadCenter.transform.DOKill();
				analogPadCenter.transform.localScale = new Vector3(3f, 3f, 3f);
				analogPadTop.transform.DOKill();
				analogPadTop.transform.localScale = new Vector3(3f, 3f, 3f);
				analogPadTop.DOKill();
				analogPadTop.DOFade(0.66f, 0.1f);
				analogPadCenter.DOKill();
				analogPadCenter.DOFade(0.66f, 0.2f);
				padDisplayState = true;
			}
			Vector3 vector = Camera.main.ScreenToWorldPoint(initialTouchCoords);
			Transform transform = analogPadCenter.transform;
			float x = vector.x;
			float y = vector.y;
			Vector3 position = analogPadCenter.transform.position;
			transform.position = new Vector3(x, y, position.z);
		}
		else if (padDisplayState)
		{
			analogPadTop.DOKill();
			analogPadTop.DOFade(0f, 0.33f);
			analogPadCenter.DOKill();
			analogPadCenter.DOFade(0f, 0.33f);
			float endValue = 1.5f;
			analogPadCenter.transform.DOScale(endValue, 0.33f);
			analogPadTop.transform.DOScale(endValue, 0.33f);
			padDisplayState = false;
		}
	}

	public override void BouncePointer()
	{
		tankPointer.transform.DOKill();
		Transform transform = tankPointer.transform;
		float x = bounceScale;
		float y = bounceScale;
		Vector3 localScale = tankPointer.transform.localScale;
		transform.localScale = new Vector3(x, y, localScale.z);
		tankPointer.transform.DOScale(initialPointerScale, 0.12f);
		float z = UnityEngine.Random.Range(-13f, 13f);
		tankPointer.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, z));
		tankPointer.transform.DORotate(Vector3.zero, 0.12f);
	}

	public override void ProcessTouchStart(Touch touch)
	{
		leftInitialTouchPosition = false;
		touchId = touch.fingerId;
		touchActive = (!GlobalCommons.Instance.globalGameStats.AutoAimEnabled || AutoAimHelper.UpdateAutoaimLogicAndGetVector().HasValue);
		touchHolding = true;
		if (GlobalCommons.Instance.AnalogControlsFilterMilliseconds > 0 && LastTimeTouchCompleted.HasValue && DateTime.Now - LastTimeTouchCompleted.Value < TimeSpan.FromMilliseconds(GlobalCommons.Instance.AnalogControlsFilterMilliseconds))
		{
			initialTouchCoords = lastInitialTouchCoords;
		}
		else
		{
			initialTouchCoords = touch.position;
		}
		internalDirectionVector = Vector2.zero;
		if (!keepLastAngle)
		{
			directionVectorToReport = Vector2.zero;
		}
		timeTapped = Time.fixedTime;
		Update();
	}

	public override void PersistTouch(Touch touch)
	{
		UpdateCurrentDirectionVector(touch);
		Update();
	}

	public override void ForceSetTouchHolding(bool val)
	{
		touchHolding = val;
	}

	public override void ProcessTouchMove(Touch touch)
	{
		UpdateCurrentDirectionVector(touch);
		Update();
	}

	private void UpdateCurrentDirectionVector(Touch touch)
	{
		touchActive = true;
		if (GlobalCommons.Instance.globalGameStats.ShowGamePads && padDisplayState)
		{
			Vector3 vector = Camera.main.ScreenToWorldPoint(touch.position);
			Transform transform = analogPadTop.transform;
			float x = vector.x;
			float y = vector.y;
			Vector3 position = analogPadTop.transform.position;
			transform.position = new Vector3(x, y, position.z);
		}
		internalDirectionVector = touch.position - initialTouchCoords;
		if (!keepLastAngle)
		{
			directionVectorToReport = internalDirectionVector;
		}
		else if (internalDirectionVector.magnitude > deadZoneToUse || leftInitialTouchPosition)
		{
			directionVectorToReport = internalDirectionVector.normalized * GlobalCommons.Instance.AnalogMax;
			leftInitialTouchPosition = true;
		}
		else if (GlobalCommons.Instance.globalGameStats.AutoAimEnabled)
		{
			Vector2? vector2 = AutoAimHelper.UpdateAutoaimLogicAndGetVector();
			if (vector2.HasValue)
			{
				directionVectorToReport = vector2.Value;
			}
			else
			{
				touchActive = false;
			}
		}
	}

	public override void EndTouch(Touch touch)
	{
		UpdateCurrentDirectionVector(touch);
		CompleteTouch();
		Update();
	}

	public override void ForceEndTouch()
	{
		CompleteTouch();
	}

	private void CompleteTouch()
	{
		LastTimeTouchCompleted = DateTime.Now;
		lastInitialTouchCoords = initialTouchCoords;
		touchId = -1;
		touchActive = false;
		touchHolding = false;
		if (!keepLastAngle)
		{
			directionVectorToReport = Vector2.zero;
		}
	}

	private Vector2 GetAnalogueVector(bool normalized)
	{
		Vector2 vector = (!normalized) ? internalDirectionVector : directionVectorToReport;
		float num = vector.magnitude;
		if (num > GlobalCommons.Instance.AnalogMax)
		{
			num = GlobalCommons.Instance.AnalogMax;
		}
		if (num < deadZoneToUse)
		{
			num = 0f;
		}
		num /= GlobalCommons.Instance.AnalogMax;
		if (float.IsNaN(num))
		{
			return vector.normalized;
		}
		return vector.normalized * num;
	}

	public override Vector2 GetNormalizedDirectionVector()
	{
		return GetAnalogueVector(normalized: true);
	}

	private void SetGameobjectToScreenPosition(Vector2 screenPt, GameObject go)
	{
		Vector2 vector = cam.ScreenToWorldPoint(screenPt);
		Transform transform = go.transform;
		float x = vector.x;
		float y = vector.y;
		Vector3 position = go.transform.position;
		transform.position = new Vector3(x, y, position.z);
	}
}
