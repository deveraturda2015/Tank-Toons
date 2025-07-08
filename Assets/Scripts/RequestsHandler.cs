using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestsHandler : MonoBehaviour
{
	private static RequestsHandler instance;

	public static RequestsHandler Instance
	{
		get
		{
			return instance;
		}
		set
		{
			if (instance == null)
			{
				instance = value;
			}
		}
	}

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			UnityEngine.Object.DestroyImmediate(this);
		}
	}

	public WWW LoadLevel(int levelId)
	{
		return POST("http://loljet.com/tanksunity.php", new Dictionary<string, string>
		{
			{
				"comm",
				"readext"
			},
			{
				"id",
				levelId.ToString()
			}
		});
	}

	public WWW LoadRandomLevel()
	{
		return POST("http://loljet.com/tanksunity.php", new Dictionary<string, string>
		{
			{
				"comm",
				"getrandomlevel"
			}
		});
	}

	private WWW GET(string url)
	{
		WWW wWW = new WWW(url);
		StartCoroutine(WaitForRequest(wWW));
		return wWW;
	}

	private WWW POST(string url, Dictionary<string, string> post)
	{
		WWWForm wWWForm = new WWWForm();
		foreach (KeyValuePair<string, string> item in post)
		{
			wWWForm.AddField(item.Key, item.Value);
		}
		WWW wWW = new WWW(url, wWWForm);
		StartCoroutine(WaitForRequest(wWW));
		return wWW;
	}

	private IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
		if (www.error == null)
		{
			DebugHelper.Log("WWW Ok!: " + www.text);
		}
		else
		{
			DebugHelper.Log("WWW Error: " + www.error);
		}
	}
}
