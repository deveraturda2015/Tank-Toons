using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("UI/Extensions/UIScrollToSelection")]
	public class UIScrollToSelection : MonoBehaviour
	{
		[Header("[ References ]")]
		[SerializeField]
		private RectTransform layoutListGroup;

		[Header("[ Settings ]")]
		[SerializeField]
		private float scrollSpeed = 10f;

		protected RectTransform LayoutListGroup => layoutListGroup;

		protected float ScrollSpeed => scrollSpeed;

		protected RectTransform TargetScrollObject
		{
			get;
			set;
		}

		protected RectTransform ScrollWindow
		{
			get;
			set;
		}

		protected ScrollRect TargetScrollRect
		{
			get;
			set;
		}

		protected virtual void Awake()
		{
			TargetScrollRect = GetComponent<ScrollRect>();
			ScrollWindow = TargetScrollRect.GetComponent<RectTransform>();
		}

		protected virtual void Start()
		{
		}

		protected virtual void Update()
		{
			ScrollRectToLevelSelection();
		}

		private void ScrollRectToLevelSelection()
		{
			if (TargetScrollRect == null || LayoutListGroup == null || ScrollWindow == null)
			{
				return;
			}
			EventSystem current = EventSystem.current;
			RectTransform rectTransform = (!(current.currentSelectedGameObject != null)) ? null : current.currentSelectedGameObject.GetComponent<RectTransform>();
			if (!(rectTransform == null) && !(rectTransform.transform.parent != LayoutListGroup.transform))
			{
				Vector2 anchoredPosition = rectTransform.anchoredPosition;
				float num = 0f - anchoredPosition.y;
				float num2 = LayoutListGroup.rect.height / (float)LayoutListGroup.transform.childCount;
				float height = ScrollWindow.rect.height;
				Vector2 anchoredPosition2 = LayoutListGroup.anchoredPosition;
				float y = anchoredPosition2.y;
				float num3 = 0f;
				if (num < y)
				{
					num3 = y - num;
				}
				else if (num + num2 > y + height)
				{
					num3 = y + height - (num + num2);
				}
				TargetScrollRect.verticalNormalizedPosition += num3 / LayoutListGroup.rect.height * Time.deltaTime * scrollSpeed;
				TargetScrollObject = rectTransform;
			}
		}
	}
}
