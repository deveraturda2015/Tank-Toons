using UnityEngine;
using UnityEngine.UI;

public class ICloudConfirmationController : MonoBehaviour
{
	public enum ConfirmationType
	{
		SaveConfirmation,
		LoadConfirmation
	}

	private Text HeaderText;

	private Button YesButton;

	private Button NoButton;

	private PGPCloudMenuController pgpCloudMenuController;

	private ConfirmationType confirmationType;

	private void Start()
	{
		YesButton = base.transform.Find("YesButton").GetComponent<Button>();
		YesButton.onClick.AddListener(delegate
		{
			YesButtonClick();
		});
		NoButton = base.transform.Find("NoButton").GetComponent<Button>();
		NoButton.onClick.AddListener(delegate
		{
			NoButtonClick();
		});
	}

	private void YesButtonClick()
	{
		if (pgpCloudMenuController != null)
		{
			pgpCloudMenuController.ProcessConfirmation(confirmationType);
		}
		SoundManager.instance.PlayButtonClickSound();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void NoButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	internal void Initialize(ConfirmationType confirmationType, PGPCloudMenuController pgpCloudMenuController = null)
	{
		this.confirmationType = confirmationType;
		this.pgpCloudMenuController = pgpCloudMenuController;
		HeaderText = base.transform.Find("HeaderText").GetComponent<Text>();
		switch (confirmationType)
		{
		case ConfirmationType.LoadConfirmation:
			HeaderText.text = LocalizationManager.Instance.GetLocalizedText("CloudLoadConfirmation");
			break;
		case ConfirmationType.SaveConfirmation:
			HeaderText.text = LocalizationManager.Instance.GetLocalizedText("CloudSaveConfirmation");
			break;
		}
	}
}
