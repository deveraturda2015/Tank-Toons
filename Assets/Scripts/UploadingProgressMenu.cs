using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UploadingProgressMenu : MonoBehaviour
{
	private Text HeaderText;

	private Text LevelCodeText;

	private Text UploadedLevelId;

	private Button OKButton;

	private Button ShareButton;

	private Button CancelButton;

	private float defaultShareButtonScale;

	private float newLevelItemShakeTimestamp;

	private float newLevelItemRotationPunch = 10f;

	private bool currentlyUploading;

	private int symbolTimer;

	private int symbolTimerMax = 8;

	private InputField PlayerNameInputField;

	private InputField LevelNameInputField;

	private UnityWebRequest uploadRequest;

	private int uploadedLevelId;

	public Sprite OKButtonSprite;

	private bool requestFired;

	private void Start()
	{
		OKButton = base.transform.Find("OKButton").GetComponent<Button>();
		OKButton.onClick.AddListener(delegate
		{
			OkButtonClick();
		});
		ShareButton = base.transform.Find("ShareButton").GetComponent<Button>();
		ShareButton.onClick.AddListener(delegate
		{
			ShareButtonClick();
		});
		ShareButton.gameObject.SetActive(value: false);
		CancelButton = base.transform.Find("CancelButton").GetComponent<Button>();
		CancelButton.onClick.AddListener(delegate
		{
			CancelButtonClick();
		});
		HeaderText = base.transform.Find("HeaderText").GetComponent<Text>();
		LevelCodeText = base.transform.Find("LevelCodeText").GetComponent<Text>();
		UploadedLevelId = base.transform.Find("UploadedLevelId").GetComponent<Text>();
		UploadedLevelId.text = string.Empty;
		PlayerNameInputField = base.transform.Find("PlayerNameInputField").GetComponent<InputField>();
		LevelNameInputField = base.transform.Find("LevelNameInputField").GetComponent<InputField>();
		PlayerNameInputField.text = GlobalCommons.Instance.globalGameStats.LevelShareNickname;
	}

	private void CancelButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OkButtonClick()
	{
		if (LevelEditorController.ValidateLevel(LevelEditorController.LevelToWorkWith) == LevelEditorController.LevelValidationResult.Fail)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		if (!requestFired)
		{
			SoundManager.instance.PlayButtonClickSound();
			if (PlayerNameInputField.text.Length == 0)
			{
				GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("MessageEnterPlayerName"));
				return;
			}
			if (LevelNameInputField.text.Length == 0)
			{
				GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("MessageEnterLevelName"));
				return;
			}
			requestFired = true;
			PlayerNameInputField.gameObject.SetActive(value: false);
			LevelNameInputField.gameObject.SetActive(value: false);
			currentlyUploading = true;
			OKButton.gameObject.SetActive(value: false);
			RectTransform component = CancelButton.GetComponent<RectTransform>();
			RectTransform rectTransform = component;
			Vector2 anchoredPosition = component.anchoredPosition;
			rectTransform.anchoredPosition = new Vector2(0f, anchoredPosition.y);
			LevelCodeText.text = ".";
			//uploadRequest = AsyncRestCaller.ShareLevel(LevelEditorController.GetLevelForUploading(), WordFilter.ToFamilyFriendlyString(PlayerNameInputField.text), WordFilter.ToFamilyFriendlyString(LevelNameInputField.text));
		}
		else
		{
			SoundManager.instance.PlayButtonClickSound();
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void ShareButtonClick()
	{
		Share.instance.ShareLink();

		//NativeShare.ShareCustomLevel(LevelNameInputField.text, uploadedLevelId.ToString());
	}

	private void Update()
	{
		if (currentlyUploading)
		{
			symbolTimer++;
			if (symbolTimer >= symbolTimerMax)
			{
				symbolTimer = 0;
				string text = LevelCodeText.text;
				switch (text)
				{
				default:
					if (text == string.Empty)
					{
						LevelCodeText.text = ".";
					}
					break;
				case ".":
					LevelCodeText.text = "..";
					break;
				case "..":
					LevelCodeText.text = "...";
					break;
				case "...":
					LevelCodeText.text = string.Empty;
					break;
				case null:
					break;
				}
			}
			if (!uploadRequest.isDone)
			{
				return;
			}
			currentlyUploading = false;
			if (string.IsNullOrEmpty(uploadRequest.error))
			{
				LevelEditorController.CanShareLevel = false;
				LevelEditorController levelEditorController = UnityEngine.Object.FindObjectOfType<LevelEditorController>();
				if (levelEditorController != null)
				{
					levelEditorController.CompleteUpload();
				}
				string text2 = null;
				try
				{
					text2 = JsonUtility.FromJson<UploadResponse>(uploadRequest.downloadHandler.text).levelid.ToString();
					uploadedLevelId = int.Parse(text2);
				}
				catch (Exception)
				{
				}
				if (string.IsNullOrEmpty(text2))
				{
					LevelCodeText.text = LocalizationManager.Instance.GetLocalizedText("LevelUploadFailTxt");
				}
				else
				{
					LevelCodeText.text = LocalizationManager.Instance.GetLocalizedText("LevelUploadCompleteTxt");
					UploadedLevelId.text = LocalizationManager.Instance.GetLocalizedText("LevelIDIsTxt") + text2;
					GlobalCommons.Instance.globalGameStats.LevelShareNickname = PlayerNameInputField.text;
					//if (NativeShare.IsCurrentPlatformSupported)
					//{
					//	ShareButton.gameObject.SetActive(value: true);
					//	Vector3 localScale = ShareButton.image.transform.localScale;
					//	defaultShareButtonScale = localScale.x;
					//	Transform transform = ShareButton.image.transform;
					//	float x = defaultShareButtonScale * 1.1f;
					//	float y = defaultShareButtonScale * 1.1f;
					//	Vector3 localScale2 = ShareButton.image.transform.localScale;
					//	transform.localScale = new Vector3(x, y, localScale2.z);
					//	ShareButton.image.material = Materials.FlashWhiteMaterial;
					//	ShareButton.image.material.SetFloat("_FlashAmount", 0f);
					//}
				}
			}
			else
			{
				LevelCodeText.text = LocalizationManager.Instance.GetLocalizedText("LevelUploadFailTxt");
			}
			OKButton.gameObject.SetActive(value: true);
			OKButton.image.sprite = OKButtonSprite;
			RectTransform component = OKButton.GetComponent<RectTransform>();
			RectTransform rectTransform = component;
			Vector2 anchoredPosition = component.anchoredPosition;
			rectTransform.anchoredPosition = new Vector2(0f, anchoredPosition.y);
			UnityEngine.Object.Destroy(CancelButton.gameObject);
		}
		else if (ShareButton.gameObject.activeInHierarchy && Time.fixedTime > newLevelItemShakeTimestamp + 1f)
		{
			newLevelItemRotationPunch *= -1f;
			newLevelItemShakeTimestamp = Time.fixedTime;
			float duration = 0.25f;
			ShareButton.image.transform.DOPunchRotation(new Vector3(0f, 0f, newLevelItemRotationPunch), duration);
			ShareButton.image.material.SetFloat("_FlashAmount", 0.5f);
			ShareButton.image.material.DOFloat(0f, "_FlashAmount", duration);
			Transform transform2 = ShareButton.image.transform;
			float x2 = defaultShareButtonScale * 1.2f;
			float y2 = defaultShareButtonScale * 1.2f;
			Vector3 localScale3 = ShareButton.image.transform.localScale;
			transform2.localScale = new Vector3(x2, y2, localScale3.z);
			ShareButton.image.transform.DOScale(defaultShareButtonScale * 1.1f, duration);
		}
	}
}
