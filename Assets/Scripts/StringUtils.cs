using System;
using System.Text;

internal class StringUtils
{
	private static Random rand = new Random();

	public static string ScrambleString(string input)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(input);
		int length = stringBuilder.Length;
		for (int i = 0; i < length * 2; i++)
		{
			int index = rand.Next() % length;
			int index2 = rand.Next() % length;
			char value = stringBuilder[index];
			stringBuilder[index] = stringBuilder[index2];
			stringBuilder[index2] = value;
		}
		return stringBuilder.ToString();
	}
}
