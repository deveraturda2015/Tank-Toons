using UnityEngine.Events;

public class ShowConfirmationDialogRequest
{
	public string Question;

	public UnityAction YesEvent;

	public UnityAction NoEvent;

	public float Scale;

	public ShowConfirmationDialogRequest(string question, UnityAction yesEvent, UnityAction noEvent, float scale = 1f)
	{
		Question = question;
		YesEvent = yesEvent;
		NoEvent = noEvent;
		Scale = scale;
	}
}
