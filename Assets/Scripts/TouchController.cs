using UnityEngine;

public abstract class TouchController
{
	public abstract int TouchID
	{
		get;
	}

	public abstract bool TouchActive
	{
		get;
	}

	public abstract bool KeepLastAngle
	{
		set;
	}

	public abstract bool TouchHolding
	{
		get;
	}

	public abstract Vector2 RawDirectionVectorWithTimeoutToFiltered
	{
		get;
	}

	public abstract void ProcessTouchStart(Touch touch);

	public abstract void ProcessTouchMove(Touch touch);

	public abstract void EndTouch(Touch touch);

	public abstract void ForceEndTouch();

	public abstract void PersistTouch(Touch touch);

	public abstract void ForceSetTouchHolding(bool val);

	public abstract void UpdateControlsPosition();

	public abstract Vector2 GetNormalizedDirectionVector();

	public abstract bool ShowAutoaimCursor();

	public abstract bool IsAutoaiming();

	public abstract void BouncePointer();
}
