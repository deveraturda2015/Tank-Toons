using System.Collections.Generic;

public class LevelsContainer
{
	private AllLevels allLevels;

	public int LevelsCount => allLevels.allLevels.Count;

	public LevelsContainer()
	{
		allLevels = new AllLevels();
	}

	public List<string> GetLevelByIndex(int index)
	{
		return allLevels.allLevels[index];
	}

	public List<string> GetSurvivalLevel()
	{
		return allLevels.survivalLevel;
	}

	public List<string> GetTutorialLevel()
	{
		return allLevels.tutorialLevel;
	}

	public int GetItemsLevel(int levelsCompletedFactor)
	{
		int num = levelsCompletedFactor;
		if (num >= LevelsCount)
		{
			num = LevelsCount - 1;
		}
		int num2 = 1;
		char[] array = "5hijklm".ToCharArray();
		List<string> levelByIndex = GetLevelByIndex(num);
		for (int i = 0; i < levelByIndex.Count; i++)
		{
			string text = levelByIndex[i];
			for (int j = 0; j < array.Length; j++)
			{
				if (text.IndexOf(array[j]) != -1)
				{
					int num3 = j + 1;
					if (num2 < num3)
					{
						num2 = num3;
					}
				}
			}
		}
		return num2;
	}
}
