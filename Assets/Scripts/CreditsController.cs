using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CreditsController : MonoBehaviour
{
	private Button OKButton;

	private Button FeedbackButton;

	private Button PrivacyPolicyButton;

	private Button Font2LicenseButton;

	private Button CompensationButton;

	private WWW compensationRequest;

	private bool CompensationRequestInProgress;

	private void Start()
	{
		//GameObject.Find("VersionText").GetComponent<Text>().text = "PID: " + GlobalCommons.Instance.globalGameStats.PlayerID;
		//base.transform.Find("AppVerTxt").GetComponent<Text>().text = "v. " + Application.version;
		OKButton = base.transform.Find("OKButton").GetComponent<Button>();
		OKButton.onClick.AddListener(delegate
		{
			OKButtonClick();
		});
		FeedbackButton = base.transform.Find("FeedbackButton").GetComponent<Button>();
		FeedbackButton.onClick.AddListener(FeedbackClick);
		PrivacyPolicyButton = base.transform.Find("PrivacyPolicyButton").GetComponent<Button>();
		PrivacyPolicyButton.onClick.AddListener(PrivacyPolicyButtonClick);
		CompensationButton = base.transform.Find("CompensationButton").GetComponent<Button>();
		CompensationButton.onClick.AddListener(delegate
		{
			CompensationButtonClick();
		});
		Font2LicenseButton = base.transform.Find("Font2LicenseButton").GetComponent<Button>();
		Font2LicenseButton.onClick.AddListener(delegate
		{
			Font2LicenseButtonClick();
		});
	}

	private void PrivacyPolicyButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		Application.OpenURL("http://emittercritter.com/privacy/");
	}

	private void FeedbackClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.MessagesController.ShowConfirmationDialog(LocalizationManager.Instance.GetLocalizedText("FeedbackConfirmation"), delegate
		{
			string text = "info@nitrouppi.com";
			string text2 = MyEscapeURL("Support id #" + GlobalCommons.Instance.globalGameStats.PlayerID);
			string text3 = MyEscapeURL("Hello,\r\n");
			Application.OpenURL("mailto:" + text + "?subject=" + text2 + "&body=" + text3);
		}, null);
	}

	private string MyEscapeURL(string url)
	{
		return WWW.EscapeURL(url).Replace("+", "%20");
	}

	private void Update()
	{
		if (compensationRequest == null || !compensationRequest.isDone)
		{
			return;
		}
		if (string.IsNullOrEmpty(compensationRequest.error))
		{
			int num = -1;
			try
			{
				string text = compensationRequest.text.Substring(compensationRequest.text.IndexOf("ItemID:"));
				string text2 = text.Substring(7);
				string s = text2.Substring(0, text2.IndexOf("}"));
				num = int.Parse(s);
			}
			catch (Exception)
			{
			}
			
		}
		compensationRequest = null;
	}

	private void CompensationButtonClick()
	{
		if (!CompensationRequestInProgress)
		{
			CompensationRequestInProgress = true;

		}
	}

	private void GetCompensationsCallback(UnityWebRequest www)
	{
		CompensationRequestInProgress = false;
		if (string.IsNullOrEmpty(www.error))
		{
			//CompensationsInfo compensationsInfo = null;
			//try
			//{
			//	string text = "{ \"CompensationItems\": " + www.downloadHandler.text + "}";
			//	UnityEngine.Debug.Log(text);
			//	compensationsInfo = JsonUtility.FromJson<CompensationsInfo>(text);
			//	compensationsInfo.CompensationItems.ToList().ForEach(delegate(CompensationItem compensationItem)
			//	{
			//		switch (compensationItem.item_id)
			//		{
			//		case "doublecoins":
			//			Purchaser.Instance.FinalizeItemPurchase(Purchaser.PRODUCT_DOUBLE_COINS);
			//			GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("MessageYouHaveReceivedAnItem") + "\n" + LocalizationManager.Instance.GetLocalizedText("TankBankNameText"));
			//			break;
			//		case "coinpack1":
			//			Purchaser.Instance.FinalizeItemPurchase(Purchaser.PRODUCT_COINPACK_1);
			//			GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("MessageYouHaveReceivedAnItem") + "\n" + Purchaser.GetCoinPackAmountForIndex(1).ToString() + " " + LocalizationManager.Instance.GetCoinsNumberEnding(Purchaser.GetCoinPackAmountForIndex(1)));
			//			break;
			//		case "coinpack2":
			//			Purchaser.Instance.FinalizeItemPurchase(Purchaser.PRODUCT_COINPACK_2);
			//			GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("MessageYouHaveReceivedAnItem") + "\n" + Purchaser.GetCoinPackAmountForIndex(2).ToString() + " " + LocalizationManager.Instance.GetCoinsNumberEnding(Purchaser.GetCoinPackAmountForIndex(2)));
			//			break;
			//		case "coinpack3":
			//			Purchaser.Instance.FinalizeItemPurchase(Purchaser.PRODUCT_COINPACK_3);
			//			GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("MessageYouHaveReceivedAnItem") + "\n" + Purchaser.GetCoinPackAmountForIndex(3).ToString() + " " + LocalizationManager.Instance.GetCoinsNumberEnding(Purchaser.GetCoinPackAmountForIndex(3)));
			//			break;
			//		case "coinpack4":
			//			Purchaser.Instance.FinalizeItemPurchase(Purchaser.PRODUCT_COINPACK_4);
			//			GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("MessageYouHaveReceivedAnItem") + "\n" + Purchaser.GetCoinPackAmountForIndex(4).ToString() + " " + LocalizationManager.Instance.GetCoinsNumberEnding(Purchaser.GetCoinPackAmountForIndex(4)));
			//			break;
			//		case "coinpack5":
			//			Purchaser.Instance.FinalizeItemPurchase(Purchaser.PRODUCT_COINPACK_5);
			//			GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("MessageYouHaveReceivedAnItem") + "\n" + Purchaser.GetCoinPackAmountForIndex(5).ToString() + " " + LocalizationManager.Instance.GetCoinsNumberEnding(Purchaser.GetCoinPackAmountForIndex(5)));
			//			break;
			//		case "coins":
			//			GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("YouHaveReceivedCoins") + compensationItem.item_count.ToString());
			//			GlobalCommons.Instance.globalGameStats.IncreaseMoney(int.Parse(compensationItem.item_count));
			//			break;
			//		case "levelscompleted":
			//			GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("LevelsCompletedOverride") + compensationItem.item_count.ToString());
			//			GlobalCommons.Instance.globalGameStats.LevelsCompleted = int.Parse(compensationItem.item_count);
			//			break;
			//		}
			//	});
			//}
			//catch (Exception)
			//{
			//}
		}
	}

	private void OKButtonClick()
	{
		AdmobManager.instance.ShowBanner();

		LevelResultsController levelResultsController = UnityEngine.Object.FindObjectOfType<LevelResultsController>();
		if ((bool)levelResultsController)
		{
			levelResultsController.tankobankDialogOpen = false;
		}
		SoundManager.instance.PlayButtonClickSound();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void Font1LicenseButtonClick()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Prefabs.FontLicense1, Vector3.zero, Quaternion.identity);
		gameObject.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
	}

	private void Font2LicenseButtonClick()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Prefabs.FontLicense2, Vector3.zero, Quaternion.identity);
		gameObject.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
	}
}
