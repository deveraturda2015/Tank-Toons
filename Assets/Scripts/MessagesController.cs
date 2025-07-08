using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class MessagesController
{
	private List<ShowSimpleMessageRequest> SimpleMessageRequests = new List<ShowSimpleMessageRequest>();

	private List<ShowConfirmationDialogRequest> ConfirmationDialogRequests = new List<ShowConfirmationDialogRequest>();

	public MessagesController()
	{
		//AsyncRestCaller.GetServerMessages(ProcessServerMessagesFetchResult);
	}

	internal void Update()
	{
		if (!ConfirmationWindow.Active && !SimpleUIMessageController.Active && !GlobalCommons.Instance.StateFaderController.IsCurrentlyFading)
		{
			if (SimpleMessageRequests.Count > 0)
			{
				ShowSimpleMessageRequest showSimpleMessageRequest = SimpleMessageRequests[0];
				SimpleMessageRequests.RemoveAt(0);
				SimpleUIMessageController.ShowMessage(showSimpleMessageRequest);
			}
			else if (ConfirmationDialogRequests.Count > 0)
			{
				ShowConfirmationDialogRequest showConfirmationDialogRequest = ConfirmationDialogRequests[0];
				ConfirmationDialogRequests.RemoveAt(0);
				ConfirmationWindow.ShowConfirmation(showConfirmationDialogRequest);
			}
		}
	}

	internal void ShowSimpleMessage(string text, float scale = 1f)
	{
		SimpleMessageRequests.Add(new ShowSimpleMessageRequest(text, scale));
	}

	internal void ShowConfirmationDialog(string question, UnityAction yesEvent, UnityAction noEvent, float scale = 1f)
	{
		ConfirmationDialogRequests.Add(new ShowConfirmationDialogRequest(question, yesEvent, noEvent, scale));
	}

	private void ProcessServerMessagesFetchResult(UnityWebRequest response)
	{
		if (string.IsNullOrEmpty(response.error))
		{
			try
			{
				string json = "{ \"Messages\": " + response.downloadHandler.text + "}";
				ServerMessagesList serverMessagesList = JsonUtility.FromJson<ServerMessagesList>(json);
				serverMessagesList.Messages.ForEach(delegate(string msg)
				{
					ShowSimpleMessage(msg);
				});
			}
			catch (Exception)
			{
			}
		}
	}
}
