using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class DBTraverseHelper : MonoBehaviour
{
	private enum ErrorType
	{
		ValidationFailed,
		ParseError
	}

	private int CurrentLevelId = 81791;

	private int LastLevelId = 82407;

	private string FilePath = "D:\\dbparseresults" + DateTime.Now.ToFileTimeUtc().ToString() + ".txt";

	private DateTime LastTimeProcessedLevel = DateTime.Now;

	private TimeSpan LevelProcessInterval = TimeSpan.FromSeconds(0.2);

	private bool ProcessingInProgress;

	private void Start()
	{
		File.WriteAllText(FilePath, "Parsing levels " + CurrentLevelId.ToString() + " to " + LastLevelId.ToString() + Environment.NewLine + Environment.NewLine);
		UnityEngine.Debug.Log("DB PARSE START...");
	}

	private void Update()
	{
		if (DateTime.Now - LastTimeProcessedLevel > LevelProcessInterval && !ProcessingInProgress)
		{
			LastTimeProcessedLevel = DateTime.Now;
			if (CurrentLevelId > LastLevelId)
			{
				WriteStringToFIle("Completed processing levels");
				UnityEngine.Debug.Log("DB PARSE COMPLETE...");
				UnityEngine.Object.Destroy(this);
			}
			else
			{
				ProcessingInProgress = true;
				UnityEngine.Debug.Log("PROCESSING LEVEL #" + CurrentLevelId.ToString());

			}
		}
	}

	private void GetLevelCallback(UnityWebRequest request)
	{
		if (request.error != null)
		{
			WriteStringToFIle("Error requesting level " + CurrentLevelId.ToString());
		}
		else
		{
			try
			{
				LoadedLevelInfo loadedLevelInfo = JsonUtility.FromJson<LoadedLevelInfo>(request.downloadHandler.text);
				if (!LevelEditorController.LoadCustomLevel(loadedLevelInfo.data))
				{
					MarkCurrentLevelInappropriate(ErrorType.ValidationFailed);
				}
				WriteStringToFIle("Level " + CurrentLevelId.ToString() + " seems alright...");
			}
			catch (Exception)
			{
				MarkCurrentLevelInappropriate(ErrorType.ParseError);
			}
		}
		CurrentLevelId++;
		ProcessingInProgress = false;
	}

	private void LevelMarkedInappropriateCallback(UnityWebRequest request)
	{
		WriteStringToFIle("Level marked inappropriate");
	}

	private void MarkCurrentLevelInappropriate(ErrorType errorType)
	{
		switch (errorType)
		{
		case ErrorType.ParseError:
			WriteStringToFIle("LEVEL FAIL: PARSE ERROR OR MISSING LEVEL, id=" + CurrentLevelId.ToString() + ", marking as inappropriate");
			break;
		case ErrorType.ValidationFailed:
			WriteStringToFIle("LEVEL FAIL: VALIDATION ERROR, id=" + CurrentLevelId.ToString() + ", marking as inappropriate");
			break;
		}

	}

	private void WriteStringToFIle(string text)
	{
		File.AppendAllText(FilePath, text + Environment.NewLine);
	}
}
