using UnityEngine;

public class ComboController
{
	private float lastTimeKicked = -200f;

	private float comboWearoffTime = 3f;

	private int currentComboValue;

	private bool comboActive;

	public bool DisplayComboCounter => comboActive && currentComboValue > 1;

	public int CurrentComboValue => currentComboValue;

	public void Kick()
	{
		if (!GameplayCommons.Instance.levelStateController.LevelCompletionPending && !GameplayCommons.Instance.playersTankController.PlayerDead)
		{
			if (!comboActive)
			{
				comboActive = true;
				currentComboValue = 0;
			}
			GameplayCommons.Instance.gameplayUIController.ShakeComboText();
			currentComboValue++;
			lastTimeKicked = Time.fixedTime;
		}
	}

	internal void Update()
	{
		if (comboActive && (Time.fixedTime - lastTimeKicked > comboWearoffTime || GameplayCommons.Instance.levelStateController.LevelCompletionPending || GameplayCommons.Instance.playersTankController.PlayerDead))
		{
			comboActive = false;
			GlobalCommons.Instance.globalGameStats.GameStatistics.UpdateMaxStat(GameStatistics.Stat.BiggestCombo, currentComboValue);
		}
	}
}
