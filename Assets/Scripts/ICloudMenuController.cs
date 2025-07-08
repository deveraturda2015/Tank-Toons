using UnityEngine;
using UnityEngine.UI;

public class ICloudMenuController : MonoBehaviour
{
	private Button OKButton;

	private Button RestoreButton;

	private Button SaveButton;

	private Text iCloudCoinsText;

	private Text iCloudLevelsCompletedText;

	private Text iCloudDateText;

	private Text LocalLevelsCompletedText;

	private Text LocalCoinsText;

	private Text LocalDateText;

	private float lastTimeUpdatedData;

	private float loadTriesInterval = 0.5f;

	private SavegameData sdata;

	private bool successfullyLoaded;

	private void Start()
	{
		base.gameObject.AddComponent<PGPCloudMenuController>();
		UnityEngine.Object.Destroy(this);
	}

	private void Update()
	{
		if (!successfullyLoaded && Time.fixedTime - lastTimeUpdatedData > loadTriesInterval)
		{
			LoadIcloudSaveAndUpdateText();
		}
	}

	private void LoadIcloudSaveAndUpdateText()
	{
		lastTimeUpdatedData = Time.fixedTime;
		sdata = GlobalCommons.Instance.SaveManager.GetCloudSaveData();
		if (sdata != null)
		{
			iCloudCoinsText.text = sdata.Money.ToString();
			iCloudDateText.text = sdata.SaveTime.ToString("yyyy MMMMM dd");
			iCloudLevelsCompletedText.text = sdata.LevelsCompleted.ToString() + "/" + GlobalCommons.Instance.levelsContainer.LevelsCount.ToString();
			successfullyLoaded = true;
		}
		else
		{
			iCloudCoinsText.text = LocalizationManager.Instance.GetLocalizedText("CloudNotAvailebleTxt");
			iCloudDateText.text = LocalizationManager.Instance.GetLocalizedText("CloudNotAvailebleTxt");
			iCloudLevelsCompletedText.text = LocalizationManager.Instance.GetLocalizedText("CloudNotAvailebleTxt");
		}
	}

	private void RestoreButtonClick()
	{
		ShowConfirmationWindow(ICloudConfirmationController.ConfirmationType.LoadConfirmation);
	}

	private void SaveButtonClick()
	{
		ShowConfirmationWindow(ICloudConfirmationController.ConfirmationType.SaveConfirmation);
	}

	private void ShowConfirmationWindow(ICloudConfirmationController.ConfirmationType confirmationType)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Prefabs.ICloudLoadConfirmation, Vector3.zero, Quaternion.identity);
		SoundManager.instance.PlayButtonClickSound();
		gameObject.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
	}

	internal void ProcessConfirmation(ICloudConfirmationController.ConfirmationType confirmationType)
	{
		switch (confirmationType)
		{
		case ICloudConfirmationController.ConfirmationType.LoadConfirmation:
			if (GlobalCommons.Instance.SaveManager.LoadGame())
			{
				GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("MessageCloudLoadSuccess"));
				GlobalCommons.Instance.SaveGame();
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("MessageCloudLoadFailed"));
			}
			break;
		case ICloudConfirmationController.ConfirmationType.SaveConfirmation:
			if (GlobalCommons.Instance.SaveManager.SaveGame())
			{
				GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("MessageCloudSaveSuccess"));
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("MessageCloudSaveFailed"));
			}
			break;
		}
	}

	private void OKButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.SaveGame();
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
