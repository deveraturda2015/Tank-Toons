using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllLevelsCompleteSceneController : MonoBehaviour
{
	private Button ContinueButton;

	private List<float> fireworksTimestamps;

	public EffectsSpawner effectsSpawner;

	private void Start()
	{
		GameObject gameObject = GameObject.Find("Canvas");
		effectsSpawner = new EffectsSpawner(EffectsSpawner.EffectsSpawnerPreset.RewardedVideoAndShopMenuAndPrizeScene);
		//ContinueButton = gameObject.transform.Find("ContinueButton").GetComponent<Button>();
		//ContinueButton.onClick.AddListener(delegate
		//{
		//	ContinueBtnClick();
		//});
		SoundManager.instance.ToggleMusic(SoundManager.MusicType.GameplayMusic);
		InitFireworks();
		GlobalCommons.Instance.SaveGame();
	}

	private void Update()
	{
		effectsSpawner.Update();
		if (fireworksTimestamps.Count > 0 && Time.fixedTime > fireworksTimestamps[0])
		{
			fireworksTimestamps.RemoveAt(0);
			SoundManager.instance.PlayFireworkSound();
			EffectsSpawner obj = effectsSpawner;
			Vector3 position = Camera.main.transform.position;
			float x = position.x - GlobalCommons.Instance.horizontalCameraBorderWithCompensation / 2f + UnityEngine.Random.value * GlobalCommons.Instance.horizontalCameraBorderWithCompensation;
			Vector3 position2 = Camera.main.transform.position;
			obj.SpawnFireworksEffect(new Vector3(x, position2.y - GlobalCommons.Instance.verticalCameraBorderWithCompensation / 2f + UnityEngine.Random.value * GlobalCommons.Instance.verticalCameraBorderWithCompensation, 0f));
		}
	}

	private void InitFireworks()
	{
		fireworksTimestamps = new List<float>();
		fireworksTimestamps.Add(Time.fixedTime + 0.5f);
		for (int i = 0; i < 6; i++)
		{
			float num = UnityEngine.Random.Range(0.1f, 0.4f);
			if (fireworksTimestamps.Count == 0)
			{
				fireworksTimestamps.Add(num);
			}
			else
			{
				fireworksTimestamps.Add(num + fireworksTimestamps[fireworksTimestamps.Count - 1]);
			}
		}
	}

	public void ContinueBtnClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		GlobalCommons.Instance.StateFaderController.ChangeSceneTo("Upgrades");
	}
}
