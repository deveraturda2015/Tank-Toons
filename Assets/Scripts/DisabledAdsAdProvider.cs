using System;

internal class DisabledAdsAdProvider : IAdProvider
{
	public void Initialize()
	{
	}

	public bool IsRewardedAdReady()
	{
		return false;
	}

	public bool IsForcedAdReady()
	{
		return false;
	}

	public void ShowForcedAd(Action<AdsProcessor.AdDisplayResult> callback)
	{
		callback(AdsProcessor.AdDisplayResult.Finished);
	}

	public void ShowRewardedAd(Action<AdsProcessor.AdDisplayResult> callback)
	{
		callback(AdsProcessor.AdDisplayResult.Finished);
	}
}
