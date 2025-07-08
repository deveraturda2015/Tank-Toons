using GooglePlayGames;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PGPCloudMenuController : MonoBehaviour
{
	private enum OperationType
	{
		InitialLoad,
		Load,
		Save,
		None
	}

	private Button OKButton;

	private Button RestoreButton;

	private Button SaveButton;

	private Text iCloudCoinsText;

	private Text iCloudLevelsCompletedText;

	private Text iCloudDateText;

	private Text LocalLevelsCompletedText;

	private Text LocalCoinsText;

	private Text LocalDateText;

	private SavegameData sdata;

	private GameObject waitCover;

	private OperationType currentOperation = OperationType.None;

	private float operationTimestamp;

	private const float OPERATION_TIMEOUT = 15f;

	private PGPSaveManager pgpSaveManager;

	private void Start()
	{
		if (Social.localUser != null /*&& !PlayGamesPlatform.Instance.IsAuthenticated()*/)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		pgpSaveManager = (GlobalCommons.Instance.SaveManager as PGPSaveManager);
		waitCover = GameObject.Find("WaitCover");
		Text component = base.transform.Find("HeaderText").GetComponent<Text>();
		component.text = LocalizationManager.Instance.GetLocalizedText("CloudMenuHeader");
		OKButton = base.transform.Find("OKButton").GetComponent<Button>();
		OKButton.onClick.AddListener(delegate
		{
			OKButtonClick();
		});
		RestoreButton = base.transform.Find("RestoreButton").GetComponent<Button>();
		RestoreButton.onClick.AddListener(delegate
		{
			RestoreButtonClick();
		});
		SaveButton = base.transform.Find("SaveButton").GetComponent<Button>();
		SaveButton.onClick.AddListener(delegate
		{
			SaveButtonClick();
		});
		LocalCoinsText = base.transform.Find("LocalCoinsText").GetComponent<Text>();
		LocalDateText = base.transform.Find("LocalDateText").GetComponent<Text>();
		LocalLevelsCompletedText = base.transform.Find("LocalLevelsCompletedText").GetComponent<Text>();
		iCloudCoinsText = base.transform.Find("iCloudCoinsText").GetComponent<Text>();
		iCloudLevelsCompletedText = base.transform.Find("iCloudLevelsCompletedText").GetComponent<Text>();
		iCloudDateText = base.transform.Find("iCloudDateText").GetComponent<Text>();
		LocalCoinsText.text = GlobalCommons.Instance.globalGameStats.Money.ToString();
		LocalLevelsCompletedText.text = GlobalCommons.Instance.globalGameStats.LevelsCompleted.ToString() + "/" + GlobalCommons.Instance.levelsContainer.LevelsCount.ToString();
		LocalDateText.text = DateTime.Now.ToString("yyyy MMMMM dd");
		LoadIcloudSaveAndUpdateText();
	}

	private void Update()
	{
		if (currentOperation != OperationType.None)
		{
			if (!pgpSaveManager.OperationInProgress)
			{
				switch (currentOperation)
				{
				case OperationType.InitialLoad:
					sdata = pgpSaveManager.loadedSaveData;
					if (sdata != null)
					{
						iCloudCoinsText.text = sdata.Money.ToString();
						iCloudDateText.text = sdata.SaveTime.ToString("yyyy MMMMM dd");
						iCloudLevelsCompletedText.text = sdata.LevelsCompleted.ToString() + "/" + GlobalCommons.Instance.levelsContainer.LevelsCount.ToString();
					}
					else
					{
						iCloudCoinsText.text = LocalizationManager.Instance.GetLocalizedText("CloudNotAvailebleTxt");
						iCloudDateText.text = LocalizationManager.Instance.GetLocalizedText("CloudNotAvailebleTxt");
						iCloudLevelsCompletedText.text = LocalizationManager.Instance.GetLocalizedText("CloudNotAvailebleTxt");
					}
					break;
				case OperationType.Load:
					if (pgpSaveManager.loadedSaveData != null)
					{
						if (GlobalCommons.Instance.ProceedWithGameLoad(pgpSaveManager.loadedSaveData, cloudLoading: true))
						{
							GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("MessageCloudLoadSuccess"));
							GlobalCommons.Instance.SaveGame();
							UnityEngine.Object.Destroy(base.gameObject);
						}
						else
						{
							GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("MessageCloudLoadFailed"));
						}
					}
					else
					{
						GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("MessageCloudLoadFailed"));
					}
					break;
				case OperationType.Save:
					if (pgpSaveManager.saveSuccessful)
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
				currentOperation = OperationType.None;
			}
			else
			{
				if (!waitCover.activeInHierarchy)
				{
					waitCover.SetActive(value: true);
				}
				if (Time.fixedTime - operationTimestamp > 15f)
				{
					GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("MessageCloudSaveTimeout"));
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}
		}
		else if (waitCover.activeInHierarchy)
		{
			waitCover.SetActive(value: false);
		}
	}

	private void LoadIcloudSaveAndUpdateText()
	{
		operationTimestamp = Time.fixedTime;
		currentOperation = OperationType.InitialLoad;
		GlobalCommons.Instance.SaveManager.GetCloudSaveData();
	}

	private void RestoreButtonClick()
	{
		if (currentOperation == OperationType.None)
		{
			ShowConfirmationWindow(ICloudConfirmationController.ConfirmationType.LoadConfirmation);
		}
	}

	private void SaveButtonClick()
	{
		if (currentOperation == OperationType.None)
		{
			ShowConfirmationWindow(ICloudConfirmationController.ConfirmationType.SaveConfirmation);
		}
	}

	private void ShowConfirmationWindow(ICloudConfirmationController.ConfirmationType confirmationType)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Prefabs.ICloudLoadConfirmation, Vector3.zero, Quaternion.identity);
		SoundManager.instance.PlayButtonClickSound();
		gameObject.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
		gameObject.GetComponent<ICloudConfirmationController>().Initialize(confirmationType, this);
	}

	internal void ProcessConfirmation(ICloudConfirmationController.ConfirmationType confirmationType)
	{
		operationTimestamp = Time.fixedTime;
		switch (confirmationType)
		{
		case ICloudConfirmationController.ConfirmationType.LoadConfirmation:
			currentOperation = OperationType.Load;
			GlobalCommons.Instance.SaveManager.LoadGame();
			break;
		case ICloudConfirmationController.ConfirmationType.SaveConfirmation:
			currentOperation = OperationType.Save;
			GlobalCommons.Instance.SaveManager.SaveGame();
			break;
		}
	}

	private void OKButtonClick()
	{
		if (currentOperation == OperationType.None)
		{
			SoundManager.instance.PlayButtonClickSound();
			GlobalCommons.Instance.SaveGame();
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
