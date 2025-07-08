using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadUserLevelMenu : MonoBehaviour
{
	private Text HeaderText;

	private Button OKButton;

	private Text LevelCodeText;

	private Text LevelNameText;

	private InputField PlayerNameInputField;

	private Button CancelButton;

	private Button RandomButton;

	public Sprite PlaySprite;

	private bool requestFired;

	private bool currentlyUploading;

	private int symbolTimer;

	private int symbolTimerMax = 8;

	public const int MAX_INFO_STRING_LENGTH = 20;

	private WWW uploadRequest;

	private bool loadSuccessful;

	private void Start()
	{
		HeaderText = base.transform.Find("HeaderText").GetComponent<Text>();
		LevelCodeText = base.transform.Find("LevelCodeText").GetComponent<Text>();
		LevelNameText = base.transform.Find("LevelNameText").GetComponent<Text>();
		PlayerNameInputField = base.transform.Find("PlayerNameInputField").GetComponent<InputField>();
		OKButton = base.transform.Find("OKButton").GetComponent<Button>();
		CancelButton = base.transform.Find("CancelButton").GetComponent<Button>();
		RandomButton = base.transform.Find("RandomButton").GetComponent<Button>();
		OKButton.onClick.AddListener(delegate
		{
			OKButtonClick();
		});
		CancelButton.onClick.AddListener(delegate
		{
			CancelButtonClick();
		});
		RandomButton.onClick.AddListener(delegate
		{
			RandomButtonClick();
		});
	}

	private void RandomButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		ProcessRequestFireActions();
		uploadRequest = RequestsHandler.Instance.LoadRandomLevel();
	}

	private void ProcessRequestFireActions()
	{
		requestFired = true;
		currentlyUploading = true;
		PlayerNameInputField.gameObject.SetActive(value: false);
		LevelCodeText.text = ".";
		HeaderText.text = LocalizationManager.Instance.GetLocalizedText("LoadingCustomLevelTxt");
		RandomButton.gameObject.SetActive(value: false);
		OKButton.gameObject.SetActive(value: false);
		RectTransform component = CancelButton.GetComponent<RectTransform>();
		RectTransform rectTransform = component;
		Vector2 anchoredPosition = component.anchoredPosition;
		rectTransform.anchoredPosition = new Vector2(0f, anchoredPosition.y);
	}

	private void OKButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		if (!requestFired)
		{
			if (PlayerNameInputField.text.Length == 0)
			{
				GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("MessageEnterLevelID"));
				return;
			}
			ProcessRequestFireActions();
			uploadRequest = RequestsHandler.Instance.LoadLevel(int.Parse(PlayerNameInputField.text));
		}
		else if (loadSuccessful)
		{
			GlobalCommons.Instance.gameplayMode = GlobalCommons.GameplayModes.CustomLevel;
			SoundManager.instance.FadeOutMusic();
			UnityEngine.Object.FindObjectOfType<LevelSelectionMenuController>().ProcessUserLevelLoad();
			GlobalCommons.Instance.StateFaderController.ChangeSceneTo("LoadingScene");
		}
		else
		{
			SoundManager.instance.PlayButtonClickSound();
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void CancelButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void Update()
	{
		if (!currentlyUploading)
		{
			return;
		}
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
		if (uploadRequest.isDone)
		{
			currentlyUploading = false;
			string localizedText = LocalizationManager.Instance.GetLocalizedText("LoadingCustomFailed");
			string localizedText2 = LocalizationManager.Instance.GetLocalizedText("LoadingCustomIncompatible");
			string localizedText3 = LocalizationManager.Instance.GetLocalizedText("LoadingCustomNoLevelForID");
			if (string.IsNullOrEmpty(uploadRequest.error))
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				try
				{
					string[] array = uploadRequest.text.Split(new string[1]
					{
						"::"
					}, StringSplitOptions.None);
					foreach (string text2 in array)
					{
						int num = text2.IndexOf(':');
						string key = text2.Substring(0, num);
						string value = text2.Substring(num + 1);
						dictionary.Add(key, value);
					}
					if (dictionary.Count == 6)
					{
						if (LevelEditorController.LoadCustomLevel(dictionary["data"]))
						{
							loadSuccessful = true;
							HeaderText.text = "\"" + dictionary["levelname"] + "\" - " + dictionary["nickname"];
							int num2 = int.Parse(dictionary["playcount"]);
							int num3 = int.Parse(dictionary["wincount"]);
							if (num2 > 0)
							{
								LevelCodeText.text = LocalizationManager.Instance.GetLocalizedText("CustomLevelWasPlayed") + num2.ToString();
							}
							else
							{
								LevelCodeText.text = LocalizationManager.Instance.GetLocalizedText("CustomLevelWasNeverPlayed");
							}
							if (num3 > num2)
							{
								num3 = num2;
							}
							if (num2 > 0)
							{
								LevelNameText.text = Mathf.FloorToInt((float)num3 / (float)num2 * 100f).ToString() + LocalizationManager.Instance.GetLocalizedText("CustomLevelPercentSuccessful");
							}
							else
							{
								LevelNameText.text = string.Empty;
							}
							LevelEditorController.LoadedCustomLevelID = dictionary["id"];
						}
						else
						{
							LevelCodeText.text = localizedText2;
						}
					}
					else
					{
						LevelCodeText.text = localizedText3;
					}
				}
				catch (Exception)
				{
					LevelCodeText.text = localizedText3;
				}
			}
			else
			{
				LevelCodeText.text = localizedText;
			}
			OKButton.gameObject.SetActive(value: true);
			if (!loadSuccessful)
			{
				RectTransform component = OKButton.GetComponent<RectTransform>();
				RectTransform rectTransform = component;
				Vector2 anchoredPosition = component.anchoredPosition;
				rectTransform.anchoredPosition = new Vector2(0f, anchoredPosition.y);
				UnityEngine.Object.Destroy(CancelButton.gameObject);
			}
			else
			{
				float duration = 0.25f;
				OKButton.image.sprite = PlaySprite;
				OKButton.transform.DOScale(1.3f, duration);
				OKButton.image.SetAlpha(0f);
				OKButton.image.DOFade(1f, duration);
				RectTransform component2 = CancelButton.GetComponent<RectTransform>();
				RectTransform target = component2;
				Vector2 anchoredPosition2 = component2.anchoredPosition;
				target.DOAnchorPosX(anchoredPosition2.x + 75f, duration);
			}
		}
	}
}
