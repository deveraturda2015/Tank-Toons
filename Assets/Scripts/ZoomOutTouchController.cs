using UnityEngine;

public class ZoomOutTouchController : TouchController
{
	internal bool touchActive;

	internal int touchId = -1;

	public override int TouchID => touchId;

	public override bool TouchActive => touchActive;

	public override bool KeepLastAngle
	{
		set
		{
		}
	}

	public override bool TouchHolding => touchActive;

	public override Vector2 RawDirectionVectorWithTimeoutToFiltered => Vector2.zero;

	public override void ProcessTouchStart(Touch touch)
	{
		touchId = touch.fingerId;
		touchActive = true;
		if (!GameplayCommons.Instance.levelStateController.GameplayStopped && GameplayCommons.Instance.weaponSwitchController.CurrentState == WeaponSwitchController.WeaponSwitchControllerState.Out && !GameplayCommons.Instance.GamePaused)
		{
			GameplayCommons.Instance.cameraController.SetZoomState(zoomOut: true);
			if (Time.timeScale == 1f)
			{
				SoundManager.instance.PlaySwooshInSound();
			}
		}
	}

	public override void ProcessTouchMove(Touch touch)
	{
	}

	public override void PersistTouch(Touch touch)
	{
	}

	public void Update()
	{
	}

	public override void EndTouch(Touch touch)
	{
		CompleteTouch();
	}

	public override void ForceEndTouch()
	{
		CompleteTouch();
	}

	private void CompleteTouch()
	{
		touchId = -1;
		touchActive = false;
		GameplayCommons.Instance.cameraController.SetZoomState(zoomOut: false);
		if (!GameplayCommons.Instance.levelStateController.GameplayStopped && Time.timeScale == 1f)
		{
			SoundManager.instance.PlayShooshOutSound();
		}
	}

	public override void UpdateControlsPosition()
	{
	}

	public override Vector2 GetNormalizedDirectionVector()
	{
		return Vector2.zero;
	}

	public override void ForceSetTouchHolding(bool val)
	{
	}

	public override bool ShowAutoaimCursor()
	{
		return false;
	}

	public override bool IsAutoaiming()
	{
		return false;
	}

	public override void BouncePointer()
	{
	}
}
