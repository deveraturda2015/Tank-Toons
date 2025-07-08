using System.Collections.Generic;

internal class LevelUtils
{
	public static void CropLevel(List<string> levelToCrop)
	{
		int num = levelToCrop[0].Length - 1;
		int num2 = 0;
		for (int num3 = levelToCrop.Count - 1; num3 >= 0; num3--)
		{
			string text = levelToCrop[num3];
			bool flag = true;
			bool flag2 = false;
			int num4 = 0;
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] == 'f')
				{
					continue;
				}
				flag = false;
				num4 = i;
				if (!flag2)
				{
					flag2 = true;
					if (i < num)
					{
						num = i;
					}
				}
			}
			if (num4 > num2)
			{
				num2 = num4;
			}
			if (flag)
			{
				levelToCrop.RemoveAt(num3);
			}
		}
		for (int j = 0; j < levelToCrop.Count; j++)
		{
			levelToCrop[j] = levelToCrop[j].Substring(num, num2 - num + 1);
		}
	}
}
