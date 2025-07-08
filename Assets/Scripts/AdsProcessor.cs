using System;
using UnityEngine;

public class AdsProcessor
{
	public enum AdDisplayResult
	{
		Finished,
		Skipped,
		Failed
	}

	private DateTime LastTimeFiredInterstitialAd = DateTime.Now;

	private DateTime LastTimeFiredRewardedAd = DateTime.Now;

	private int RewadIntervalSeconds = 180;

	private int InitialRewadAvailabilitySeconds = 90;

	private int SkippableAdIntervalSeconds = 540;

	private static int InterstitialAdsLevelsCompletedRequired = 4;

	private static bool InterstitialAdsRequireShotgunToShowOnLevelFailed = true;

	private static bool InterstitialAdsAllowedBeforeLevelStart;

	private static bool InterstitialAdsAllowedAfterLevelFinish = true;

	private static bool InterstitialAdsAllowedOnLevelRestart;

	//private IAdProvider CurrentAdProvider;

	private bool RewardedAdInProgress;

	private bool InterstitialAdInProgress;

	private void HandleRemoteSettingsUpdate()
	{
		RewadIntervalSeconds = RemoteSettings.GetInt("REWAD_INTERVAL_SECONDS", 180);
		InitialRewadAvailabilitySeconds = RemoteSettings.GetInt("INITIAL_REWAD_AVAIL_SECONDS", 90);
		SkippableAdIntervalSeconds = RemoteSettings.GetInt("SKIPPABLE_AD_INTERVAL_SECONDS", 540);
		InterstitialAdsLevelsCompletedRequired = RemoteSettings.GetInt("INTERSTITIAL_ADS_LEVELS_COMPLETED_REQUIRED", 4);
		InterstitialAdsRequireShotgunToShowOnLevelFailed = RemoteSettings.GetBool("INTERSTITIAL_ADS_REQUIRE_SHOTGUN_TO_SHOW_ON_LEVEL_FAILED", defaultValue: true);
		InterstitialAdsAllowedBeforeLevelStart = RemoteSettings.GetBool("INTERSTITIAL_ADS_ALLOWED_BEFORE_LEVEL_START", defaultValue: false);
		InterstitialAdsAllowedAfterLevelFinish = RemoteSettings.GetBool("INTERSTITIAL_ADS_ALLOWED_AFTER_LEVEL_FINISH", defaultValue: true);
		InterstitialAdsAllowedOnLevelRestart = RemoteSettings.GetBool("INTERSTITIAL_ADS_ALLOWED_ON_LEVEL_RESTART", defaultValue: false);
	}

	internal static int GetInterstitialAdsLevelsCompletedRequired()
	{
		return InterstitialAdsLevelsCompletedRequired;
	}

	internal static bool GetInterstitialAdsRequireShotgunToShowOnLevelFailed()
	{
		return InterstitialAdsRequireShotgunToShowOnLevelFailed;
	}

	internal static bool GetInterstitialAdsAllowedBeforeLevelStart()
	{
		return InterstitialAdsAllowedBeforeLevelStart;
	}

	internal static bool GetInterstitialAdsAllowedAfterLevelFinish()
	{
		return InterstitialAdsAllowedAfterLevelFinish;
	}

	internal static bool GetInterstitialAdsAllowedOnLevelRestart()
	{
		return InterstitialAdsAllowedOnLevelRestart;
	}

	internal bool CanShowRewardedAd()
	{
		if (RewardedAdInProgress || InterstitialAdInProgress)
		{
			return false;
		}
		if (LastTimeFiredRewardedAd + TimeSpan.FromSeconds(RewadIntervalSeconds) > DateTime.Now)
		{
			return false;
		}
		//if (!CurrentAdProvider.IsRewardedAdReady())
		//{
		//	return false;
		//}
		return true;
	}

	internal void Init()
	{
		RemoteSettings.Updated += HandleRemoteSettingsUpdate;
		LastTimeFiredRewardedAd = DateTime.Now - TimeSpan.FromSeconds(InitialRewadAvailabilitySeconds);
		//if (Application.isEditor)
		//{
		//	CurrentAdProvider = new StubAdProvider();
		//}
		//else
		//{
		//	CurrentAdProvider = new AdMobAdProvider();
		//}
		//CurrentAdProvider.Initialize();
	}

	internal bool ShowRewardedVideo(Action<AdDisplayResult> callback)
	{
		if (!CanShowRewardedAd())
		{
			return false;
		}
		LastTimeFiredRewardedAd = DateTime.Now;
		LastTimeFiredInterstitialAd = DateTime.Now;
		SoundManager.instance.StopAllAudioSources();
		RewardedAdInProgress = true;
		//CurrentAdProvider.ShowRewardedAd(callback);
		return true;
	}

	private bool IsForcedAdReady()
	{
		if (RewardedAdInProgress || InterstitialAdInProgress)
		{
			return false;
		}
		//if (!CurrentAdProvider.IsForcedAdReady() || GlobalCommons.Instance.globalGameStats.isPayingPlayer)
		//{
		//	return false;
		//}
		if (DateTime.Now - LastTimeFiredInterstitialAd >= TimeSpan.FromSeconds(SkippableAdIntervalSeconds))
		{
			return true;
		}
		return false;
	}

	internal void ProcessInterstitialAdComplete()
	{
		SoundManager.instance.RestoreMusic();
		InterstitialAdInProgress = false;
	}

	internal void ProcessRewardedAdComplete()
	{
		SoundManager.instance.RestoreMusic();
		RewardedAdInProgress = false;
	}

	internal bool ShowForcedAd(Action<AdDisplayResult> callback)
	{
		if (!IsForcedAdReady())
		{
			return false;
		}
		SoundManager.instance.StopAllAudioSources();
		InterstitialAdInProgress = true;
        //CurrentAdProvider.ShowForcedAd(callback);
		LastTimeFiredInterstitialAd = DateTime.Now;
		return true;
	}

	public bool IsAdInProgress()
	{
		return InterstitialAdInProgress || RewardedAdInProgress;
	}

	internal int GetTimeSinceLastInterstititalAdFiredMilliseconds()
	{
		return (int)(DateTime.Now - LastTimeFiredInterstitialAd).TotalMilliseconds;
	}

	internal int GetTimeSinceLastRewardedAdFiredMilliseconds()
	{
		return (int)(DateTime.Now - LastTimeFiredRewardedAd).TotalMilliseconds;
	}

	internal void ShiftAdTimers(TimeSpan pauseTimespan)
	{
		if (!RewardedAdInProgress && !InterstitialAdInProgress)
		{
			LastTimeFiredInterstitialAd += pauseTimespan;
			LastTimeFiredRewardedAd += pauseTimespan;
		}
	}
}
