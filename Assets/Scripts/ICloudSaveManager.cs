using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

public class ICloudSaveManager : CloudSaveManager
{
	private const string saveDataKey = "saveData";


	private static extern bool iCloudKV_Synchronize();


	private static extern void iCloudKV_SetInt(string key, int value);


	private static extern void iCloudKV_SetFloat(string key, float value);


	private static extern void iCloudKV_SetString(string key, string value);


	private static extern int iCloudKV_GetInt(string key);


	private static extern float iCloudKV_GetFloat(string key);


	private static extern string iCloudKV_GetString(string key);


	private static extern void iCloudKV_Reset();

	public void Initialize()
	{
		iCloudKV_Synchronize();
	}

	public SavegameData GetCloudSaveData()
	{
		string text = iCloudKV_GetString("saveData");
		if (string.IsNullOrEmpty(text))
		{
			return null;
		}
		try
		{
			byte[] buffer = Convert.FromBase64String(text);
			MemoryStream serializationStream = new MemoryStream(buffer);
			return (SavegameData)new BinaryFormatter().Deserialize(serializationStream);
		}
		catch (Exception)
		{
			return null;
		}
	}

	public bool LoadGame()
	{
		string text = iCloudKV_GetString("saveData");
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		try
		{
			byte[] buffer = Convert.FromBase64String(text);
			MemoryStream serializationStream = new MemoryStream(buffer);
			SavegameData savegameData = (SavegameData)new BinaryFormatter().Deserialize(serializationStream);
			return GlobalCommons.Instance.ProceedWithGameLoad(savegameData, cloudLoading: true);
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool SaveGame()
	{
		string savegameDataString = GlobalCommons.Instance.GetSavegameDataString();
		iCloudKV_SetString("saveData", savegameDataString);
		bool flag = iCloudKV_Synchronize();
		if (flag)
		{
			GlobalCommons.Instance.globalGameStats.DidSaveToCloud = true;
		}
		return flag;
	}
}
