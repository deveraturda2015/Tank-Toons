using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

public static class EncryptString
{
	private static byte[] key;

	private static byte[] iv;

	private static bool isInitialized;

	public static string Encrypt(string plainText)
	{
		if (!isInitialized)
		{
			Initialize();
		}
		SymmetricAlgorithm symmetricAlgorithm = DES.Create();
		ICryptoTransform cryptoTransform = symmetricAlgorithm.CreateEncryptor(key, iv);
		byte[] bytes = Encoding.Unicode.GetBytes(plainText);
		byte[] inArray = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
		return Convert.ToBase64String(inArray);
	}

	public static string Decrypt(string cipherText)
	{
		if (!isInitialized)
		{
			Initialize();
		}
		SymmetricAlgorithm symmetricAlgorithm = DES.Create();
		ICryptoTransform cryptoTransform = symmetricAlgorithm.CreateDecryptor(key, iv);
		byte[] array = Convert.FromBase64String(cipherText);
		byte[] bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
		return Encoding.Unicode.GetString(bytes);
	}

	private static void Initialize()
	{
		isInitialized = true;
		key = GetByteList(8, 3);
		iv = GetByteList(8, 7);
	}

	private static byte[] GetByteList(int length, int seed)
	{
		Random random = new Random(seed);
		List<byte> list = new List<byte>();
		for (int i = 0; i < length; i++)
		{
			byte item;
			do
			{
				byte[] array = new byte[1];
				random.NextBytes(array);
				item = array[0];
			}
			while (list.Contains(item));
			list.Add(item);
		}
		return list.ToArray();
	}
}
