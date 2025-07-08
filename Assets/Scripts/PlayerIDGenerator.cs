using System.Text;
using UnityEngine;

public class PlayerIDGenerator
{
	public static string GenerateID()
	{
		string text = "ABCDEFGHIJKLMNPQRSTUVWXYZ0123456789";
		StringBuilder stringBuilder = new StringBuilder();
		switch (Application.platform)
		{
		case RuntimePlatform.Android:
			stringBuilder.Append("A");
			break;
		case RuntimePlatform.IPhonePlayer:
			stringBuilder.Append("I");
			break;
		default:
			stringBuilder.Append("D");
			break;
		}
		for (int i = 0; i < 9; i++)
		{
			stringBuilder.Append(text[Random.Range(0, text.Length)]);
		}
		string text2 = stringBuilder.ToString();
		if (WordFilter.ContainsProfanity(text2))
		{
			return GenerateID();
		}
		return text2;
	}
}
