using UnityEngine;

public class DebugHelper
{
	public static void Log(object message)
	{
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log(message);
		}
	}
}
