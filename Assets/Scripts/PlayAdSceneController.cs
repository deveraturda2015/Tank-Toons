using UnityEngine;

public class PlayAdSceneController : MonoBehaviour
{
	private void Start()
	{
		if (GlobalCommons.Instance.globalGameStats.IsPayingPlayer || GlobalCommons.Instance.globalGameStats.LevelsCompleted < AdsProcessor.GetInterstitialAdsLevelsCompletedRequired() || (GlobalCommons.Instance.lastLevelResolution == GlobalCommons.LevelResolution.Failed && GlobalCommons.Instance.globalGameStats.AvailableWeaponsCount < 2 && AdsProcessor.GetInterstitialAdsRequireShotgunToShowOnLevelFailed()))
		{
			GlobalCommons.Instance.StateFaderController.ChangeSceneTo(GlobalCommons.Instance.SceneToTransferTo, immediate: true);
		}
		else if (!GlobalCommons.Instance.AdsProcessor.ShowForcedAd(ProcessForcedAdCompleted))
		{
			GlobalCommons.Instance.StateFaderController.ChangeSceneTo(GlobalCommons.Instance.SceneToTransferTo, immediate: true);
		}
	}

	private void ProcessForcedAdCompleted(AdsProcessor.AdDisplayResult obj)
	{
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo(GlobalCommons.Instance.SceneToTransferTo, immediate: true);
	}
}
