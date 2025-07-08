using System;

internal interface IAdProvider
{
	void Initialize();

	bool IsRewardedAdReady();

	bool IsForcedAdReady();

	void ShowForcedAd(Action<AdsProcessor.AdDisplayResult> callback);

	void ShowRewardedAd(Action<AdsProcessor.AdDisplayResult> callback);
}
