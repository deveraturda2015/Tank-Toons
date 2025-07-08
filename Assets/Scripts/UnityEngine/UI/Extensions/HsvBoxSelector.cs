using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	public class HsvBoxSelector : MonoBehaviour, IDragHandler, IPointerDownHandler, IEventSystemHandler
	{
		public HSVPicker picker;

		private void PlaceCursor(PointerEventData eventData)
		{
			Vector2 position = eventData.position;
			float x = position.x;
			Vector3 position2 = picker.hsvImage.rectTransform.position;
			float x2 = x - position2.x;
			float height = picker.hsvImage.rectTransform.rect.height;
			Vector3 lossyScale = picker.hsvImage.transform.lossyScale;
			float num = height * lossyScale.y;
			Vector3 position3 = picker.hsvImage.rectTransform.position;
			float y = position3.y;
			Vector2 position4 = eventData.position;
			Vector2 vector = new Vector2(x2, num - (y - position4.y));
			float x3 = vector.x;
			float width = picker.hsvImage.rectTransform.rect.width;
			Vector3 lossyScale2 = picker.hsvImage.transform.lossyScale;
			vector.x = x3 / (width * lossyScale2.x);
			float y2 = vector.y;
			float height2 = picker.hsvImage.rectTransform.rect.height;
			Vector3 lossyScale3 = picker.hsvImage.transform.lossyScale;
			vector.y = y2 / (height2 * lossyScale3.y);
			vector.x = Mathf.Clamp(vector.x, 0f, 0.9999f);
			vector.y = Mathf.Clamp(vector.y, 0f, 0.9999f);
			picker.MoveCursor(vector.x, vector.y);
		}

		public void OnDrag(PointerEventData eventData)
		{
			PlaceCursor(eventData);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			PlaceCursor(eventData);
		}
	}
}
