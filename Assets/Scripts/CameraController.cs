using DG.Tweening;
using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private float shakeFactor;

	private bool initialCameraSetComplete;

	internal float initialCameraOrtoSize;

	private float zoomedOutCameraOrtoSize;

	private float playerDeadCameraOrtoSize;

	private float zoomInOutSpeed = 18f;

	private bool zoomOut;

	public const float SHAKE_MEDIUM = 4f;

	public const float SHAKE_HARD = 6f;

	public const float SHAKE_VERYHARD = 9f;

	public const float SHAKE_EXTRAHARD = 12f;

	public const float SHAKE_LIGHT = 3f;

	public const float SHAKE_VERYLIGHT = 2f;

	public static bool ShakeModSet;

	private float currentRotation;

	private float rotationSpeed;

	private float rotationSpeedMax = 10f;

	private float targetCameraOrtoSize;

	private bool initialCameraZoomComplete;

	public static float SHAKE_FACTOR_DEPLETIONRATE = 30f;

	public static float SHAKE_FACTOR_MOD = 0.0175f;

	public static float SHAKE_ORTO_MOD = 0.04f;

	public static float SHAKE_ROTATION_MOD = 0.22f;

	internal const float MIN_ORTO_SIZE = 5f;

	internal const float DEFAULT_ORTO = 5f;

	private float cameraVecticalCompensation;

	private bool shakeXdirection;

	private bool shakeYdirection;

	private bool shakeOrtoDirection;

	private bool shakeRotationDirection;

	public bool IsZoomedOut => zoomOut;

	private void SetShakeMod()
	{
		ShakeModSet = true;
		float num = Screen.dpi;
		//if (Application.platform == RuntimePlatform.Android)
		//{
		//	num = GetDPI();
		//}
		float num2 = (float)Screen.height / num;
		float num3 = 2.5f / num2;
		if (num3 > 0.9f)
		{
			num3 = 0.9f;
		}
		num3 = (num3 + 0.9f) / 2f;
		SHAKE_FACTOR_MOD *= num3;
		SHAKE_ORTO_MOD *= num3;
		SHAKE_ROTATION_MOD *= num3;
	}

	private void Awake()
	{
		Camera.main.orthographicSize = GetOrthoSize();
		if (!ShakeModSet)
		{
			SetShakeMod();
		}
		cameraVecticalCompensation = 0f - 0.093f * Camera.main.orthographicSize / 6.817109f;
		targetCameraOrtoSize = (initialCameraOrtoSize = Camera.main.orthographicSize);
		zoomedOutCameraOrtoSize = initialCameraOrtoSize * 1.35f;
		playerDeadCameraOrtoSize = initialCameraOrtoSize * 0.75f;
		Camera.main.orthographicSize *= 1.5f;
		Camera.main.DOOrthoSize(initialCameraOrtoSize, TileFlyIn.AllTilesSetTime * 2f).OnCompleteWithCoroutine(ProcessInitialCameraZoomComplete);
	}

	internal static float GetOrthoSize()
	{
		float num = Screen.dpi;
		//if (Application.platform == RuntimePlatform.Android)
		//{
		//	num = GetDPI();
		//}
		float num2 = (float)Screen.height / num;
		DebugHelper.Log("HEIGHT IN INCHES = " + num2.ToString());
		float value = 5f + (num2 - 2.48f) * ((float)Math.PI * 35f / 213f);
		return Mathf.Clamp(value, 5f, 7f);
	}

	//private static float GetDPI()
	//{
	//	AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
	//	AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
	//	AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.util.DisplayMetrics");
	//	@static.Call<AndroidJavaObject>("getWindowManager", new object[0]).Call<AndroidJavaObject>("getDefaultDisplay", new object[0]).Call("getMetrics", androidJavaObject);
	//	return (androidJavaObject.Get<float>("xdpi") + androidJavaObject.Get<float>("ydpi")) * 0.5f;
	//}

	private void ProcessInitialCameraZoomComplete()
	{
		initialCameraZoomComplete = true;
	}

	internal void SetZoomState(bool zoomOut)
	{
		if (GameplayCommons.Instance.gameplayUIController.BinucularsEnabled)
		{
			this.zoomOut = zoomOut;
		}
	}

	private void Update()
	{
		if (GameplayCommons.Instance.playersTankController == null)
		{
			return;
		}
		if (GameplayCommons.Instance.weaponsController.ActiveGuidedRocket == null)
		{
			Vector3 v;
			if (GameplayCommons.Instance.weaponsController.GuidedRocketAftershotPeriodActive)
			{
				v = GameplayCommons.Instance.weaponsController.lastTimeGuidedRocketExplosionCoords;
			}
			else
			{
				v = GameplayCommons.Instance.playersTankController.TankBase.transform.position;
				if (!GameplayCommons.Instance.GamePaused && !GameplayCommons.Instance.touchesController.ShootTouchController.IsAutoaiming() && GameplayCommons.Instance.weaponsController.SelectedWeapon != WeaponTypes.mines && !GameplayCommons.Instance.playersTankController.PlayerDead && (GameplayCommons.Instance.touchesController.ShootTouchController.TouchActive || !Input.touchSupported) && !GameplayCommons.Instance.levelStateController.GameplayStopped)
				{
					Vector2 rawDirectionVectorWithTimeoutToFiltered = GameplayCommons.Instance.touchesController.ShootTouchController.RawDirectionVectorWithTimeoutToFiltered;
					v = new Vector3(v.x + rawDirectionVectorWithTimeoutToFiltered.x * 1.66f, v.y + rawDirectionVectorWithTimeoutToFiltered.y * 1.66f, v.z);
				}
			}
			float num = Vector2.Distance(v, base.transform.position);
			if (!(num < 0.01f) && initialCameraSetComplete)
			{
				bool isEditor = Application.isEditor;
				if (0 == 0)
				{
					float x = v.x;
					Vector3 position = base.transform.position;
					float num2 = (x + position.x * 9f) / 10f;
					float y = v.y;
					Vector3 position2 = base.transform.position;
					float num3 = (y + position2.y * 9f) / 10f;
					Transform transform = base.transform;
					float x2 = num2;
					float y2 = num3;
					Vector3 position3 = base.transform.position;
					transform.position = new Vector3(x2, y2, position3.z);
					goto IL_031f;
				}
			}
			initialCameraSetComplete = true;
			Transform transform2 = base.transform;
			float x3 = v.x;
			float y3 = v.y;
			Vector3 position4 = base.transform.position;
			transform2.position = new Vector3(x3, y3, position4.z);
		}
		else
		{
			Vector3 position5 = GameplayCommons.Instance.weaponsController.ActiveGuidedRocket.transform.position;
			float x4 = position5.x;
			Vector3 position6 = GameplayCommons.Instance.weaponsController.ActiveGuidedRocket.transform.position;
			float y4 = position6.y;
			Vector3 position7 = base.transform.position;
			Vector3 v = new Vector3(x4, y4, position7.z);
			float x5 = v.x;
			Vector3 position8 = base.transform.position;
			float num4 = (x5 + position8.x * 9f) / 10f;
			float y5 = v.y;
			Vector3 position9 = base.transform.position;
			float num5 = (y5 + position9.y * 9f) / 10f;
			Transform transform3 = base.transform;
			float x6 = num4;
			float y6 = num5;
			Vector3 position10 = base.transform.position;
			transform3.position = new Vector3(x6, y6, position10.z);
		}
		goto IL_031f;
		IL_031f:
		if (GameplayCommons.Instance.playersTankController.PlayerDead)
		{
			ProcessCameraZoom(playerDeadCameraOrtoSize, zoomInOutSpeed / 40f);
		}
		else if (zoomOut)
		{
			ProcessCameraZoom(zoomedOutCameraOrtoSize, zoomInOutSpeed);
		}
		else
		{
			ProcessCameraZoom(initialCameraOrtoSize, zoomInOutSpeed);
		}
		Transform transform4 = base.transform;
		Vector3 position11 = base.transform.position;
		float x7 = position11.x;
		Vector3 position12 = base.transform.position;
		float y7 = position12.y + cameraVecticalCompensation;
		Vector3 position13 = base.transform.position;
		transform4.position = new Vector3(x7, y7, position13.z);
		float orthographicSize;
		if (Time.timeScale > 0f && (shakeFactor > 0f || GameplayCommons.Instance.playersTankController.PlayerDead))
		{
			shakeFactor -= Time.deltaTime * SHAKE_FACTOR_DEPLETIONRATE;
			if (shakeFactor < 0f)
			{
				shakeFactor = 0f;
			}
			float num6 = shakeFactor * SHAKE_FACTOR_MOD;
			float num7 = num6;
			float num8 = num6;
			float num9 = shakeFactor * SHAKE_ORTO_MOD;
			float num10 = shakeFactor * SHAKE_ROTATION_MOD;
			if (shakeXdirection)
			{
				num7 *= -1f;
			}
			if (shakeYdirection)
			{
				num8 *= -1f;
			}
			if (shakeOrtoDirection)
			{
				num9 *= -1f;
			}
			if (shakeRotationDirection)
			{
				num10 *= -1f;
			}
			Transform transform5 = base.transform;
			Vector3 position14 = base.transform.position;
			float x8 = position14.x + num7;
			Vector3 position15 = base.transform.position;
			float y8 = position15.y + num8;
			Vector3 position16 = base.transform.position;
			transform5.position = new Vector3(x8, y8, position16.z);
			orthographicSize = targetCameraOrtoSize + num9;
			Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, currentRotation + num10);
		}
		else
		{
			orthographicSize = targetCameraOrtoSize;
		}
		if (initialCameraZoomComplete)
		{
			Camera.main.orthographicSize = orthographicSize;
		}
		if (!GameplayCommons.Instance.playersTankController.PlayerDead)
		{
			return;
		}
		if (rotationSpeed < rotationSpeedMax)
		{
			rotationSpeed += Time.deltaTime * 3f;
			if (rotationSpeed > rotationSpeedMax)
			{
				rotationSpeed = rotationSpeedMax;
			}
		}
		currentRotation += Time.deltaTime * rotationSpeed;
	}

	private void ProcessCameraZoom(float zoomFactor, float zoomSpeed)
	{
		if (!initialCameraZoomComplete || targetCameraOrtoSize == zoomFactor)
		{
			return;
		}
		if (targetCameraOrtoSize < zoomFactor)
		{
			targetCameraOrtoSize += zoomSpeed * Time.deltaTime;
			if (targetCameraOrtoSize > zoomFactor)
			{
				targetCameraOrtoSize = zoomFactor;
			}
		}
		else
		{
			targetCameraOrtoSize -= zoomSpeed * Time.deltaTime;
			if (targetCameraOrtoSize < zoomFactor)
			{
				targetCameraOrtoSize = zoomFactor;
			}
		}
	}

	public void ShakeCamera(float amount = 4f)
	{
		if (!GlobalCommons.Instance.globalGameStats.ScreenShakeEnabled)
		{
			return;
		}
		bool isEditor = Application.isEditor;
		if (0 == 0)
		{
			if (shakeFactor < amount)
			{
				shakeFactor = amount;
			}
			shakeXdirection = ((UnityEngine.Random.value > 0.5f) ? true : false);
			shakeYdirection = ((UnityEngine.Random.value > 0.5f) ? true : false);
			shakeOrtoDirection = ((UnityEngine.Random.value > 0.5f) ? true : false);
			shakeRotationDirection = ((UnityEngine.Random.value > 0.5f) ? true : false);
		}
	}

	internal float GetCameraScaleCoefficient()
	{
		return Camera.main.orthographicSize / initialCameraOrtoSize;
	}

	internal float GetCameraDefaultScaleCoefficient()
	{
		return Camera.main.orthographicSize / 5f;
	}
}
