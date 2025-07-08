
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialWorker : MonoBehaviour
{
	private static SocialWorker instance;

	internal bool SocialAuthenticated;

	public static SocialWorker Instance
	{
		get
		{
			return instance;
		}
		set
		{
			if (instance == null)
			{
				instance = value;
			}
		}
	}

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			ProcessStartup();
		}
		else
		{
			UnityEngine.Object.DestroyImmediate(this);
		}
	}

	private void ProcessStartup()
	{
		switch (Application.platform)
		{
		case RuntimePlatform.IPhonePlayer:
			StartCoroutine(InitSocialIOS(0));
			break;
		case RuntimePlatform.Android:
			InitSocialAndroid();
			break;
		}
	}

	private void InitSocialAndroid()
	{
		//PlayGamesClientConfiguration configuration = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
		//PlayGamesPlatform.InitializeInstance(configuration);
		//PlayGamesPlatform.DebugLogEnabled = true;
		//PlayGamesPlatform.Activate();
		//Social.localUser.Authenticate(delegate(bool success)
		//{
		//	if (success)
		//	{
		//		DebugHelper.Log("Successfully authenticated in google social platform.");
		//		string message = "Username: " + Social.localUser.userName + "\nUser ID: " + Social.localUser.id + "\nIsUnderage: " + Social.localUser.underage;
		//		DebugHelper.Log(message);
		//		SocialAuthenticated = true;
		//	}
		//	else
		//	{
		//		DebugHelper.Log("Failed to authenticate google social platform.");
		//	}
		//});
	}

	private IEnumerator InitSocialIOS(int triesCount)
	{
		yield return new WaitForSecondsRealtime(1f);
		//triesCount++;
		//int triesMax = 5;
		//Social.localUser.Authenticate(delegate(bool success)
		//{
		//	if (success)
		//	{
		//		DebugHelper.Log("Authentication successful");
		//		string message = "Username: " + Social.localUser.userName + "\nUser ID: " + Social.localUser.id + "\nIsUnderage: " + Social.localUser.underage;
		//		DebugHelper.Log(message);
		//		SocialAuthenticated = true;
		//	}
		//	else if (triesCount < triesMax)
		//	{
		//		DebugHelper.Log("Authentication failed " + triesCount.ToString() + " times. Retrying...");
		//		StartCoroutine(InitSocialIOS(triesCount));
		//	}
		//	else
		//	{
		//		DebugHelper.Log("Authentication failed " + triesCount.ToString() + " times, which is max.");
		//	}
		//});
	}

	public void ReportAchievement(AchievementsTracker.Achievement achievementType)
	{
		//if (!SocialAuthenticated)
		//{
		//	return;
		//}
		//string text = string.Empty;
		//Dictionary<AchievementsTracker.Achievement, string> dictionary = new Dictionary<AchievementsTracker.Achievement, string>();
		//dictionary.Add(AchievementsTracker.Achievement.TanksDestroyer, "CgkIttan_rkSEAIQAw");
		//dictionary.Add(AchievementsTracker.Achievement.TowersDestroyer, "CgkIttan_rkSEAIQBA");
		//dictionary.Add(AchievementsTracker.Achievement.SpawnersDestroyer, "CgkIttan_rkSEAIQBQ");
		//dictionary.Add(AchievementsTracker.Achievement.AwesomeCombo, "CgkIttan_rkSEAIQBg");
		//dictionary.Add(AchievementsTracker.Achievement.WallsDestroyer, "CgkIttan_rkSEAIQBw");
		//Dictionary<AchievementsTracker.Achievement, string> dictionary2 = dictionary;
		//Dictionary<AchievementsTracker.Achievement, string> dictionary3 = new Dictionary<AchievementsTracker.Achievement, string>();
		//switch (Application.platform)
		//{
		//case RuntimePlatform.Android:
		//	if (dictionary2.ContainsKey(achievementType))
		//	{
		//		text = dictionary2[achievementType];
		//	}
		//	break;
		//case RuntimePlatform.IPhonePlayer:
		//	if (dictionary3.ContainsKey(achievementType))
		//	{
		//		text = dictionary3[achievementType];
		//	}
		//	break;
		//}
		//if (!string.IsNullOrEmpty(text))
		//{
		//	Social.ReportProgress(text, 100.0, delegate(bool success)
		//	{
		//		DebugHelper.Log((!success) ? ("Failed to report achievement: " + achievementType.ToString()) : ("Reported achievement successfully: " + achievementType.ToString()));
		//	});
		//}
	}

	public void ReportHiScore()
	{
		//if (SocialAuthenticated)
		//{
			//string board;
			//switch (Application.platform)
			//{
			//case RuntimePlatform.IPhonePlayer:
			//	board = "hiscore";
			//	break;
			//case RuntimePlatform.Android:
			//	board = "CgkIttan_rkSEAIQAQ";
			//	break;
			//default:
			//	throw new Exception("scores table id not specified for platform: " + Application.platform.ToString());
			//}
			//Social.ReportScore(GlobalCommons.Instance.globalGameStats.IntHiScore, board, delegate(bool success)
			//{
			//	DebugHelper.Log((!success) ? "Failed to report score" : "Reported score successfully");
			//});
		//}
	}

	public void ReportSurvivalEnemiesCrushed()
	{
		//if (SocialAuthenticated)
		//{
		//	string board;
		//	switch (Application.platform)
		//	{
		//	case RuntimePlatform.IPhonePlayer:
		//		board = "survivalEnemies";
		//		break;
		//	case RuntimePlatform.Android:
		//		board = "CgkIttan_rkSEAIQAg";
		//		break;
		//	default:
		//		throw new Exception("scores table id not specified for platform: " + Application.platform.ToString());
		//	}
		//	Social.ReportScore(GameplayCommons.Instance.levelStateController.currentGameStats.GameStatistics.GetStat(GameStatistics.Stat.TanksDestroyed), board, delegate(bool success)
		//	{
		//		DebugHelper.Log((!success) ? "Failed to report survival tanks count" : "Reported survival tanks count successfully");
		//	});
		//}
	}
}
