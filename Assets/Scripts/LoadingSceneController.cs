using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
	public enum TankmanPhraseType
	{
		Stat,
		Tip
	}

	private bool proceededToGameplay;

	private float transferTimeout;

	private Image LoadingImage;

	public static List<string> TipsPool;

	public static List<GameStatistics.Stat> StatsPhrases = new List<GameStatistics.Stat>
	{
		GameStatistics.Stat.BarrelsExploded,
		GameStatistics.Stat.BiggestCombo,
		GameStatistics.Stat.BonusCratesPopped,
		GameStatistics.Stat.CoinsCollected,
		GameStatistics.Stat.EnemyRocketsDodged,
		GameStatistics.Stat.SpawnersDestroyed,
		GameStatistics.Stat.TanksDestroyed,
		GameStatistics.Stat.TowersDestroyed
	};

	public static TankmanPhraseType currentTankmanPhraseType = TankmanPhraseType.Tip;

	public static int currentTankmanPhraseID = 0;

	public static int currentTankmanImageID = -1;

	public Sprite[] TankmanSprites;

	public static Sprite[] tankmanSprites;

	private Image TankmanImage;

	private void Start()
	{
		PopulateTips();
		if (tankmanSprites == null)
		{
			tankmanSprites = TankmanSprites;
		}
		GameObject gameObject = GlobalCommons.Instance.CanvasBoundsController.UIRectTransform.Find("PauseItems").gameObject;
		switch (GlobalCommons.Instance.gameplayMode)
		{
		case GlobalCommons.GameplayModes.RegularLevel:
			transferTimeout = 3f + (float)GlobalCommons.Instance.ActualSelectedLevel / 75f;
			break;
		case GlobalCommons.GameplayModes.TutorialLevel:
			transferTimeout = 1.5f;
			break;
		default:
			transferTimeout = 3f;
			break;
		}
		float num = 3f;
		LoadingImage = gameObject.transform.Find("LoadingImage").GetComponent<Image>();
		RectTransform rectTransform = LoadingImage.rectTransform;
		Vector2 anchoredPosition = LoadingImage.rectTransform.anchoredPosition;
		float x = anchoredPosition.x;
		Vector2 anchoredPosition2 = LoadingImage.rectTransform.anchoredPosition;
		rectTransform.anchoredPosition = new Vector2(x, anchoredPosition2.y + num);
		RectTransform rectTransform2 = LoadingImage.rectTransform;
		Vector2 anchoredPosition3 = LoadingImage.rectTransform.anchoredPosition;
		float x2 = anchoredPosition3.x;
		Vector2 anchoredPosition4 = LoadingImage.rectTransform.anchoredPosition;
		rectTransform2.DOAnchorPos(new Vector2(x2, anchoredPosition4.y - num * 2f), transferTimeout + 0.17f).SetEase(Ease.Linear);
		float num2 = 0.95f;
		Transform transform = LoadingImage.transform;
		float x3 = num2;
		float y = num2;
		Vector3 localScale = LoadingImage.transform.localScale;
		transform.localScale = new Vector3(x3, y, localScale.z);
		LoadingImage.transform.DOScale(1f, transferTimeout + 0.17f).SetEase(Ease.Linear);
		TankmanImage = gameObject.transform.Find("TankmanImage").GetComponent<Image>();
		TankmanImage.SetAlpha(0f);
		if (GlobalCommons.Instance.gameplayMode != GlobalCommons.GameplayModes.TutorialLevel)
		{
			TankmanImage.DOFade(1f, 0.25f).SetDelay(0.25f);
		}
		Text component = gameObject.transform.Find("TipText").GetComponent<Text>();
		InitializePhrase();
		component.text = GetCurrentTankmanPhrase();
		component.SetAlpha(0f);
		component.DOFade(1f, 0.25f).SetDelay(0.5f);
	}

	private void PopulateTips()
	{
		List<string> list = new List<string>();
		list.Add(LocalizationManager.Instance.GetLocalizedText("TankmanTipText1"));
		list.Add(LocalizationManager.Instance.GetLocalizedText("TankmanTipText2"));
		list.Add(LocalizationManager.Instance.GetLocalizedText("TankmanTipText3"));
		list.Add(LocalizationManager.Instance.GetLocalizedText("TankmanTipText4"));
		list.Add(LocalizationManager.Instance.GetLocalizedText("TankmanTipText5"));
		list.Add(LocalizationManager.Instance.GetLocalizedText("TankmanTipText6") + Mathf.CeilToInt(25f).ToString() + "% " + LocalizationManager.Instance.GetLocalizedText("TankmanTipText7"));
		list.Add(LocalizationManager.Instance.GetLocalizedText("TankmanTipText8"));
		list.Add(LocalizationManager.Instance.GetLocalizedText("TankmanTipText9"));
		list.Add(LocalizationManager.Instance.GetLocalizedText("TankmanTipText10"));
		list.Add(LocalizationManager.Instance.GetLocalizedText("TankmanTipText11"));
		list.Add(LocalizationManager.Instance.GetLocalizedText("TankmanTipText12"));
		list.Add(LocalizationManager.Instance.GetLocalizedText("TankmanTipText13"));
		list.Add(LocalizationManager.Instance.GetLocalizedText("TankmanTipText14"));
		TipsPool = list;
		if (GlobalCommons.Instance.globalGameStats.ShowGamePads)
		{
			TipsPool.Add(LocalizationManager.Instance.GetLocalizedText("TankmanTipText15"));
		}
		TipsPool.Add(LocalizationManager.Instance.GetLocalizedText("TankmanTipText16"));
	}

	private void InitializePhrase()
	{
		TankmanPhraseType tankmanPhraseType = currentTankmanPhraseType;
		int num = currentTankmanPhraseID;
		if (UnityEngine.Random.value > 0.5f)
		{
			currentTankmanPhraseType = TankmanPhraseType.Stat;
		}
		else
		{
			currentTankmanPhraseType = TankmanPhraseType.Tip;
		}
		switch (currentTankmanPhraseType)
		{
		case TankmanPhraseType.Stat:
			currentTankmanPhraseID = UnityEngine.Random.Range(0, StatsPhrases.Count);
			break;
		case TankmanPhraseType.Tip:
			currentTankmanPhraseID = UnityEngine.Random.Range(0, TipsPool.Count);
			break;
		}
		if (currentTankmanPhraseType == TankmanPhraseType.Stat && (GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(StatsPhrases[currentTankmanPhraseID]) <= 1 || GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(StatsPhrases[currentTankmanPhraseID]) == 13 || GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(StatsPhrases[currentTankmanPhraseID]) == 666 || GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(StatsPhrases[currentTankmanPhraseID]) == 4 || GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(StatsPhrases[currentTankmanPhraseID]) == 9 || GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(StatsPhrases[currentTankmanPhraseID]) == 17 || GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(StatsPhrases[currentTankmanPhraseID]) == 26 || GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(StatsPhrases[currentTankmanPhraseID]) == 191))
		{
			currentTankmanPhraseType = TankmanPhraseType.Tip;
			currentTankmanPhraseID = UnityEngine.Random.Range(0, TipsPool.Count);
		}
		if (currentTankmanPhraseType == tankmanPhraseType && currentTankmanPhraseID == num)
		{
			InitializePhrase();
		}
		if (currentTankmanPhraseType == TankmanPhraseType.Stat)
		{
			currentTankmanImageID = 2;
		}
		else
		{
			int num2 = 0;
			do
			{
				num2 = UnityEngine.Random.Range(0, tankmanSprites.Length);
			}
			while (currentTankmanImageID == num2);
			currentTankmanImageID = num2;
		}
		TankmanImage.sprite = tankmanSprites[currentTankmanImageID];
	}

	public static string GetCurrentTankmanPhrase()
	{
		if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.TutorialLevel)
		{
			return string.Empty;
		}
		switch (currentTankmanPhraseType)
		{
		case TankmanPhraseType.Tip:
			return TipsPool[currentTankmanPhraseID];
		case TankmanPhraseType.Stat:
		{
			string str = GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(StatsPhrases[currentTankmanPhraseID]).ToString();
			switch (StatsPhrases[currentTankmanPhraseID])
			{
			case GameStatistics.Stat.BarrelsExploded:
				return LocalizationManager.Instance.GetLocalizedText("StatTip1Pt1") + str + LocalizationManager.Instance.GetLocalizedText("StatTip1Pt2");
			case GameStatistics.Stat.BiggestCombo:
				return LocalizationManager.Instance.GetLocalizedText("StatTip2Pt1") + str + LocalizationManager.Instance.GetLocalizedText("StatTip2Pt2");
			case GameStatistics.Stat.BonusCratesPopped:
				return LocalizationManager.Instance.GetLocalizedText("StatTip3Pt1") + str + LocalizationManager.Instance.GetLocalizedText("StatTip3Pt2");
			case GameStatistics.Stat.CoinsCollected:
				return LocalizationManager.Instance.GetLocalizedText("StatTip4Pt1") + str + LocalizationManager.Instance.GetLocalizedText("StatTip4Pt2");
			case GameStatistics.Stat.EnemyRocketsDodged:
				return LocalizationManager.Instance.GetLocalizedText("StatTip5Pt1") + str + LocalizationManager.Instance.GetLocalizedText("StatTip5Pt2");
			case GameStatistics.Stat.SpawnersDestroyed:
				return LocalizationManager.Instance.GetLocalizedText("StatTip6Pt1") + str + LocalizationManager.Instance.GetLocalizedText("StatTip6Pt2");
			case GameStatistics.Stat.TanksDestroyed:
				return LocalizationManager.Instance.GetLocalizedText("StatTip7Pt1") + str + LocalizationManager.Instance.GetLocalizedText("StatTip7Pt2");
			case GameStatistics.Stat.TowersDestroyed:
				return LocalizationManager.Instance.GetLocalizedText("StatTip8Pt1") + str + LocalizationManager.Instance.GetLocalizedText("StatTip8Pt2");
			}
			break;
		}
		}
		return LocalizationManager.Instance.GetLocalizedText("FailoverTip");
	}

	private void Update()
	{
		if (!proceededToGameplay && Time.timeSinceLevelLoad >= transferTimeout)
		{
			proceededToGameplay = true;
			GlobalCommons.Instance.SetLoadingGO(GameObject.Find("PauseItems"));
			GlobalCommons.Instance.StateFaderController.ChangeSceneTo("Gameplay", immediate: true);
		}
	}
}
