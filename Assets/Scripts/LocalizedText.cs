using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
	public string LocalizationKey;

	private void Awake()
	{
		Text component = GetComponent<Text>();
		if (component != null)
		{
			component.text = LocalizationManager.Instance.GetLocalizedText(LocalizationKey);
			return;
		}
		TextMeshPro component2 = GetComponent<TextMeshPro>();
		if (component2 != null)
		{
			GetComponent<MeshRenderer>().sortingLayerName = "UpperHitEffects";
			component2.text = LocalizationManager.Instance.GetLocalizedText(LocalizationKey);
		}
	}
}
