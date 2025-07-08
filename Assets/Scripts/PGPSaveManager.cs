
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public class PGPSaveManager : CloudSaveManager
{
	private static string m_saveName = "saveData";

	internal bool OperationInProgress;

	internal SavegameData loadedSaveData;

	internal bool saveSuccessful;

	public void Initialize()
	{
	}

	public SavegameData GetCloudSaveData()
	{
		if (OperationInProgress)
		{
			return null;
		}
		OperationInProgress = true;
		return null;
	}

	//private void ProcessSaveGameGetResult(SavedGameRequestStatus status, ISavedGameMetadata game)
	//{
	//	if (status == SavedGameRequestStatus.Success)
	//	{
	//		DebugHelper.Log("loading GPG game succeeded.");
	//		((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(game, SavedGameLoaded);
	//	}
	//	else
	//	{
	//		DebugHelper.Log("loading GPG game failed.");
	//		OperationInProgress = false;
	//	}
	//}

	//private void SavedGameLoaded(SavedGameRequestStatus status, byte[] data)
	//{
	//	try
	//	{
	//		if (status == SavedGameRequestStatus.Success)
	//		{
	//			DebugHelper.Log("SaveGameLoaded, success=" + status);
	//			ProcessCloudData(data);
	//		}
	//		else
	//		{
	//			DebugHelper.Log("Error reading game: " + status);
	//		}
	//	}
	//	catch (Exception)
	//	{
	//	}
	//	OperationInProgress = false;
	//}

	private void ProcessCloudData(byte[] cloudData)
	{
		if (cloudData == null)
		{
			DebugHelper.Log("No data saved to the cloud yet...");
			return;
		}
		DebugHelper.Log("Decoding cloud data from bytes.");
		string s = FromBytes(cloudData);
		byte[] buffer = Convert.FromBase64String(s);
		MemoryStream serializationStream = new MemoryStream(buffer);
		loadedSaveData = (SavegameData)new BinaryFormatter().Deserialize(serializationStream);
	}

	public bool LoadGame()
	{
		if (OperationInProgress)
		{
			return false;
		}
		loadedSaveData = null;
		OperationInProgress = true;
		//((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(m_saveName, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, ProcessSaveGameGetResult);
		return false;
	}

	public bool SaveGame()
	{
		if (OperationInProgress)
		{
			return false;
		}
		OperationInProgress = true;
		saveSuccessful = false;
		//((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(m_saveName, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, ProcessSaveGameSetResult);
		return false;
	}

	//private void ProcessSaveGameSetResult(SavedGameRequestStatus status, ISavedGameMetadata game)
	//{
	//	if (status == SavedGameRequestStatus.Success)
	//	{
	//		byte[] bytes = Encoding.UTF8.GetBytes(GlobalCommons.Instance.GetSavegameDataString());
	//		SavedGameMetadataUpdate updateForMetadata = default(SavedGameMetadataUpdate.Builder).Build();
	//		((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(game, updateForMetadata, bytes, SavedGameWritten);
	//		GlobalCommons.Instance.globalGameStats.DidSaveToCloud = true;
	//	}
	//	else
	//	{
	//		DebugHelper.Log("Error saving game: " + status);
	//		OperationInProgress = false;
	//	}
	//}

	//private void SavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
	//{
	//	if (status == SavedGameRequestStatus.Success)
	//	{
	//		DebugHelper.Log("Game written");
	//		saveSuccessful = true;
	//	}
	//	else
	//	{
	//		DebugHelper.Log("Error saving game: " + status);
	//	}
	//	OperationInProgress = false;
	//}

	private string FromBytes(byte[] bytes)
	{
		return Encoding.UTF8.GetString(bytes);
	}
}
