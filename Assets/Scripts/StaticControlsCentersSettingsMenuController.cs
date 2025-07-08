using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StaticControlsCentersSettingsMenuController : MonoBehaviour
{
	private Button OKButton;

	private Button ResetButton;

	private Image ShootPadCenter;

	private Image MovePadCenter;

	private Canvas Canvas;

	private RectTransform CanvasRT;

	private void Start()
	{
		Canvas = UnityEngine.Object.FindObjectOfType<Canvas>();
		CanvasRT = Canvas.GetComponent<RectTransform>();
		OKButton = base.transform.Find("OKButton").GetComponent<Button>();
		OKButton.onClick.AddListener(delegate
		{
			OKButtonClick();
		});
		ResetButton = base.transform.Find("ResetButton").GetComponent<Button>();
		ResetButton.onClick.AddListener(ProcessResetButtonClick);
		ShootPadCenter = base.transform.Find("ShootPadCenter").GetComponent<Image>();
		MovePadCenter = base.transform.Find("MovePadCenter").GetComponent<Image>();
		SetCentersToCurrentSettingsValues();
	}

	private void SetCentersToCurrentSettingsValues()
	{
		RectTransform rectTransform = ShootPadCenter.rectTransform;
		Vector3 vector = ScreenToCanvas(new Vector3(GlobalCommons.Instance.globalGameStats.StaticShootingPadPositionX, GlobalCommons.Instance.globalGameStats.StaticShootingPadPositionY));
		float x = vector.x;
		Vector3 vector2 = ScreenToCanvas(new Vector3(GlobalCommons.Instance.globalGameStats.StaticShootingPadPositionX, GlobalCommons.Instance.globalGameStats.StaticShootingPadPositionY));
		rectTransform.anchoredPosition = new Vector2(x, vector2.y);
		RectTransform rectTransform2 = MovePadCenter.rectTransform;
		Vector3 vector3 = ScreenToCanvas(new Vector3(GlobalCommons.Instance.globalGameStats.StaticMovementPadPositionX, GlobalCommons.Instance.globalGameStats.StaticMovementPadPositionY));
		float x2 = vector3.x;
		Vector3 vector4 = ScreenToCanvas(new Vector3(GlobalCommons.Instance.globalGameStats.StaticMovementPadPositionX, GlobalCommons.Instance.globalGameStats.StaticMovementPadPositionY));
		rectTransform2.anchoredPosition = new Vector2(x2, vector4.y);
	}

	private void ProcessResetButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.SetDefaultStaticControlsValues();
		SetCentersToCurrentSettingsValues();
	}

	private void Update()
	{
		Input.touches.ToList().ForEach(delegate(Touch touch)
		{
			if (!RectTransformUtility.RectangleContainsScreenPoint(OKButton.image.rectTransform, touch.position, Camera.main) && !RectTransformUtility.RectangleContainsScreenPoint(ResetButton.image.rectTransform, touch.position, Camera.main))
			{
				Vector2 position = touch.position;
				if (position.x < (float)Screen.width / 2f)
				{
					RectTransform rectTransform = MovePadCenter.rectTransform;
					Vector3 vector = ScreenToCanvas(touch.position);
					float x = vector.x;
					Vector3 vector2 = ScreenToCanvas(touch.position);
					rectTransform.anchoredPosition = new Vector2(x, vector2.y);
				}
				else
				{
					RectTransform rectTransform2 = ShootPadCenter.rectTransform;
					Vector3 vector3 = ScreenToCanvas(touch.position);
					float x2 = vector3.x;
					Vector3 vector4 = ScreenToCanvas(touch.position);
					rectTransform2.anchoredPosition = new Vector2(x2, vector4.y);
				}
			}
		});
	}

	private void OKButtonClick()
	{
		GlobalGameStats globalGameStats = GlobalCommons.Instance.globalGameStats;
		Vector2 vector = RectTransformUtility.WorldToScreenPoint(Camera.main, ShootPadCenter.transform.position);
		globalGameStats.StaticShootingPadPositionX = Mathf.RoundToInt(vector.x);
		GlobalGameStats globalGameStats2 = GlobalCommons.Instance.globalGameStats;
		Vector2 vector2 = RectTransformUtility.WorldToScreenPoint(Camera.main, ShootPadCenter.transform.position);
		globalGameStats2.StaticShootingPadPositionY = Mathf.RoundToInt(vector2.y);
		GlobalGameStats globalGameStats3 = GlobalCommons.Instance.globalGameStats;
		Vector2 vector3 = RectTransformUtility.WorldToScreenPoint(Camera.main, MovePadCenter.transform.position);
		globalGameStats3.StaticMovementPadPositionX = Mathf.RoundToInt(vector3.x);
		GlobalGameStats globalGameStats4 = GlobalCommons.Instance.globalGameStats;
		Vector2 vector4 = RectTransformUtility.WorldToScreenPoint(Camera.main, MovePadCenter.transform.position);
		globalGameStats4.StaticMovementPadPositionY = Mathf.RoundToInt(vector4.y);
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.SaveGame();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	internal static void Show()
	{
		StaticControlsCentersSettingsMenuController component = UnityEngine.Object.Instantiate(Prefabs.StaticControlsCentersSettingsMenu, Vector3.zero, Quaternion.identity).GetComponent<StaticControlsCentersSettingsMenuController>();
		component.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
	}

	private Vector3 ScreenToCanvas(Vector3 screenPosition)
	{
		Vector2 sizeDelta = CanvasRT.sizeDelta;
		Vector3 result;
		Vector2 vector;
		Vector2 vector2;
		if (Canvas.renderMode == RenderMode.ScreenSpaceOverlay || (Canvas.renderMode == RenderMode.ScreenSpaceCamera && Canvas.worldCamera == null))
		{
			result = screenPosition;
			vector = Vector2.zero;
			vector2 = sizeDelta;
		}
		else
		{
			Ray ray = Canvas.worldCamera.ScreenPointToRay(screenPosition);
			if (!new Plane(CanvasRT.forward, CanvasRT.position).Raycast(ray, out float enter))
			{
				throw new Exception("Is it practically possible?");
			}
			Vector3 position = ray.origin + ray.direction * enter;
			result = CanvasRT.InverseTransformPoint(position);
			vector = -Vector2.Scale(sizeDelta, CanvasRT.pivot);
			vector2 = Vector2.Scale(sizeDelta, Vector2.one - CanvasRT.pivot);
		}
		result.x = Mathf.Clamp(result.x, vector.x, vector2.x);
		result.y = Mathf.Clamp(result.y, vector.y, vector2.y);
		return result;
	}
}
