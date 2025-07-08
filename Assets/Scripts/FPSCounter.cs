using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
	private Text fpsText;

	private void Start()
	{
		fpsText = GetComponent<Text>();
		fpsText.text = "wololo";
		UnityEngine.Object.Destroy(fpsText.gameObject);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void Update()
	{
		fpsText.text = Mathf.Floor(1f / Time.deltaTime).ToString();
	}
}
