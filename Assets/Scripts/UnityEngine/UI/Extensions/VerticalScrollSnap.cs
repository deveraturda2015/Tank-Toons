using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("Layout/Extensions/Vertical Scroll Snap")]
	public class VerticalScrollSnap : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IEventSystemHandler
	{
		private Transform _screensContainer;

		private int _screens = 1;

		private int _startingScreen = 1;

		private bool _fastSwipeTimer;

		private int _fastSwipeCounter;

		private int _fastSwipeTarget = 30;

		private List<Vector3> _positions;

		private ScrollRect _scroll_rect;

		private Vector3 _lerp_target;

		private bool _lerp;

		private int _containerSize;

		[Tooltip("The gameobject that contains toggles which suggest pagination. (optional)")]
		public GameObject Pagination;

		[Tooltip("Button to go to the next page. (optional)")]
		public GameObject NextButton;

		[Tooltip("Button to go to the previous page. (optional)")]
		public GameObject PrevButton;

		public bool UseFastSwipe = true;

		public int FastSwipeThreshold = 100;

		private bool _startDrag = true;

		private Vector3 _startPosition = default(Vector3);

		private int _currentScreen;

		private bool fastSwipe;

		private void Start()
		{
			_scroll_rect = base.gameObject.GetComponent<ScrollRect>();
			_screensContainer = _scroll_rect.content;
			DistributePages();
			_screens = _screensContainer.childCount;
			_lerp = false;
			_positions = new List<Vector3>();
			if (_screens > 0)
			{
				for (int i = 0; i < _screens; i++)
				{
					_scroll_rect.verticalNormalizedPosition = (float)i / (float)(_screens - 1);
					_positions.Add(_screensContainer.localPosition);
				}
			}
			_scroll_rect.verticalNormalizedPosition = (float)(_startingScreen - 1) / (float)(_screens - 1);
			Vector2 offsetMax = _screensContainer.gameObject.GetComponent<RectTransform>().offsetMax;
			_containerSize = (int)offsetMax.y;
			ChangeBulletsInfo(CurrentScreen());
			if ((bool)NextButton)
			{
				NextButton.GetComponent<Button>().onClick.AddListener(delegate
				{
					NextScreen();
				});
			}
			if ((bool)PrevButton)
			{
				PrevButton.GetComponent<Button>().onClick.AddListener(delegate
				{
					PreviousScreen();
				});
			}
		}

		private void Update()
		{
			if (_lerp)
			{
				_screensContainer.localPosition = Vector3.Lerp(_screensContainer.localPosition, _lerp_target, 7.5f * Time.deltaTime);
				if (Vector3.Distance(_screensContainer.localPosition, _lerp_target) < 0.005f)
				{
					_lerp = false;
				}
				if (Vector3.Distance(_screensContainer.localPosition, _lerp_target) < 10f)
				{
					ChangeBulletsInfo(CurrentScreen());
				}
			}
			if (_fastSwipeTimer)
			{
				_fastSwipeCounter++;
			}
		}

		public void NextScreen()
		{
			if (CurrentScreen() < _screens - 1)
			{
				_lerp = true;
				_lerp_target = _positions[CurrentScreen() + 1];
				ChangeBulletsInfo(CurrentScreen() + 1);
			}
		}

		public void PreviousScreen()
		{
			if (CurrentScreen() > 0)
			{
				_lerp = true;
				_lerp_target = _positions[CurrentScreen() - 1];
				ChangeBulletsInfo(CurrentScreen() - 1);
			}
		}

		private void NextScreenCommand()
		{
			if (_currentScreen < _screens - 1)
			{
				_lerp = true;
				_lerp_target = _positions[_currentScreen + 1];
				ChangeBulletsInfo(_currentScreen + 1);
			}
		}

		private void PrevScreenCommand()
		{
			if (_currentScreen > 0)
			{
				_lerp = true;
				_lerp_target = _positions[_currentScreen - 1];
				ChangeBulletsInfo(_currentScreen - 1);
			}
		}

		private Vector3 FindClosestFrom(Vector3 start, List<Vector3> positions)
		{
			Vector3 result = Vector3.zero;
			float num = float.PositiveInfinity;
			foreach (Vector3 position in _positions)
			{
				if (Vector3.Distance(start, position) < num)
				{
					num = Vector3.Distance(start, position);
					result = position;
				}
			}
			return result;
		}

		public int CurrentScreen()
		{
			Vector2 offsetMin = _screensContainer.gameObject.GetComponent<RectTransform>().offsetMin;
			float value = Math.Abs(offsetMin.y);
			value = Mathf.Clamp(value, 1f, _containerSize - 1);
			float num = value / (float)_containerSize * (float)_screens;
			return (int)num;
		}

		private void ChangeBulletsInfo(int currentScreen)
		{
			if ((bool)Pagination)
			{
				for (int i = 0; i < Pagination.transform.childCount; i++)
				{
					Pagination.transform.GetChild(i).GetComponent<Toggle>().isOn = ((currentScreen == i) ? true : false);
				}
			}
		}

		private void DistributePages()
		{
			float num = 0f;
			float num2 = Screen.height;
			float num3 = 0f;
			Vector2 sizeDelta = base.gameObject.GetComponent<RectTransform>().sizeDelta;
			float num4 = 0f;
			for (int i = 0; i < _screensContainer.transform.childCount; i++)
			{
				RectTransform component = _screensContainer.transform.GetChild(i).gameObject.GetComponent<RectTransform>();
				num4 = num + (float)i * num2;
				component.sizeDelta = new Vector2(sizeDelta.x, sizeDelta.y);
				component.anchoredPosition = new Vector2(0f - sizeDelta.x / 2f, num4 + sizeDelta.y / 2f);
			}
			num3 = num4 + num * -1f;
			_screensContainer.GetComponent<RectTransform>().offsetMax = new Vector2(0f, num3);
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			_startPosition = _screensContainer.localPosition;
			_fastSwipeCounter = 0;
			_fastSwipeTimer = true;
			_currentScreen = CurrentScreen();
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			_startDrag = true;
			if (!_scroll_rect.vertical)
			{
				return;
			}
			if (UseFastSwipe)
			{
				fastSwipe = false;
				_fastSwipeTimer = false;
				if (_fastSwipeCounter <= _fastSwipeTarget)
				{
					float y = _startPosition.y;
					Vector3 localPosition = _screensContainer.localPosition;
					if (Math.Abs(y - localPosition.y) > (float)FastSwipeThreshold)
					{
						fastSwipe = true;
					}
				}
				if (fastSwipe)
				{
					float y2 = _startPosition.y;
					Vector3 localPosition2 = _screensContainer.localPosition;
					if (y2 - localPosition2.y > 0f)
					{
						NextScreenCommand();
					}
					else
					{
						PrevScreenCommand();
					}
				}
				else
				{
					_lerp = true;
					_lerp_target = FindClosestFrom(_screensContainer.localPosition, _positions);
				}
			}
			else
			{
				_lerp = true;
				_lerp_target = FindClosestFrom(_screensContainer.localPosition, _positions);
			}
		}

		public void OnDrag(PointerEventData eventData)
		{
			_lerp = false;
			if (_startDrag)
			{
				OnBeginDrag(eventData);
				_startDrag = false;
			}
		}
	}
}
