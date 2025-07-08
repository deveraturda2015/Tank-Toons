using UnityEngine;

public class WireSparksSpawner : MonoBehaviour
{
	private float lastSpawnTimestamp;

	private float nextSpawnInterval;

	private UpgradesMenuController umc;

	private void Start()
	{
		lastSpawnTimestamp = Time.fixedTime;
		umc = UnityEngine.Object.FindObjectOfType<UpgradesMenuController>();
		ReinitSpawnInterval();
	}

	private void ReinitSpawnInterval()
	{
		nextSpawnInterval = UnityEngine.Random.Range(1.5f, 10f);
	}

	private void Update()
	{
		if (umc.upgradeMachinegunTutorActive || umc.upgradeArmorTutorActive || umc.prizeTutorActive || umc.UpgradeConfirmationActive)
		{
			lastSpawnTimestamp += Time.deltaTime;
		}
		else if (Time.fixedTime - (lastSpawnTimestamp + nextSpawnInterval) > 0f)
		{
			lastSpawnTimestamp = Time.fixedTime;
			ReinitSpawnInterval();
			if (UnityEngine.Object.FindObjectsOfType<ConfirmationWindow>().Length == 0 && UnityEngine.Object.FindObjectsOfType<SimpleUIMessageController>().Length == 0)
			{
				SoundManager.instance.PlaySparkSound();
				umc.effectsSpawner.SpawnWireSparksEffect(base.transform.position);
			}
		}
	}
}
