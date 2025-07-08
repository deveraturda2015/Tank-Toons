using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	public class HsvSliderPicker : MonoBehaviour, IDragHandler, IPointerDownHandler, IEventSystemHandler
	{
		public HSVPicker picker;

		private void PlacePointer(PointerEventData eventData)
		{
			Vector2 position = eventData.position;
			float x = position.x;
			Vector3 position2 = picker.hsvSlider.rectTransform.position;
			float x2 = x - position2.x;
			Vector3 position3 = picker.hsvSlider.rectTransform.position;
			float y = position3.y;
			Vector2 position4 = eventData.position;
			Vector2 vector = new Vector2(x2, y - position4.y);
			float y2 = vector.y;
			float height = picker.hsvSlider.rectTransform.rect.height;
			Vector3 lossyScale = picker.hsvSlider.canvas.transform.lossyScale;
			vector.y = y2 / (height * lossyScale.y);
			vector.y = Mathf.Clamp(vector.y, 0f, 1f);
			picker.MovePointer(vector.y);
		}

		public void OnDrag(PointerEventData eventData)
		{
			PlacePointer(eventData);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			PlacePointer(eventData);
		}
	}
}
