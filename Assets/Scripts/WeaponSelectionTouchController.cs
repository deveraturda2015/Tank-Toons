using UnityEngine;

public class WeaponSelectionTouchController : TouchController
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
		GameplayCommons.Instance.weaponSwitchController.FadeIn();
	}

	public override void ProcessTouchMove(Touch touch)
	{
		GameplayCommons.Instance.weaponSwitchController.FadeIn();
	}

	public override void PersistTouch(Touch touch)
	{
		GameplayCommons.Instance.weaponSwitchController.FadeIn();
	}

	public override void EndTouch(Touch touch)
	{
		touchId = -1;
		touchActive = false;
		GameplayCommons.Instance.weaponSwitchController.FadeOut();
	}

	public void Update()
	{
	}

	public override void ForceEndTouch()
	{
		CompleteTouch();
	}

	private void CompleteTouch()
	{
		touchId = -1;
		touchActive = false;
		GameplayCommons.Instance.weaponSwitchController.FadeOut();
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
