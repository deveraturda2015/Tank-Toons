using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	[RequireComponent(typeof(RectTransform))]
	[DisallowMultipleComponent]
	[AddComponentMenu("UI/Extensions/Re-orderable list")]
	public class ReorderableList : MonoBehaviour
	{
		[Serializable]
		public struct ReorderableListEventStruct
		{
			public GameObject DroppedObject;

			public int FromIndex;

			public ReorderableList FromList;

			public bool IsAClone;

			public GameObject SourceObject;

			public int ToIndex;

			public ReorderableList ToList;
		}

		[Serializable]
		public class ReorderableListHandler : UnityEvent<ReorderableListEventStruct>
		{
		}

		[Tooltip("Child container with re-orderable items in a layout group")]
		public LayoutGroup ContentLayout;

		[Tooltip("Parent area to draw the dragged element on top of containers. Defaults to the root Canvas")]
		public RectTransform DraggableArea;

		[Tooltip("Can items be dragged from the container?")]
		public bool IsDraggable = true;

		[Tooltip("Should the draggable components be removed or cloned?")]
		public bool CloneDraggedObject;

		[Tooltip("Can new draggable items be dropped in to the container?")]
		public bool IsDropable = true;

		[Header("UI Re-orderable Events")]
		public ReorderableListHandler OnElementDropped = new ReorderableListHandler();

		public ReorderableListHandler OnElementGrabbed = new ReorderableListHandler();

		public ReorderableListHandler OnElementRemoved = new ReorderableListHandler();

		private RectTransform _content;

		private ReorderableListContent _listContent;

		public RectTransform Content
		{
			get
			{
				if (_content == null)
				{
					_content = ContentLayout.GetComponent<RectTransform>();
				}
				return _content;
			}
		}

		private void Awake()
		{
			if (ContentLayout == null)
			{
				UnityEngine.Debug.LogError("You need to have a child LayoutGroup content set for the list: " + base.name, base.gameObject);
				return;
			}
			if (DraggableArea == null)
			{
				DraggableArea = base.transform.root.GetComponentInChildren<Canvas>().GetComponent<RectTransform>();
			}
			if (IsDropable && !GetComponent<Graphic>())
			{
				UnityEngine.Debug.LogError("You need to have a Graphic control (such as an Image) for the list [" + base.name + "] to be droppable", base.gameObject);
				return;
			}
			_listContent = ContentLayout.gameObject.AddComponent<ReorderableListContent>();
			_listContent.Init(this);
		}

		public void TestReOrderableListTarget(ReorderableListEventStruct item)
		{
			UnityEngine.Debug.Log("Event Received");
			UnityEngine.Debug.Log("Hello World, is my item a clone? [" + item.IsAClone + "]");
		}
	}
}
