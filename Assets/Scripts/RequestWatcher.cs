using System;
using UnityEngine.Networking;

internal class RequestWatcher
{
	private UnityWebRequest UnityWebRequest;

	private Action<UnityWebRequest> Callback;
}
