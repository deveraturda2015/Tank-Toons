namespace UnityEngine.UI.Extensions
{
	[AddComponentMenu("UI/Extensions/VR Cursor")]
	public class VRCursor : MonoBehaviour
	{
		public float xSens;

		public float ySens;

		private Collider currentCollider;

		private void Update()
		{
			Vector3 mousePosition = UnityEngine.Input.mousePosition;
			Vector3 position = default(Vector3);
			position.x = mousePosition.x * xSens;
			Vector3 mousePosition2 = UnityEngine.Input.mousePosition;
			position.y = mousePosition2.y * ySens - 1f;
			Vector3 position2 = base.transform.position;
			position.z = position2.z;
			base.transform.position = position;
			VRInputModule.cursorPosition = base.transform.position;
			if (Input.GetMouseButtonDown(0) && (bool)currentCollider)
			{
				VRInputModule.PointerSubmit(currentCollider.gameObject);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			VRInputModule.PointerEnter(other.gameObject);
			currentCollider = other;
		}

		private void OnTriggerExit(Collider other)
		{
			VRInputModule.PointerExit(other.gameObject);
			currentCollider = null;
		}
	}
}
