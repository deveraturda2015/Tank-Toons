using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetCanvasBounds : MonoBehaviour
{
	private Canvas CurrentCanvas;

	private RectTransform CurrentCanvasRT;

	private RectTransform _UIRectTransform;

	internal RectTransform UIRectTransform
	{
		get
		{
			CheckBounds();
			return _UIRectTransform;
		}
	}

	private Rect GetSafeArea()
	{
		float x = 100f;
		float y = 100f;
		float width = Screen.width - 200;
		float height = Screen.height - 200;
		return new Rect(x, y, width, height);
	}

	private void Awake()
	{
		SceneManager.activeSceneChanged += ActiveSceneChanged;
	}

	private void ActiveSceneChanged(Scene arg0, Scene arg1)
	{
		CheckBounds();
	}

	private void CheckBounds()
	{
		Canvas canvas = UnityEngine.Object.FindObjectOfType<Canvas>();
		if (CurrentCanvas == null || CurrentCanvas != canvas)
		{
			CurrentCanvas = canvas;
			if (CurrentCanvas != null)
			{
				CurrentCanvasRT = CurrentCanvas.GetComponent<RectTransform>();
				List<Transform> list = new List<Transform>();
				IEnumerator enumerator = CurrentCanvas.transform.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Transform item = (Transform)enumerator.Current;
						list.Add(item);
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				_UIRectTransform = new GameObject("BaseRectTransform", typeof(RectTransform)).GetComponent<RectTransform>();
				_UIRectTransform.anchorMax = new Vector2(1f, 1f);
				_UIRectTransform.anchorMin = new Vector2(0f, 0f);
				_UIRectTransform.offsetMin = Vector2.zero;
				_UIRectTransform.offsetMax = Vector2.zero;
				_UIRectTransform.SetParent(CurrentCanvas.transform, worldPositionStays: false);
				list.ForEach(delegate(Transform child)
				{
					child.SetParent(_UIRectTransform, worldPositionStays: false);
				});
				ApplySafeArea();
			}
		}
	}

	internal void Cleanup()
	{
		SceneManager.activeSceneChanged -= ActiveSceneChanged;
	}

	private void ApplySafeArea()
	{
		Rect safeArea = Screen.safeArea;
		Vector2 position = safeArea.position;
		Vector2 anchorMax = safeArea.position + safeArea.size;
		position.x = 0f;
		position.y /= Screen.height;
		anchorMax.x = 1f;
		anchorMax.y /= Screen.height;
		_UIRectTransform.anchorMin = position;
		_UIRectTransform.anchorMax = anchorMax;
		if (safeArea.y > 0f)
		{
			RectTransform component = UnityEngine.Object.Instantiate(Prefabs.VisibleAreaCover).GetComponent<RectTransform>();
			component.anchorMin = new Vector2(0.5f, 0f);
			component.anchorMax = new Vector2(0.5f, 0f);
			component.sizeDelta = new Vector2(CurrentCanvasRT.rect.width, safeArea.y * 1.25f);
			RectTransform rectTransform = component;
			Vector2 sizeDelta = component.sizeDelta;
			rectTransform.anchoredPosition = new Vector2(0f, (0f - sizeDelta.y) / 2f);
			component.SetParent(_UIRectTransform, worldPositionStays: false);
			component.SetParent(CurrentCanvasRT);
			component.SetAsLastSibling();
		}
		if (safeArea.y + safeArea.height < (float)Screen.height)
		{
			float num = (float)Screen.height - (safeArea.y + safeArea.height);
			RectTransform component = UnityEngine.Object.Instantiate(Prefabs.VisibleAreaCover).GetComponent<RectTransform>();
			component.anchorMin = new Vector2(0.5f, 1f);
			component.anchorMax = new Vector2(0.5f, 1f);
			component.sizeDelta = new Vector2(CurrentCanvasRT.rect.width, num * 1.25f);
			RectTransform rectTransform2 = component;
			Vector2 sizeDelta2 = component.sizeDelta;
			rectTransform2.anchoredPosition = new Vector2(0f, sizeDelta2.y / 2f);
			component.SetParent(_UIRectTransform, worldPositionStays: false);
			component.SetParent(CurrentCanvasRT);
			component.SetAsLastSibling();
		}
	}

	private void Update()
	{
	}
}
