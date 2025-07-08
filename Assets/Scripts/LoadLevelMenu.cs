using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadLevelMenu : MonoBehaviour
{
	private LoadedLevelInfo LoadedLevelInfo;

	private Text LevelNameText;

	private Text ByText;

	private Text NicknameText;

	private Text LikeCountText;

	private Text GameplayCountText;

	private Text SuccessfulPercentageText;

	private Button PlayButton;

	private Button CancelButton;

	private Button EditButton;

	private Vector2 InitialClancelButtonPosition;

	private bool AllowEditing;

	internal void Init(int levelId, bool allowEditing)
	{
		AllowEditing = allowEditing;
		LevelNameText = base.transform.Find("LevelNameText").GetComponent<Text>();
		ByText = base.transform.Find("ByText").GetComponent<Text>();
		NicknameText = base.transform.Find("NicknameText").GetComponent<Text>();
		GameplayCountText = base.transform.Find("GameplayCountText").GetComponent<Text>();
		SuccessfulPercentageText = base.transform.Find("SuccessfulPercentageText").GetComponent<Text>();
		LikeCountText = base.transform.Find("LikeCountText").GetComponent<Text>();
		LevelNameText.text = string.Empty;
		ByText.text = LocalizationManager.Instance.GetLocalizedText("LoadingLevelInfo") + "...";
		NicknameText.text = string.Empty;
		GameplayCountText.text = string.Empty;
		SuccessfulPercentageText.text = string.Empty;
		LikeCountText.text = string.Empty;
		PlayButton = base.transform.Find("PlayButton").GetComponent<Button>();
		PlayButton.onClick.AddListener(PlayButtonClick);
		PlayButton.gameObject.SetActive(value: false);
		CancelButton = base.transform.Find("CancelButton").GetComponent<Button>();
		CancelButton.onClick.AddListener(CancelButtonClick);
		EditButton = base.transform.Find("EditButton").GetComponent<Button>();
		if (allowEditing)
		{
			EditButton.onClick.AddListener(ProcessEditButtonClick);
			EditButton.gameObject.SetActive(value: false);
		}
		else
		{
			UnityEngine.Object.Destroy(EditButton.gameObject);
		}
		//AsyncRestCaller.GetLevel(levelId, LoadLevelCallback);
		InitialClancelButtonPosition = CancelButton.image.rectTransform.anchoredPosition;
	}

	private void ProcessEditButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		if (LoadedLevelInfo != null)
		{
			LevelEditorController.LoadPlayersCustomLevel = true;
			GlobalCommons.Instance.StateFaderController.ChangeSceneTo("LevelEditor");
		}
	}

	private void LoadLevelCallback(UnityWebRequest request)
	{
		if (request.error != null)
		{
			ProcessFailure();
			return;
		}
		try
		{
			LoadedLevelInfo = JsonUtility.FromJson<LoadedLevelInfo>(request.downloadHandler.text);
		}
		catch (Exception)
		{
			ProcessFailure();
			return;
		}
		//AsyncRestCaller.GetLikesCount(LoadedLevelInfo.id, LoadLikesCountCallback);
	}

	private void LoadLikesCountCallback(UnityWebRequest request)
	{
		if (request.error != null)
		{
			ProcessFailure();
			return;
		}
		int num = 0;
		try
		{
			LikesCountResponse likesCountResponse = JsonUtility.FromJson<LikesCountResponse>(request.downloadHandler.text);
			num = likesCountResponse.likescount;
		}
		catch (Exception)
		{
		}
		if (LevelEditorController.LoadCustomLevel(LoadedLevelInfo.data))
		{
			if (!(LevelNameText == null))
			{
				LevelEditorController.LoadedCustomLevelID = LoadedLevelInfo.id.ToString();
				LevelEditorController.LoadedCustomLevelCreatorID = LoadedLevelInfo.user_id;
				LevelNameText.text = "\"" + LoadedLevelInfo.level_name + "\"";
				ByText.text = string.Empty;
				NicknameText.text = LoadedLevelInfo.nickname;
				if (LoadedLevelInfo.play_count > 0)
				{
					LikeCountText.text = num.ToString() + " " + LocalizationManager.Instance.GetLikesNumberEnding(num);
					GameplayCountText.text = LoadedLevelInfo.play_count.ToString() + " " + LocalizationManager.Instance.GetGameplaysNumberEnding(LoadedLevelInfo.play_count);
				}
				else
				{
					LikeCountText.text = string.Empty;
					GameplayCountText.text = string.Empty;
				}
				SuccessfulPercentageText.text = ((LoadedLevelInfo.play_count > 0) ? (Mathf.FloorToInt((float)LoadedLevelInfo.win_count / (float)LoadedLevelInfo.play_count * 100f).ToString() + "% " + LocalizationManager.Instance.GetLocalizedText("SuccessfulAttampts")) : ((!string.Equals(LoadedLevelInfo.user_id, GlobalCommons.Instance.globalGameStats.PlayerID)) ? LocalizationManager.Instance.GetLocalizedText("CustomLevelWasNeverPlayed") : LocalizationManager.Instance.GetLocalizedText("OwnCustomLevelWasNeverPlayed")));
				float duration = 0.33f;
				PlayButton.gameObject.SetActive(value: true);
				PlayButton.image.SetAlpha(0f);
				PlayButton.image.DOFade(1f, duration);
				if (AllowEditing && EditButton != null && EditButton.gameObject != null)
				{
					EditButton.gameObject.SetActive(value: true);
					EditButton.image.SetAlpha(0f);
					EditButton.image.DOFade(1f, duration);
				}
			}
		}
		else
		{
			ProcessFailure();
		}
	}

	private void ProcessFailure()
	{
		ByText.text = "Whoops! Something went wrong. Please try again later.";
		PlayButton.gameObject.SetActive(value: false);
	}

	private void PlayButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		if (LoadedLevelInfo != null)
		{
			GlobalCommons.Instance.gameplayMode = GlobalCommons.GameplayModes.CustomLevel;
			SoundManager.instance.FadeOutMusic();
			if (AdsProcessor.GetInterstitialAdsAllowedBeforeLevelStart())
			{
				GlobalCommons.Instance.SceneToTransferTo = "LoadingScene";
				GlobalCommons.Instance.StateFaderController.ChangeSceneTo("PlayAdScene");
			}
			else
			{
				GlobalCommons.Instance.StateFaderController.ChangeSceneTo("LoadingScene");
			}
		}
	}

	private void CancelButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	internal static LoadLevelMenu ShowMenu(int levelId, bool allowEditing)
	{
		LoadLevelMenu component = UnityEngine.Object.Instantiate(Prefabs.LoadLevelMenu).GetComponent<LoadLevelMenu>();
		component.transform.SetParent(UnityEngine.Object.FindObjectOfType<Canvas>().transform, worldPositionStays: false);
		component.Init(levelId, allowEditing);
		return component;
	}
}
