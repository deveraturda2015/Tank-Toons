using DG.Tweening;
using UnityEngine;

using UnityEngine.UI;

public class TankobankDialog : MonoBehaviour
{
	private Button ConfirmPurchaseButton;

	private Button CancelPurchaseButton;

	private Text CostText;

	private Image TankoBankDialogImage;

	private CanvasGroup dialogCG;

	private float fadeTime = 0.15f;

	private float inScale;

	private void Start()
	{
		TankoBankDialogImage = base.transform.Find("TankoBankDialogImage").GetComponent<Image>();
		ConfirmPurchaseButton = TankoBankDialogImage.transform.Find("ConfirmPurchaseButton").GetComponent<Button>();
		//ConfirmPurchaseButton.onClick.AddListener(delegate
		//{
		//	ConfirmPurchaseButtonClick();
		//});
		CancelPurchaseButton = TankoBankDialogImage.transform.Find("CancelPurchaseButton").GetComponent<Button>();
		//CancelPurchaseButton.onClick.AddListener(delegate
		//{
		//	CancelPurchaseButtonClick();
		//});
		//CostText = TankoBankDialogImage.transform.Find("CostText").GetComponent<Text>();
		//CostText.text = Purchaser.Instance.GetLocalizedPrice(Purchaser.PRODUCT_DOUBLE_COINS);
		dialogCG = GetComponent<CanvasGroup>();
		dialogCG.alpha = 0f;
		dialogCG.DOFade(1f, fadeTime);
		Vector3 localScale = TankoBankDialogImage.transform.localScale;
		float x = localScale.x;
		Vector3 localScale2 = TankoBankDialogImage.transform.localScale;
		inScale = localScale2.x * 0.9f;
		Transform transform = TankoBankDialogImage.transform;
		float x2 = inScale;
		float y = inScale;
		Vector3 localScale3 = TankoBankDialogImage.transform.localScale;
		transform.localScale = new Vector3(x2, y, localScale3.z);
		Transform transform2 = TankoBankDialogImage.transform;
		float x3 = x;
		float y2 = x;
		Vector3 localScale4 = TankoBankDialogImage.transform.localScale;
		transform2.DOScale(new Vector3(x3, y2, localScale4.z), fadeTime);
	}

	public void ConfirmPurchaseButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		//Object.FindObjectOfType<GraphicRaycaster>().enabled = false;
		Purchaser.instance.BuyCoinPack1();
	}

	public void ProcessDoubleCoinsPurchase()
	{
		Object.FindObjectOfType<GraphicRaycaster>().enabled = true;
		GameObject gameObject = UnityEngine.Object.Instantiate(Prefabs.ThankYouMessage, Vector3.zero, Quaternion.identity);
		gameObject.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
		LevelResultsController levelResultsController = UnityEngine.Object.FindObjectOfType<LevelResultsController>();
		levelResultsController.ProcessDoubleCoinsPurchase();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	//public void ProcessPurchaseFailed(PurchaseFailureReason failureReason)
	//{
	//	Object.FindObjectOfType<GraphicRaycaster>().enabled = true;
	//	if (failureReason != PurchaseFailureReason.UserCancelled)
	//	{
	//		GameObject gameObject = UnityEngine.Object.Instantiate(Prefabs.PurchaseFailedMessage, Vector3.zero, Quaternion.identity);
	//		gameObject.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
	//	}
	//}

	public void CancelPurchaseButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		dialogCG.DOKill();
		dialogCG.DOFade(0f, fadeTime).OnCompleteWithCoroutine(ProcessCancel);
		TankoBankDialogImage.transform.DOKill();
		Transform transform = TankoBankDialogImage.transform;
		float x = inScale;
		float y = inScale;
		Vector3 localScale = TankoBankDialogImage.transform.localScale;
		transform.DOScale(new Vector3(x, y, localScale.z), fadeTime);
	}

	private void ProcessCancel()
	{
		LevelResultsController levelResultsController = UnityEngine.Object.FindObjectOfType<LevelResultsController>();
		if ((bool)levelResultsController)
		{
			levelResultsController.tankobankDialogOpen = false;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
