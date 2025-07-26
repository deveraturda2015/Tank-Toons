using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatsAchievementsMenuController : MonoBehaviour
{
	private Button continueButton;

	private Text descriptionHeader;

	private Text descriptionText;

	public Sprite AchievementLevelFullSprite;

	public Sprite[] AchievementsSprites;

	private void Start()
	{
		AdmobManager.instance.HideBanner();

		Canvas component = GameObject.Find("Canvas").GetComponent<Canvas>();
		descriptionHeader = GameObject.Find("DescriptionHeader").GetComponent<Text>();
		descriptionText = GameObject.Find("DescriptionText").GetComponent<Text>();
		continueButton = GameObject.Find("ContinueButton").GetComponent<Button>();
		continueButton.onClick.AddListener(delegate
		{
			ContinueButtonClick();
		});
		SoundManager.instance.ToggleMusic(SoundManager.MusicType.MenusMusic);
		int num = 0;
		int num2 = 0;
		float num3 = 67.2000046f;
		float num4 = 105f;
		float num5 = -10f;
		float num6 = 10f;
		float num7 = -45f;
		float num8 = 0.5f;
		float num9 = 0.7f;
		float num10 = 0.4f;
		IEnumerator enumerator = Enum.GetValues(typeof(AchievementsTracker.Achievement)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				AchievementsTracker.Achievement achievement = (AchievementsTracker.Achievement)enumerator.Current;
				int achievementLevel = GlobalCommons.Instance.globalGameStats.AchievementsTracker.GetAchievementLevel(achievement);
				Button newButtonGO = UnityEngine.Object.Instantiate(Prefabs.achievementItem, Vector3.zero, Quaternion.identity).GetComponent<Button>();
				newButtonGO.transform.SetParent(component.transform, worldPositionStays: false);
				RectTransform component2 = newButtonGO.GetComponent<RectTransform>();
				float num11 = num3 * 3.5f;
				component2.anchoredPosition = new Vector2((float)num * num3 - num11, (float)(-num2) * num4 + num4 + num5);
				newButtonGO.name = achievement.ToString();
				newButtonGO.GetComponent<Image>().sprite = AchievementsSprites[(int)achievement];
				for (int i = 0; i < 5; i++)
				{
					Image component3 = UnityEngine.Object.Instantiate(Prefabs.ahcievementLevelIndicator, Vector3.zero, Quaternion.identity).GetComponent<Image>();
					component3.transform.SetParent(component.transform, worldPositionStays: false);
					RectTransform component4 = component3.GetComponent<RectTransform>();
					Vector2 anchoredPosition = component2.anchoredPosition;
					float x = anchoredPosition.x + (float)i * num6 - num6 * 2f;
					Vector2 anchoredPosition2 = component2.anchoredPosition;
					component4.anchoredPosition = new Vector2(x, anchoredPosition2.y + num7);
					if (achievementLevel > i)
					{
						component3.sprite = AchievementLevelFullSprite;
					}
					RectTransform component5 = component3.GetComponent<RectTransform>();
					RectTransform rectTransform = component5;
					float x2 = num9;
					float y = num9;
					Vector3 localScale = component5.localScale;
					rectTransform.localScale = new Vector3(x2, y, localScale.z);
				}
				newButtonGO.onClick.AddListener(delegate
				{
					AchievementButtonClick(newButtonGO);
				});
				Image component6 = newButtonGO.GetComponent<Image>();
				component6.SetNativeSize();
				RectTransform rectTransform2 = component2;
				float x3 = num8;
				float y2 = num8;
				Vector3 localScale2 = component2.localScale;
				rectTransform2.localScale = new Vector3(x3, y2, localScale2.z);
				num++;
				if (num >= 8)
				{
					num = 0;
					num2++;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		SetHeaderText(LocalizationManager.Instance.GetLocalizedText("AchievementsHeader"));
		SetDescriptionText(LocalizationManager.Instance.GetLocalizedText("TapAchievementForDetails"));
		GlobalCommons.Instance.SaveGame();
	}

	private void AchievementButtonClick(Button achievementButton)
	{
		SoundManager.instance.PlayButtonClickSound();
		RectTransform component = achievementButton.GetComponent<RectTransform>();
		component.transform.DOKill(complete: true);
		float num = 0.1f;
		Transform transform = component.transform;
		float x = num;
		float y = num;
		Vector3 localScale = component.transform.localScale;
		transform.DOPunchScale(new Vector3(x, y, localScale.z), 0.2f);
		component.SetSiblingIndex(component.gameObject.transform.parent.childCount - 1);
		switch ((AchievementsTracker.Achievement)Enum.Parse(typeof(AchievementsTracker.Achievement), achievementButton.name))
		{
		case AchievementsTracker.Achievement.TanksDestroyer:
			SetHeaderText(LocalizationManager.Instance.GetLocalizedText("AchievementHeader1"));
			SetDescriptionText(LocalizationManager.Instance.GetLocalizedText("AchievementDescr1"));
			break;
		case AchievementsTracker.Achievement.AwesomeCombo:
			SetHeaderText(LocalizationManager.Instance.GetLocalizedText("AchievementHeader2"));
			SetDescriptionText(LocalizationManager.Instance.GetLocalizedText("AchievementDescr2"));
			break;
		case AchievementsTracker.Achievement.BarrelsExploder:
			SetHeaderText(LocalizationManager.Instance.GetLocalizedText("AchievementHeader3"));
			SetDescriptionText(LocalizationManager.Instance.GetLocalizedText("AchievementDescr3"));
			break;
		case AchievementsTracker.Achievement.BonusCratesPopper:
			SetHeaderText(LocalizationManager.Instance.GetLocalizedText("AchievementHeader4"));
			SetDescriptionText(LocalizationManager.Instance.GetLocalizedText("AchievementDescr4"));
			break;
		case AchievementsTracker.Achievement.CoinsCollecter:
			SetHeaderText(LocalizationManager.Instance.GetLocalizedText("AchievementHeader5"));
			SetDescriptionText(LocalizationManager.Instance.GetLocalizedText("AchievementDescr5"));
			break;
		case AchievementsTracker.Achievement.Missilolo:
			SetHeaderText(LocalizationManager.Instance.GetLocalizedText("AchievementHeader6"));
			SetDescriptionText(LocalizationManager.Instance.GetLocalizedText("AchievementDescr6"));
			break;
		case AchievementsTracker.Achievement.EnemyRocketsDodger:
			SetHeaderText(LocalizationManager.Instance.GetLocalizedText("AchievementHeader7"));
			SetDescriptionText(LocalizationManager.Instance.GetLocalizedText("AchievementDescr7"));
			break;
		case AchievementsTracker.Achievement.LevelsCompleter:
			SetHeaderText(LocalizationManager.Instance.GetLocalizedText("AchievementHeader8"));
			SetDescriptionText(LocalizationManager.Instance.GetLocalizedText("AchievementDescr8"));
			break;
		case AchievementsTracker.Achievement.Marksman:
			SetHeaderText(LocalizationManager.Instance.GetLocalizedText("AchievementHeader9"));
			SetDescriptionText(LocalizationManager.Instance.GetLocalizedText("AchievementDescr9"));
			break;
		case AchievementsTracker.Achievement.MineEnemyExploder:
			SetHeaderText(LocalizationManager.Instance.GetLocalizedText("AchievementHeader10"));
			SetDescriptionText(LocalizationManager.Instance.GetLocalizedText("AchievementDescr10"));
			break;
		case AchievementsTracker.Achievement.Penetrator:
			SetHeaderText(LocalizationManager.Instance.GetLocalizedText("AchievementHeader11"));
			SetDescriptionText(LocalizationManager.Instance.GetLocalizedText("AchievementDescr11"));
			break;
		case AchievementsTracker.Achievement.SpawnersDestroyer:
			SetHeaderText(LocalizationManager.Instance.GetLocalizedText("AchievementHeader12"));
			SetDescriptionText(LocalizationManager.Instance.GetLocalizedText("AchievementDescr12"));
			break;
		case AchievementsTracker.Achievement.Survivor:
			SetHeaderText(LocalizationManager.Instance.GetLocalizedText("AchievementHeader13"));
			SetDescriptionText(LocalizationManager.Instance.GetLocalizedText("AchievementDescr13"));
			break;
		case AchievementsTracker.Achievement.TimePlaying:
			SetHeaderText(LocalizationManager.Instance.GetLocalizedText("AchievementHeader14"));
			SetDescriptionText(LocalizationManager.Instance.GetLocalizedText("AchievementDescr14"));
			break;
		case AchievementsTracker.Achievement.TowersDestroyer:
			SetHeaderText(LocalizationManager.Instance.GetLocalizedText("AchievementHeader15"));
			SetDescriptionText(LocalizationManager.Instance.GetLocalizedText("AchievementDescr15"));
			break;
		case AchievementsTracker.Achievement.WallsDestroyer:
			SetHeaderText(LocalizationManager.Instance.GetLocalizedText("AchievementHeader16"));
			SetDescriptionText(LocalizationManager.Instance.GetLocalizedText("AchievementDescr16"));
			break;
		}
	}

	private void SetHeaderText(string text)
	{
		descriptionHeader.text = text;
	}

	private void SetDescriptionText(string text)
	{
		descriptionText.text = text;
	}

	private void Update()
	{
	}

	private void ContinueButtonClick()
	{
		RectTransform component = continueButton.GetComponent<RectTransform>();
		Vector3 localScale = component.transform.localScale;
		float x = localScale.x;
		Transform transform = component.transform;
		float x2 = x * 0.7f;
		float y = x * 0.7f;
		Vector3 localScale2 = component.transform.localScale;
		transform.localScale = new Vector3(x2, y, localScale2.z);
		RectTransform target = component;
		float x3 = x * 1.1f;
		float y2 = x * 1.1f;
		Vector3 localScale3 = component.localScale;
		target.DOScale(new Vector3(x3, y2, localScale3.z), 0.17f);
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("Upgrades");
	}
}
