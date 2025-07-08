using System;

internal class StubAdProvider : IAdProvider
{
	public void Initialize()
	{
	}

	public bool IsRewardedAdReady()
	{
		return true;
	}

	public bool IsForcedAdReady()
	{
		return true;
	}

	public void ShowForcedAd(Action<AdsProcessor.AdDisplayResult> callback)
	{
		callback(AdsProcessor.AdDisplayResult.Finished);
		GlobalCommons.Instance.AdsProcessor.ProcessInterstitialAdComplete();
	}

	public void ShowRewardedAd(Action<AdsProcessor.AdDisplayResult> callback)
	{
		callback(AdsProcessor.AdDisplayResult.Finished);
		GlobalCommons.Instance.AdsProcessor.ProcessRewardedAdComplete();
	}
}
