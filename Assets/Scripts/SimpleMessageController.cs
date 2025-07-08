using UnityEngine;
using UnityEngine.UI;

public class SimpleMessageController : MonoBehaviour
{
	private Button OKButton;

	private void Awake()
	{
		OKButton = base.transform.Find("OKButton").GetComponent<Button>();
		OKButton.onClick.AddListener(delegate
		{
			OKButtonClick();
		});
	}

	private void OKButtonClick()
	{
		LevelResultsController levelResultsController = UnityEngine.Object.FindObjectOfType<LevelResultsController>();
		if ((bool)levelResultsController)
		{
			levelResultsController.tankobankDialogOpen = false;
		}
		SoundManager.instance.PlayButtonClickSound();
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
