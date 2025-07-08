using UnityEngine;
using UnityEngine.UI;

public class LevelsListLoadingItemController : MonoBehaviour
{
	private Text LoadingText;

	private float LastTimeProcessedText;

	private int DotCount;

	private void Start()
	{
		LoadingText = base.transform.Find("Text").GetComponent<Text>();
	}

	private void Update()
	{
		if (Time.fixedTime - LastTimeProcessedText > 0.2f)
		{
			LastTimeProcessedText = Time.fixedTime;
			LoadingText.text = LocalizationManager.Instance.GetLocalizedText("LevelListLoadingItemText");
			DotCount++;
			if (DotCount > 3)
			{
				DotCount = 0;
			}
			for (int i = 0; i < DotCount; i++)
			{
				LoadingText.text += ".";
			}
		}
	}
}
