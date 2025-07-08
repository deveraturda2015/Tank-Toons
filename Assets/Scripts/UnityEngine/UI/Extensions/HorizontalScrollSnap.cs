using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("Layout/Extensions/Horizontal Scroll Snap")]
	public class HorizontalScrollSnap : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IEventSystemHandler
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
					_scroll_rect.horizontalNormalizedPosition = (float)i / (float)(_screens - 1);
					_positions.Add(_screensContainer.localPosition);
				}
			}
			_scroll_rect.horizontalNormalizedPosition = 0f;
			Vector2 offsetMax = _screensContainer.gameObject.GetComponent<RectTransform>().offsetMax;
			_containerSize = (int)offsetMax.x;
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
			SetPositionToStartingScreen();
		}

		private void SetPositionToStartingScreen()
		{
			if (!(_scroll_rect == null))
			{
				_scroll_rect.horizontalNormalizedPosition = (float)(_startingScreen - 1) / (float)(_screens - 1);
			}
		}

		public void SelectStartingScreen(int screenIndex)
		{
			_startingScreen = screenIndex;
			SetPositionToStartingScreen();
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

		public void SelectScreen(int screenIndex)
		{
			_lerp = true;
			_lerp_target = _positions[screenIndex];
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
			float value = Math.Abs(offsetMin.x);
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
			int num = 0;
			int width = Screen.width;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < _screensContainer.transform.childCount; i++)
			{
				RectTransform component = _screensContainer.transform.GetChild(i).gameObject.GetComponent<RectTransform>();
				num3 = num + i * width;
				component.anchoredPosition = new Vector2(num3, 0f);
				component.sizeDelta = new Vector2(base.gameObject.GetComponent<RectTransform>().rect.width, base.gameObject.GetComponent<RectTransform>().rect.height);
			}
			num2 = num3 + num * -1;
			_screensContainer.GetComponent<RectTransform>().offsetMax = new Vector2(num2, 0f);
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
			if (!_scroll_rect.horizontal)
			{
				return;
			}
			if (UseFastSwipe)
			{
				fastSwipe = false;
				_fastSwipeTimer = false;
				if (_fastSwipeCounter <= _fastSwipeTarget)
				{
					float x = _startPosition.x;
					Vector3 localPosition = _screensContainer.localPosition;
					if (Math.Abs(x - localPosition.x) > (float)FastSwipeThreshold)
					{
						fastSwipe = true;
					}
				}
				if (fastSwipe)
				{
					float x2 = _startPosition.x;
					Vector3 localPosition2 = _screensContainer.localPosition;
					if (x2 - localPosition2.x > 0f)
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
