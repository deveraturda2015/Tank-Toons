using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoneyLootCounter
{
	private static Dictionary<int, float> levelCoefficientModifiers = new Dictionary<int, float>
	{
		{
			60,
			0.5f
		},
		{
			40,
			0.65f
		},
		{
			20,
			0.8f
		}
	};

	public static int GetEnemyMoneyLoot(bool forSurvival = false)
	{
		int currentCoefficient = (!forSurvival) ? (GlobalCommons.Instance.ActualSelectedLevel - 1) : GlobalCommons.Instance.globalGameStats.LevelsCompleted;
		currentCoefficient = GetModifiedLevelCoefficient(currentCoefficient);
		int num = Mathf.FloorToInt((float)(50 + 15 * currentCoefficient) * 0.12195f);
		if (!GlobalCommons.Instance.globalGameStats.AutoAimEnabled)
		{
			num *= Mathf.FloorToInt(1.25f);
		}
		return ProcessArenaCoefficient(num);
	}

	public static int GetSpawnerMoneyLoot()
	{
		int modifiedLevelCoefficient = GetModifiedLevelCoefficient(GlobalCommons.Instance.ActualSelectedLevel - 1);
		int num = Mathf.FloorToInt((float)(100 + 30 * modifiedLevelCoefficient) * 0.12195f);
		if (!GlobalCommons.Instance.globalGameStats.AutoAimEnabled)
		{
			num *= Mathf.FloorToInt(1.25f);
		}
		return ProcessArenaCoefficient(num);
	}

	private static int GetModifiedLevelCoefficient(int currentCoefficient)
	{
		float num = 0f;
		int num2 = currentCoefficient;
		for (int i = 0; i < levelCoefficientModifiers.Count; i++)
		{
			KeyValuePair<int, float> keyValuePair = levelCoefficientModifiers.ElementAt(i);
			if (num2 > keyValuePair.Key)
			{
				num += (float)(num2 - keyValuePair.Key) * keyValuePair.Value;
				num2 -= num2 - keyValuePair.Key;
			}
		}
		return Mathf.FloorToInt(num + (float)num2);
	}

	public static int GetCrateCoinsLoot()
	{
		int loot = Mathf.FloorToInt((float)GetEnemyMoneyLoot() * 3f);
		return ProcessArenaCoefficient(loot);
	}

	public static int GetSpecialCrateCoinsLoot()
	{
		return Mathf.FloorToInt((float)GetCrateCoinsLoot() * 4f);
	}

	private static int ProcessArenaCoefficient(int loot)
	{
		if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.ArenaLevel)
		{
			return Mathf.FloorToInt((float)loot * 1.25f);
		}
		return loot;
	}
}
